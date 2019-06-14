//using Aran.Aim.BusinessRules;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.SQLite;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SbvrParser.TesterConsole
//{
//    public class RulesDb : IDisposable, IRuleDbSetter
//    {
//        private SQLiteConnection _conn;

//        public RulesDb()
//        {
//            _conn = new SQLiteConnection();
//        }

//        public void Open()
//        {
//            var fileName = Environment.GetEnvironmentVariable("BRULE_DB", EnvironmentVariableTarget.User);

//            if (string.IsNullOrWhiteSpace(fileName))
//            {
//                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
//                fileName = Path.Combine(Path.GetDirectoryName(loc), "brule-db.sdb");
//            }

//            _conn.ConnectionString = @"Data Source = " + fileName;
//            _conn.Open();
//        }

//        public void Close()
//        {
//            _conn.Close();
//        }

//        public void Dispose()
//        {
//            _conn.Dispose();
//        }

//        public string GetTaggedText(string ruleId)
//        {
//            var cmd = _conn.CreateCommand();
//            cmd.CommandText = 
//                "SELECT TaggedDescription " +
//                $"FROM rules WHERE UID = 'AIXM-5.1_RULE-{ruleId}'";

//            var dr = cmd.ExecuteReader();
//            var result = string.Empty;

//            if (dr.Read())
//                result = dr[0].ToString();
//            dr.Close();
//            return result;
//        }

//        /// <param name="ruleType">'Error' / 'Warning' / 'Error', 'Warning'</param>
//        public IEnumerable<BRuleTaggedDescription> GetAll(string ruleType)
//        {
//            using (var cmd = _conn.CreateCommand())
//            {
//                cmd.CommandText =
//                    "SELECT UID, TaggedDescription " +
//                    $"FROM rules WHERE Profile IN ({ruleType}) ORDER BY UID";

//                var dr = cmd.ExecuteReader();

//                while (dr.Read())
//                    yield return new BRuleTaggedDescription { Uid = dr[0].ToString(), TaggedDescription = dr[1].ToString() };

//                dr.Close();
//            }
//        }

//        /// <param name="ruleType">'Error' / 'Warning' / 'Error', 'Warning'</param>
//        public IEnumerable<BRuleCommandInfoPair> GetCommandInfos(string ruleType, bool isActive = true)
//        {
//            using (var cmd = _conn.CreateCommand())
//            {
//                cmd.CommandText =
//                    "SELECT " +
//                        "UID, Name, Profile, Comments, " +
//                        "Category, Source, coalesce(command_info_edited, command_info) AS ci, TextualDescription, " +
//                        "is_custom " +
//                    "FROM rules " +
//                    "WHERE " +
//                        $"is_active = {(isActive ? 1 : 0)} AND " + 
//                        "(command_info IS NOT NULL OR command_info_edited IS NOT NULL) AND " +
//                        $"Profile IN ({ruleType}) " +
//                        "ORDER BY UID";

//                var dr = cmd.ExecuteReader();

//                while (dr.Read())
//                {
//                    var json = dr[6].ToString();
//                    if (json.Length == 0)
//                        continue;

//                    var ruleInfo = new BRuleInfo
//                    {
//                        Uid = dr[0].ToString(),
//                        Name = dr[1].ToString(),
//                        Profile = dr[2].ToString(),
//                        Comment = dr[3].ToString(),
//                        Category = dr[4].ToString(),
//                        Source = dr[5].ToString(),
//                        TextualDescription = dr[5].ToString(),
//                        IsCustom = Convert.ToBoolean(dr[6])
//                    };
                    
//                    var cil = CommandInfoList.FromJson(json);

//                    yield return new BRuleCommandInfoPair { Rule = ruleInfo, Commands = cil };
//                }

//                dr.Close();
//            }
//        }

//        public IEnumerable<BRuleCommandInfoPair> GetCommandInfosByUid(IEnumerable<string> uids)
//        {
//            var uidsText = new StringBuilder();

//            foreach (var uid in uids)
//            {
//                if (uidsText.Length > 0)
//                    uidsText.Append(",");
//                uidsText.Append("'" + uid + "'");
//            }

//            if (uidsText.Length == 0)
//                yield break;

//            using (var cmd = _conn.CreateCommand())
//            {
//                cmd.CommandText =
//                    "SELECT " +
//                        "UID, Name, Profile, Comments, " +
//                        "Category, Source, coalesce(command_info_edited, command_info) AS ci, TextualDescription, " +
//                        "is_custom " +
//                    "FROM rules " +
//                    "WHERE UID IN (" + uidsText + ")";

