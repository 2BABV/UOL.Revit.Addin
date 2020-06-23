using Cadac.Logging;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using UOL.Revit.SampleAddin.Models;

namespace UOL.Revit.SampleAddin.Utilities
{
    /// <summary>
    /// Provides window related functions.
    /// </summary>
    public class WindowTools
    {
        private readonly IMonitoredExecutionContext monitoredExecutionContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="WindowTools"/> class.
        /// </summary>
        /// <param name="monitoredExecutionContext">The <see cref="MonitoredExecutionContext"/> to use for logging purposes.</param>
        public WindowTools(IMonitoredExecutionContext monitoredExecutionContext)
        {
            this.monitoredExecutionContext = monitoredExecutionContext;
        }

        /// <summary>
        /// Gets the window handle of the active process.
        /// </summary>
        /// <param name="processName">The name of the process to get the handle of.</param>
        /// <returns>The window handle of the active process.</returns>
        public IWin32Window GetWindowHandle(string processName)
        {
            using (var executionBlock = monitoredExecutionContext
                .MonitorMethod<WindowTools>(nameof(GetWindowHandle))
                .WithTiming())
            {
                try
                {
                    var processes = Process.GetProcessesByName(processName);

                    if (processes.Length > 0)
                    {
                        var h = processes[0].MainWindowHandle;
                        return new WindowHandle(h);
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception exception)
                {
                    executionBlock.LogException(exception);
                    return null;
                }
            }
        }
    }
}
