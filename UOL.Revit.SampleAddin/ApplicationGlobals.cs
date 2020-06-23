using Autodesk.Revit.UI;
using Cadac.ServiceProvider;
using System;
using System.IO;
using System.Reflection;

namespace UOL.Revit.SampleAddin
{
    /// <summary>
    /// Provides a class with global variables for the addin.
    /// </summary>
    public static class ApplicationGlobals
    {
        /// <summary>
        /// Gets or sets the application context to use.
        /// </summary>
        /// <value>A <see cref="ApplicationContext"/> object.</value>
        public static ApplicationContext ApplicationContext { get; set; }

        /// <summary>
        /// Gets or sets the value of UIControlledApplication.
        /// </summary>
        /// <value>A <see cref="UIControlledApplication"/> object.</value>
        public static UIControlledApplication UiControlledApplication { get; set; }

        /// <summary>
        /// Gets or sets the value of externalCommandData.
        /// </summary>
        /// <value>A <see cref="ExternalCommandData"/> object.</value>
        public static ExternalCommandData ExternalCommandData { get; set; }

        /// <summary>
        /// Gets or sets the window handle of Revit to use as host for message boxes and forms in the addin.
        /// </summary>
        /// <value>A <see cref="System.Windows.Forms.IWin32Window"/> object.</value>
        public static System.Windows.Forms.IWin32Window HostWindow { get; set; }

        /// <summary>
        /// Gets the current revit version based on the UIControlledApplication, if that isn't set than return an empty string.
        /// </summary>
        /// <returns>The RevitVersion</returns>
        public static string GetRevitVersion()
        {
            return UiControlledApplication != null ? UiControlledApplication.ControlledApplication.VersionNumber : string.Empty;
        }

        /// <summary>
        /// Gets the app folder, which is the execution assemblies location.
        /// </summary>
        /// <returns>The AppFolder</returns>
        public static string GetAppFolder()
        {
            var assemblyPath = Assembly.GetAssembly(typeof(ExternalApplication)).Location;
            return Path.GetDirectoryName(assemblyPath);
        }

        /// <summary>
        /// Gets the CadContentFolder and create it if it doesn't exit.
        /// </summary>
        /// <returns>The CadContentFolder</returns>
        public static string GetCadContentFolder()
        {
            var cadContentFolder = Path.Combine(GetAppFolder(), "CadContent");
            Directory.CreateDirectory(cadContentFolder);
            return cadContentFolder;
        }

        /// <summary>
        /// Gets the assembly version.
        /// </summary>
        /// <returns>The AssemblyVersion</returns>
        public static Version GetAssemblyVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Gets the assembly copyright attribute.
        /// </summary>
        /// <returns>The AssemblyCopyright</returns>
        public static string GetAssemblyCopyright()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }

        /// <summary>
        /// Gets the assembly description attribute.
        /// </summary>
        /// <returns>The AssemblyDescription</returns>
        public static string GetAssemblyDescription()
        {
            var attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            return attributes.Length == 0 ? string.Empty : ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }
}
