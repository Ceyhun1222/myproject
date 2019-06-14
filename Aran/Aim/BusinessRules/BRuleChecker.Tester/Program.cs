using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BRuleCheckerTester
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateTester(TestType.RuleChecker1).Test();

            Console.WriteLine("Done");
            Console.ReadKey();
        }

        static ITester CreateTester(TestType type)
        {
            switch (type)
            {
                case TestType.RuleChecker2:
                    return new BRuleCheckerTesterV2();
                case TestType.RuleChecker3:
                    return new BRuleCheckerTesterV3();
                case TestType.PropGetter:
                    return new AimPropertyGetterTester();
                case TestType.SqlConverter:
                    return new BRuleSqlConverterTester();
                default:
                    return new BRuleCheckerTesterV1();
            }
        }

        private enum TestType
        {
            RuleChecker1,
            RuleChecker2,
            RuleChecker3,
            PropGetter,
            SqlConverter
        }
    }
}
