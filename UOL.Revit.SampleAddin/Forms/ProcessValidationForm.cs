using Microsoft.Extensions.DependencyInjection;
using System;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Windows.Forms;
using UOL.Revit.SampleAddin.Properties;
using UOL.Revit.SampleAddin.Utilities;
using UOL.SDK;
using UOL.SDK.Authorization;

namespace UOL.Revit.SampleAddin.Forms
{
    internal partial class ProcessValidationForm : System.Windows.Forms.Form
    {
        private readonly IUOLClient uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
        private readonly ParameterHelper parameterHelper = new ParameterHelper();
        private string projectNumber = string.Empty;

        public ProcessValidationForm()
        {
            InitializeComponent();
            SetLabels();
        }

        private void SetLabels()
        {
            Icon = Resources.Logo_UOB;
            Text = Resources.ApplicationTitle;
            labelTitle.Text = Resources.ProcessValidation_Title;
            labelValidations.Text = Resources.ProcessValidation_Validations;
            buttonClose.Text = Resources.Buttons_Close;
            buttonProgressOK.Text = Resources.Buttons_OK;
            buttonCancel.Text = Resources.Buttons_Cancel;
            toolTipForm.SetToolTip(buttonRefresh, Resources.ProcessValidation_RefreshList);

            SetTableHeaders();
            FillValidations();
        }

        private void SetTableHeaders()
        {
            tableLayoutPanelValidations.RowStyles.Clear();
            tableLayoutPanelValidations.RowStyles.Add(new RowStyle
            {
                Height = 30,
                SizeType = SizeType.Absolute
            });

            tableLayoutPanelValidations.Controls.Clear();
            tableLayoutPanelValidations.RowCount = 1;
            tableLayoutPanelValidations.Controls.Add(CreateControl<Label>($"labelTicketNumber", Resources.ProcessValidation_TicketNumber, false), 0, 0);
            tableLayoutPanelValidations.Controls.Add(CreateControl<Label>($"labelRequestDate", Resources.ProcessValidation_RequestDate, false), 1, 0);
            tableLayoutPanelValidations.Controls.Add(CreateControl<Label>($"labelStatus", Resources.ProcessValidation_RequestStatus, false), 2, 0);
            tableLayoutPanelValidations.Controls.Add(CreateControl<Label>($"labelValidationResult", Resources.ProcessValidation_ValidationResult, false), 3, 0);
        }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ButtonSelection_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private Control CreateControl<T>(string name, string value, bool showReadonly) where T : Control, new()
        {
            if (typeof(T).Equals(typeof(CheckBox)))
            {
                return new CheckBox
                {
                    Name = name,
                    Checked = bool.Parse(value),
                    Anchor = AnchorStyles.Top,
                    Size = new System.Drawing.Size(25, 25),
                    Font = new System.Drawing.Font("Segoe UI", 10),
                    Enabled = !showReadonly
                };
            }
            else
            {
                return new T
                {
                    Name = name,
                    Text = value,
                    ForeColor = Color.FromArgb(100, 99, 99),
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 10, typeof(T).Equals(typeof(Label)) ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
                    Enabled = !showReadonly
                };
            }
        }

