namespace UOL.Revit.SampleAddin.Forms
{
    internal partial class RequestValidationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolTip toolTipForm;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelSelectionOptions;
        private System.Windows.Forms.GroupBox groupBoxSelectionOptions;
        private System.Windows.Forms.Button buttonSelection;
        private System.Windows.Forms.RadioButton radioButtonSelection;
        private System.Windows.Forms.RadioButton radioButtonActiveView;
        private System.Windows.Forms.RadioButton radioButtonProject;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label labelEmailAddress;
        private System.Windows.Forms.Label labelNumberOfInstances;
        private System.Windows.Forms.Label labelRequestDate;
        private System.Windows.Forms.Label labelTicketNumber;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.GroupBox groupBoxResultInCAD;
        private System.Windows.Forms.RadioButton radioButtonResultDifferences;
        private System.Windows.Forms.RadioButton radioButtonResultFull;
        private System.Windows.Forms.Label labelResultInCAD;
        private System.Windows.Forms.Label labelNumberOfInstancesValue;
        private System.Windows.Forms.Label labelRequestDateValue;
        private System.Windows.Forms.Label labelTicketNumberValue;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.TextBox textBoxEmailAddress;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.ProgressBar progressBarRequest;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestValidationForm));
            this.toolTipForm = new System.Windows.Forms.ToolTip(this.components);
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.progressBarRequest = new System.Windows.Forms.ProgressBar();
            this.groupBoxResultInCAD = new System.Windows.Forms.GroupBox();
            this.radioButtonResultDifferences = new System.Windows.Forms.RadioButton();
            this.radioButtonResultFull = new System.Windows.Forms.RadioButton();
            this.labelResultInCAD = new System.Windows.Forms.Label();
            this.labelNumberOfInstancesValue = new System.Windows.Forms.Label();
            this.labelRequestDateValue = new System.Windows.Forms.Label();
            this.labelTicketNumberValue = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.textBoxEmailAddress = new System.Windows.Forms.TextBox();
            this.labelDescription = new System.Windows.Forms.Label();
            this.labelEmailAddress = new System.Windows.Forms.Label();
            this.labelNumberOfInstances = new System.Windows.Forms.Label();
            this.labelRequestDate = new System.Windows.Forms.Label();
            this.labelTicketNumber = new System.Windows.Forms.Label();
            this.labelProperties = new System.Windows.Forms.Label();
            this.labelSelectionOptions = new System.Windows.Forms.Label();
            this.groupBoxSelectionOptions = new System.Windows.Forms.GroupBox();
            this.buttonSelection = new System.Windows.Forms.Button();
            this.radioButtonSelection = new System.Windows.Forms.RadioButton();
            this.radioButtonActiveView = new System.Windows.Forms.RadioButton();
            this.radioButtonProject = new System.Windows.Forms.RadioButton();
            this.labelProgress = new System.Windows.Forms.Label();
            this.panelBottom.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelMiddle.SuspendLayout();
            this.groupBoxResultInCAD.SuspendLayout();
            this.groupBoxSelectionOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.AutoSize = true;
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(218)))), ((int)(((byte)(218)))));
            this.panelBottom.Controls.Add(this.panelButtons);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 510);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(15);
            this.panelBottom.MinimumSize = new System.Drawing.Size(353, 30);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(15);
            this.panelBottom.Size = new System.Drawing.Size(677, 63);
            this.panelBottom.TabIndex = 46;
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSize = true;
            this.panelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelButtons.Controls.Add(this.buttonCancel);
            this.panelButtons.Controls.Add(this.buttonStart);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelButtons.Location = new System.Drawing.Point(15, 15);
            this.panelButtons.Margin = new System.Windows.Forms.Padding(15);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(647, 33);
            this.panelButtons.TabIndex = 10;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCancel.Font = new System.Drawing.Font("Segoe UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCancel.Location = new System.Drawing.Point(548, 1);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(98, 31);
            this.buttonCancel.TabIndex = 11;
            this.buttonCancel.Text = "ANNULEREN";
            this.buttonCancel.UseVisualStyleBackColor = false;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonStart.AutoSize = true;
            this.buttonStart.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.buttonStart.CausesValidation = false;
            this.buttonStart.Enabled = false;
            this.buttonStart.FlatAppearance.BorderSize = 0;
            this.buttonStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonStart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.buttonStart.ForeColor = System.Drawing.Color.White;
            this.buttonStart.Location = new System.Drawing.Point(427, 1);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(0);
            this.buttonStart.MinimumSize = new System.Drawing.Size(100, 30);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(100, 30);
            this.buttonStart.TabIndex = 10;
            this.buttonStart.Text = "START";
            this.buttonStart.UseVisualStyleBackColor = false;
            this.buttonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(64)))), ((int)(((byte)(112)))));
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(5);
            this.panelTop.Size = new System.Drawing.Size(677, 46);
            this.panelTop.TabIndex = 47;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(12)))), ((int)(((byte)(64)))), ((int)(((byte)(112)))));
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(11, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Padding = new System.Windows.Forms.Padding(10);
            this.labelTitle.Size = new System.Drawing.Size(248, 41);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "TheModus UOL about screen";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelMiddle
            // 
            this.panelMiddle.AutoSize = true;
            this.panelMiddle.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelMiddle.BackColor = System.Drawing.Color.White;
            this.panelMiddle.Controls.Add(this.progressBarRequest);
            this.panelMiddle.Controls.Add(this.groupBoxResultInCAD);
            this.panelMiddle.Controls.Add(this.labelResultInCAD);
            this.panelMiddle.Controls.Add(this.labelNumberOfInstancesValue);
            this.panelMiddle.Controls.Add(this.labelRequestDateValue);
            this.panelMiddle.Controls.Add(this.labelTicketNumberValue);
            this.panelMiddle.Controls.Add(this.textBoxDescription);
            this.panelMiddle.Controls.Add(this.textBoxEmailAddress);
            this.panelMiddle.Controls.Add(this.labelDescription);
            this.panelMiddle.Controls.Add(this.labelEmailAddress);
            this.panelMiddle.Controls.Add(this.labelNumberOfInstances);
            this.panelMiddle.Controls.Add(this.labelRequestDate);
            this.panelMiddle.Controls.Add(this.labelTicketNumber);
            this.panelMiddle.Controls.Add(this.labelProperties);
            this.panelMiddle.Controls.Add(this.labelSelectionOptions);
            this.panelMiddle.Controls.Add(this.groupBoxSelectionOptions);
            this.panelMiddle.Controls.Add(this.labelProgress);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 0);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Padding = new System.Windows.Forms.Padding(10);
            this.panelMiddle.Size = new System.Drawing.Size(677, 573);
            this.panelMiddle.TabIndex = 48;
            // 
            // progressBarRequest
            // 
            this.progressBarRequest.Cursor = System.Windows.Forms.Cursors.Default;
            this.progressBarRequest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.progressBarRequest.Location = new System.Drawing.Point(28, 135);
            this.progressBarRequest.Name = "progressBarRequest";
            this.progressBarRequest.Size = new System.Drawing.Size(633, 19);
            this.progressBarRequest.Step = 1;
            this.progressBarRequest.TabIndex = 29;
            this.progressBarRequest.Visible = false;
            // 
            // groupBoxResultInCAD
            // 
            this.groupBoxResultInCAD.Controls.Add(this.radioButtonResultDifferences);
            this.groupBoxResultInCAD.Controls.Add(this.radioButtonResultFull);
            this.groupBoxResultInCAD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxResultInCAD.Location = new System.Drawing.Point(25, 411);
            this.groupBoxResultInCAD.Name = "groupBoxResultInCAD";
            this.groupBoxResultInCAD.Size = new System.Drawing.Size(225, 66);
            this.groupBoxResultInCAD.TabIndex = 4;
            this.groupBoxResultInCAD.TabStop = false;
            // 
            // radioButtonResultDifferences
            // 
            this.radioButtonResultDifferences.AutoSize = true;
            this.radioButtonResultDifferences.Checked = true;
            this.radioButtonResultDifferences.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonResultDifferences.Location = new System.Drawing.Point(3, 12);
            this.radioButtonResultDifferences.Name = "radioButtonResultDifferences";
            this.radioButtonResultDifferences.Size = new System.Drawing.Size(88, 21);
            this.radioButtonResultDifferences.TabIndex = 17;
            this.radioButtonResultDifferences.TabStop = true;
            this.radioButtonResultDifferences.Text = "Verschillen";
            this.radioButtonResultDifferences.UseVisualStyleBackColor = true;
            // 
            // radioButtonResultFull
            // 
            this.radioButtonResultFull.AutoSize = true;
            this.radioButtonResultFull.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonResultFull.Location = new System.Drawing.Point(3, 39);
            this.radioButtonResultFull.Name = "radioButtonResultFull";
            this.radioButtonResultFull.Size = new System.Drawing.Size(74, 21);
            this.radioButtonResultFull.TabIndex = 16;
            this.radioButtonResultFull.Text = "Volledig";
            this.radioButtonResultFull.UseVisualStyleBackColor = true;
            // 
            // labelResultInCAD
            // 
            this.labelResultInCAD.AutoSize = true;
            this.labelResultInCAD.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelResultInCAD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelResultInCAD.Location = new System.Drawing.Point(11, 389);
            this.labelResultInCAD.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelResultInCAD.Name = "labelResultInCAD";
            this.labelResultInCAD.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelResultInCAD.Size = new System.Drawing.Size(158, 19);
            this.labelResultInCAD.TabIndex = 26;
            this.labelResultInCAD.Text = "Rapportage in CAD";
            // 
            // labelNumberOfInstancesValue
            // 
            this.labelNumberOfInstancesValue.AutoSize = true;
            this.labelNumberOfInstancesValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInstancesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelNumberOfInstancesValue.Location = new System.Drawing.Point(234, 282);
            this.labelNumberOfInstancesValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNumberOfInstancesValue.Name = "labelNumberOfInstancesValue";
            this.labelNumberOfInstancesValue.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumberOfInstancesValue.Size = new System.Drawing.Size(33, 17);
            this.labelNumberOfInstancesValue.TabIndex = 25;
            this.labelNumberOfInstancesValue.Text = "-";
            // 
            // labelRequestDateValue
            // 
            this.labelRequestDateValue.AutoSize = true;
            this.labelRequestDateValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRequestDateValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelRequestDateValue.Location = new System.Drawing.Point(234, 255);
            this.labelRequestDateValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRequestDateValue.Name = "labelRequestDateValue";
            this.labelRequestDateValue.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelRequestDateValue.Size = new System.Drawing.Size(94, 17);
            this.labelRequestDateValue.TabIndex = 24;
            this.labelRequestDateValue.Text = "01-12-2019";
            // 
            // labelTicketNumberValue
            // 
            this.labelTicketNumberValue.AutoSize = true;
            this.labelTicketNumberValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTicketNumberValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelTicketNumberValue.Location = new System.Drawing.Point(234, 228);
            this.labelTicketNumberValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTicketNumberValue.Name = "labelTicketNumberValue";
            this.labelTicketNumberValue.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelTicketNumberValue.Size = new System.Drawing.Size(84, 17);
            this.labelTicketNumberValue.TabIndex = 23;
            this.labelTicketNumberValue.Text = "12535264";
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDescription.Font = new System.Drawing.Font("Segoe UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.textBoxDescription.Location = new System.Drawing.Point(242, 336);
            this.textBoxDescription.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.Size = new System.Drawing.Size(419, 25);
            this.textBoxDescription.TabIndex = 3;
            this.textBoxDescription.TextChanged += new System.EventHandler(this.TextBoxDescription_TextChanged);
            // 
            // textBoxEmailAddress
            // 
            this.textBoxEmailAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEmailAddress.Font = new System.Drawing.Font("Segoe UI", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEmailAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.textBoxEmailAddress.Location = new System.Drawing.Point(242, 307);
            this.textBoxEmailAddress.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxEmailAddress.Name = "textBoxEmailAddress";
            this.textBoxEmailAddress.Size = new System.Drawing.Size(419, 25);
            this.textBoxEmailAddress.TabIndex = 2;
            this.textBoxEmailAddress.TextChanged += new System.EventHandler(this.TextBoxEmailAddress_TextChanged);
            this.textBoxEmailAddress.Validating += new System.ComponentModel.CancelEventHandler(this.TextBoxEmailAddress_Validating);
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelDescription.Location = new System.Drawing.Point(12, 338);
            this.labelDescription.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelDescription.Size = new System.Drawing.Size(103, 17);
            this.labelDescription.TabIndex = 20;
            this.labelDescription.Text = "Omschrijving";
            // 
            // labelEmailAddress
            // 
            this.labelEmailAddress.AutoSize = true;
            this.labelEmailAddress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEmailAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelEmailAddress.Location = new System.Drawing.Point(13, 309);
            this.labelEmailAddress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEmailAddress.Name = "labelEmailAddress";
            this.labelEmailAddress.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelEmailAddress.Size = new System.Drawing.Size(92, 17);
            this.labelEmailAddress.TabIndex = 19;
            this.labelEmailAddress.Text = "Emailadres";
            // 
            // labelNumberOfInstances
            // 
            this.labelNumberOfInstances.AutoSize = true;
            this.labelNumberOfInstances.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNumberOfInstances.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelNumberOfInstances.Location = new System.Drawing.Point(13, 282);
            this.labelNumberOfInstances.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelNumberOfInstances.Name = "labelNumberOfInstances";
            this.labelNumberOfInstances.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelNumberOfInstances.Size = new System.Drawing.Size(122, 17);
            this.labelNumberOfInstances.TabIndex = 18;
            this.labelNumberOfInstances.Text = "Aantal instanties";
            // 
            // labelRequestDate
            // 
            this.labelRequestDate.AutoSize = true;
            this.labelRequestDate.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRequestDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelRequestDate.Location = new System.Drawing.Point(13, 255);
            this.labelRequestDate.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRequestDate.Name = "labelRequestDate";
            this.labelRequestDate.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelRequestDate.Size = new System.Drawing.Size(66, 17);
            this.labelRequestDate.TabIndex = 17;
            this.labelRequestDate.Text = "Datum";
            // 
            // labelTicketNumber
            // 
            this.labelTicketNumber.AutoSize = true;
            this.labelTicketNumber.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTicketNumber.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelTicketNumber.Location = new System.Drawing.Point(12, 228);
            this.labelTicketNumber.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTicketNumber.Name = "labelTicketNumber";
            this.labelTicketNumber.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelTicketNumber.Size = new System.Drawing.Size(109, 17);
            this.labelTicketNumber.TabIndex = 16;
            this.labelTicketNumber.Text = "Ticketnummer";
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelProperties.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelProperties.Location = new System.Drawing.Point(12, 197);
            this.labelProperties.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelProperties.Size = new System.Drawing.Size(128, 19);
            this.labelProperties.TabIndex = 15;
            this.labelProperties.Text = "Eigenschappen";
            // 
            // labelSelectionOptions
            // 
            this.labelSelectionOptions.AutoSize = true;
            this.labelSelectionOptions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.labelSelectionOptions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelSelectionOptions.Location = new System.Drawing.Point(12, 61);
            this.labelSelectionOptions.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelSelectionOptions.Name = "labelSelectionOptions";
            this.labelSelectionOptions.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelSelectionOptions.Size = new System.Drawing.Size(126, 19);
            this.labelSelectionOptions.TabIndex = 13;
            this.labelSelectionOptions.Text = "Selectie opties";
            // 
            // groupBoxSelectionOptions
            // 
            this.groupBoxSelectionOptions.Controls.Add(this.buttonSelection);
            this.groupBoxSelectionOptions.Controls.Add(this.radioButtonSelection);
            this.groupBoxSelectionOptions.Controls.Add(this.radioButtonActiveView);
            this.groupBoxSelectionOptions.Controls.Add(this.radioButtonProject);
            this.groupBoxSelectionOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxSelectionOptions.Location = new System.Drawing.Point(25, 80);
            this.groupBoxSelectionOptions.Name = "groupBoxSelectionOptions";
            this.groupBoxSelectionOptions.Size = new System.Drawing.Size(225, 93);
            this.groupBoxSelectionOptions.TabIndex = 1;
            this.groupBoxSelectionOptions.TabStop = false;
            // 
            // buttonSelection
            // 
            this.buttonSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSelection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(96)))), ((int)(((byte)(0)))));
            this.buttonSelection.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSelection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSelection.ForeColor = System.Drawing.Color.White;
            this.buttonSelection.Location = new System.Drawing.Point(122, 63);
            this.buttonSelection.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelection.Name = "buttonSelection";
            this.buttonSelection.Size = new System.Drawing.Size(98, 23);
            this.buttonSelection.TabIndex = 19;
            this.buttonSelection.Text = "SELECTEREN";
            this.buttonSelection.UseVisualStyleBackColor = false;
            this.buttonSelection.Click += new System.EventHandler(this.ButtonSelection_Click);
            // 
            // radioButtonSelection
            // 
            this.radioButtonSelection.AutoSize = true;
            this.radioButtonSelection.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonSelection.Location = new System.Drawing.Point(3, 63);
            this.radioButtonSelection.Name = "radioButtonSelection";
            this.radioButtonSelection.Size = new System.Drawing.Size(70, 21);
            this.radioButtonSelection.TabIndex = 18;
            this.radioButtonSelection.Text = "Selectie";
            this.radioButtonSelection.UseVisualStyleBackColor = true;
            this.radioButtonSelection.CheckedChanged += new System.EventHandler(this.RadioButtonSelection_CheckedChanged);
            // 
            // radioButtonActiveView
            // 
            this.radioButtonActiveView.AutoSize = true;
            this.radioButtonActiveView.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonActiveView.Location = new System.Drawing.Point(3, 36);
            this.radioButtonActiveView.Name = "radioButtonActiveView";
            this.radioButtonActiveView.Size = new System.Drawing.Size(96, 21);
            this.radioButtonActiveView.TabIndex = 17;
            this.radioButtonActiveView.Text = "Actieve view";
            this.radioButtonActiveView.UseVisualStyleBackColor = true;
            this.radioButtonActiveView.CheckedChanged += new System.EventHandler(this.RadioButtonActiveView_CheckedChanged);
            // 
            // radioButtonProject
            // 
            this.radioButtonProject.AutoSize = true;
            this.radioButtonProject.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButtonProject.Location = new System.Drawing.Point(3, 9);
            this.radioButtonProject.Name = "radioButtonProject";
            this.radioButtonProject.Size = new System.Drawing.Size(66, 21);
            this.radioButtonProject.TabIndex = 16;
            this.radioButtonProject.Text = "Project";
            this.radioButtonProject.UseVisualStyleBackColor = true;
            this.radioButtonProject.CheckedChanged += new System.EventHandler(this.RadioButtonProject_CheckedChanged);
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProgress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(99)))), ((int)(((byte)(99)))));
            this.labelProgress.Location = new System.Drawing.Point(25, 157);
            this.labelProgress.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.labelProgress.Size = new System.Drawing.Size(636, 29);
            this.labelProgress.TabIndex = 28;
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelProgress.Visible = false;
            // 
            // RequestValidationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(677, 573);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelMiddle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RequestValidationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Activated += new System.EventHandler(this.RequestValidationForm_Activated);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMiddle.ResumeLayout(false);
            this.panelMiddle.PerformLayout();
            this.groupBoxResultInCAD.ResumeLayout(false);
            this.groupBoxResultInCAD.PerformLayout();
            this.groupBoxSelectionOptions.ResumeLayout(false);
            this.groupBoxSelectionOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}