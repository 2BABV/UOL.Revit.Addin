using Autodesk.Revit.DB;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using UOL.Models;
using UOL.Revit.SampleAddin.Properties;
using UOL.Revit.SampleAddin.Utilities;
using UOL.SDK;
using UOL.SDK.Authorization;

namespace UOL.Revit.SampleAddin.Forms
{
    internal partial class RequestValidationForm : System.Windows.Forms.Form
    {
        private readonly ParameterHelper parameterHelper = new ParameterHelper();

        public RequestValidationForm()
        {
            InitializeComponent();
            SetLabels();
        }

        internal CADValidationRequest CADValidationRequest { get; set; } = new CADValidationRequest();

        internal IList<Element> FamilyInstances { get; set; } = new List<Element>();

        private void SetLabels()
        {
            Icon = Resources.Logo_UOB;
            Text = Resources.ApplicationTitle;
            labelTitle.Text = Resources.RequestValidation_Title;
            labelSelectionOptions.Text = Resources.RequestValidation_SelectionOptions;
            radioButtonProject.Text = Resources.RequestValidation_Project;
            radioButtonActiveView.Text = Resources.RequestValidation_ActiveView;
            radioButtonSelection.Text = Resources.RequestValidation_Selection;
            labelProperties.Text = Resources.RequestValidation_Properties;
            labelTicketNumber.Text = Resources.RequestValidation_TicketNumber;
            labelRequestDate.Text = Resources.RequestValidation_RequestDate;
            labelNumberOfInstances.Text = Resources.RequestValidation_NumberOfInstances;
            labelEmailAddress.Text = Resources.RequestValidation_EmailAddress;
            labelDescription.Text = Resources.RequestValidation_Description;
            labelResultInCAD.Text = Resources.RequestValidation_ResultInCAD;
            radioButtonResultDifferences.Text = Resources.RequestValidation_ResultDifferences;
            radioButtonResultFull.Text = Resources.RequestValidation_ResultFull;
            buttonSelection.Text = Resources.RequestValidation_ButtonSelection;
            buttonStart.Text = Resources.Buttons_OK;
            buttonCancel.Text = Resources.Buttons_Cancel;

            labelTicketNumberValue.Text = DateTime.Now.Ticks.ToString();
            labelRequestDateValue.Text = DateTime.Now.ToString("dd-MM-yyyy");
            labelNumberOfInstancesValue.Text = "0";

            var selection = GetSelectedFamilyInstances();

            if (selection.Count > 0)
            {
                radioButtonSelection.Checked = true;
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (buttonStart.Text == Resources.Buttons_Close)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                foreach (System.Windows.Forms.Control control in panelMiddle.Controls)
                {
                    control.Visible = false;
                }

                progressBarRequest.Visible = true;
                labelProgress.Text = Resources.RequestValidation_Progress;
                labelProgress.Visible = true;
                buttonStart.Enabled = false;
                Application.DoEvents();
                FillValidationRequest();
                progressBarRequest.Visible = false;
                buttonStart.Enabled = true;
                Application.DoEvents();
                labelProgress.Text = Resources.RequestValidation_ProgressFinish;
                buttonStart.Text = Resources.Buttons_Close;
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TextBoxEmailAddress_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (textBoxEmailAddress.Text.Length > 0)
            {
                if (!regex.IsMatch(textBoxEmailAddress.Text))
                {
                    MessageBox.Show(Resources.MessageBoxEmailExpected, Resources.CaptionEmailExpected, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    textBoxEmailAddress.SelectAll();

                    e.Cancel = true;
                }
            }
        }

        private bool CheckRequired()
        {
            return labelNumberOfInstancesValue.Text != "0" && !string.IsNullOrEmpty(textBoxEmailAddress.Text) && !string.IsNullOrEmpty(textBoxDescription.Text);
        }

        private void FillValidationRequest()
        {
            var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
            uolClient.Initialize();

            var projectId = parameterHelper.GetParameterValue(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document.ProjectInformation, Autodesk.Revit.DB.BuiltInParameter.PROJECT_NUMBER);
            var projectName = parameterHelper.GetParameterValue(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document.ProjectInformation, Autodesk.Revit.DB.BuiltInParameter.PROJECT_NAME);
            var authorize = (IAuthorizationService)ApplicationGlobals.ApplicationContext.GetService(typeof(IAuthorizationService));
            var token = authorize.GetAccessToken();

            CADValidationRequest.AccessToken = token;
            CADValidationRequest.ProjectId = projectId.ToString();
            CADValidationRequest.ProjectName = projectName.ToString();
            CADValidationRequest.TicketNumber = labelTicketNumberValue.Text;
            CADValidationRequest.Description = textBoxDescription.Text;
            CADValidationRequest.LanguageCode = "nl";
            CADValidationRequest.RequestDate = DateTime.Parse(labelRequestDateValue.Text);
            CADValidationRequest.RequestedBy = textBoxEmailAddress.Text;
            CADValidationRequest.Status = ValidationStatus.Requested;
            CADValidationRequest.ValidationResult = 0;
            CADValidationRequest.ResultInCAD = radioButtonResultDifferences.Checked ? ValidationResultType.OnlyDifferences : ValidationResultType.Full;
            CADValidationRequest.CADProducts = new ElementConverter().ConvertToCADProducts(FamilyInstances, progressBarRequest);
        }

        private void ButtonSelection_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void RadioButtonProject_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonProject.Checked)
            {
                FamilyInstances = GetFamilyInstancesInProject();
                labelNumberOfInstancesValue.Text = FamilyInstances.Count.ToString();
            }

            buttonStart.Enabled = CheckRequired();
        }

