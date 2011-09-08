using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MsysGitWrapper
{
    /// <summary>
    /// Mimics the 'checkout' functionality of msysgit.
    /// Is used to switch to a specific branch or SHA on a local Git repository.
    /// </summary>
    public class Checkout : ToolTask
    {
        protected override string GenerateFullPathToTool()
        {
            return MsysgitWrapper.Locator.GenerateFullPathToTool(Log);
        }

        protected override string ToolName
        {
            get { return MsysgitWrapper.Locator.ToolName; }
        }

        protected override string GenerateCommandLineCommands()
        {
            return string.Empty;
        }
    }
}
