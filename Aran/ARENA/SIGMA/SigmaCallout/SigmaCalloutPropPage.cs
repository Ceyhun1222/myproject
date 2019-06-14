using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Framework;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.esriSystem;
using System.Collections;
using ESRI.ArcGIS.Display;

namespace SigmaCallout
{
	public partial class SigmaCalloutPropPage : UserControl, IComPropertyPage, IPropertyPageContext, ISymbolPropertyPage
	{
		private esriUnits _units;
		private IComPropertyPageSite _pageSite;
		private SigmaCallout _sigmaCallout;

		private IRgbColor _color;
		private IRgbColor _shadowBackColor;
		private IRgbColor _airspaceBackColor;
		//private double _leaderTolerance;
		private double _headerHorizontalMargin;
		private double _topMargin;
		private double _footerHorizontalMargin;
		private double _bottomMargin;
		private int _snap;
		private int _width;
		private bool _hasLeader;
		private bool _isDirty;
		private bool _handle;
		private bool _hasHeader;
		private bool _hasFooter;
		private int _shadow;
		private string _airspaceSymbols;
		private bool _onLeftSide;
		//private double _morseVertical;
		private double _morseShiftOnX;
		private double _morseShiftOnY;
		private double _morseLeading;
		private ISimpleLineSymbol _lineSymbol;


		public SigmaCalloutPropPage ( )
		{
			InitializeComponent ( );
			_handle = true;
			Title = "Sigma Callout";
			_units = esriUnits.esriPoints;
		}

		#region ISymbolPropertyPage Members

		public esriUnits Units
		{
			get
			{
				return _units;
			}
			set
			{
				_units = value;
			}
		}

		#endregion

		#region IPropertyPageContext Members

		public bool Applies ( object unkArray )
		{
			var type = unkArray.GetType ( );
			if ( type.IsArray )
			{
				var list = ( IEnumerable ) unkArray;
				foreach ( var item in list )
				{
					if ( item is SigmaCallout )
						return true;
				}
			}
			return false;
		}

		public void Cancel ( )
		{
			if ( IsDirty )
				IsDirty = false;
		}

		public object CreateCompatibleObject ( object kind )
		{
			throw new NotImplementedException ( );
		}

		public string GetHelpFile ( int controlID )
		{
			throw new NotImplementedException ( );
		}

		public int GetHelpId ( int controlID )
		{
			throw new NotImplementedException ( );
		}

		public void QueryObject ( object theObject )
		{
			_sigmaCallout = ( ( SigmaCallout ) theObject );
			//_sigmaCallout.LeaderTolerance = PointsToMapUnits ( _leaderTolerance );
			_sigmaCallout.Shadow = _shadow;
			_sigmaCallout.Width = ( ( IComPropertyPage ) this ).Width;
			_sigmaCallout.BackColor = _color;
			_sigmaCallout.LineSymbol = _lineSymbol;
			_sigmaCallout.ShadowBackColor = _shadowBackColor;
			_sigmaCallout.AirspaceBackColor = _airspaceBackColor;
			_sigmaCallout.AirspaceSymbols = _airspaceSymbols;
			_sigmaCallout.HeaderHorizontalMargin = _headerHorizontalMargin;
			_sigmaCallout.TopMargin = _topMargin;
			//if ( _onLeftSide.HasValue )
				_sigmaCallout.AirspaceOnLeftSide = _onLeftSide;
			_sigmaCallout.HasFooter = _hasFooter;
			_sigmaCallout.HasHeader = _hasHeader;
			_sigmaCallout.DrawLeader = _hasLeader;
			_sigmaCallout.FooterHorizontalMargin = _footerHorizontalMargin;
			_sigmaCallout.BottomMargin = _bottomMargin;
			//if ( _morseVertical.HasValue )
			//	_sigmaCallout.MorseVertical = _morseVertical.Value;
			_sigmaCallout.MorseLocationShiftOnX = _morseShiftOnX;
			_sigmaCallout.MorseLocationShiftOnY = _morseShiftOnY;
			_sigmaCallout.MorseLeading = _morseLeading;
			_sigmaCallout.Snap = _snap;
			_sigmaCallout.DrawLeader = _hasLeader;
		}

		private double PointsToMapUnits ( double value )
		{
			switch ( _units )
			{
				case esriUnits.esriCentimeters:
					return value * ( 2.54 / 72.0 );
				case esriUnits.esriInches:
					return value / 72.0;
				case esriUnits.esriMillimeters:
					return value * ( 25.4 / 72.0 );
			}
			return value;
		}

		#endregion

		#region IComPropertyPage Members

		public int Activate ( )
		{
			return this.Handle.ToInt32 ( );
		}

