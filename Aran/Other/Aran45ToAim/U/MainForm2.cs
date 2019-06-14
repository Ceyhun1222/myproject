using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using AIXM45_AIM_UTIL;
using Aran.Aim.Enums;
using Aran.Aim.DataTypes;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using System.Threading;
using AF = Aran.Aim.Features;

namespace Aran45ToAixm
{
    public partial class MainForm2 : Form
    {
        private Converter _converter;
        private IFeatureWorkspace _featWS;
		private string _mdbFileName;

        public MainForm2 ()
        {
            InitializeComponent ();

            _converter = new Converter ();
        }

        private void OpenMdbFile_Click (object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog ();
            ofd.Filter = "MS Access Files (*.mdb)|*.mdb";
            ofd.FileName = ui_mdbFileTB.Text;

            if (ofd.ShowDialog () != DialogResult.OK)
                return;
			
			_mdbFileName = ofd.FileName;

            try
            {
                IWorkspaceFactory wsFact = new AccessWorkspaceFactory ();
                IWorkspace ws = wsFact.OpenFromFile (ofd.FileName, 0);
                _featWS = ws as IFeatureWorkspace;

                _converter.Open (_featWS);

                ui_mdbFileTB.Text = ofd.FileName;

                FillSupportedFeatures ();
            }
            catch (Exception ex)
            {
                Global.ShowError (ex);
            }
        }

        private void FillSupportedFeatures ()
        {
            ui_featuresDGV.Rows.Clear ();

            var suppTableList = _converter.GetSupportedTables45 ();

			var ca = new char [] {','};
			IWorkspace2 workSpace = ( IWorkspace2 ) _featWS;
            foreach (var suppTable in suppTableList)
            {
				var sa = suppTable.Split (ca, StringSplitOptions.RemoveEmptyEntries);
				var rowTag = new RowTag ();
				rowTag.Name45 = sa [0].Trim ();
				rowTag.Name51 = (sa.Length > 1 ? sa [1] : sa [0]).Trim ();

                try
                {
					if (!workSpace.get_NameExists (esriDatasetType.esriDTTable, rowTag.Name45) &&
						!workSpace.get_NameExists (esriDatasetType.esriDTFeatureClass, rowTag.Name45))
					{
						continue;
					}

					rowTag.Table = _featWS.OpenTable ( rowTag.Name45 );
					rowTag.RowCount = rowTag.Table.RowCount ( null );

                    var rowIndex = ui_featuresDGV.Rows.Add ();
                    var dgvRow = ui_featuresDGV.Rows [rowIndex];
					dgvRow.Tag = rowTag;
					dgvRow.Cells [0].Value = rowTag.Name51;
					dgvRow.Cells [1].Value = rowTag.Name45;
					dgvRow.Cells [2].Value = rowTag.RowCount;
                }
                catch (Exception ex)
                {
					System.Diagnostics.Debug.WriteLine (ex.Message);
                }
            }
        }

