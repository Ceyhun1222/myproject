using Aran.Aim.BusinessRules.SbvrParser;
using Aran.Aim.BusinessRules.SbvrParser.Parser;
using BusinessRules.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SbvrParser.TesterConsole
{
    public class Parser2
    {
        public void Parse()
        {
            
            using (var ruleDb = new RulesDb())
            {
                ruleDb.Open();

                var ruleInfos = ruleDb.GetAllTaggedDescriptions(RuleFilterProfileType.Both);

                foreach (var ruleInfo in ruleInfos)
                {
                    var taggedDoc = new TaggedDocument();
                    taggedDoc.Init(ruleInfo.TaggedDescription);
                    taggedDoc.Next();

                    if (taggedDoc.Current.IsEqual(TaggedKey.Keyword, "It is prohibited that"))
                    {
                        var parser = new SbvrProhibitedParser();
                        parser.Parse(taggedDoc);
                    }
               } 
            }
        }
    }
}