		public bool Applies ( ISet objects )
		{
			var obj = objects.Next ( );
			while ( obj != null )
			{
				if ( obj is SigmaCallout )
					return true;
				obj = objects.Next ( );
			}
			return false;
		}

		public void Apply ( )
		{
			if ( !_isDirty )
				return;

			QueryObject ( _sigmaCallout );

			_isDirty = false;
		}

		void IComPropertyPage.Hide ( )
		{
			( ( UserControl ) this ).Hide ( );
		}

		void IComPropertyPage.Show ( )
		{
			if ( _sigmaCallout != null )
			{
				_handle = false;
				nmrcUpDwnWidth.Value = ( decimal ) _sigmaCallout.Width;

				//nmrcUpDwnTolerance.Value = ( decimal ) MapUnitsToPoints ( _sigmaCallout.LeaderTolerance );
				if ( _sigmaCallout.BackColor != null )
					_color = _sigmaCallout.BackColor;
				if ( _sigmaCallout.LineSymbol != null )
					_lineSymbol = _sigmaCallout.LineSymbol;
				pnlColor.BackColor = System.Drawing.Color.FromArgb ( _color.Red, _color.Green, _color.Blue );
				if ( _sigmaCallout.ShadowBackColor != null )
					_shadowBackColor = _sigmaCallout.ShadowBackColor;
				if ( _sigmaCallout.AirspaceBackColor != null )
					_airspaceBackColor = _sigmaCallout.AirspaceBackColor;
				pnlShadowBackColor.BackColor = System.Drawing.Color.FromArgb ( _shadowBackColor.Red, _shadowBackColor.Green, _shadowBackColor.Blue );
				//pnlAirspaceBackColor.BackColor = System.Drawing.Color.FromArgb ( _airspaceBackColor.Red, _airspaceBackColor.Green, _airspaceBackColor.Blue );

				_airspaceBackColor = _sigmaCallout.AirspaceBackColor;
				_onLeftSide = _sigmaCallout.AirspaceOnLeftSide;
				_airspaceSymbols = _sigmaCallout.AirspaceSymbols;
				_morseLeading = _sigmaCallout.MorseLeading;
				_morseShiftOnX = _sigmaCallout.MorseLocationShiftOnX;
				_morseShiftOnY = _sigmaCallout.MorseLocationShiftOnY;
				_sigmaCallout.MorseHandler = ( ) =>
				{

					btnMorse.Enabled = !string.IsNullOrEmpty ( _sigmaCallout.MorseFullText );
				};

				chckBxHasHeader.Checked = _sigmaCallout.HasHeader;
				chckBxHasFooter.Checked = _sigmaCallout.HasFooter;
				chckBxLeaderDraw.Checked = _sigmaCallout.DrawLeader;
				nmrcUpDwnShadow.Value = ( decimal ) _sigmaCallout.Shadow;
				nmrcUpDwnHeaderHorizontalMargin.Value = ( decimal ) _sigmaCallout.HeaderHorizontalMargin;
				nmrcUpDwnTopMargin.Value = ( decimal ) _sigmaCallout.TopMargin;
				nmrcUpDwnFooterHorizontalMargin.Value = ( decimal ) _sigmaCallout.FooterHorizontalMargin;
				nmrcUpDwnBottomMargin.Value = ( decimal ) _sigmaCallout.BottomMargin;

				ApplyAnchorSnap ( _sigmaCallout.Snap );
				_handle = true;
			}
			//else
			//{
			//	pnlColor.BackColor = System.Drawing.Color.FromArgb ( 0, 255, 255 );

			//	radBtnNearest.Checked = true;
			//	nmrcUpDwnTolerance.Value = 1;
			//	nmrcUpDwnLeftMargin.Value = 1;
			//	nmrcUpDwnTopMargin.Value = 1;
			//	nmrcUpDwnRightMargin.Value = 1;
			//	nmrcUpDwnBottomMargin.Value = 1;

			//	_units = esriUnits.esriPoints;
			//	IsDirty = false;
			//}
			( ( UserControl ) this ).Show ( );
		}

		private void ApplyAnchorSnap ( int snap )
		{
			switch ( snap )
			{
				case 0:
					radBtnNearest.Checked = true;
					break;
				case 1:
					radBtnLeftTop.Checked = true;
					break;
				case 2:
					radBtnMidleTop.Checked = true;
					break;
				case 3:
					radBtnRightTop.Checked = true;
					break;
				case 4:
					radBtnLeftMiddle.Checked = true;
					break;
				case 5:
					radBtnCenter.Checked = true;
					break;
				case 6:
					radBtnRightMiddle.Checked = true;
					break;
				case 7:
					radBtnLeftBottom.Checked = true;
					break;
				case 8:
					radBtnMiddleBottom.Checked = true;
					break;
				case 9:
					radBtnRightBottom.Checked = true;
					break;
				default:
					break;
			}
		}

