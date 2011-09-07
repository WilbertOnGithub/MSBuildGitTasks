using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;

namespace MsysGit
{
    /// <summary>
    /// Implement the 'git clone' task using msysgit
    /// </summary>
    public class Clone : ToolTask
    {
        protected override string GenerateFullPathToTool()
        {
            throw new NotImplementedException();
        }

        protected override string ToolName
        {
            get { throw new NotImplementedException(); }
        }

        protected override string GenerateCommandLineCommands()
        {
            //StringBuilder builder = new StringBuilder();
            //AppendIfPresent(builder, "--target", Target);
            //AppendIfPresent(builder, "--target-work-dir", WorkingDirectory);
            //AppendIfPresent(builder, "--target-args", QuoteIfNeeded(TargetArgs));
            //AppendIfPresent(builder, "--output", Output);

            //AppendMultipleItemsTo(builder, "--include", Include);
            //AppendMultipleItemsTo(builder, "--exclude", Exclude);

            //Log.LogCommandLine(builder.ToString());

            //return builder.ToString();
            return string.Empty;
        }
    }
}
