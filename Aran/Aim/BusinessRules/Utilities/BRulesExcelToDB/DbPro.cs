using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Dynamic;
using Aran.Aim;

namespace BRulesExcelToDB
{
    public class DbPro
    {
        private SQLiteConnection _conn;
        private object _state;

        public DbPro()
        {
            _conn = new SQLiteConnection();
        }

        public Func<bool> DeleteExistedTable { get; set; }

        public void Open()
        {
            var fileName = Environment.GetEnvironmentVariable("BRULE_DB", EnvironmentVariableTarget.User);

            if (string.IsNullOrWhiteSpace(fileName))
            {
                var loc = System.Reflection.Assembly.GetExecutingAssembly().Location;
                fileName = Path.Combine(Path.GetDirectoryName(loc), "brule-db.sdb");
            }
#warning new file name
            fileName += "2";

            _conn.ConnectionString = (new SQLiteConnectionStringBuilder { DataSource = fileName }.ToString());
            _conn.Open();
        }

        public void Test()
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT TextualDescription FROM rules";
            var dr = cmd.ExecuteReader();

            var phraseItems = new[] {
                new Phrase { Text = "It is prohibited" },
                new Phrase { Text = "Each" },
                new Phrase { Text = "It is obligatory" } };

            var unknownPhrase = new Phrase { Text = "Unknown" };

            while(dr.Read())
            {
                var phrase = dr[0].ToString();

                bool isKnown = false;

                foreach(var kpi in phraseItems)
                {
                    if (phrase.StartsWith(kpi.Text))
                    {
                        kpi.Count++;
                        isKnown = true;
                        break;
                    }
                }

                if (!isKnown)
                    unknownPhrase.Count++;
            }

            var list = new List<Phrase>(phraseItems);
            list.Add(unknownPhrase);

            list.ForEach((item) =>
            {
                Debug.WriteLine($"{item.Text}\t{item.Count}");
            });
        }

        public void Close()
        {
            _conn.Close();
        }

        public void BeginInsertRule()
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT EXISTS (SELECT 1 FROM sqlite_master WHERE type='table' AND name='rules')";

            var x = cmd.ExecuteScalar();
            if (Convert.ToBoolean(x))
            {
                if (DeleteExistedTable != null && DeleteExistedTable())
                {
                    cmd.CommandText = "DELETE FROM rules";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    cmd.Dispose();
                    return;
                }
            }
            else
            {
                cmd.CommandText =
                    "CREATE TABLE rules ( " +
                    "    UID TEXT UNIQUE, " +
                    "    Name TEXT, " +
                    "    Profile TEXT, " +
                    "    Source TEXT, " +
                    "    TextualDescription TEXT, " +
                    "    TaggedDescription TEXT, " +
                    "    Comments TEXT, " +
                    "    AixmClass TEXT, " +
                    "    AixmAttribute TEXT, " +
                    "    AixmAssociation TEXT, " +
                    "    TimesliceApplicability TEXT, " +
                    "    Category TEXT," +
                    "    is_custom INT NOT NULL DEFAULT 0)";

                cmd.ExecuteNonQuery();

                cmd.CommandText = "CREATE UNIQUE INDEX inx_rules ON rules (UID)";
                cmd.ExecuteNonQuery();
            }

            cmd.CommandText = "INSERT INTO rules VALUES (" +
                "?, ?, ?, ?," +
                "?, ?, ?, ?," +
                "?, ?, ?, ?)";

            var prms = cmd.Parameters;

            for (var i = 0; i < 12; i++)
                prms.Add(new SQLiteParameter());

