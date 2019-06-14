using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetTopologySuite.Geometries;
using PDM;
using CDOTMA.Drawings;

namespace CDOTMA
{
	public partial class WithCommonPoint : Form
	{
		const double minIntersectAngle = 15.0;
		const double maxIntersectAngle = 135.0;

		int _selectedSigPtID;
		int _selectedPathID;
		int _crossingPathID;
		int _conflictAreaID;

		LegPoint _currPt;
		Drawings.PointElement _selectedSigPtElem;
		Drawings.LineElement _selectedPathElem;
		Drawings.LineElement _crossingPathElem;
		Drawings.FillElement _conflictAreaElem;

		public WithCommonPoint()
		{
			InitializeComponent();

			_selectedSigPtElem = new Drawings.PointElement();
			_selectedSigPtElem.Color = 255;										// Functions.RGB(255, 128, 126);
			_selectedSigPtElem.Size = 12;
			_selectedSigPtElem.Style = 3;

			_selectedPathElem = new Drawings.LineElement();
			_selectedPathElem.Color = 16776960;
			_selectedPathElem.Width = 2;
			_selectedPathElem.Style = 3;

			_crossingPathElem = new Drawings.LineElement();
			_crossingPathElem.Color = Functions.RGB(255, 255, 0);				//65535
			_crossingPathElem.Width = 2;
			_crossingPathElem.Style = 4;

			_conflictAreaElem = new Drawings.FillElement();
			_conflictAreaElem.Color = Functions.RGB(255, 128, 128);				//8421631
			_conflictAreaElem.Style = FillElementStyle.Percent20;

			//listView1.Columns[2].Text = listView1.Columns[2].Text + " (" + GlobalVars.unitConverter.HeightUnit + ")";
			//listView1.Columns[3].Text = listView1.Columns[3].Text + " (" + GlobalVars.unitConverter.HeightUnit + ")";

			listView1.Columns[2].Text = listView1.Columns[2].Text + " (FL)";
			listView1.Columns[3].Text = listView1.Columns[3].Text + " (FL)";

			label6.Text = GlobalVars.unitConverter.DistanceUnit;
		}

		private void WithCommonPoint_FormClosed(object sender, FormClosedEventArgs e)
		{
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedSigPtID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedPathID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_crossingPathID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictAreaID);
		}

		public DialogResult ShowModal(IWin32Window owner)
		{
			return this.ShowDialog(owner);
		}

		public void ShowForm(IWin32Window owner)
		{
			Cursor.Current = Cursors.WaitCursor;

			_selectedPathID = 0;
			_crossingPathID = 0;
			_conflictAreaID = 0;
			_currPt = null;
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();

			List<ProcedureType> procedures = new List<ProcedureType>();

			foreach (LegPoint lp in GlobalVars.LegPoints)
			{
				int n = lp.legs.Count;
				procedures.Clear();
				lp.selctedLegs = new List<TraceLeg>();

				if (n > 1)
				{
					for (int i = 0; i < n; i++)
					{
						TraceLeg lg0 = lp.legs[i], lg1 = null;
						lp.selctedLegs.Add(lg0);

						if (i < n - 1)
							lg1 = lp.legs[i + 1];

						if (lg1 != null && lg0.Owner.Name == lg1.Owner.Name)
							continue;

						procedures.Add(lg0.Owner);

						//TreeNode tNode = treeView1.Nodes.Add(lp.CallSign);
						//foreach (TraceLeg lg in lp.legs)
						//	tNode.Nodes.Add(lg.Owner.Name);
					}

					if (procedures.Count > 1)
					{
						TreeNode tNode = treeView1.Nodes.Add(lp.CallSign);
						tNode.Tag = lp;

						//foreach (var procedure in procedures)
						//{
						//	TreeNode node = tNode.Nodes.Add(procedure.Name); //new TreeNode(procedure.Name);
						//	node.Tag = procedure;
						//}
					}
				}
			}

			treeView1.Sort();
			Cursor.Current = Cursors.Default;
			treeView1.EndUpdate();
			this.Show(owner);

			if (treeView1.Nodes.Count > 0)
			{
				treeView1.SelectedNode = treeView1.Nodes[0];
				treeView1_AfterSelect(treeView1, new TreeViewEventArgs(treeView1.SelectedNode, TreeViewAction.Expand));
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Action == TreeViewAction.Unknown)
				return;

			comboBox1.Items.Clear();
			TreeNode currNode = e.Node;

			double minlev = 10000000.0, maxlev = -100, lev;

			if (currNode.Tag is ProcedureType)
			{
			}
			else if (!(currNode.Tag is LegPoint))
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedSigPtID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedPathID);

