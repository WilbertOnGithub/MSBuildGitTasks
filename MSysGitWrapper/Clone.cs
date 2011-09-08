using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace MsysGitWrapper
{
    /// <summary>
    /// MSBuild task that mimics the 'git clone' functionality.
    /// With this task, you can clone a repository and optionally switch to a specified branch.
    /// </summary>
    public class Clone : ToolTask
    {
        /// <summary>
        /// Gets or sets the branch to switch to.
        /// </summary>
        /// <value>
        /// The branch to switch to.
        /// </value>
        public string BranchToSwitchTo { get; set; }

        /// <summary>
        /// Gets or sets the repository to clone.
        /// </summary>
        /// <value>
        /// The repository to clone.
        /// </value>
        [Required]
        public string RepositoryToClone { get; set; }

        /// <summary>
        /// Gets or sets the target directory.
        /// </summary>
        /// <value>
        /// The target directory.
        /// </value>
        [Required]
        public string TargetDirectory { get; set; }

        /// <summary>
        /// Returns the fully qualified path to the executable file.
        /// </summary>
        /// <returns>
        /// The fully qualified path to the executable file.
        /// </returns>
        protected override string GenerateFullPathToTool()
        {
            return MsysgitWrapper.Locator.GenerateFullPathToTool(Log);
        }

        /// <summary>
        /// Gets the name of the executable file to run.
        /// </summary>
        /// <returns>The name of the executable file to run.</returns>
        protected override string ToolName
        {
            get { return MsysgitWrapper.Locator.ToolName; }
        }

        /// <summary>
        /// Returns a string value containing the command line arguments to pass directly to the executable file.
        /// </summary>
        /// <returns>
        /// A string value containing the command line arguments to pass directly to the executable file.
        /// </returns>
        protected override string GenerateCommandLineCommands()
        {
            CommandLineBuilder builder = new CommandLineBuilder();

            builder.AppendSwitch("clone");

            if (!string.IsNullOrEmpty(BranchToSwitchTo))
            {
                builder.AppendSwitch(string.Format("-b {0}", BranchToSwitchTo));
            }

            builder.AppendSwitch(RepositoryToClone);
            builder.AppendSwitch(TargetDirectory);

            return builder.ToString();
        }
    }
}