		private double MapUnitsToPoints ( double value )
		{
			switch ( _units )
			{
				case esriUnits.esriCentimeters:
					return value * ( 72.0 / 2.54 );
				case esriUnits.esriInches:
					return value * 72.0;
				case esriUnits.esriMillimeters:
					return value * ( 72.0 / 25.4 );
			}
			return value;
		}

		public void Deactivate ( )
		{
			this.Dispose ( );
		}

		public string HelpFile
		{
			get
			{
				return string.Empty;
			}
		}

		int IComPropertyPage.Width
		{
			get
			{
				return _width;
			}
		}

		bool IComPropertyPage.IsPageDirty
		{
			get
			{
				return _isDirty;
			}
		}

		public bool IsDirty
		{
			get
			{
				return _isDirty;
			}
			set
			{
				if ( _isDirty != value && _handle )
				{
					_isDirty = value;
					if ( _pageSite != null )
						_pageSite.PageChanged ( );
					Apply ( );
				}
			}
		}

		public IComPropertyPageSite PageSite
		{
			set
			{
				_pageSite = value;
			}
		}

		public int Priority
		{
			get;
			set;
		}

		public void SetObjects ( ISet objects )
		{
			if ( objects == null || objects.Count == 0 )
				return;
			objects.Reset ( );
			_sigmaCallout = ( SigmaCallout ) objects.Next ( );
		}

		public string Title
		{
			get;
			set;
		}

		public int get_HelpContextID ( int controlID )
		{
			return 0;
		}

		#endregion

		#region GUI events

		private void btnColor_Click ( object sender, EventArgs e )
		{
            //ISymbolSelector pSymbolSelector = new SymbolSelector ( );
            //IFillSymbol pFillSymbol = ( IFillSymbol ) new SimpleFillSymbol ( );
            //pFillSymbol.Color = _color;
            //pFillSymbol.Outline = _lineSymbol;
            //pSymbolSelector.AddSymbol ( ( ISymbol ) pFillSymbol );
            //if ( pSymbolSelector.SelectSymbol ( 0 ) )
            //{
            //    IFillSymbol symbolResult = ( IFillSymbol ) pSymbolSelector.GetSymbolAt ( 0 );
            //    _color = ( IRgbColor ) symbolResult.Color;
            //    _lineSymbol = symbolResult.Outline;
            //    pnlColor.BackColor = System.Drawing.Color.FromArgb ( _color.Transparency, _color.Red, _color.Green, _color.Blue );
            //    IsDirty = true;
            //}
			
		}

		private void nmrcUpDwnWidth_ValueChanged ( object sender, EventArgs e )
		{
			if ( ( ( int ) nmrcUpDwnWidth.Value ) != _width )
			{
				_width = ( int ) nmrcUpDwnWidth.Value;
				IsDirty = true;
			}
		}

		private void nmrcUpDwnHeaderHorizontalMargin_ValueChanged ( object sender, EventArgs e )
		{
			if ( _headerHorizontalMargin != ( double ) nmrcUpDwnHeaderHorizontalMargin.Value )
			{
				_headerHorizontalMargin = ( double ) nmrcUpDwnHeaderHorizontalMargin.Value;
				IsDirty = true;
			}
		}

		private void nmrcUpDwnTopMargin_ValueChanged ( object sender, EventArgs e )
		{
			if ( _topMargin != ( double ) nmrcUpDwnTopMargin.Value )
			{
				_topMargin = ( double ) nmrcUpDwnTopMargin.Value;
				IsDirty = true;
			}
		}

		private void nmrcUpDwnFooterHorizontalMargin_ValueChanged ( object sender, EventArgs e )
		{
			if ( _footerHorizontalMargin != ( double ) nmrcUpDwnFooterHorizontalMargin.Value )
			{
				_footerHorizontalMargin = ( double ) nmrcUpDwnFooterHorizontalMargin.Value;
				IsDirty = true;
			}
		}

		private void nmrcUpDwnBottomMargin_ValueChanged ( object sender, EventArgs e )
		{
			if ( _bottomMargin != ( double ) nmrcUpDwnBottomMargin.Value )
			{
				_bottomMargin = ( double ) nmrcUpDwnBottomMargin.Value;
				IsDirty = true;
			}
		}

		private void MorseShiftOnXChanged ( object sender, EventArgs e )
		{
			if ( _morseShiftOnX != ( double ) ( ( NumericUpDown ) sender ).Value )
			{
				_morseShiftOnX = ( double ) ( ( NumericUpDown ) sender ).Value;
				IsDirty = true;
			}
		}