			_selectedPathID = 0;

			_currPt = (LegPoint)currNode.Tag;
			GlobalVars.mainForm.viewControl1.CenterAt(_currPt.pPtPrj);

			_selectedSigPtID = GlobalVars.mainForm.viewControl1.DrawGeometry(_currPt.pPtPrj, _selectedSigPtElem);

			listView1.BeginUpdate();
			listView1.Items.Clear();

			foreach (TraceLeg lg in _currPt.selctedLegs)
			{
				ProcedureType proc = lg.Owner;
				ListViewItem lvi = listView1.Items.Add(proc.Name);
				lvi.Tag = lg;

				string strLower = "", strUpper = "";

				if (lg.HFinish > lg.HStart)
				{
					lev = 0.01 * GlobalVars.unitConverter.HeightToFt(lg.HStart, eRoundMode.SPECIAL_FLOOR);
					if (lev < minlev)
						minlev = lev;
					strLower = lev.ToString();

					lev = 0.01 * GlobalVars.unitConverter.HeightToFt(lg.HFinish, eRoundMode.SPECIAL_CEIL);
					if (lev > maxlev)
						maxlev = lev;
					strUpper = lev.ToString();
				}
				else
				{
					lev = 0.01 * GlobalVars.unitConverter.HeightToFt(lg.HFinish, eRoundMode.SPECIAL_FLOOR);
					if (lev < minlev)
						minlev = lev;
					strLower = lev.ToString();

					lev = 0.01 * GlobalVars.unitConverter.HeightToFt(lg.HStart, eRoundMode.SPECIAL_CEIL);
					if (lev > maxlev)
						maxlev = lev;
					strUpper = lev.ToString();
				}

				if (proc.procType == PDM.PROC_TYPE_code.Multiple)
				{
					RouteSegment routeSeg = (RouteSegment)lg.pProcLeg;
					string sDirection = "Route / " + routeSeg.CodeDir;

					switch (routeSeg.CodeDir)
					{
						case CODE_ROUTE_SEGMENT_DIR.FORWARD:
							lg.Direction = RouteSegmentDir.Forward;
							lvi.SubItems.Add(sDirection);
							lvi.SubItems.Add(strLower);
							lvi.SubItems.Add(strUpper);

							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add("");
								lvi.SubItems.Add(lg.AztOut.ToString("0.00"));
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add(lg.AztIn.ToString("0.00"));
								lvi.SubItems.Add("");
							}
							break;

						case CODE_ROUTE_SEGMENT_DIR.BACKWARD:
							lg.Direction = RouteSegmentDir.Backward;
							lvi.SubItems.Add(sDirection);
							lvi.SubItems.Add(strLower);
							lvi.SubItems.Add(strUpper);

							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add(NativeMethods.Modulus(lg.AztIn + 180.0).ToString("0.00"));
								lvi.SubItems.Add("");
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add("");
								lvi.SubItems.Add(NativeMethods.Modulus(lg.AztOut + 180.0).ToString("0.00"));
							}
							break;

						case CODE_ROUTE_SEGMENT_DIR.BOTH:
							// Forward ===============================
							lg.Direction = RouteSegmentDir.Both;
							lvi.SubItems.Add(sDirection + "-" + RouteSegmentDir.Forward);
							lvi.SubItems.Add(strLower);
							lvi.SubItems.Add(strUpper);

							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add("");
								lvi.SubItems.Add(lg.AztOut.ToString("0.00"));
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add(lg.AztIn.ToString("0.00"));
								lvi.SubItems.Add("");
							}
							// Backward ===============================
							lvi = listView1.Items.Add(proc.Name);
							lvi.Tag = lg;
							lvi.SubItems.Add(sDirection + "-" + RouteSegmentDir.Backward);
							lvi.SubItems.Add(strLower);
							lvi.SubItems.Add(strUpper);

							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add(NativeMethods.Modulus(lg.AztIn + 180.0).ToString("0.00"));
								lvi.SubItems.Add("");
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								lvi.SubItems.Add("");
								lvi.SubItems.Add(NativeMethods.Modulus(lg.AztOut + 180.0).ToString("0.00"));
							}

							break;
					}
				}
				else
				{
					lvi.SubItems.Add(proc.procType.ToString());
					lvi.SubItems.Add(strLower);
					lvi.SubItems.Add(strUpper);

					if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
					{
						lvi.SubItems.Add("");
						lvi.SubItems.Add(lg.AztOut.ToString("0.00"));
					}
					else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
					{
						lvi.SubItems.Add(lg.AztIn.ToString("0.00"));
						lvi.SubItems.Add("");
					}
				}
			}

			listView1.EndUpdate();
			listView2.Items.Clear();

			//listView1.SelectedIndices.Add(0);
			//listView1_SelectedIndexChanged(listView1, new EventArgs());

			if(minlev <= 190.0 && maxlev >= 200.0)
			{
				comboBox1.Items.Add(minlev);
				comboBox1.Items.Add(maxlev);
			}
			else
				comboBox1.Items.Add(minlev);

			comboBox1.SelectedIndex = 0;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex < 0)
				return;

			double cpDist, fl = (double)comboBox1.Items[comboBox1.SelectedIndex];
			if (fl <= 190) cpDist = 15.0 * 1852.0;
			else cpDist = 23.0 * 1852.0;

			textBox1.Text = GlobalVars.unitConverter.DistanceToDisplayUnits(cpDist, eRoundMode.NERAEST).ToString();

			if (listView1.SelectedIndices.Count == 0)
				listView1.SelectedIndices.Add(0);

			listView1_SelectedIndexChanged(listView1, new EventArgs());
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView1.SelectedIndices.Count == 0)
				return;

			GlobalVars.mainForm.viewControl1.DeleteGeometry(_selectedPathID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_crossingPathID);
			GlobalVars.mainForm.viewControl1.DeleteGeometry(_conflictAreaID);

			ListViewItem lvi0 = ((ListView)sender).SelectedItems[0];
			TraceLeg currLeg = (TraceLeg)lvi0.Tag;
			ProcedureType currProc = currLeg.Owner;

			_selectedPathID = GlobalVars.mainForm.viewControl1.DrawGeometry(currLeg.PathGeomPrj, _selectedPathElem);
			double baseLowerH, baseUpperH;
			double baseDir = 0.0, baseAzt;

			/*========================================================================*/
			if (!double.TryParse(lvi0.SubItems[4].Text, out baseAzt))
				if (!double.TryParse(lvi0.SubItems[5].Text, out baseAzt))
					return;

			if (currLeg.ptStartPrj != null && Functions.ReturnDistanceInMeters(_currPt.pPtPrj, currLeg.ptStartPrj) > 11.0)
				baseDir = Functions.ReturnAngleInDegrees(_currPt.pPtPrj, currLeg.ptStartPrj);
			else if (currLeg.ptEndPrj != null && Functions.ReturnDistanceInMeters(_currPt.pPtPrj, currLeg.ptEndPrj) > 11.0)
				baseDir = Functions.ReturnAngleInDegrees(_currPt.pPtPrj, currLeg.ptEndPrj);
			/*========================================================================*/

			if (currLeg.HStart > currLeg.HFinish)
			{
				baseLowerH = currLeg.HFinish;
				baseUpperH = currLeg.HStart;
			}
			else
			{
				baseLowerH = currLeg.HStart;
				baseUpperH = currLeg.HFinish;
			}

			if (currProc.procType == PDM.PROC_TYPE_code.Multiple)
			{
				RouteSegment routeSeg = (RouteSegment)currLeg.pProcLeg;

				switch (routeSeg.CodeDir)
				{
					case CODE_ROUTE_SEGMENT_DIR.FORWARD:

						if (currLeg.StartPoint != null && currLeg.StartPoint.ID == _currPt.ID)
							baseAzt = currLeg.AztOut;
						else if (currLeg.EndPoint != null && currLeg.EndPoint.ID == _currPt.ID)
							baseAzt = currLeg.AztIn;
						break;

					case CODE_ROUTE_SEGMENT_DIR.BACKWARD:
						if (currLeg.StartPoint != null && currLeg.StartPoint.ID == _currPt.ID)
							baseAzt = currLeg.AztIn + 180.0;
						else if (currLeg.EndPoint != null && currLeg.EndPoint.ID == _currPt.ID)
							baseAzt = currLeg.AztOut + 180.0;
						break;

					case CODE_ROUTE_SEGMENT_DIR.BOTH:
						if (lvi0.SubItems[1].Text.EndsWith("-" + RouteSegmentDir.Forward))
						{
							// Forward ===============================
							if (currLeg.StartPoint != null && currLeg.StartPoint.ID == _currPt.ID)
								baseAzt = currLeg.AztOut;
							else if (currLeg.EndPoint != null && currLeg.EndPoint.ID == _currPt.ID)
								baseAzt = currLeg.AztIn;
						}
						else
						{
							// Backward ===============================
							if (currLeg.StartPoint != null && currLeg.StartPoint.ID == _currPt.ID)
								baseAzt = currLeg.AztIn + 180.0;
							else if (currLeg.EndPoint != null && currLeg.EndPoint.ID == _currPt.ID)
								baseAzt = currLeg.AztOut + 180.0;
						}
						break;
				}
			}
			else
			{
				if (currLeg.StartPoint != null && currLeg.StartPoint.ID == _currPt.ID)
					baseAzt = currLeg.AztOut;
				else if (currLeg.EndPoint != null && currLeg.EndPoint.ID == _currPt.ID)
					baseAzt = currLeg.AztIn;
			}

			double cpDist, cpDir = 0.0, fl = (double)comboBox1.Items[comboBox1.SelectedIndex]; //0.01 * GlobalVars.unitConverter.HeightToFt(baseUpperH, eRoundMode.SPECIAL_CEIL);

			if (fl <= 190) cpDist = 15.0 * 1852.0;
			else cpDist = 23.0 * 1852.0;

			List<Point> ptList = new List<Point>();

			ptList.Add(_currPt.pPtPrj);

			bool VerticallySeperated = (100.0 * fl - 304.8 >= baseLowerH) || (100.0 * fl + 304.8 <= baseUpperH);
			if (VerticallySeperated)
			{
				Point Ptt = Functions.PointAlongPlane(_currPt.pPtPrj, baseDir, cpDist);
				Ptt.Z = baseUpperH;
				ptList.Add(Ptt);
			}

			listView2.BeginUpdate();
			listView2.Items.Clear();

			System.Windows.Forms.ListViewGroup SameDirectionGroup = listView2.Groups[0];
			System.Windows.Forms.ListViewGroup CrossingTracksGroup = listView2.Groups[1];
			System.Windows.Forms.ListViewGroup ReciprocalTracksGroup = listView2.Groups[2];
			System.Windows.Forms.ListViewGroup VerticallySeperatedsGroup = listView2.Groups[3];

			foreach (TraceLeg lg in _currPt.selctedLegs)
			{
				if (lg == currLeg)
					continue;

				ProcedureType proc = lg.Owner;
				double LowerH, UpperH, currDir = 0.0, currAzt, da = 0.0;

				if (lg.HStart > lg.HFinish)
				{
					LowerH = lg.HFinish;
					UpperH = lg.HStart;
				}
				else
				{
					LowerH = lg.HStart;
					UpperH = lg.HFinish;
				}

				//fl = 0.01 * GlobalVars.unitConverter.HeightToFt(UpperH, eRoundMode.SPECIAL_FLOOR);
				//if (fl <= 190) cpDist = 15.0 * 1852.0;
				//else cpDist = 23.0 * 1852.0;

				VerticallySeperated = (baseLowerH - 304.8 >= UpperH) || (baseUpperH + 304.8 <= LowerH);

				ListViewItem lvi1 = listView2.Items.Add(proc.Name);

				if (proc.procType == PDM.PROC_TYPE_code.Multiple)
				{
					string sProcType;
					RouteSegment routeSeg = (RouteSegment)lg.pProcLeg;

					switch (routeSeg.CodeDir)
					{
						case CODE_ROUTE_SEGMENT_DIR.FORWARD:
							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								currAzt = lg.AztOut;
								currDir = lg.DirOut;
								cpDir = lg.DirOut;
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								currAzt = lg.AztIn;
								currDir = lg.DirIn;
								cpDir = lg.DirIn + 180.0;
							}
							else
								continue;

							da = Functions.SubtractAzimuths(baseAzt, currAzt);
							sProcType = "Route / " + routeSeg.CodeDir;

							lvi1.Tag = lg;
							lvi1.SubItems.Add(sProcType);
							lvi1.SubItems.Add(currAzt.ToString("0.00"));
							lvi1.SubItems.Add(da.ToString("0.00"));

							break;

						case CODE_ROUTE_SEGMENT_DIR.BACKWARD:
							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								currAzt = NativeMethods.Modulus(lg.AztIn + 180.0);
								currDir = lg.DirIn + 180.0;
								cpDir = lg.DirIn;
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								currAzt = NativeMethods.Modulus(lg.AztOut + 180.0);
								currDir = lg.DirOut + 180.0;
								cpDir = lg.DirOut + 180.0;
							}
							else
								continue;

							da = Functions.SubtractAzimuths(baseAzt, currAzt);
							sProcType = "Route / " + routeSeg.CodeDir;

							lvi1.Tag = lg;
							lvi1.SubItems.Add(sProcType);
							lvi1.SubItems.Add(currAzt.ToString("0.00"));
							lvi1.SubItems.Add(da.ToString("0.00"));

							break;
						case CODE_ROUTE_SEGMENT_DIR.BOTH:
							// Forward ===============================

							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								currAzt = lg.AztOut;
								currDir = lg.DirOut;
								cpDir = lg.DirOut;
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								currAzt = lg.AztIn;
								currDir = lg.DirIn;
								cpDir = lg.DirIn + 180.0;
							}
							else
								continue;

							da = Functions.SubtractAzimuths(baseAzt, currAzt);
							sProcType = "Route / " + routeSeg.CodeDir + "-" + RouteSegmentDir.Forward;

							lvi1.Tag = lg;
							lvi1.SubItems.Add(sProcType);
							lvi1.SubItems.Add(currAzt.ToString("0.00"));
							lvi1.SubItems.Add(da.ToString("0.00"));

							if (VerticallySeperated)
								lvi1.Group = VerticallySeperatedsGroup;
							else if (da <= minIntersectAngle)
								lvi1.Group = SameDirectionGroup;
							else if (da >= maxIntersectAngle)
								lvi1.Group = ReciprocalTracksGroup;
							else
							{
								lvi1.Group = CrossingTracksGroup;

								if (!VerticallySeperated)
								{
									Point Ptt = Functions.PointAlongPlane(_currPt.pPtPrj, cpDir, cpDist);
									Ptt.Z = UpperH;
									ptList.Add(Ptt);
								}
							}

							// Backward ===============================
							if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
							{
								currAzt = NativeMethods.Modulus(lg.AztIn + 180.0);
								currDir = lg.DirIn + 180.0;
								cpDir = lg.DirIn;
							}
							else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
							{
								currAzt = NativeMethods.Modulus(lg.AztOut + 180.0);
								currDir = lg.DirOut + 180.0;
								cpDir = lg.DirOut + 180.0;
							}
							else
								continue;

							da = Functions.SubtractAzimuths(baseAzt, currAzt);
							sProcType = "Route / " + routeSeg.CodeDir + "-" + RouteSegmentDir.Backward;

							lvi1 = listView2.Items.Add(proc.Name);

							lvi1.Tag = lg;
							lvi1.SubItems.Add(sProcType);
							lvi1.SubItems.Add(currAzt.ToString("0.00"));
							lvi1.SubItems.Add(da.ToString("0.00"));

							break;
					}
				}
				else
				{
					if (lg.StartPoint != null && lg.StartPoint.ID == _currPt.ID)
					{
						currAzt = lg.AztOut;
						currDir = lg.DirOut;
						cpDir = lg.DirOut;
					}
					else if (lg.EndPoint != null && lg.EndPoint.ID == _currPt.ID)
					{
						currAzt = lg.AztIn;
						currDir = lg.DirIn;
						cpDir = lg.DirIn + 180.0;
					}
					else
						continue;

					da = Functions.SubtractAzimuths(baseAzt, currAzt);

					lvi1.Tag = lg;
					lvi1.SubItems.Add(proc.procType.ToString());
					lvi1.SubItems.Add(currAzt.ToString("0.00"));
					lvi1.SubItems.Add(da.ToString("0.00"));
				}

				if (VerticallySeperated)
					lvi1.Group = VerticallySeperatedsGroup;
				else if (da <= minIntersectAngle)
					lvi1.Group = SameDirectionGroup;
				else if (da >= maxIntersectAngle)
					lvi1.Group = ReciprocalTracksGroup;
				else
				{
					lvi1.Group = CrossingTracksGroup;

					if (!VerticallySeperated)
					{
						Point Ptt = Functions.PointAlongPlane(_currPt.pPtPrj, cpDir, cpDist);
						Ptt.Z = UpperH;
						ptList.Add(Ptt);
					}
				}
			}

			NetTopologySuite.Geometries.MultiPoint mpt = new MultiPoint(ptList.ToArray());
			NetTopologySuite.Geometries.Geometry conflictGeom = (NetTopologySuite.Geometries.Geometry)mpt.ConvexHull();

			if (conflictGeom.OgcGeometryType == GeoAPI.Geometries.OgcGeometryType.Polygon)
			{
				NetTopologySuite.Geometries.Polygon conflictPoly = (NetTopologySuite.Geometries.Polygon)conflictGeom;
				_conflictAreaID = GlobalVars.mainForm.viewControl1.DrawGeometry(conflictPoly, _conflictAreaElem, -1);
			}
			//else
			//{
			//}

			listView2.EndUpdate();
		}

		private void listView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (listView2.SelectedIndices.Count == 0)
				return;
			//int inx = listView2.SelectedIndices[0];
			//ListViewItem lvi = listView2.Items[inx];

			if (_crossingPathID != 0)
				GlobalVars.mainForm.viewControl1.DeleteGeometry(_crossingPathID);

			ListViewItem lvi = ((ListView)sender).SelectedItems[0];
			TraceLeg currLeg = (TraceLeg)lvi.Tag;
			_crossingPathID = GlobalVars.mainForm.viewControl1.DrawGeometry(currLeg.PathGeomPrj, _crossingPathElem);
		}

	}
}
