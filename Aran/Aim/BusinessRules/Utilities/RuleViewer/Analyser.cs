using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Xml;
using Aran.Aim.BusinessRules.SbvrParser;

namespace RuleViewer
{
    public class Analyser : IDisposable
    {
        private SQLiteConnection _conn;

        public Analyser()
        {
            _conn = new SQLiteConnection();
        }

        public void Open()
        {
            var fileName = @"D:\TMP\b-rules-0.7.2.sdb";

            _conn.ConnectionString = (new SQLiteConnectionStringBuilder
            {
                DataSource = fileName
            }).ToString();

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

        public void Test()
        {
        }

        //private void Test1()
        //{
        //    using (var cmd = _conn.CreateCommand())
        //    {
        //        cmd.CommandText =
        //            "SELECT substr(r.uid, 15, 6), r.TaggedDescription " +
        //            "FROM rules r " +
        //            "INNER JOIN tagged_pair p ON p.rule_id = r.rowid AND p.order_index = 0 AND " +
        //            "   p.type = 'Keyword' AND p.text = 'Each' AND r.Profile = 'Error'";

        //        var dict = new Dictionary<TaggedKey, HashSet<string>>();

        //        using (var dr = cmd.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                var ruleId = dr[0].ToString();

        //                try
        //                {
        //                    var reader = new TaggedDocument();
        //                    reader.Init(dr.GetString(1));

        //                    reader.Read();
        //                    if (reader.LastRead.Key != TaggedKey.Keyword || reader.LastRead.Text != "Each")
        //                    {
        //                        Console.Write("Rule item is not each item, id: " + ruleId);
        //                        continue;
        //                    }

        //                    reader.Next();

        //                    var noun = new Noun();
        //                    noun.Parse(reader);

        //                    HashSet<string> textSet;
        //                    if (dict.ContainsKey(reader.LastRead.Key))
        //                    {
        //                        textSet = dict[reader.LastRead.Key];
        //                    }
        //                    else
        //                    {
        //                        textSet = new HashSet<string>();
        //                        dict.Add(reader.LastRead.Key, textSet);
        //                    }

        //                    textSet.Add(reader.LastRead.Text);
        //                }
        //                catch (Exception pex)
        //                {
        //                    Console.WriteLine("Error on parse, id: " + ruleId + ", details: " + pex.Message);
        //                }
        //            }
        //            dr.Close();
        //        }
        //    }
        //}

        //private void Test2()
        //{
        //    using (var cmd = _conn.CreateCommand())
        //    {
        //        cmd.CommandText =
        //            "SELECT substr(r.uid, 15, 6), r.TaggedDescription " +
        //            "FROM rules r " +
        //            "WHERE r.Profile = 'Error'";

        //        var tgRoot = new TgNode();

        //        using (var dr = cmd.ExecuteReader())
        //        {
        //            while (dr.Read())
        //            {
        //                var ruleId = dr[0].ToString();

        //                try
        //                {
        //                    var ttr = new TaggedDocument();
        //                    ttr.Init(dr.GetString(1));

        //                    var tgNode = new TgNode();
        //                    var tgCurrNode = tgNode;

        //                    TaggedItem ti;
        //                    while ((ti = ttr.Read()).Key != TaggedKey.None)
        //                    {
        //                        var newItem = ti;
        //                        if (ti.Key == TaggedKey.Noun)
        //                        {
        //                            var noun = new Noun();
        //                            noun.Parse(ttr);

        //                            tgCurrNode.Key = TaggedKey.Noun;
        //                            tgCurrNode.Text = string.Empty;
        //                        }
        //                        else
        //                        {
        //                            tgCurrNode.Key = ti.Key;
        //                            tgCurrNode.Text = ti.Text;

        //                            ttr.Next();
        //                        }

        //                        var childNode = new TgNode();
        //                        tgCurrNode.Children.Add(childNode);
        //                        tgCurrNode = childNode;
        //                    }

        //                    tgRoot.Children.Add(tgNode);
        //                }
        //                catch (Exception pex)
        //                {
        //                    Console.WriteLine("Error on parse, id: " + ruleId + ", details: " + pex.Message);
        //                }
        //            }
        //            dr.Close();
        //        }

        //        GrouppingNode(tgRoot);

        //        var xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(
        //            "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +
        //            "<item></item>");
        //        ToXml(tgRoot, xmlDoc.DocumentElement);

        //        xmlDoc.Save(@"D:\TMP\123.xml");
        //    }
        //}

        private void Test3()
        {
            var root = new TgNode
            {
                Children =
                {
                    new TgNode
                    {
                        Text = "100",
                        Children =
                        {
                            new TgNode
                            {
                                Text = "110",
                                Children =
                                {
                                    new TgNode
                                    {
                                        Text = "112"
                                    }
                                }
                            },
                            new TgNode
                            {
                                Text = "120"
                            }
                        }
                    },
                    new TgNode
                    {
                        Text = "200",
                        Children =
                        {
                            new TgNode
                            {
                                Text = "210"
                            },
                            new TgNode
                            {
                                Text = "220"
                            }
                        }
                    },
                    new TgNode
                    {
                        Text = "100",
                        Children =
                        {
                            new TgNode
                            {
                                Text = "110",
                                Children =
                                {
                                    new TgNode
                                    {
                                        Text = "111"
                                    }
                                }
                            },
                            new TgNode
                            {
                                Text = "130"
                            }
                        }
                    }
                }
            };

            GrouppingNode(root);
        }

        private static void GrouppingNode(TgNode node)
        {
            for (var i = 0; i < node.Children.Count; i++)
            {
                for (var j = i + 1; j < node.Children.Count; j++)
                {
                    if (node.Children[j].Equals(node.Children[i]))
                    {
                        node.Children[i].Children.AddRange(node.Children[j].Children);
                        node.Children.RemoveAt(j);
                        j--;
                    }
                }
            }

            foreach(var item in node.Children)
                GrouppingNode(item);
        }

        private static void ToXml(TgNode node, XmlElement xmlElem)
        {
            xmlElem.SetAttribute("type", node.Key.ToString());
            xmlElem.SetAttribute("name", node.Text ?? "");

            foreach(var childNode in node.Children)
            {
                var childXmlElem = xmlElem.OwnerDocument.CreateElement("item");
                ToXml(childNode, childXmlElem);
                xmlElem.AppendChild(childXmlElem);
            }
        }
    }

    public class TgNode
    {
        public TgNode()
        {
            Children = new List<TgNode>();
        }

        public TaggedKey Key { get; set; }

        public string Text { get; set; }

        public List<TgNode> Children { get; private set; }

        public override int GetHashCode()
        {
            return Key.GetHashCode() + (Text ?? "").GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherNode = obj as TgNode;
            if (otherNode == null)
                return false;
            return Key == otherNode.Key && string.Equals(Text, otherNode.Text);
        }

        public override string ToString()
        {
            return $"[{Key}] [Childred: {Children.Count}] {Text ?? string.Empty}";
        }
    }
}
