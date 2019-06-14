using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Omega.TypeBEsri
{
    public interface WhiteboxPlugin
    {
        /// <summary>
        /// Used to identify which toolboxes this plugin tool should be listed in. </summary>
        /// <returns> Array of Strings. </returns>
        string[] Toolbox { get; }

        /// <summary>
        /// Used to retrieve the plugin tool's name. This is a short, unique name containing no spaces. </summary>
        /// <returns> String containing plugin name. </returns>
        string Name { get; }

        /// <summary>
        /// Used to retrieve the plugin tool's descriptive name. This can be a longer name (containing spaces) and is used in the interface to list the tool. </summary>
        /// <returns> String containing the plugin descriptive name. </returns>
        string DescriptiveName { get; }

        /// <summary>
        /// Used to retrieve a short description of what the plugin tool does. </summary>
        /// <returns> String containing the plugin's description. </returns>
        string ToolDescription { get; }

        /// <summary>
        /// Used to run the plugin tool. </summary>
        /// <param name="args"> Array of Strings containing the parameters used to run the plugin tool. </param>
        void run();

        /// <summary>
        /// Sets the arguments (parameters) used by the plugin. </summary>
        /// <param name="args">  </param>
        string[] Args { set; }

    }
}