//                using (var dr = cmd.ExecuteReader())
//                {
//                    while (dr.Read())
//                    {
//                        var json = dr[6].ToString();
//                        if (json.Length == 0)
//                            continue;

//                        var ruleInfo = new BRuleInfo
//                        {
//                            Uid = dr[0].ToString(),
//                            Name = dr[1].ToString(),
//                            Profile = dr[2].ToString(),
//                            Comment = dr[3].ToString(),
//                            Category = dr[4].ToString(),
//                            Source = dr[5].ToString(),
//                            TextualDescription = dr[6].ToString(),
//                            IsCustom = Convert.ToBoolean(dr[7])
//                        };

//                        var cil = CommandInfoList.FromJson(json);

//                        yield return new BRuleCommandInfoPair { Rule = ruleInfo, Commands = cil };
//                    }

//                    dr.Close();
//                }
//            }
//        }

//        void IRuleDbSetter.SetSqlText(string ruleId, string text, bool isCustom)
//        {
//            using (var cmd = _conn.CreateCommand())
//            {
//                cmd.CommandText = $"UPDATE rules SET {(isCustom ? "command_info_edited" : "command_info")} = :sqlText WHERE uid = '{ruleId}'";
//                cmd.Parameters.AddWithValue("sqlText", text);
//                cmd.ExecuteNonQuery();
//            }
//        }

//        void IRuleDbSetter.SetCustomBusinessRule(BRuleInfo ruleInfo)
//        {
//            var taggedDescription = ConvertToTaggedDescription(ruleInfo.TextualDescription);
//            string commandInfoText = "";

//            using (var cmd = _conn.CreateCommand())
//            {
//                cmd.CommandText = "SELECT is_custom FROM rules WHERE UID = '" + ruleInfo.Uid + "'";
//                var isCustomObj = cmd.ExecuteScalar();
//                if (isCustomObj.Equals(0))
//                    throw new Exception("Only custom Business Rules could be changed");

//                cmd.CommandText = "INSERT OR REPLACE INTO rules (" +
//                    "UID, Name, Profile, TextualDescription, " +
//                    "TaggedDescription, Comments, Category, Source, " +
//                    "command_info, is_custom) " +
//                    "VALUES (" +
//                    ":UID, :Name, :Profile, :TextualDescription, " +
//                    ":TaggedDescription, :Comments, :Category, :Source, " +
//                    ":command_info, 1)";

//                cmd.Parameters.AddWithValue("UID", ruleInfo.Uid);
//                cmd.Parameters.AddWithValue("Name", ruleInfo.Name);
//                cmd.Parameters.AddWithValue("Profile", ruleInfo.Profile);
//                cmd.Parameters.AddWithValue("TextualDescription", ruleInfo.TextualDescription);
//                cmd.Parameters.AddWithValue("TaggedDescription", taggedDescription);
//                cmd.Parameters.AddWithValue("Comments", ruleInfo.Comment);
//                cmd.Parameters.AddWithValue("Category", ruleInfo.Category);
//                cmd.Parameters.AddWithValue("Source", ruleInfo.Source);
//                cmd.Parameters.AddWithValue("command_info", commandInfoText);

//                cmd.ExecuteNonQuery();
//            }
//        }

//        private string ConvertToTaggedDescription(string textualDescription)
//        {
//            return string.Empty;
//        }
//    }

//    public interface IRuleDbSetter
//    {
//        void SetSqlText(string ruleId, string text, bool isCustom = false);
//        void SetCustomBusinessRule(BRuleInfo ruleInfo);
//    }

//    public class BRuleInfo
//    {
//        public string Uid { get; set; }
//        public string Name { get; set; }
//        public string Profile { get; set; }
//        public string Comment { get; set; }
//        public string Category { get; set; }
//        public string Source { get; set; }
//        public string TextualDescription { get; set; }
//        public bool IsCustom { get; set; }
//    }

//    public class BRuleTaggedDescription
//    {
//        public string Uid { get; set; }
//        public string TaggedDescription { get; set; }
//    }

//    public class BRuleCommandInfoPair
//    {
//        public BRuleInfo Rule { get; set; }
//        public CommandInfoList Commands { get; set; }
//    }
//}