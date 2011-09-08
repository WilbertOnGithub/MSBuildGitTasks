using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MsysGitWrapper
{
    /// <summary>
    /// 
    /// </summary>
    public class Checkout : ToolTask
    {
        protected override string GenerateFullPathToTool()
        {
            return string.Empty;
        }

        protected override string ToolName
        {
            get { return "git.exe"; }
        }

        protected override string GenerateCommandLineCommands()
        {
            return string.Empty;
        }
    }
}