        private void RadioButtonActiveView_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonActiveView.Checked)
            {
                FamilyInstances = GetFamilyInstancesInView();
                labelNumberOfInstancesValue.Text = FamilyInstances.Count.ToString();
            }

            buttonStart.Enabled = CheckRequired();
        }

        private void RadioButtonSelection_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSelection.Checked)
            {
                FamilyInstances = GetSelectedFamilyInstances();
                labelNumberOfInstancesValue.Text = FamilyInstances.Count.ToString();
            }

            buttonStart.Enabled = CheckRequired();
        }

        private IList<Element> GetFamilyInstancesInProject()
        {
            return new FilteredElementCollector(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document).WhereElementIsNotElementType().WherePasses(new ElementClassFilter(typeof(FamilyInstance))).ToElements().Where(e => ((FamilyInstance)e).Symbol.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null || e.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null).ToList();
        }

        private IList<Element> GetFamilyInstancesInView()
        {
            return new FilteredElementCollector(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document, ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.ActiveView.Id).WhereElementIsNotElementType().WherePasses(new ElementClassFilter(typeof(FamilyInstance))).ToElements().Where(e => ((FamilyInstance)e).Symbol.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null || e.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null).ToList();
        }

        private IList<Element> GetSelectedFamilyInstances()
        {
            var familyInstances = new List<Element>();
            var elementIds = ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Selection.GetElementIds();
            foreach (var elementId in elementIds)
            {
                var element = ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document.GetElement(elementId);

                if (element != null && (element as FamilyInstance) != null)
                {
                    familyInstances.Add(element);
                }
            }

            return familyInstances.Where(e => ((FamilyInstance)e).Symbol.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null || e.GetOrderedParameters().FirstOrDefault(p => p.Definition != null && p.Definition.Name.StartsWith("MC_")) != null).ToList();
        }

        private void RequestValidationForm_Activated(object sender, EventArgs e)
        {
            labelNumberOfInstancesValue.Text = FamilyInstances.Count.ToString();
        }

        private void TextBoxEmailAddress_TextChanged(object sender, EventArgs e)
        {
            buttonStart.Enabled = CheckRequired();
        }

        private void TextBoxDescription_TextChanged(object sender, EventArgs e)
        {
            buttonStart.Enabled = CheckRequired();
        }
    }
}