            _state = cmd;
        }

        public void SetRule(Rule rule)
        {
            var cmd = _state as SQLiteCommand;

            var prms = cmd.Parameters;

            prms[0].Value = rule.UID;
            prms[1].Value = rule.Name;
            prms[2].Value = rule.Profile;
            prms[3].Value = rule.Source;

            prms[4].Value = rule.TextualDescription;
            prms[5].Value = rule.TaggedDescription;
            prms[6].Value = rule.Comments;
            prms[7].Value = rule.AixmClass;

            prms[8].Value = rule.AixmAttribute;
            prms[9].Value = rule.AixmAssociation;
            prms[10].Value = rule.TimesliceApplicability;
            prms[11].Value = rule.Category;

            cmd.ExecuteNonQuery();
        }

        public string GetNotImplementedReason(string uid)
        {
            var cmd = _conn.CreateCommand();
            cmd.CommandText = "SELECT not_implemented_reason FROM rules WHERE uid = '" + uid + "'";
            var obj = cmd.ExecuteScalar();
            return obj == null ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// </summary>
        /// <param name="which">may be: {'isComposedOf'}</param>
        public void SetCustomCommandInfo(string which)
        {
            if (which == "isComposedOf")
                SetCustomCommandInfo_isComposedOf();
        }

        private void SetCustomCommandInfo_isComposedOf()
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = 
                    "SELECT r.rowid, tp.type, tp.text " +
                    "FROM rules r, tagged_pair tp " +
                    "WHERE r.rowid = tp.rule_id AND " +
                    "   r.not_implemented_reason = 'isComposedOf' " +
                    "ORDER BY r.rowid";

                var list = new List<ExpandoObject>();

                using (var reader = cmd.ExecuteReader())
                {
                    dynamic currItem = new ExpandoObject();
                    currItem.Id = 0;

                    while (reader.Read())
                    {
                        var ruleId = reader.GetInt64(0);

                        if (ruleId != currItem.Id)
                        {
                            currItem = new ExpandoObject();
                            currItem.Id = ruleId;
                            currItem.Items = new List<ExpandoObject>();
                            list.Add(currItem);
                        }

                        dynamic eo = new ExpandoObject();
                        eo.Type = reader[1].ToString();
                        eo.Text = reader[2].ToString();
                        currItem.Items.Add(eo);
                    }
                }

                Setter_isComposedOf(list);
            }
        }

        private void Setter_isComposedOf(List<ExpandoObject> list)
        {
            using (var cmd = _conn.CreateCommand())
            {
                cmd.CommandText = "UPDATE rules SET command_info_edited = :info WHERE rowid = :id";
                cmd.Parameters.AddWithValue("id", null);
                cmd.Parameters.AddWithValue("info", null);

                foreach (dynamic item in list)
                {
                    var cmdInfo = GetTextFrom_isComposedOf(item.Items);

                    cmd.Parameters[0].Value = item.Id;
                    cmd.Parameters[1].Value = cmdInfo;

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private string GetTextFrom_isComposedOf(List<ExpandoObject> pairList)
        {
            //Keyword       It is prohibited that
            //Keyword       a
            //Noun          Navaid
            //Keyword       with
            //Keyword       assigned
            //Noun          type
            //Keyword       equal-to
            //Name          'VOR'
            //Verb          isComposedOf
            //Noun          NavaidEquipment
            //Verb          specialisation
            //Noun          DME

            if (pairList.Count < 12)
                return null;

            dynamic namePair = pairList[7];
            dynamic nounPair = pairList[11];

            if (!string.Equals(namePair.Type, "Name", StringComparison.InvariantCultureIgnoreCase) ||
                !string.Equals(nounPair.Type, "Noun", StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            var typePropValue = namePair.Text;

            if (!Enum.TryParse<FeatureType>(nounPair.Text, out FeatureType eqType))
                return null;

            return
            "{" +
                "\"Items\": [" +
                    "{ " +
                        "\"Command\": " +
                            "\"SELECT identifier_p1, identifier_p2 FROM features " +
                            "WHERE feat_type = :featType AND (" +
                                "($AimEqualTo$('type', :v0)) AND " +
                                "($AimRefCount$('navaidEquipment.NavaidComponent.theNavaidEquipment', :v1, 1))" +
                            ")\", " +

                        "\"CommandValues\": [[\"" + typePropValue + "\"], " + ((int)eqType) + "], " +
                        "\"FeatureType\": " + ((int)FeatureType.Navaid) + " " +
                    "}" +
                "]" +
            "}";
        }
    }

    class Phrase
    {
        public string Text { get; set; }
        public int Count { get; set; }
    }

}