        private void Convert_Click (object sender, EventArgs e)
        {
			//SetWait (true);

			var features = new List<Aran.Aim.Features.Feature> ();
			var dict = new Dictionary<string, List<ConvertedObj>> ();

            foreach (DataGridViewRow row in ui_featuresDGV.Rows)
            {
                var rowTag = row.Tag as RowTag;
                var errorList = new List<string> ();
				
				if (rowTag.Name45 == "Airspace" ||
					rowTag.Name45 == "DesignatedPoint" ||
					rowTag.Name45 == "Obstacle" ||
					rowTag.Name45 == "RouteSegment")
				{
					var convObjList = _converter.Convert (rowTag.Name51, rowTag.Table, errorList);
					if (convObjList != null)
					{
						if (ui_checkCRCChB.Checked)
							AddDiffCRC (dict, rowTag.Name45, CheckCRC (convObjList));

						foreach (var convObj in convObjList)
							features.Add (convObj.Obj);
					}
				}
            }

			var effectiveDate = DateTime.Now;

			var rTmpList = GetConvertedOtherFeaturesR (dict, ref effectiveDate);

			if (dict.Count > 0)
			{
				var df = new U.DifferentCRCForm ();
				if (df.ShowDiffCRC (dict) != DialogResult.OK)
				{
					SetWait (false);
					return;
				}
			}

			features.AddRange (rTmpList);


            //#region ForTest

            //for (int i = 0; i < features.Count; ++i){
            //    var feat = features[i];
            //    if (feat.FeatureType != Aran.Aim.FeatureType.Route && feat.FeatureType != Aran.Aim.FeatureType.RouteSegment) {
            //        features.RemoveAt(i);
            //        i--;
            //    }
            //}

            //#endregion


            var mbRes = MessageBox.Show (this, string.Format (
				"{0} Features converted from ARAN 4.5.\n" +
				"Do you want to insert all Features to the ARAN 5.1?", features.Count),
				"Converter", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			if (mbRes != DialogResult.Yes)
			{
				SetWait (false);
				return;
			}

            //try
            //{
            //    InsertFeatures (features, effectiveDate);

            //    Global.AranEvn.RefreshAllAimLayers ();

            //    SetWait (false);

            //    MessageBox.Show ("Done!", "Converter");
            //}
            //catch (Exception ex)
            //{
            //    SetWait (false);

            //    MessageBox.Show (this, "Error:\n" + ex.Message, 
            //        "Converter", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}


            ui_progressBar.Maximum = features.Count;
            ui_progressBar.Visible = true;
            new Thread(InsertFeatures).Start(new object[] { features, effectiveDate });
        }

        private void InsertFeatures (object obj)
        {
            var objArr = obj as object[];
            InsertFeatures(objArr[0] as List<Aran.Aim.Features.Feature>, (DateTime)objArr[1]);
        }

		private void InsertFeatures (List<Aran.Aim.Features.Feature> features, DateTime effectiveDate)
		{
			var dbPro = Global.DbProvider;
			var transId = dbPro.BeginTransaction ();

            Action tmpAction = () => {
                ui_progressBar.Value++;
            };

			foreach (var feat in features)
			{
				try
				{
					#region Fill TimeSlice

					feat.TimeSlice = new TimeSlice ();
					feat.TimeSlice.Interpretation = TimeSliceInterpretationType.BASELINE;
					feat.TimeSlice.SequenceNumber = 1;
					feat.TimeSlice.CorrectionNumber = 0;
					feat.TimeSlice.ValidTime = new TimePeriod (effectiveDate);
					feat.TimeSlice.FeatureLifetime = feat.TimeSlice.ValidTime;

					#endregion


					var ir = dbPro.Insert (feat, transId, true);

					if (!ir.IsSucceed)
					{
						dbPro.Rollback (transId);
						throw new Exception (ir.Message);
					}
                    
                    Invoke(tmpAction);

				}
				catch (Exception ex)
				{
					//dbPro.Rollback (transId);
                    
                    tmpAction = () => { ui_progressBar.Visible = false; };
                    Invoke(tmpAction);

                    tmpAction = () => {
                        MessageBox.Show(this, "Error:\n" + ex.Message,
                            "Converter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    };
                    Invoke(tmpAction);
                    
                    return;
				}
			}

			dbPro.Commit (transId);

            tmpAction = () => {
                ui_progressBar.Visible = false;
                Global.AranEvn.RefreshAllAimLayers();
                MessageBox.Show ("Done!", "Converter");
            };
            Invoke(tmpAction);
		}

		private List<Aran.Aim.Features.Feature> GetConvertedOtherFeaturesR(Dictionary<string, List<ConvertedObj>> dict, ref DateTime effectiveDate)
		{
			var result = new List<Aran.Aim.Features.Feature> ();

			var orgReader = Aixm45AimUtil.CreateReader (_mdbFileName, "Organisation");
			var ADHP_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "AD_HP");
			var RWY_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "RWY");
			var RDN_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "RwyDirection");
			var RCP_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "RwyClinePoint");
			var rwyDirDeclDistReader = Aixm45AimUtil.CreateReader (_mdbFileName, "RunwayDirectionDeclaredDistance");

