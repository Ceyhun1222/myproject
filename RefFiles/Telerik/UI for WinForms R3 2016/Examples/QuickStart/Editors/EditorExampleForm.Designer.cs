using Telerik.QuickStart.WinControls;
namespace Telerik.Examples.WinControls.Editors.ComboBox
{
    partial class EditorExampleBaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.radPanelDemoHolder = new Telerik.WinControls.UI.RadPanel();
            this.roudRectShape = new Telerik.WinControls.RoundRectShape(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsPanel
            // 
            this.settingsPanel.Location = new System.Drawing.Point(716, 1);
            this.settingsPanel.Size = new System.Drawing.Size(200, 598);
            // 
            // radPanelDemoHolder
            // 
            this.radPanelDemoHolder.Location = new System.Drawing.Point(404, 285);
            this.radPanelDemoHolder.Name = "radPanelDemoHolder";
            this.radPanelDemoHolder.Size = new System.Drawing.Size(200, 100);
            this.radPanelDemoHolder.TabIndex = 1;
            // 
            // roudRectShape
            // 
            this.roudRectShape.Radius = 10;
            // 
            // EditorExampleBaseForm
            // 
            this.Controls.Add(this.radPanelDemoHolder);
            this.Name = "EditorExampleBaseForm";
            this.Size = new System.Drawing.Size(1170, 671);
            this.Controls.SetChildIndex(this.radPanelDemoHolder, 0);
            this.Controls.SetChildIndex(this.settingsPanel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.settingsPanel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanelDemoHolder)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        protected Telerik.WinControls.RoundRectShape roudRectShape;
        protected Telerik.WinControls.UI.RadPanel radPanelDemoHolder;
    }
}