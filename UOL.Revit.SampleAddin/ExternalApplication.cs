using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using Cadac.ClientTools;
using Cadac.Logging;
using Cadac.ServiceProvider;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using UOL.Revit.SampleAddin.Utilities;
using WindowsApplication = System.Windows.Forms;

namespace UOL.Revit.SampleAddin
{
    /// <summary>
    /// Represents the ribbon class for creating the Revit add in ribbon.
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]
    [Journaling(JournalingMode.NoCommandData)]
    public class ExternalApplication : IExternalApplication
    {
        private IRegistryManager registryManager;

        /// <summary>
        /// The On Shutdown function that is called by Revit.
        /// </summary>
        /// <param name="application">The UI Controlled Application of Revit.</param>
        /// <returns>
        /// An Autodesk.Revit.UI.Result if shutdown is succeeded or failed.
        /// </returns>
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        /// <summary>
        /// The On Startup function that is called by Revit.
        /// </summary>
        /// <param name="application">The UI Controlled Application of Revit.</param>
        /// <returns>
        /// An Autodesk.Revit.UI.Result if startup is succeeded or failed.
        /// </returns>
        [STAThread]
        public Result OnStartup(UIControlledApplication application)
        {
            if (application != null)
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                InitializeApplication(application);
                var monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();
                using (var executionBlock = monitoredExecutionContext
                    .MonitorMethod<ExternalApplication>(nameof(OnStartup))
                    .WithTiming())
                {
                    new RibbonHelper().CreateRibbon(application);
                    return Result.Succeeded;
                }
            }

            return Result.Failed;
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            // Ignore missing resources
            if (args.Name.Contains(".resources"))
            {
                return null;
            }

            // check for assemblies already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

            // Try to load by filename - split out the filename of the full assembly name
            // and append the base path of the original assembly (ie. look in the same dir)
            var filename = args.Name.Split(',')[0] + ".dll".ToLower();
            var dependencyFolder = ApplicationGlobals.GetAppFolder();
            var asmFile = Path.Combine(dependencyFolder, filename);

            try
            {
                if (File.Exists(asmFile))
                {
                    return System.Reflection.Assembly.LoadFrom(asmFile);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        private void InitializeApplication(UIControlledApplication application)
        {
            ApplicationGlobals.UiControlledApplication = application;
            WindowsApplication.Application.EnableVisualStyles();
            var applicationContext = new ApplicationContext("UOL Revit Sample Addin", "1.0.0.0", "UOLSample");
            ApplicationGlobals.ApplicationContext = applicationContext;

            // Add all services to the ApplicationContext that this Revit Add-in requires.
            applicationContext.AddUOLRevitAddIn();

            var monitoredExecutionContext = ApplicationGlobals.ApplicationContext.GetService<IMonitoredExecutionContext>();
            var windowTools = new WindowTools(monitoredExecutionContext);
            ApplicationGlobals.HostWindow = windowTools.GetWindowHandle("Revit");

            registryManager = ApplicationGlobals.ApplicationContext.GetService<IRegistryManager>();
            var registryHelper = new RegistryHelper();
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(registryHelper.GetLanguage());
        }
    }
}
