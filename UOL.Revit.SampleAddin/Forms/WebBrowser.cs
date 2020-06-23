using System.Windows.Forms;

namespace UOL.Revit.SampleAddin.Forms
{
    internal partial class WebBrowser : Form
    {
        public WebBrowser(string title, string content)
        {
            InitializeComponent();
            Text = title;
            webViewCompatibleUOL.NavigateToString(content);
        }
    }
}
