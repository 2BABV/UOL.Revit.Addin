using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Windows.Forms;
using UOL.Revit.SampleAddin.Forms;
using UOL.Revit.SampleAddin.Properties;
using UOL.SDK;

namespace UOL.Revit.SampleAddin.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class AddValidationRequestExternalCommand : IExternalCommand
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

                /// RequestValidationForm is initialized and shown.
                using (var requestValidationForm = new RequestValidationForm())
                {
                    var dialogResult = requestValidationForm.ShowDialog(ApplicationGlobals.HostWindow);

                    if (dialogResult == DialogResult.No)
                    {
                        do
                        {
                            requestValidationForm.FamilyInstances = new ElementHelper().SelectElements(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument, typeof(FamilyInstance), Resources.MessageSelectFamilyInstancesToValidate, requestValidationForm.FamilyInstances).Where(e => ((FamilyInstance)e).Symbol.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null || e.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null).ToList();

                            dialogResult = requestValidationForm.ShowDialog(ApplicationGlobals.HostWindow);
                        }
                        while (dialogResult == DialogResult.No);
                    }

                    if (dialogResult == DialogResult.OK && requestValidationForm.CADValidationRequest != null && requestValidationForm.CADValidationRequest.CADProducts.Count() > 0)
                    {
                        try
                        {
                            var ticket = UOLAddInUtilities.UIThreadSafeAsync(
                                () =>
                                {
                                    var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
                                    var returnValue = uolClient.AddValidationRequestAsync(requestValidationForm.CADValidationRequest);
                                    return returnValue;
                                }).Result;

                            var started = UOLAddInUtilities.UIThreadSafeAsync(
                                () =>
                                {
                                    var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
                                    var returnValue = "gestart";
                                    uolClient.StartValidationAsync(requestValidationForm.CADValidationRequest.ProjectId, $"{requestValidationForm.CADValidationRequest.TicketNumber}");
                                    return returnValue;
                                });
                        }
                        catch (UOLClientException)
                        {
                            MessageBox.Show(Resources.MessageBoxAddValidationRequestError_Text, Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                return Result.Succeeded;
            }
        }
    }
}
