/*
 * Copyright 2011 Wilbert van Dolleweerd (wilbert@arentheym.com)
 * This file is part of the MsysgitWrapper project
 *
 * MsysgitWrapper is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * MsysgitWrapper is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with MsysgitWrapper.  If not, see <http://www.gnu.org/licenses/>.
 */
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

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
