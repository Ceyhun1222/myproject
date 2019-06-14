namespace ChoosePointNS
{
    partial class SignificantPointChoiceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose ( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose ();
            }
            base.Dispose ( disposing );
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent ( )
        {
			this.ChckBxNavaid = new System.Windows.Forms.CheckBox();
			this.ChckBxRunwayCntrlinePnt = new System.Windows.Forms.CheckBox();
			this.ChckBxAirportHeliport = new System.Windows.Forms.CheckBox();
			this.ChckBxTouchDownLiftOff = new System.Windows.Forms.CheckBox();
			this.ChckBxDesignatedPoint = new System.Windows.Forms.CheckBox();
			this.ChckBxPoint = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// ChckBxNavaid
			// 
			this.ChckBxNavaid.AutoSize = true;
			this.ChckBxNavaid.Location = new System.Drawing.Point(7, 5);
			this.ChckBxNavaid.Name = "ChckBxNavaid";
			this.ChckBxNavaid.Size = new System.Drawing.Size(60, 17);
			this.ChckBxNavaid.TabIndex = 0;
			this.ChckBxNavaid.Text = "Navaid";
			this.ChckBxNavaid.UseVisualStyleBackColor = true;
			this.ChckBxNavaid.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// ChckBxRunwayCntrlinePnt
			// 
			this.ChckBxRunwayCntrlinePnt.AutoSize = true;
			this.ChckBxRunwayCntrlinePnt.Location = new System.Drawing.Point(7, 25);
			this.ChckBxRunwayCntrlinePnt.Name = "ChckBxRunwayCntrlinePnt";
			this.ChckBxRunwayCntrlinePnt.Size = new System.Drawing.Size(136, 17);
			this.ChckBxRunwayCntrlinePnt.TabIndex = 1;
			this.ChckBxRunwayCntrlinePnt.Text = "RunwayCentrelinePoint";
			this.ChckBxRunwayCntrlinePnt.UseVisualStyleBackColor = true;
			this.ChckBxRunwayCntrlinePnt.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// ChckBxAirportHeliport
			// 
			this.ChckBxAirportHeliport.AutoSize = true;
			this.ChckBxAirportHeliport.Location = new System.Drawing.Point(7, 45);
			this.ChckBxAirportHeliport.Name = "ChckBxAirportHeliport";
			this.ChckBxAirportHeliport.Size = new System.Drawing.Size(92, 17);
			this.ChckBxAirportHeliport.TabIndex = 2;
			this.ChckBxAirportHeliport.Text = "AirportHeliport";
			this.ChckBxAirportHeliport.UseVisualStyleBackColor = true;
			this.ChckBxAirportHeliport.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// ChckBxTouchDownLiftOff
			// 
			this.ChckBxTouchDownLiftOff.AutoSize = true;
			this.ChckBxTouchDownLiftOff.Location = new System.Drawing.Point(7, 65);
			this.ChckBxTouchDownLiftOff.Name = "ChckBxTouchDownLiftOff";
			this.ChckBxTouchDownLiftOff.Size = new System.Drawing.Size(113, 17);
			this.ChckBxTouchDownLiftOff.TabIndex = 3;
			this.ChckBxTouchDownLiftOff.Text = "TouchDownLiftOff";
			this.ChckBxTouchDownLiftOff.UseVisualStyleBackColor = true;
			this.ChckBxTouchDownLiftOff.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// ChckBxDesignatedPoint
			// 
			this.ChckBxDesignatedPoint.AutoSize = true;
			this.ChckBxDesignatedPoint.Location = new System.Drawing.Point(7, 85);
			this.ChckBxDesignatedPoint.Name = "ChckBxDesignatedPoint";
			this.ChckBxDesignatedPoint.Size = new System.Drawing.Size(104, 17);
			this.ChckBxDesignatedPoint.TabIndex = 4;
			this.ChckBxDesignatedPoint.Text = "DesignatedPoint";
			this.ChckBxDesignatedPoint.UseVisualStyleBackColor = true;
			this.ChckBxDesignatedPoint.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// ChckBxPoint
			// 
			this.ChckBxPoint.AutoSize = true;
			this.ChckBxPoint.Location = new System.Drawing.Point(7, 105);
			this.ChckBxPoint.Name = "ChckBxPoint";
			this.ChckBxPoint.Size = new System.Drawing.Size(50, 17);
			this.ChckBxPoint.TabIndex = 5;
			this.ChckBxPoint.Text = "Point";
			this.ChckBxPoint.UseVisualStyleBackColor = true;
			this.ChckBxPoint.CheckedChanged += new System.EventHandler(this.ChoiceCheckBoxesCheckedChanged);
			// 
			// SignificantPointChoiceControl
			// 
			this.Controls.Add(this.ChckBxPoint);
			this.Controls.Add(this.ChckBxDesignatedPoint);
			this.Controls.Add(this.ChckBxTouchDownLiftOff);
			this.Controls.Add(this.ChckBxAirportHeliport);
			this.Controls.Add(this.ChckBxRunwayCntrlinePnt);
			this.Controls.Add(this.ChckBxNavaid);
			this.Name = "SignificantPointChoiceControl";
			this.Size = new System.Drawing.Size(150, 127);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ChckBxNavaid;
        private System.Windows.Forms.CheckBox ChckBxRunwayCntrlinePnt;
        private System.Windows.Forms.CheckBox ChckBxAirportHeliport;
        private System.Windows.Forms.CheckBox ChckBxTouchDownLiftOff;
        private System.Windows.Forms.CheckBox ChckBxDesignatedPoint;
        private System.Windows.Forms.CheckBox ChckBxPoint;

    }
}
