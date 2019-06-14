using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PDM;

namespace ARENA.Project
{
    public class PandaProject : ArenaProject
    {
        public PandaProject(Environment.Environment environment) : base(environment)
        {

           
        }

        public override ArenaProjectType ProjectType
        {
            get { return ArenaProjectType.PANDA; }
        }
    }
}