		private void MorseShiftOnYChanged ( object sender, EventArgs e )
		{
			if ( _morseShiftOnY != ( double ) ( ( NumericUpDown ) sender ).Value )
			{
				_morseShiftOnY = ( double ) ( ( NumericUpDown ) sender ).Value;
				IsDirty = true;
			}
		}

		private void MorseLeadingChanged ( object sender, EventArgs e )
		{
			if ( _morseLeading != ( double ) ( ( NumericUpDown ) sender ).Value - 10)
			{
				_morseLeading = ( double ) ( ( NumericUpDown ) sender ).Value - 10;
				IsDirty = true;
			}
		}

		private void Margin_Changed ( object sender, EventArgs a )
		{
			var radBtn = ( RadioButton ) sender;
			if ( radBtn.Checked )
			{
				var t = radBtn.Tag;
				if ( int.TryParse ( t.ToString ( ), out _snap ) )
				{
					//_snap = ( int ) radBtn.Tag;
					IsDirty = true;
				}
			}
		}

		private void chckBxHasHeader_CheckedChanged ( object sender, EventArgs e )
		{
			_hasHeader = chckBxHasHeader.Checked;
			IsDirty = true;
		}

		private void chckBxHasFooter_CheckedChanged ( object sender, EventArgs e )
		{
			_hasFooter = chckBxHasFooter.Checked;
			IsDirty = true;
		}

		private void nmrcUpDwnShadow_ValueChanged ( object sender, EventArgs e )
		{
			if ( _shadow != (int) nmrcUpDwnShadow.Value )
			{
				_shadow = (int) nmrcUpDwnShadow.Value;
				IsDirty = true;
			}
		}

		private void AirspaceSymbolChanged ( object sender, EventArgs e )
		{
			if ( _airspaceSymbols != ((TextBox)sender).Text.ToUpper ( ))
			{
				_airspaceSymbols = ((TextBox)sender).Text.ToUpper ( );
				IsDirty = true;
			}
		}

		private void btnShadowBackColor_Click ( object sender, EventArgs e )
		{
			IColorBrowser colorBrowser = new ColorBrowserClass ( );
			colorBrowser.Color = _shadowBackColor;
			if ( colorBrowser.DoModal ( this.Handle.ToInt32 ( ) ) )
			{
				_shadowBackColor = ( IRgbColor ) colorBrowser.Color;
				pnlShadowBackColor.BackColor = System.Drawing.Color.FromArgb ( _shadowBackColor.Transparency, _shadowBackColor.Red, _shadowBackColor.Green, _shadowBackColor.Blue );
				IsDirty = true;
			}

		}

		private void AirspaceBackColorChanged ( object sender, EventArgs e )
		{
			var color = ( ( Panel ) sender ).BackColor;
			_airspaceBackColor.Red = color.R;
			_airspaceBackColor.Green = color.G;
			_airspaceBackColor.Blue = color.B;
			IsDirty = true;
		}

		private void AirspaceLocationChanged ( object sender, EventArgs e )
		{
			bool tmp = ( ( RadioButton ) sender ).Checked;
			if ( tmp != _onLeftSide)
			{
				_onLeftSide = tmp;
				IsDirty = true;
			}
		}

		private void btnAirspace_Click ( object sender, EventArgs e )
		{
			//bool t = false;
			//if ( _onLeftSide.HasValue )
			//	t = _onLeftSide.Value;
			//_airspaceBackColor = new RgbColorClass ( );
			//_airspaceBackColor.Red = 0;
			//_airspaceBackColor.Green = 160;
			//_airspaceBackColor.Blue = 255;

			FormAirspace form = new FormAirspace ( _airspaceSymbols, _airspaceBackColor, _onLeftSide );
			form.AddSymbolChangedHandler ( new EventHandler ( AirspaceSymbolChanged ) );
			form.AddBackColorChangedHandler ( new EventHandler ( AirspaceBackColorChanged ) );
			form.AddLocationChangedHandler ( new EventHandler ( AirspaceLocationChanged ) );
			form.ShowDialog ( );
		}

		private void btnMorse_Click ( object sender, EventArgs e )
		{
			FormMorse form = new FormMorse ( _morseShiftOnX, _morseShiftOnY, _morseLeading + 10 );
			form.AddShiftOnXChangedHandler ( new EventHandler ( MorseShiftOnXChanged ) );
			form.AddShiftOnYChangedHandler ( new EventHandler ( MorseShiftOnYChanged ) );
			form.AddLeadingChangedHandler ( new EventHandler ( MorseLeadingChanged ) );
			form.ShowDialog ( );
		}

		private void chckBxLiderDraw_CheckedChanged ( object sender, EventArgs e )
		{
			_hasLeader = chckBxLeaderDraw.Checked;
			IsDirty = true;
		}
		#endregion
	}
}