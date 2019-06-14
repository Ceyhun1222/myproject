using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRules.Data
{
    public class RulesDb : IRuleDbSetter , IDisposable
    {
        private SQLiteConnection _conn;

        public RulesDb()
        {
            _conn = new SQLiteConnection();
        }

        public void Open()
        {
            var fileName = Environment.GetEnvironmentVariable("BRULE_DB", EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                fileName = Path.Combine(Path.GetDirectoryName(loc), "brule-db.sdb");
            }

            _conn.ConnectionString = @"Data Source = " + fileName;
            _conn.Open();
        }

        public void Close()
        {
            _conn.Close();
        }

        public void Dispose()
        {
            _conn.Dispose();
        }

        public string GetTaggedText(string ruleId)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT coalesce(TaggedDescription_edited, TaggedDescription) " +
                    $"FROM rules WHERE UID = 'AIXM-5.1_RULE-{ruleId}'";

                var dr = cmd.ExecuteReader();
                var result = string.Empty;

                if (dr.Read())
                    result = dr[0].ToString();
                dr.Close();
                return result;
            }
        }

        public IEnumerable<BRuleTaggedDescription> GetAllTaggedDescriptions(RuleFilterProfileType profileType)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT UID, coalesce(TaggedDescription_edited, TaggedDescription) " +
                    $"FROM rules WHERE Profile IN ({ProfileToString(profileType)}) ORDER BY UID";

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    yield return new BRuleTaggedDescription
                    {
                        Uid = dr[0].ToString(),
                        TaggedDescription = dr[1].ToString()
                    };
                }

                dr.Close();
            }
        }

        public IEnumerable<BRuleTaggedDescription> GetAllTaggedDescriptions2(RuleFilterProfileType profileType)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT UID, coalesce(TaggedDescription_edited, TaggedDescription) " +
                    "FROM rules " +
                    "WHERE " +
                        $"Profile IN ({ProfileToString(profileType)}) AND " + 
                        "rowid NOT IN (SELECT rule_id FROM ignored_rules) " +
                    "ORDER BY UID";

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    yield return new BRuleTaggedDescription
                    {
                        Uid = dr[0].ToString(),
                        TaggedDescription = dr[1].ToString()
                    };
                }

                dr.Close();
            }
        }

        /// <param name="filter">
        /// Profile: RuleFilterProfileType / 
        /// IsActive: bool / 
        /// UID: List of string
        /// </param>
        public IEnumerable<BRuleCommandInfoPair> GetRuleAndCommandInfos(RuleFilterValues filter)
        {
            var filterText = string.Empty;
            if (filter != null)
            {
                if (filter.TryGetValue(RuleFilterType.IsActive, out object isActiveValue))
                    filterText = " AND is_active = " + (true.Equals(isActiveValue) ? 1 : 0);

                if (filter.TryGetValue(RuleFilterType.Profile, out object profileValue))
                    filterText += " AND Profile IN (" + ProfileToString((RuleFilterProfileType)profileValue) + ") ";

                if (filter.TryGetValue(RuleFilterType.UID, out object uidValue))
                {
                    var uidsText = new StringBuilder();

                    foreach (string uid in (uidValue as System.Collections.IList))
                    {
                        if (uidsText.Length > 0)
                            uidsText.Append(",");
                        uidsText.Append("'" + uid + "'");
                    }

                    if (uidsText.Length == 0)
                        yield break;

                    filterText += " AND UID IN (" + uidsText + ")";
                }
            }

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT " +
                        "rowid," +
                        "UID, " +
                        "Name, " +
                        "Profile, " +
                        "Comments, " +
                        "Category, " +
                        "Source, " +
                        "coalesce(command_info_edited, command_info) AS ci, " +
                        "coalesce(TaggedDescription_edited, TaggedDescription) AS TaggedDescription, " +
                        "tags, " +
                        "is_custom, " +
                        "is_active " +
                    "FROM rules " +
                    "WHERE " +
                        "(is_implemented = 1 AND (command_info IS NOT NULL OR command_info_edited IS NOT NULL)) " + 
                        filterText +
                        " ORDER BY UID";

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    var json = dr[7].ToString();
                    if (json.Length == 0)
                        continue;

                    var ruleInfo = new BRuleInfo
                    {
                        DbItemId = Convert.ToInt64(dr[0]),
                        Uid = dr[1].ToString(),
                        Name = dr[2].ToString(),
                        Profile = dr[3].ToString(),
                        Comment = dr[4].ToString(),
                        Category = dr[5].ToString(),
                        Source = dr[6].ToString(),
                        TaggedDescription = dr[8].ToString(),
                        Tags = dr[9].ToString(),
                        IsCustom = Convert.ToBoolean(dr[10]),
                        IsActive = Convert.ToBoolean(dr[11])
                    };

                    yield return new BRuleCommandInfoPair
                    {
                        Rule = ruleInfo,
                        CommandsJson = json
                    };
                }

                dr.Close();
            }
        }

        public Dictionary<string /*ruleId*/, string /*desc*/> GetTextualDescription(IEnumerable<string> ruleIds)
        {
            var result = new Dictionary<string /*ruleId*/, string /*desc*/>();

            var uids = string.Join<string>("','", ruleIds);
            if (uids.Length == 0)
                return result;

            uids = "'" + uids + "'";

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText =
                    "SELECT UID, TextualDescription " +
                    $"FROM rules WHERE UID IN ({uids})";

                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    result[dr[0].ToString()] = dr[1].ToString();
                }

                dr.Close();
            }

            return result;
        }

        #region IRuleDbSetter

        void IRuleDbSetter.SetSqlText(string ruleId, string text, bool isEdited)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = $"UPDATE rules SET {(isEdited ? "command_info_edited" : "command_info")} = :sqlText WHERE uid = '{ruleId}'";
                cmd.Parameters.AddWithValue("sqlText", text);
                cmd.ExecuteNonQuery();
            }
        }   

        void IRuleDbSetter.SetCustomRule(BRuleInfo ruleInfo, string commandInfoText)
        {
            var textualDescription = ConvertToTextualDescription(ruleInfo.TaggedDescription);

            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "SELECT EXISTS(SELECT 1 FROM rules WHERE is_custom = 0 AND UID = '" + ruleInfo.Uid + "')";
                var isCustomObj = cmd.ExecuteScalar();
                if (isCustomObj.Equals(0))
                    throw new Exception("Only custom Business Rules could be changed");

                if (ruleInfo.DbItemId < 1)
                {
                    cmd.CommandText = "INSERT INTO rules (" +
                        "UID, Name, Profile, TextualDescription, " +
                        "TaggedDescription, Comments, Category, Source, " +
                        "command_info, is_custom, is_active, is_implemented, tags) " +
                        "VALUES (" +
                        ":UID, :Name, :Profile, :TextualDescription, " +
                        ":TaggedDescription, :Comments, :Category, :Source, " +
                        ":cmdInfo, 1, :isActive, 1, :tags)";
                }
                else
                {
                    cmd.CommandText = "UPDATE rules SET " +
                        "UID=:UID, Name=:Name, Profile=:Profile, TextualDescription=:TextualDescription, " +
                        "TaggedDescription=:TaggedDescription, Comments=:Comments, Category=:Category, Source=:Source, " +
                        "command_info=:cmdInfo, is_active=:isActive, " +
                        "tags=:tags " +
                        "WHERE rowid=:rowId ";

                    cmd.Parameters.AddWithValue("rowId", ruleInfo.DbItemId);
                }

                cmd.Parameters.AddWithValue("UID", ruleInfo.Uid);
                cmd.Parameters.AddWithValue("Name", ruleInfo.Name);
                cmd.Parameters.AddWithValue("Profile", ruleInfo.Profile);
                cmd.Parameters.AddWithValue("TextualDescription", textualDescription);
                cmd.Parameters.AddWithValue("TaggedDescription", ruleInfo.TaggedDescription);
                cmd.Parameters.AddWithValue("Comments", ruleInfo.Comment);
                cmd.Parameters.AddWithValue("Category", ruleInfo.Category);
                cmd.Parameters.AddWithValue("Source", ruleInfo.Source);
                cmd.Parameters.AddWithValue("cmdInfo", commandInfoText);
                cmd.Parameters.AddWithValue("isActive", ruleInfo.IsActive);
                cmd.Parameters.AddWithValue("tags", ruleInfo.Tags);

                cmd.ExecuteNonQuery();
            }
        }

        void IRuleDbSetter.ActivateRule(IEnumerable<long> ruleIds, bool value)
        {
            SQLiteCommand cmd = null;

            try
            {
                var ruleIdsText = string.Join<long>(",", ruleIds);

                if (ruleIdsText.Length > 0)
                {
                    cmd = _conn.CreateCommand();
                    cmd.CommandText = "UPDATE rules SET is_active = " + (value ? 1 : 0) + " WHERE rowid IN (" + ruleIdsText + ")";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error in ActivateRule: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        void IRuleDbSetter.DeleteCustomRule(long ruleId)
        {
            SQLiteCommand cmd = null;

            try
            {
                cmd = _conn.CreateCommand();
                cmd.CommandText = "DELETE FROM rules WHERE rowid = " + ruleId;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error in DeleteCustomRule: " + ex.Message);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();
            }
        }

        #endregion

        #region static

        private static string ProfileToString(RuleFilterProfileType profileType)
        {
            if (profileType == RuleFilterProfileType.Both)
                return "'Error', 'Warning', ''";
            else
                return "'" + profileType + "'";
        }

        private static string ConvertToTextualDescription(string textualDescription)
        {
            return textualDescription
                .Replace("<keyword>", "")
                .Replace("</keyword>", " ")
                .Replace("<NounConcept>", "")
                .Replace("</NounConcept>", " ")
                .Replace("<Verb-concept>", "")
                .Replace("</Verb-concept>", " ")
                .Replace("<Name>", "")
                .Replace("</Name>", " ");
        }

        #endregion
    }

    public interface IRuleDbSetter
    {
        void SetSqlText(string ruleId, string text, bool isCustom = false);
        void SetCustomRule(BRuleInfo ruleInfo, string commandInfoText);
        void ActivateRule(IEnumerable<long> ruleIds, bool value);
        void DeleteCustomRule(long ruleId);
    }

    public class RuleFilterValues : Dictionary<RuleFilterType, object> { }

    public enum RuleFilterType { Profile, IsActive, UID }

    public enum RuleFilterProfileType { Error, Warning, Both }
}
