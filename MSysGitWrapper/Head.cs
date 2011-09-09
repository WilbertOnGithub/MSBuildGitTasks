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
using System;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MsysgitWrapper
{
    /// <summary>
    /// MSBuild task that mimics the 'git rev-parse' functionality.
    /// Given a specific local repository, this task will return the SHA of the head. 
    /// This is necessary if you want to include in your build logfile against what SHA you have built.
    /// </summary>
    public class Head : ToolTask
    {
        private string _SHA;

        /// <summary>
        /// Gets or sets the local repository on where to get the latest SHA.
        /// </summary>
        /// <value>
        /// The local repository.
        /// </value>
        [Required]
        public string LocalRepository { get; set; }

        /// <summary>
        /// Gets the SHA of the latest commit in the given repository.
        /// </summary>
        [Output]
        public string SHA
        {
            get { return _SHA; }
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
        /// Returns the directory in which to run the executable file.
        /// </summary>
        /// <returns>
        /// The directory in which to run the executable file, or a null reference if the executable file should be run in the current directory.
        /// </returns>
        protected override string GetWorkingDirectory()
        {
            // Because the log command has to work on the local repository, we have to change the working directory
            // to the directory where the local repository is stored.
            return LocalRepository;
        }

        /// <summary>
        /// Parses a single line of text to identify any errors or warnings in canonical format.
        /// </summary>
        /// <param name="singleLine">A single line of text for the method to parse.</param>
        /// <param name="messageImportance">A value of <see cref="T:Microsoft.Build.Framework.MessageImportance"/> that indicates the importance level with which to log the message.</param>
        protected override void LogEventsFromTextOutput(string singleLine, MessageImportance messageImportance)
        {
            // Match any hexidecimal string that is 40 characters long, ie. a SHA.
            string pattern = "[0123456789aAbBcCdDeEfF]{40}";

            Match match = Regex.Match(singleLine, pattern);
            if (match.Success)
            {
                _SHA = match.Value;
            }
            else
            {
                Log.LogWarning(String.Format("Could not parse the SHA from the following output: {0}", singleLine));
            }

            base.LogEventsFromTextOutput(singleLine, messageImportance);
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
            builder.AppendSwitch("rev-parse");
            builder.AppendSwitch("HEAD");

            return builder.ToString();
        }
    }
}
