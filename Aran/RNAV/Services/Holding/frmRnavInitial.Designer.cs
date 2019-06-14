using System.Windows.Forms;
namespace Holding
{
	partial class frmRnavInitial
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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
            this.significantPointBox1 = new ChoosePointNS.SignificantPointBox();
            this.sheetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.grpFlightCondition = new System.Windows.Forms.GroupBox();
            this.lblPBN = new System.Windows.Forms.Label();
            this.lblFlightReciever = new System.Windows.Forms.Label();
            this.cmbPBN = new System.Windows.Forms.ComboBox();
            this.cmbFlightReciever = new System.Windows.Forms.ComboBox();
            this.lblFlightPhase = new System.Windows.Forms.Label();
            this.cmbFlightPhases = new System.Windows.Forms.ComboBox();
            this.dMEDataBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblDistanceUnit = new System.Windows.Forms.Label();
            this.lblDistance = new System.Windows.Forms.Label();
            this.txtPtDistance = new System.Windows.Forms.TextBox();
            this.pointPicker1 = new ChoosePointNS.PointPicker();
            this.btnNext = new System.Windows.Forms.Button();
            this.modelPointChoiseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.organisationListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.significantPointBox2 = new ChoosePointNS.SignificantPointBox();
            this.adhpListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.sheetBindingSource)).BeginInit();
            this.grpFlightCondition.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dMEDataBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelPointChoiseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.organisationListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.adhpListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // significantPointBox1
            // 
            this.significantPointBox1.AirportHeliportList = null;
            this.significantPointBox1.BackColor = System.Drawing.Color.White;
            this.significantPointBox1.Location = new System.Drawing.Point(9, 19);
            this.significantPointBox1.Name = "significantPointBox1";
            this.significantPointBox1.OrganisationAuthorityList = null;
            this.significantPointBox1.ShowAirportHeliport = true;
            this.significantPointBox1.ShowFrame = true;
            this.significantPointBox1.ShowOrganisationAuthority = true;
            this.significantPointBox1.SignificantPointChoiceList = ((ChoosePointNS.SignificantPointChoiceList)((ChoosePointNS.SignificantPointChoiceList.DesignatedPoint | ChoosePointNS.SignificantPointChoiceList.Point)));
            this.significantPointBox1.SignificantPointList = null;
            this.significantPointBox1.Size = new System.Drawing.Size(228, 162);
            this.significantPointBox1.TabIndex = 15;
            this.significantPointBox1.PointChanged += new System.EventHandler(this.significantPointBox1_PointChanged);
            // 
            // grpFlightCondition
            // 
            this.grpFlightCondition.Controls.Add(this.lblPBN);
            this.grpFlightCondition.Controls.Add(this.lblFlightReciever);
            this.grpFlightCondition.Controls.Add(this.cmbPBN);
            this.grpFlightCondition.Controls.Add(this.cmbFlightReciever);
            this.grpFlightCondition.Controls.Add(this.lblFlightPhase);
            this.grpFlightCondition.Controls.Add(this.cmbFlightPhases);
            this.grpFlightCondition.Location = new System.Drawing.Point(12, 2);
            this.grpFlightCondition.Name = "grpFlightCondition";
            this.grpFlightCondition.Size = new System.Drawing.Size(564, 53);
            this.grpFlightCondition.TabIndex = 20;
            this.grpFlightCondition.TabStop = false;
            this.grpFlightCondition.Text = "PBN";
            // 
            // lblPBN
            // 
            this.lblPBN.AutoSize = true;
            this.lblPBN.Location = new System.Drawing.Point(379, 19);
            this.lblPBN.Name = "lblPBN";
            this.lblPBN.Size = new System.Drawing.Size(32, 13);
            this.lblPBN.TabIndex = 5;
            this.lblPBN.Text = "PBN:";
            // 
            // lblFlightReciever
            // 
            this.lblFlightReciever.AutoSize = true;
            this.lblFlightReciever.Location = new System.Drawing.Point(188, 19);
            this.lblFlightReciever.Name = "lblFlightReciever";
            this.lblFlightReciever.Size = new System.Drawing.Size(83, 13);
            this.lblFlightReciever.TabIndex = 4;
            this.lblFlightReciever.Text = "FlightRecievers:";
            // 
            // cmbPBN
            // 
            this.cmbPBN.FormattingEnabled = true;
            this.cmbPBN.Location = new System.Drawing.Point(427, 16);
            this.cmbPBN.Name = "cmbPBN";
            this.cmbPBN.Size = new System.Drawing.Size(100, 21);
            this.cmbPBN.TabIndex = 3;
            
            // 
            // cmbFlightReciever
            // 
            this.cmbFlightReciever.FormattingEnabled = true;
            this.cmbFlightReciever.Location = new System.Drawing.Point(288, 16);
            this.cmbFlightReciever.Name = "cmbFlightReciever";
            this.cmbFlightReciever.Size = new System.Drawing.Size(74, 21);
            this.cmbFlightReciever.TabIndex = 2;
            
            // 
            // lblFlightPhase
            // 
            this.lblFlightPhase.AutoSize = true;
            this.lblFlightPhase.Location = new System.Drawing.Point(13, 19);
            this.lblFlightPhase.Name = "lblFlightPhase";
            this.lblFlightPhase.Size = new System.Drawing.Size(70, 13);
            this.lblFlightPhase.TabIndex = 1;
            this.lblFlightPhase.Text = "FlightPhases:";
            // 
            // cmbFlightPhases
            // 
            this.cmbFlightPhases.FormattingEnabled = true;
            this.cmbFlightPhases.Location = new System.Drawing.Point(89, 16);
            this.cmbFlightPhases.Name = "cmbFlightPhases";
            this.cmbFlightPhases.Size = new System.Drawing.Size(93, 21);
            this.cmbFlightPhases.TabIndex = 0;
            
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblDistanceUnit);
            this.groupBox1.Controls.Add(this.lblDistance);
            this.groupBox1.Controls.Add(this.txtPtDistance);
            this.groupBox1.Controls.Add(this.significantPointBox1);
            this.groupBox1.Controls.Add(this.pointPicker1);
            this.groupBox1.Location = new System.Drawing.Point(6, 84);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(570, 187);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Choose Point";
            // 
            // lblDistanceUnit
            // 
            this.lblDistanceUnit.AutoSize = true;
            this.lblDistanceUnit.Location = new System.Drawing.Point(179, 149);
            this.lblDistanceUnit.Name = "lblDistanceUnit";
            this.lblDistanceUnit.Size = new System.Drawing.Size(21, 13);
            this.lblDistanceUnit.TabIndex = 27;
            this.lblDistanceUnit.Text = "km";
            // 
            // lblDistance
            // 
            this.lblDistance.AutoSize = true;
            this.lblDistance.Location = new System.Drawing.Point(19, 149);
            this.lblDistance.Name = "lblDistance";
            this.lblDistance.Size = new System.Drawing.Size(49, 13);
            this.lblDistance.TabIndex = 26;
            this.lblDistance.Text = "Distance";
            // 
            // txtPtDistance
            // 
            this.txtPtDistance.Location = new System.Drawing.Point(97, 146);
            this.txtPtDistance.Name = "txtPtDistance";
            this.txtPtDistance.Size = new System.Drawing.Size(79, 20);
            this.txtPtDistance.TabIndex = 25;
            // 
            // pointPicker1
            // 
            this.pointPicker1.DDAccuracy = 4;
            this.pointPicker1.DMSAccuracy = 2;
            this.pointPicker1.IsDD = true;
            this.pointPicker1.Latitude = 0D;
            this.pointPicker1.Location = new System.Drawing.Point(239, 19);
            this.pointPicker1.Longitude = 0D;
            this.pointPicker1.Name = "pointPicker1";
            this.pointPicker1.Size = new System.Drawing.Size(330, 162);
            this.pointPicker1.TabIndex = 6;
            this.pointPicker1.ByClickChanged += new System.EventHandler(this.pointPicker1_ByClickChanged);
            
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(503, 297);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 26;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // modelPointChoiseBindingSource
            // 
            this.modelPointChoiseBindingSource.DataSource = typeof(Holding.ModelPointChoise);
            // 
            // organisationListBindingSource
            // 
            this.organisationListBindingSource.DataMember = "OrganisationList";
            this.organisationListBindingSource.DataSource = this.modelPointChoiseBindingSource;
            // 
            // significantPointBox2
            // 
            this.significantPointBox2.AirportHeliportList = null;
            this.significantPointBox2.Location = new System.Drawing.Point(107, 339);
            this.significantPointBox2.Name = "significantPointBox2";
            this.significantPointBox2.OrganisationAuthorityList = null;
            this.significantPointBox2.ShowAirportHeliport = true;
            this.significantPointBox2.ShowFrame = true;
            this.significantPointBox2.ShowOrganisationAuthority = true;
            this.significantPointBox2.SignificantPointChoiceList = ChoosePointNS.SignificantPointChoiceList.None;
            this.significantPointBox2.SignificantPointList = null;
            this.significantPointBox2.Size = new System.Drawing.Size(228, 143);
            this.significantPointBox2.TabIndex = 27;
            // 
            // adhpListBindingSource
            // 
            this.adhpListBindingSource.DataMember = "AdhpList";
            this.adhpListBindingSource.DataSource = this.modelPointChoiseBindingSource;
            // 
            // frmRnavInitial
            // 
            this.AllowDrop = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(590, 558);
            this.Controls.Add(this.significantPointBox2);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpFlightCondition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "frmRnavInitial";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = "frmRnavInitial";
            this.Load += new System.EventHandler(this.frmRnavInitial_Load);
            ((System.ComponentModel.ISupportInitialize)(this.sheetBindingSource)).EndInit();
            this.grpFlightCondition.ResumeLayout(false);
            this.grpFlightCondition.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dMEDataBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.modelPointChoiseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.organisationListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.adhpListBindingSource)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.BindingSource sheetBindingSource;
		private System.Windows.Forms.BindingSource dMEDataBindingSource;
        private ChoosePointNS.SignificantPointBox significantPointBox1;
		private GroupBox grpFlightCondition;
		private Label lblPBN;
		private Label lblFlightReciever;
		private ComboBox cmbPBN;
		private ComboBox cmbFlightReciever;
		private Label lblFlightPhase;
        private ComboBox cmbFlightPhases;
		private GroupBox groupBox1;
		private Button btnNext;
		private Label lblDistanceUnit;
		private Label lblDistance;
        private TextBox txtPtDistance;
        private ChoosePointNS.PointPicker pointPicker1;
        private BindingSource modelPointChoiseBindingSource;
        private BindingSource organisationListBindingSource;
        private ChoosePointNS.SignificantPointBox significantPointBox2;
        private BindingSource adhpListBindingSource;


	}
}