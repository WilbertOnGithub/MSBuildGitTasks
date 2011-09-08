/*
 * Copyright 2011 Wilbert van Dolleweerd
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
