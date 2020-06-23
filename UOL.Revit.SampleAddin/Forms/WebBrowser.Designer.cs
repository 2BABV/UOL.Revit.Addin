namespace UOL.Revit.SampleAddin.Forms
{
    internal partial class WebBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private Microsoft.Toolkit.Forms.UI.Controls.WebViewCompatible webViewCompatibleUOL;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WebBrowser));
            this.webViewCompatibleUOL = new Microsoft.Toolkit.Forms.UI.Controls.WebViewCompatible();
            this.SuspendLayout();
            // 
            // webViewCompatibleUOL
            // 
            this.webViewCompatibleUOL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webViewCompatibleUOL.Location = new System.Drawing.Point(0, 0);
            this.webViewCompatibleUOL.Name = "webViewCompatibleUOL";
            this.webViewCompatibleUOL.Size = new System.Drawing.Size(1098, 577);
            this.webViewCompatibleUOL.TabIndex = 0;
            // 
            // WebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1098, 577);
            this.Controls.Add(this.webViewCompatibleUOL);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WebBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);

        }

        #endregion
    }
}