			var NDB_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "NDB");
			var DME_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "DME");
			var VOR_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "VOR");
			var TACAN_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "TACAN");

			var ILS_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "ILS");
			var GlidePath_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "IGP");
			var Localizer_rdr = Aixm45AimUtil.CreateReader (_mdbFileName, "ILZ");

			if (ui_checkCRCChB.Checked)
			{
				AddDiffCRC (dict, "AD_HP", CheckCRC (ADHP_rdr.ListOfObjects));
				AddDiffCRC (dict, "RWY", CheckCRC (RWY_rdr.ListOfObjects));
				AddDiffCRC (dict, "RwyDirection", CheckCRC (RDN_rdr.ListOfObjects));
				AddDiffCRC (dict, "RunwayCentrelinePoint", CheckCRC (RCP_rdr.ListOfObjects));
				AddDiffCRC (dict, "NDB", CheckCRC (NDB_rdr.ListOfObjects));
				AddDiffCRC (dict, "DME", CheckCRC (DME_rdr.ListOfObjects));
				AddDiffCRC (dict, "VOR", CheckCRC (VOR_rdr.ListOfObjects));
				AddDiffCRC (dict, "TACAN", CheckCRC (TACAN_rdr.ListOfObjects));
				AddDiffCRC (dict, "ILS", CheckCRC (ILS_rdr.ListOfObjects));
				AddDiffCRC (dict, "IGP", CheckCRC (GlidePath_rdr.ListOfObjects));
				AddDiffCRC (dict, "ILZ", CheckCRC (Localizer_rdr.ListOfObjects));
			}

			var orgTag = orgReader.ListOfObjects.Where (org => org.Tag is DateTime).FirstOrDefault ();
			if (orgTag != null)
				effectiveDate = (DateTime) orgTag.Tag;

            var removedLocNavList = new List<Localizer>();
            var removedDmeNavList = new List<DME>();
            var removedVorNavList = new List<VOR>();

			#region Relation ADHP -> Organisation

			foreach (var org in orgReader.ListOfObjects)
			{
				var ahList = ADHP_rdr.ListOfObjects.Where (co => co.RelatedMid == org.mid).Select (co => co.Obj).ToList ();
				foreach (AirportHeliport ah in ahList)
				{
					ah.ResponsibleOrganisation = new AirportHeliportResponsibilityOrganisation ();
					ah.ResponsibleOrganisation.TheOrganisationAuthority = new FeatureRef (org.Obj.Identifier);
				}

				result.Add (org.Obj);
			}


			#endregion

			#region relation ADHP - RWY

			foreach (ConvertedObj adhp in ADHP_rdr.ListOfObjects)
			{
				List<ConvertedObj> lst = (from obj in RWY_rdr.ListOfObjects where obj.RelatedMid == adhp.mid select obj).ToList ();

				foreach (ConvertedObj rwy in lst)
				{
					((Runway) rwy.Obj).AssociatedAirportHeliport = new FeatureRef (((AirportHeliport) adhp.Obj).Identifier);
				}

				result.Add ((AirportHeliport) adhp.Obj);
			}

			#endregion

			#region relation RWY - RwyDirection
			
			{
				var rwyDirList = new List<Aran.Aim.Features.Feature> ();
				//var rwyCPList = new List<Aran.Aim.Features.Feature> ();

				foreach (ConvertedObj rwy in RWY_rdr.ListOfObjects)
				{
					List<ConvertedObj> convRwyDirList = (from obj in RDN_rdr.ListOfObjects where obj.RelatedMid == rwy.mid select obj).ToList ();

					foreach (ConvertedObj rdn in convRwyDirList)
					{
						var rwyDir = rdn.Obj as RunwayDirection;
						rwyDir.UsedRunway = new FeatureRef (rwy.Obj.Identifier);
						rwyDirList.Add (rwyDir);
					}

					#region Create Fake RunwayCentrelinePoints.

					//if (convRwyDirList.Count > 1)
					//{
					//    var rwyCP = new RunwayCentrelinePoint ();
					//    rwyCP.Identifier = Guid.NewGuid ();
					//    rwyCP.Role = CodeRunwayPointRole.START;
					//    rwyCP.Location = convRwyDirList [0].Tag as ElevatedPoint;
					//    rwyCP.OnRunway = new FeatureRef (convRwyDirList [0].Obj.Identifier);
					//    rwyCPList.Add (rwyCP);

					//    rwyCP = new RunwayCentrelinePoint ();
					//    rwyCP.Identifier = Guid.NewGuid ();
					//    rwyCP.Role = CodeRunwayPointRole.THR;
					//    rwyCP.Location = convRwyDirList [0].Tag as ElevatedPoint;
					//    rwyCP.OnRunway = new FeatureRef (convRwyDirList [0].Obj.Identifier);
					//    rwyCPList.Add (rwyCP);

					//    rwyCP = new RunwayCentrelinePoint ();
					//    rwyCP.Identifier = Guid.NewGuid ();
					//    rwyCP.Role = CodeRunwayPointRole.END;
					//    rwyCP.Location = convRwyDirList [1].Tag as ElevatedPoint;
					//    rwyCP.OnRunway = new FeatureRef (convRwyDirList [0].Obj.Identifier);
					//    rwyCPList.Add (rwyCP);
					//}

					#endregion

					result.Add ((Runway) rwy.Obj);

				}

				result.AddRange (rwyDirList);
				//result.AddRange (rwyCPList);
			}

			#endregion

			#region Relation RunwayDirection - RwyClinePoint

			foreach (ConvertedObj rwy in RWY_rdr.ListOfObjects)
			{
				var rwyDirList = (from obj in RDN_rdr.ListOfObjects 
								  where obj.RelatedMid == rwy.mid 
								  select obj).ToList ();

				if (rwyDirList.Count == 2)
				{
					var rcpList = (from obj in RCP_rdr.ListOfObjects 
								   where obj.RelatedMid == rwy.mid 
								   select obj.Obj as RunwayCentrelinePoint).ToList ();

					var newRCPList1 = SetRunwayCentrelinePointsRef (rwyDirList [0], rwyDirList [1], rcpList);
					result.AddRange (newRCPList1);

					var newRCPList2 = SetRunwayCentrelinePointsRef (rwyDirList[1], rwyDirList[0], rcpList);
					result.AddRange (newRCPList2);

					foreach (var item in newRCPList1)
					{
						try
						{
							if (item.Role == CodeRunwayPointRole.START)
							{
								SetDeclaredDistance (item, rwyDirList, rwyDirDeclDistReader.ListOfObjects);

								var rcpItem = newRCPList2.Where (rcp =>
									(rcp.Location.Geo.X == item.Location.Geo.X && 
									rcp.Location.Geo.Y == item.Location.Geo.Y)).FirstOrDefault ();

								if (rcpItem != null)
								{
									rcpItem.Role = CodeRunwayPointRole.END;
									break;
								}
							}
						}
						catch { }
					}

					foreach (var item in newRCPList2)
					{
						try
						{
							if (item.Role == CodeRunwayPointRole.START)
							{
								SetDeclaredDistance (item, rwyDirList, rwyDirDeclDistReader.ListOfObjects);

								var rcpItem = newRCPList1.Where (rcp =>
									(rcp.Location.Geo.X == item.Location.Geo.X && 
									rcp.Location.Geo.Y == item.Location.Geo.Y)).FirstOrDefault ();

								if (rcpItem != null)
								{
									rcpItem.Role = CodeRunwayPointRole.END;
									break;
								}
							}
						}
						catch { }
					}
				}
			}

			#endregion


            #region VOR, DME, NDB, TACAN, Localizer and Glidepath Equipments

            result.AddRange(VOR_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));
            result.AddRange(DME_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));
            result.AddRange(NDB_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));
            result.AddRange(TACAN_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));
            result.AddRange(Localizer_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));
            result.AddRange(GlidePath_rdr.ListOfObjects.Select(item => item.Obj as AF.Feature));

            #endregion


            #region Navaid ILS

            foreach (ConvertedObj ils in ILS_rdr.ListOfObjects) {
                var ilsNavaid = ils.Obj as Navaid;
                ilsNavaid.Type = CodeNavaidService.ILS;
                ilsNavaid.Identifier = Guid.NewGuid();

                var lst = (from obj in Localizer_rdr.ListOfObjects where obj.RelatedMid == ils.mid select obj).ToList();

                foreach (ConvertedObj localizer in lst) {
                    var locF = localizer.Obj as Localizer;

                    var navaidComponent = new NavaidComponent();
                    navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.Localizer;
                    navaidComponent.TheNavaidEquipment.Identifier = locF.Identifier;
                    ilsNavaid.NavaidEquipment.Add(navaidComponent);

                    if (ilsNavaid.Designator == null) {
                        ilsNavaid.Designator = locF.Designator;
                        ilsNavaid.Location = locF.Location.Clone() as ElevatedPoint;
                    }

                    removedLocNavList.Add(locF);
                }

                lst = (from obj in GlidePath_rdr.ListOfObjects where obj.RelatedMid == ils.mid select obj).ToList();

                foreach (ConvertedObj glidepath in lst) {
                    var glidepathF = glidepath.Obj as Glidepath;

                    var navaidComponent = new NavaidComponent();
                    navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.Glidepath;
                    navaidComponent.TheNavaidEquipment.Identifier = glidepathF.Identifier;
                    ilsNavaid.NavaidEquipment.Add(navaidComponent);

                    if (ilsNavaid.Designator == null) {
                        ilsNavaid.Designator = glidepathF.Designator;
                        ilsNavaid.Location = glidepathF.Location.Clone() as ElevatedPoint;
                    }
                }

                if (ilsNavaid.NavaidEquipment.Count > 0) {

                    lst = (from obj in DME_rdr.ListOfObjects where obj.mid == ilsNavaid.Name select obj).ToList();
                    if ((lst != null) && (lst.Count > 0)) {
                        ilsNavaid.Type = CodeNavaidService.ILS_DME;
                        foreach (ConvertedObj dme in lst) {
                            var dmeF = dme.Obj as DME;

                            var navaidComponent = new NavaidComponent();
                            navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                            navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.DME;
                            navaidComponent.TheNavaidEquipment.Identifier = dmeF.Identifier;
                            ilsNavaid.NavaidEquipment.Add(navaidComponent);

                            if (ilsNavaid.Designator == null) {
                                ilsNavaid.Designator = dmeF.Designator;
                                ilsNavaid.Location = dmeF.Location.Clone() as ElevatedPoint;
                            }

                            removedDmeNavList.Add(dmeF);
                        }
                    }

                    ilsNavaid.Name = ilsNavaid.Designator;

                    lst = (from obj in RDN_rdr.ListOfObjects where obj.mid == ils.RelatedMid select obj).ToList();
                    foreach (ConvertedObj rdn in lst) {
                        var fea = new FeatureRefObject();
                        fea.Feature = new FeatureRef(((RunwayDirection)rdn.Obj).Identifier);
                        ilsNavaid.RunwayDirection.Add(fea);
                    }

                    result.Add(ilsNavaid);
                }
            }

            #endregion

            #region Localizer Navaid with single equipment

            foreach (ConvertedObj locObj in Localizer_rdr.ListOfObjects) {
                var loc = locObj.Obj as Localizer;

                if (!removedLocNavList.Contains(loc)) {

                    var navaid = new Navaid();
                    navaid.Type = CodeNavaidService.LOC;
                    navaid.Identifier = Guid.NewGuid();
                    navaid.Designator = loc.Designator;

                    if (loc.Location != null)
                        navaid.Location = loc.Location.Clone() as ElevatedPoint;

                    var navaidComponent = new NavaidComponent();
                    navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.Localizer;
                    navaidComponent.TheNavaidEquipment.Identifier = loc.Identifier;
                    navaid.NavaidEquipment.Add(navaidComponent);

                    result.Add(navaid);
                }
            }
            

            #endregion


            #region Navaid VOR/DME

            foreach (ConvertedObj dme in DME_rdr.ListOfObjects) {

                var lst = (from obj in VOR_rdr.ListOfObjects where obj.mid == dme.RelatedMid select obj).ToList();

                if (lst != null && lst.Count > 0) {

                    var dmeF = dme.Obj as DME;
                    var vorF = lst[0].Obj as VOR;

                    var navaid = new Navaid();
                    navaid.Type = CodeNavaidService.VOR_DME;
                    navaid.Identifier = Guid.NewGuid();
                    navaid.Designator = dmeF.Designator;
                    navaid.Location = dmeF.Location.Clone() as ElevatedPoint;

                    var dmeNavaidComponent = new NavaidComponent();
                    dmeNavaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    dmeNavaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.DME;
                    dmeNavaidComponent.TheNavaidEquipment.Identifier = dmeF.Identifier;
                    navaid.NavaidEquipment.Add(dmeNavaidComponent);

                    var vorNavaidComponent = new NavaidComponent();
                    vorNavaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    vorNavaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.VOR;
                    vorNavaidComponent.TheNavaidEquipment.Identifier = vorF.Identifier;
                    navaid.NavaidEquipment.Add(vorNavaidComponent);

                    result.Add(navaid);

                    removedDmeNavList.Add(dmeF);
                    removedVorNavList.Add(vorF);
                }
            }

            #endregion

            #region DME Navaid with single equipment

            foreach (ConvertedObj dme in DME_rdr.ListOfObjects) {
                var dmeF = dme.Obj as DME;

                if (!removedDmeNavList.Contains(dmeF)) {
                    var navaid = new Navaid();
                    navaid.Type = CodeNavaidService.DME;
                    navaid.Identifier = Guid.NewGuid();
                    navaid.Designator = dmeF.Designator;
                    navaid.Location = dmeF.Location.Clone() as ElevatedPoint;

                    var navaidComponent = new NavaidComponent();
                    navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.DME;
                    navaidComponent.TheNavaidEquipment.Identifier = dmeF.Identifier;
                    navaid.NavaidEquipment.Add(navaidComponent);
                    
                    result.Add(navaid);
                }

            }
            #endregion

            #region VOR Navaid with single equipment

            foreach (ConvertedObj vor in VOR_rdr.ListOfObjects) {
                var vorF = vor.Obj as NavaidEquipment;
                if (!removedVorNavList.Contains(vorF)) {

                    var navaid = new Navaid();
                    navaid.Type = CodeNavaidService.VOR;
                    navaid.Identifier = Guid.NewGuid();
                    navaid.Designator = vorF.Designator;
                    navaid.Location = vorF.Location.Clone() as ElevatedPoint;

                    var navaidComponent = new NavaidComponent();
                    navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef();
                    navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.VOR;
                    navaidComponent.TheNavaidEquipment.Identifier = vorF.Identifier;
                    navaid.NavaidEquipment.Add(navaidComponent);

                    result.Add(navaid);
                }
            }

            #endregion
            

            #region Navaid NDB

            foreach (ConvertedObj ndb in NDB_rdr.ListOfObjects)
			{
				var navEquip = ndb.Obj as NavaidEquipment;

				var navaid = new Navaid ();
				navaid.Type = CodeNavaidService.NDB;
				navaid.Identifier = Guid.NewGuid ();
				navaid.Designator = navEquip.Designator;
				navaid.Location = navEquip.Location.Clone () as ElevatedPoint;

				var navaidComponent = new NavaidComponent ();
				navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
				navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.NDB;
				navaidComponent.TheNavaidEquipment.Identifier = navEquip.Identifier;
				navaid.NavaidEquipment.Add (navaidComponent);

				result.Add (navaid);
			}

			#endregion

			#region Navaid TACAN

			foreach (ConvertedObj tacan in TACAN_rdr.ListOfObjects)
			{
				var navEquip = tacan.Obj as NavaidEquipment;

				var navaid = new Navaid ();
				navaid.Type = CodeNavaidService.TACAN;
				navaid.Identifier = Guid.NewGuid ();
				navaid.Designator = navEquip.Designator;
				navaid.Location = navEquip.Location.Clone () as ElevatedPoint;

				var navaidComponent = new NavaidComponent ();
				navaidComponent.TheNavaidEquipment = new AbstractNavaidEquipmentRef ();
				navaidComponent.TheNavaidEquipment.Type = Aran.Aim.NavaidEquipmentType.TACAN;
				navaidComponent.TheNavaidEquipment.Identifier = navEquip.Identifier;
				navaid.NavaidEquipment.Add (navaidComponent);

				result.Add (navaid);
			}

			#endregion

			return result;
		}

		private void SetDeclaredDistance(RunwayCentrelinePoint startRCP, List<ConvertedObj> rwyDirList, List<ConvertedObj> rwyDirDeclDistList)
		{
			var rwyDirConvObj = rwyDirList.Where (rdco => rdco.Obj.Identifier == startRCP.OnRunway.Identifier).FirstOrDefault ();
			if (rwyDirConvObj == null)
				return;
			var rwyDirDeclDists = rwyDirDeclDistList.Where (rdddco => rdddco.RelatedMid == rwyDirConvObj.mid);

			foreach (var co in rwyDirDeclDists)
			{
				var rdd = co.Tag as RunwayDeclaredDistance;
				if (rdd != null)
					startRCP.AssociatedDeclaredDistance.Add (rdd);
			}

		}

		private List<RunwayCentrelinePoint> SetRunwayCentrelinePointsRef(ConvertedObj rwyDir1, ConvertedObj rwyDir2, List<RunwayCentrelinePoint> rcpList)
		{
			var ptRD1 = (rwyDir1.Tag as AixmPoint).Geo;
			var ptRD2 = (rwyDir2.Tag as AixmPoint).Geo;

			var dir = Functions.ReturnAngleInDegrees (ptRD1, ptRD2) + 90;
			var list = new List<RunwayCentrelinePoint> ();
			var isTHRsetted = false;

			for (int i = 0; i < rcpList.Count; i++)
			{
				var newRCP = rcpList[i].Clone () as RunwayCentrelinePoint;
				newRCP.Identifier = Guid.NewGuid ();
				newRCP.OnRunway = new FeatureRef (rwyDir1.Obj.Identifier);

				var pt2 = newRCP.Location.Geo;
				var sideDefVal = Functions.SideDef (ptRD1, dir, pt2);
				var d = DistanceCalculator.DistanceCalculator.CalcDistance (ptRD1.X, ptRD1.Y, pt2.X, pt2.Y);

				if (Math.Abs(d) < 1 && !isTHRsetted)
				{
					isTHRsetted = true;
					newRCP.Role = CodeRunwayPointRole.THR;
				}
				else if (sideDefVal < 0)
				{
					newRCP.Role = CodeRunwayPointRole.START;
				}
				else
				{
					newRCP.Role = CodeRunwayPointRole.MID;
				}

				list.Add (newRCP);
			}

			if (!list.Exists (rcp => rcp.Role == CodeRunwayPointRole.START))
			{
				var thrRCP = list.Where (rcp => rcp.Role == CodeRunwayPointRole.THR).FirstOrDefault ();
				if (thrRCP != null)
				{
					var startRCP = thrRCP.Clone () as RunwayCentrelinePoint;
					startRCP.Identifier = Guid.NewGuid ();
					startRCP.Role = CodeRunwayPointRole.START;
					startRCP.OnRunway = new FeatureRef (rwyDir1.Obj.Identifier);
					list.Add (startRCP);
				}
			}

			return list;
		}


		private void AddDiffCRC (Dictionary<string, List<ConvertedObj>> dict, string name, List<ConvertedObj> list)
		{
			if (list.Count > 0)
				dict.Add (name, list);
		}

		private List<ConvertedObj> CheckCRC (List<ConvertedObj> list)
		{
			var removedList = new List<ConvertedObj> ();

			for (int i = 0; i < list.Count; i++)
			{
				var item = list [i];
				
				if (item.CRCInfo == null)
					continue;

				item.CRCInfo.CalcNewCRC ();
				if (!item.CRCInfo.IsCRCOK)
				{
					list.RemoveAt (i);
					i--;
					removedList.Add (item);
				}
			}

			return removedList;
		}

		private void SetWait (bool visible)
		{
			Cursor = (visible ? Cursors.WaitCursor : Cursors.Default);
		}
    }

	public static class Functions
	{
		public const double DegToRadValue = Math.PI / 180.0;
		public const double RadToDegValue = 180.0 / Math.PI;

		public static int SideDef(Aran.Geometries.Point PtInLine, double LineAngle, Aran.Geometries.Point PtOutLine)
		{
			double Angle12 = ReturnAngleInDegrees (PtInLine, PtOutLine);
			double dAngle = Utils.NativeMethods.Modulus (LineAngle - Angle12, 360.0);

			if (dAngle == 0.0 || dAngle == 180.0)
				return 0;

			if (dAngle < 180.0)
				return 1;

			return -1;
		}

		public static double ReturnAngleInDegrees(Aran.Geometries.Point ptFrom, Aran.Geometries.Point ptTo)
		{
			double fdX = ptTo.X - ptFrom.X;
			double fdY = ptTo.Y - ptFrom.Y;
			return Utils.NativeMethods.Modulus (RadToDeg (Utils.NativeMethods.ATan2 (fdY, fdX)), 360.0);
		}

		public static double DegToRad(double val)
		{
			return val * DegToRadValue;
		}

		public static double RadToDeg(double val)
		{
			return val * RadToDegValue;
		}
	}

    public class RowTag
    {
        public ITable Table { get; set; }

        public string Name51 { get; set; }

		public string Name45 { get; set; }

		public int RowCount { get; set; }
    }
}
