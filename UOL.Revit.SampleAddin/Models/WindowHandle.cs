using System;
using System.Windows.Forms;

namespace UOL.Revit.SampleAddin.Models
{
    /// <summary>
    /// Provides methods and properties for the window handle.
    /// </summary>
    public class WindowHandle : IWin32Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowHandle"/> class.
        /// </summary>
        /// <param name="h">The <see cref="IntPtr"/> of the window.</param>
        public WindowHandle(IntPtr h)
        {
            Handle = h;
        }

        /// <summary>
        /// Gets the handle of the window.
        /// </summary>
        /// <value>Type: <see cref="IntPtr"/><br />The handle of the window.</value>
        public IntPtr Handle { get; }
    }
}
