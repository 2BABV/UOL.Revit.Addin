using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Cadac.Logging;
using Microsoft.Extensions.DependencyInjection;
using UOL.Revit.SampleAddin.Forms;

namespace UOL.Revit.SampleAddin.Commands
{
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    internal class ProcessValidationRequestExternalCommand : IExternalCommand
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

                using (var processValidationForm = new ProcessValidationForm())
                {
                    var dialogResult = processValidationForm.ShowDialog(ApplicationGlobals.HostWindow);
                }

                return Result.Succeeded;
            }
        }
    }
}
