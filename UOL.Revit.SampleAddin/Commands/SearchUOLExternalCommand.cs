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
using System.Net;
using System.Windows.Forms;
using UOL.Models;
using UOL.SDK;
using UOL.SDK.Controls;

namespace UOL.Revit.SampleAddin.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class SearchUOLExternalCommand : IExternalCommand
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

                /// Save the current security protocol settings for web requests.
                var currentSecurityProtocol = ServicePointManager.SecurityProtocol;
                try
                {
                    /// Set the security protocol to TLS12.
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    var dialogResult = uolClientControls.ShowEtimClassPickerDialog(ApplicationGlobals.HostWindow, Properties.Resources.ApplicationTitle);

                    if (dialogResult.Cancelled || dialogResult.CADFilterResult.CADProducts.Count() == 0)
                    {
                        return Result.Cancelled;
                    }

                    foreach (var cadProduct in dialogResult.CADFilterResult.CADProducts)
                    {
                        try
                        {
                            /// Get the metadata of the cadproduct
                            var cadMetadataList = UOLAddInUtilities.UIThreadSafeAsync(
                            () =>
                            {
                                var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
                                return uolClient.GetMetadataAsync(new List<CADMetadataCriteria> { UOLAddInUtilities.GetCADMetadataCriteria(cadProduct) });
                            }).Result;

                            /// Do a thread safe call for downloading CAD Content.
                            var cadContent = UOLAddInUtilities.UIThreadSafeAsync(
                                () =>
                                {
                                    var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
                                    return uolClient.DownloadCadContentAsync(
                                        new CADApplication
                                        {
                                            Name = "Revit",
                                            Version = ApplicationGlobals.GetRevitVersion()
                                        },
                                        new ETIM
                                        {
                                            ClassCode = cadProduct.EtimClassCode,
                                            ClassCodeVersion = cadProduct.EtimClassCodeVersion,
                                            ModelCode = cadProduct.ModellingClassCode,
                                            ModelCodeVersion = cadProduct.ModellingClassCodeVersion
                                        },
                                        cadMetadataList.FirstOrDefault() != null ? cadMetadataList.FirstOrDefault().Type : "Template");
                                }).Result;

                            if (cadContent.Exists)
                            {
                                var rfaPath = CreateRfaFile(cadContent);
                                var typeName = $"{cadProduct.EtimClassCode}_{cadProduct.ModellingClassCode}_{DateTime.Now.Ticks}";

                                if (string.IsNullOrEmpty(typeName))
                                {
                                    return Result.Cancelled;
                                }

                                dialogResult.CADFilterResult.CADProducts.FirstOrDefault().Name = typeName;

                                // Load rfa into Revit project
                                var result = revitHost.InsertCadContent(rfaPath, dialogResult.CADFilterResult.CADProducts.FirstOrDefault().Name, dialogResult.CADFilterResult.CADProducts.FirstOrDefault().Properties, dialogResult.CADFilterResult.CADProducts.Count() == 1, cadMetadataList.FirstOrDefault());
                                if (result.State == CADHostResultState.Succeeded)
                                {
                                    result = revitHost.WriteAdditionalData(result.CADObject, JsonConvert.SerializeObject(dialogResult.CADFilterResult));
                                }

                                return result.State == CADHostResultState.Succeeded ? Result.Succeeded : Result.Failed;
                            }
                            else
                            {
                                MessageBox.Show(Properties.Resources.MessageBoxCadContentNotFound_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (UOLClientException)
                        {
                            MessageBox.Show(Properties.Resources.MessageBoxCadacContentDownloadError_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (UOLClientDataException)
                {
                    MessageBox.Show(Properties.Resources.MessageBoxCadContentIncomplete_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (UOLClientException)
                {
                    MessageBox.Show(Properties.Resources.MessageBoxCadacContentDownloadError_Text, Properties.Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ServicePointManager.SecurityProtocol = currentSecurityProtocol;
                }

                return Result.Failed;
            }
        }

        private static string CreateRfaFile(CADContent cadContent)
        {
            // Write the filestream to disk
            var rfaPath = Path.Combine(ApplicationGlobals.GetCadContentFolder(), cadContent.Filename);

            using (var fileStream = new FileStream(rfaPath, FileMode.Create))
            {
                cadContent.CadByteStream.CopyTo(fileStream);
            }

            return rfaPath;
        }
    }
}
