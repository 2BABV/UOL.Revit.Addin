using Autodesk.Revit.UI;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace UOL.Revit.SampleAddin
{
    internal class RibbonHelper
    {
        public void CreateRibbon(UIControlledApplication uiControlledApplication)
        {
            // Creates the Ribbon with two panels and buttons on it.
            var tabName = Properties.Resources.RibbonName;
            uiControlledApplication.CreateRibbonTab(tabName);

            var searchPanel = uiControlledApplication.CreateRibbonPanel(tabName, Properties.Resources.RibbonSearchPanel);
            var validationPanel = uiControlledApplication.CreateRibbonPanel(tabName, Properties.Resources.RibbonPanel_Validate);

            searchPanel.AddItem(CreatePushButton("BtnSearch", Properties.Resources.RibbonButtonSearchDescription, "UOL.Revit.SampleAddin.Commands.SearchUOLExternalCommand", UOLAddInUtilities.ImageToByte(Properties.Resources.Add), UOLAddInUtilities.ImageToByte(Properties.Resources.Add_64x64)));
            searchPanel.AddItem(CreatePushButton("BtnEditInstances", Properties.Resources.RibbonButtonEditInstancesDescription, "UOL.Revit.SampleAddin.Commands.EditInstancesExternalCommand", UOLAddInUtilities.ImageToByte(Properties.Resources.Edit), UOLAddInUtilities.ImageToByte(Properties.Resources.Edit_64x64)));
            validationPanel.AddItem(CreatePushButton("btnRequestValidation", Properties.Resources.RibbonButton_ValidateSubRequestValidationDescription, "UOL.Revit.SampleAddin.Commands.AddValidationRequestExternalCommand", UOLAddInUtilities.ImageToByte(Properties.Resources.iconV2), UOLAddInUtilities.ImageToByte(Properties.Resources.iconV2)));
            validationPanel.AddItem(CreatePushButton("btnProcessValidation", Properties.Resources.RibbonButton_ValidateSubProcessValidationDescription, "UOL.Revit.SampleAddin.Commands.ProcessValidationRequestExternalCommand", UOLAddInUtilities.ImageToByte(Properties.Resources.iconV3), UOLAddInUtilities.ImageToByte(Properties.Resources.iconV3)));
        }

        public PushButtonData CreatePushButton(string name, string description, string commandName, byte[] iconSmall, byte[] iconLarge)
        {
            return new PushButtonData(name, description, Assembly.GetExecutingAssembly().Location, commandName)
            {
                Image = ByteToImage(iconSmall),
                LargeImage = ByteToImage(iconLarge)
            };
        }

        private ImageSource ByteToImage(byte[] imageData)
        {
            var biImg = new BitmapImage();
            var ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            var imgSrc = biImg as ImageSource;

            return imgSrc;
        }
    }
}
