using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;

namespace Aran.Temporality.CommonUtil.Util
{
    [Serializable]
    public class ColumnModel
    {
        public string Name;
        public int Position;

		private string _header;
		public string Header
		{
			get
			{
				if ( string.IsNullOrEmpty ( _header ) )
					return Name;
				else
					return _header;
			}
			set
			{
				_header = value;
			}
		}

        public bool CustomVisible;
    }

    public class UiHelperMetadata
    {
        private static List<ColumnModel> DefaultRulesColumns()
        {
            return new List<ColumnModel>
                             {
                                 new ColumnModel { Name = "IsActive", Position = 0, CustomVisible = true },
                                //  new ColumnModel { Name = "RuleId", Position = 1, CustomVisible = true },
                                 new ColumnModel { Name = "RuleUID", Position = 2, CustomVisible = true },
                                 new ColumnModel { Name = "Name", Position = 3, CustomVisible = true },
                                 new ColumnModel { Name = "ApplicableObject", Position = 4, CustomVisible = true },
                                 new ColumnModel { Name = "Source", Position = 5, CustomVisible = true },
                                 new ColumnModel { Name = "Svbr", Position = 6, CustomVisible = true },
                                 new ColumnModel { Name = "Comments", Position = 7, CustomVisible = true },
                                 new ColumnModel { Name = "Level", Position = 8, CustomVisible = true },
                             };
        }

		private static List<ColumnModel> DefaultLinkProblemColumns ( )
		{
			return new List<ColumnModel>
                             {
                                 new ColumnModel { Name = "PropertyPath", Header="Property Path", Position = 0, CustomVisible = true },
                                 new ColumnModel { Name = "ReferenceFeatureType", Header ="Reference Feature Type",  Position = 1, CustomVisible = true },
								 new ColumnModel { Name = "ReferenceFeatureIdentifier", Header ="Reference Feature Identifier",  Position = 2, CustomVisible = true }
                             };
		}

        private static List<ColumnModel> _allRulesColumns;
        public static List<ColumnModel> AllRulesColumns
        {
            get
            {
                if (_allRulesColumns==null)
                {
                    try
                    {
                        if (File.Exists("BusinessRules.cfg"))
                        {
                            using (var f = new FileStream("BusinessRules.cfg", FileMode.Open))
                            {
                                var bytes = new byte[f.Length];
                                f.Read(bytes, 0, bytes.Length);
                                f.Close();
                                _allRulesColumns = FormatterUtil.ObjectFromBytes<List<ColumnModel>>(bytes);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(ModifyRegistry)).Error(ex, ex.Message);
                    }

                    if (_allRulesColumns==null)
                    {
                        _allRulesColumns = DefaultRulesColumns();
                    }
                }
                return _allRulesColumns;
            }
        }

		private static List<ColumnModel> _linkProblemColumns;
		public static List<ColumnModel> LinkProblemColumns
		{
			get
			{
				if ( _linkProblemColumns == null )
				{
					try
					{
						if ( File.Exists ( "LinkProblemColumns.cfg" ) )
						{
							using ( var f = new FileStream ( "LinkProblemColumns.cfg", FileMode.Open ) )
							{
								var bytes = new byte[ f.Length ];
								f.Read ( bytes, 0, bytes.Length );
								f.Close ( );
								_linkProblemColumns = FormatterUtil.ObjectFromBytes<List<ColumnModel>> ( bytes );
							}
						}
					}
					catch ( Exception ex)
					{
                        LogManager.GetLogger(typeof(ModifyRegistry)).Error(ex, ex.Message);
                    }

					if ( _linkProblemColumns == null )
					{
						_linkProblemColumns = DefaultLinkProblemColumns ( );
					}
				}
				return _linkProblemColumns;
			}
		}

        public static void SetRuleColumn(string name, bool customVisible)
        {
            try
            {
                AllRulesColumns.First(t => t.Name == name).CustomVisible = customVisible;
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(ModifyRegistry)).Error(ex, ex.Message);
            }
        }

        public static void SaveBusinessRules()
        {
            using (var f = new FileStream("BusinessRules.cfg", FileMode.Create))
            {
                var bytes = FormatterUtil.ObjectToBytes(AllRulesColumns);
                f.Write(bytes, 0, bytes.Length);
                f.Flush();
                f.Close();
            }
        }

		public static void SaveLinkProblemColumns ( )
		{
			using ( var f = new FileStream ( "LinkProblemColumns.cfg", FileMode.Create ) )
			{
				var bytes = FormatterUtil.ObjectToBytes ( LinkProblemColumns );
				f.Write ( bytes, 0, bytes.Length );
				f.Flush ( );
				f.Close ( );
			}
		}
    }
}
