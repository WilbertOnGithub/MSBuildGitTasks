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
        /// <summary>
        /// Gets or sets the local repository on where to perform the checkout.
        /// </summary>
        /// <value>
        /// The local repository.
        /// </value>
        [Required]
        public string LocalRepository { get; set; }

        /// <summary>
        /// Gets or sets the SHA/branch to switch to.
        /// </summary>
        /// <value>
        /// The SHA/branch to switch to.
        /// </value>
        [Required]
        public string SwitchTo { get; set; }


        /// <summary>
        /// Returns the directory in which to run the executable file.
        /// </summary>
        /// <returns>
        /// The directory in which to run the executable file, or a null reference if the executable file should be run in the current directory.
        /// </returns>
        protected override string GetWorkingDirectory()
        {
            // Because the checkout has to work on the local repository, we have to change the working directory
            // to the directory where the local repository is stored.
            return LocalRepository;
        }

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
            builder.AppendSwitch("checkout");
            builder.AppendSwitch(SwitchTo);

            return builder.ToString();
        }
    }
}