        private void FillValidations()
        {
            var uolClient = ApplicationGlobals.ApplicationContext.GetService<IUOLClient>();
            uolClient.Initialize();
            var authorize = (IAuthorizationService)ApplicationGlobals.ApplicationContext.GetService(typeof(IAuthorizationService));
            authorize.GetAccessToken();

            var projectId = parameterHelper.GetParameterValue(ApplicationGlobals.ExternalCommandData.Application.ActiveUIDocument.Document.ProjectInformation, Autodesk.Revit.DB.BuiltInParameter.PROJECT_NUMBER);

            if (projectId == null || string.IsNullOrEmpty(projectId.ToString()))
            {
                MessageBox.Show(Resources.MessageBoxProcessValidationNoProjectId_Text, Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            projectNumber = projectId.ToString();

            try
            {
                var assembly = GetType().Assembly;
                var resourceManager = new ResourceManager(typeof(Resources));

                var validations = UOLAddInUtilities.UIThreadSafeAsync(
                    () => uolClient.GetValidationRequestsAsync(projectId.ToString())).Result;

                foreach (var validation in validations)
                {
                    tableLayoutPanelValidations.RowCount += 1;
                    tableLayoutPanelValidations.RowStyles.Add(new RowStyle
                    {
                        Height = 35,
                        SizeType = SizeType.Absolute
                    });

                    tableLayoutPanelValidations.Controls.Add(CreateLabelControl("labelTicketNumber", validation.TicketNumber, Color.FromArgb(255, 96, 0)), 0, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateLabelControl("labelRequestDate", validation.RequestDate.ToString("dd-MM-yyyy"), Color.FromArgb(100, 99, 99)), 1, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateLabelControl("labelRequestStatus", resourceManager.GetString($"ValidationStatus_{(int)validation.Status}"), Color.FromArgb(100, 99, 99)), 2, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateLabelControl("labelValidationResult", validation.ValidationResult.ToString("0.0"), Color.FromArgb(100, 99, 99)), 3, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateButtonControl("buttonRestart", "icon1"), 4, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateButtonControl("buttonReport", "icon2"), 5, tableLayoutPanelValidations.RowCount - 1);
                    tableLayoutPanelValidations.Controls.Add(CreateButtonControl("buttonDelete", "icon4"), 6, tableLayoutPanelValidations.RowCount - 1);
                    toolTipForm.SetToolTip(tableLayoutPanelValidations.Controls[$"buttonRestart_{tableLayoutPanelValidations.RowCount - 1}"], Resources.ProcessValidation_TooltipRestart);
                    toolTipForm.SetToolTip(tableLayoutPanelValidations.Controls[$"buttonReport_{tableLayoutPanelValidations.RowCount - 1}"], Resources.ProcessValidation_TooltipReport);
                    toolTipForm.SetToolTip(tableLayoutPanelValidations.Controls[$"buttonDelete_{tableLayoutPanelValidations.RowCount - 1}"], Resources.ProcessValidation_TooltipDelete);
                }
            }
            catch (UOLClientException)
            {
                MessageBox.Show(Resources.MessageBoxGetValidationsError_Text, Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Control CreateLabelControl(string name, string value, Color color)
        {
            return new Label
            {
                Name = $"{name}_{tableLayoutPanelValidations.RowCount - 1}",
                Text = value,
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 5, 0, 0),
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                ForeColor = color
            };
        }

        private Control CreateButtonControl(string name, string image)
        {
            var resourceManager = Resources.ResourceManager;

            var button = new Button
            {
                Name = $"{name}_{tableLayoutPanelValidations.RowCount - 1}",
                Image = (Bitmap)resourceManager.GetObject(image),
                Anchor = AnchorStyles.Top,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
            };
            
            button.FlatAppearance.BorderSize = 0;
            button.Click += Button_Click;

            return button;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            var button = sender as Button;

            var buttonName = button.Name.Split('_');

            var ticketNumber = GetTicketNumber(buttonName[1]);

            switch (buttonName[0])
            {
                case "buttonRestart":
                    RestartRequest(ticketNumber);
                    break;
                case "buttonReport":
                    ReportRequest(ticketNumber);
                    break;
                case "buttonDelete":
                    DeleteRequest(ticketNumber);
                    break;
            }
        }

        private void RestartRequest(string ticketNumber)
        {
            uolClient.StartValidationAsync(projectNumber, $"{ticketNumber}");

            MessageBox.Show(string.Format(Resources.MessageBoxValidationRestart_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            SetTableHeaders();
            FillValidations();
        }

        private void ReportRequest(string ticketNumber)
        {
            var requests = UOLAddInUtilities.UIThreadSafeAsync(
                () => uolClient.GetValidationRequestsAsync(projectNumber, ticketNumber)).Result;

            if (requests != null && requests.FirstOrDefault() != null && !string.IsNullOrEmpty(requests.FirstOrDefault().UserFriendlyResultBlobId))
            {
                var content = UOLAddInUtilities.UIThreadSafeAsync(
                    () => uolClient.GetValidationBlobAsStringAsync(requests.FirstOrDefault().UserFriendlyResultBlobId)).Result;

                if (!string.IsNullOrEmpty(content))
                {
                    var webBrowser = new WebBrowser($"Ticket {ticketNumber}", content);
                    webBrowser.ShowDialog();
                }
                else
                {
                    MessageBox.Show(string.Format(Resources.MessageBoxValidationReportNotAvailable_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show(string.Format(Resources.MessageBoxValidationReportNotAvailable_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void DeleteRequest(string ticketNumber)
        {
            var reallyDelete = MessageBox.Show(string.Format(Resources.MessageBoxValidationDelete_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (reallyDelete == DialogResult.Yes)
            {
                var content = UOLAddInUtilities.UIThreadSafeAsync(
                    () => uolClient.DeleteValidationRequestAsync(projectNumber, ticketNumber)).Result;

                MessageBox.Show(string.Format(Resources.MessageBoxValidationDeleted_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetTableHeaders();
                FillValidations();
            }
            else
            {
                MessageBox.Show(string.Format(Resources.MessageBoxValidationNotDeleted_Text, ticketNumber), Resources.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetTicketNumber(string rowNumber)
        {
            return tableLayoutPanelValidations.Controls[$"labelTicketNumber_{rowNumber}"].Text;
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            SetTableHeaders();
            FillValidations();
        }

        private void ButtonProgressOK_Click(object sender, EventArgs e)
        {
            buttonRefresh.Visible = true;
            tableLayoutPanelValidations.Visible = true;
            progressBarProcess.Visible = false;
            labelProgress.Visible = false;
            buttonProgressOK.Visible = false;
        }
    }
}
