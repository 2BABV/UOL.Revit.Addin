using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UOL.Models;
using UOL.SDK;
using UOL.SDK.Controls;

namespace UOL.Revit.SampleAddin.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class EditInstancesExternalCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            /// monitoredExecutionContext is used for logging purposes.
            var monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();

            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<ExternalApplication>(nameof(Execute))
                .WithTiming())
            {
                /// ExternalCommandData is stored in a static variable for later use.
                ApplicationGlobals.ExternalCommandData = commandData;

                var uolClientControls = ApplicationGlobals.ApplicationContext.GetService<IUOLClientControls>();
                var revitHost = ApplicationGlobals.ApplicationContext.GetService<ICADHost>();

                /// Select families you want to edit.
                var familyInstanceList = new ElementHelper().SelectElements(commandData.Application.ActiveUIDocument, typeof(FamilyInstance), "Select the familyinstances to edit", false);

                if (familyInstanceList != null && familyInstanceList.Count > 0)
                {
                    /// Filter out the unique family symbols to send online.
                    var uniqueFamilySymbols = new List<FamilySymbol>();
                    foreach (FamilyInstance familyInstance in familyInstanceList)
                    {
                        var foundFamilySymbol = uniqueFamilySymbols.FirstOrDefault(familySymbol => familySymbol.Id.Equals(familyInstance.Symbol.Id));
                        if (foundFamilySymbol == null)
                        {
                            uniqueFamilySymbols.Add(familyInstance.Symbol);
                        }
                    }

                    /// This function only works when there is only one unique family symbol!
                    if (uniqueFamilySymbols.Count == 1)
                    {
                        try
                        {
                            var path = commandData.Application.ActiveUIDocument.Document.PathName;
                            var cadFilterResult = new CADFilterResult();

                            if (!string.IsNullOrEmpty(path))
                            {
                                /// Read the metadata for the family from file.
                                var filterresult = File.ReadAllText(path.Replace(".rvt", $"_{uniqueFamilySymbols.FirstOrDefault().Id.IntegerValue.ToString()}.met"));
                                cadFilterResult = JsonConvert.DeserializeObject<CADFilterResult>(filterresult);
                            }

                            /// Show the edit dialog.
                            var dialogResult = uolClientControls.ShowUOLEditDialog(new List<CADFilterResult>() { cadFilterResult }, ApplicationGlobals.HostWindow, Properties.Resources.ApplicationTitle);

                            if (dialogResult.Cancelled || dialogResult.CADFilterResult.CADProducts.Count() == 0)
                            {
                                return Result.Cancelled;
                            }

                            var cadfilterResultCADProduct = cadFilterResult.CADProducts.FirstOrDefault();
                            var dialogResultCADProduct = dialogResult.CADFilterResult.CADProducts.FirstOrDefault();

                            if (cadfilterResultCADProduct.EtimClassCode.Equals(dialogResultCADProduct.EtimClassCode) && cadfilterResultCADProduct.ModellingClassCode.Equals(dialogResultCADProduct.ModellingClassCode))
                            {
                                if (HasChangedProperties(cadfilterResultCADProduct, dialogResultCADProduct))
                                {
                                    var typeName = $"{dialogResultCADProduct.EtimClassCode}_{dialogResultCADProduct.ModellingClassCode}_{DateTime.Now.Ticks}";

                                    dialogResultCADProduct.Name = typeName;

                                    var cadMetadataList = UOLAddInUtilities.UIThreadSafeAsync(
                                    () =>
                                    {
                                        var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
                                        return uolClient.GetMetadataAsync(new List<CADMetadataCriteria> { UOLAddInUtilities.GetCADMetadataCriteria(dialogResultCADProduct) });
                                    }).Result;

                                    var result = revitHost.InsertCadContent(uniqueFamilySymbols.FirstOrDefault().FamilyName, dialogResultCADProduct.Name, dialogResultCADProduct.Properties, false, cadMetadataList.FirstOrDefault());
                                    if (result.State == CADHostResultState.Succeeded)
                                    {
                                        result = revitHost.WriteAdditionalData(result.CADObject, JsonConvert.SerializeObject(dialogResult.CADFilterResult));

                                        if (result.State == CADHostResultState.Succeeded && result.CADObject is FamilySymbol familySymbolNew)
                                        {
                                            using (var transaction = new Transaction(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document, "Edit Family Instances"))
                                            {
                                                transaction.Start();
                                                foreach (FamilyInstance item in familyInstanceList)
                                                {
                                                    item.Symbol = familySymbolNew;
                                                }

                                                transaction.Commit();
                                            }
                                        }
                                    }

                                    return result.State == CADHostResultState.Succeeded ? Result.Succeeded : Result.Failed;
                                }
                                else
                                {
                                    var result = revitHost.WriteAdditionalData(uniqueFamilySymbols.FirstOrDefault(), JsonConvert.SerializeObject(dialogResult.CADFilterResult));
                                    return result.State == CADHostResultState.Succeeded ? Result.Succeeded : Result.Failed;
                                }
                            }
                            else
                            {
                                MessageBox.Show(Properties.Resources.MessageBoxECcodeMCcodeChangeNotAllowed_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            return Result.Succeeded;
                        }
                        catch (UOLClientDataException)
                        {
                            MessageBox.Show(Properties.Resources.MessageBoxCadContentIncomplete_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        catch (UOLClientException)
                        {
                            MessageBox.Show(Properties.Resources.MessageBoxCadacContentDownloadError_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show(Properties.Resources.MessageBoxMultipleTypesInSelectionNotAllowed_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }

                return Result.Failed;
            }
        }

        private bool HasChangedProperties(CADProduct cadProductOld, CADProduct cadProductNew)
        {
            var hasChangedPropertyValues = false;

            foreach (var propertyNew in cadProductNew.Properties)
            {
                var propertyOld = cadProductOld.Properties.FirstOrDefault(property => property.Code.Equals(propertyNew.Code, StringComparison.OrdinalIgnoreCase) && property.PortCode.Equals(propertyNew.PortCode));

                if (propertyOld == null ||
                    (propertyNew.Value == null && (propertyOld.UserDefinedValue != null || propertyOld.Value != null)) ||
                    (propertyNew.Value != null && propertyOld.UserDefinedValue == null && propertyOld.Value == null))
                {
                    hasChangedPropertyValues = true;
                    break;
                }

                if (propertyNew.Value != null)
                {
                    var propertyOldCode = propertyOld.UserDefinedValue == null || propertyOld.UserDefinedValue.Code == null ? propertyOld.Value.Code : propertyOld.UserDefinedValue.Code;
                    var propertyOldDescription = propertyOld.UserDefinedValue == null || propertyOld.UserDefinedValue.Description == null ? propertyOld.Value.Description : propertyOld.UserDefinedValue.Description;
                    var propertyOldLogicalValue = propertyOld.UserDefinedValue == null || propertyOld.UserDefinedValue.LogicalValue == null ? propertyOld.Value.LogicalValue : propertyOld.UserDefinedValue.LogicalValue;
                    var propertyOldNumericValue = propertyOld.UserDefinedValue == null || propertyOld.UserDefinedValue.NumericValue == null ? propertyOld.Value.NumericValue : propertyOld.UserDefinedValue.NumericValue;

                    if (propertyNew.Selected != propertyOld.Selected)
                    {
                        hasChangedPropertyValues = true;
                        break;
                    }
                    else if (propertyNew.Selected)
                    {
                        if (!(string.IsNullOrEmpty(propertyNew.Value.Code).Equals(string.IsNullOrEmpty(propertyOldCode)) || string.Equals(propertyNew.Value.Code, propertyOldCode, StringComparison.OrdinalIgnoreCase)) ||
                            !(string.IsNullOrEmpty(propertyNew.Value.Description).Equals(string.IsNullOrEmpty(propertyOldDescription)) || string.Equals(propertyNew.Value.Description, propertyOldDescription, StringComparison.OrdinalIgnoreCase) ||
                                (propertyNew.Value.Code != null && string.Equals(propertyNew.Value.Code, propertyOldCode, StringComparison.OrdinalIgnoreCase))) ||
                            !bool.Equals(propertyNew.Value.LogicalValue, propertyOldLogicalValue) ||
                            !decimal.Equals(propertyNew.Value.NumericValue, propertyOldNumericValue))
                        {
                            hasChangedPropertyValues = true;
                            break;
                        }
                    }
                }
            }

            return hasChangedPropertyValues;
        }
    }
}