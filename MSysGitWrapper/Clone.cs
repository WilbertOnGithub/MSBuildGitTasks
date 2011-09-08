using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using Microsoft.Win32;

namespace MsysGit
{
    /// <summary>
    /// MSBuild task that mimics the 'git clone' functionality.
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
        /// Indicates whether all task parameters are valid.
        /// </summary>
        /// <returns>
        /// true if all task parameters are valid; otherwise, false.
        /// </returns>
        protected override bool ValidateParameters()
        {
            return base.ValidateParameters();
        }


        /// <summary>
        /// Returns the fully qualified path to the executable file.
        /// </summary>
        /// <returns>
        /// The fully qualified path to the executable file.
        /// </returns>
        protected override string GenerateFullPathToTool()
        {
            // Use the registry to search for the uninstall information of the msysgit installer.
            string regKey = @"software\microsoft\windows\currentversion\uninstall\git_is1";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
            {
                if (key != null)
                {
                    string version = key.GetValue ("DisplayName").ToString();
                    if (version != null)
                    {
                        Log.LogMessage(MessageImportance.Low, string.Format("Found an installed msysgit. Version: {0}", version));
                    }

                    string partialPath = key.GetValue("InstallLocation").ToString();
                    if (version == null)
                    {
                        Log.LogError(string.Format ("Cannot read the value 'InstallLocation' using the registry key: HKEY_LOCAL_MACHINE\\{0}", regKey));
                        return null;
                    }
                
                    return Path.Combine (partialPath, "bin", ToolName);
                }
                else
                {
                    Log.LogError(String.Format("Cannot locate the install location of msysgit using the registry key: HKEY_LOCAL_MACHINE\\{0}", regKey));
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the name of the executable file to run.
        /// </summary>
        /// <returns>The name of the executable file to run.</returns>
        protected override string ToolName
        {
            get { return "git.exe"; }
        }


        /// <summary>
        /// Returns a string value containing the command line arguments to pass directly to the executable file.
        /// </summary>
        /// <returns>
        /// A string value containing the command line arguments to pass directly to the executable file.
        /// </returns>
        protected override string GenerateCommandLineCommands()
        {
            //CommandLineBuilder builder = new CommandLineBuilder();

            //// We don't need the tool's logo information shown
            //builder.AppendSwitch("/nologo");

            //string targetType = Bag["TargetType"] as string;
            //// Be explicit with our switches
            //if (targetType != null)
            //{
            //    if (String.Compare(targetType, "DLL", true) == 0)
            //    {
            //        builder.AppendSwitch("/DLL");
            //    }
            //    else if (String.Compare(targetType, "EXE", true) == 0)
            //    {
            //        builder.AppendSwitch("/EXE");
            //    }
            //    else
            //    {
            //        Log.LogWarning("Invalid TargetType (valid values are DLL and EXE) specified: {0}", targetType);
            //    }
            //}

            //// Add the filename that we want the tool to process
            //builder.AppendFileNameIfNotNull(Bag["Source"] as ITaskItem);

            //// Log a High importance message stating the file that we are assembling
            //Log.LogMessage(MessageImportance.High, "Assembling {0}", Bag["Source"]);

            //// We have all of our switches added, return the commandline as a string
            //return builder.ToString();
            return string.Empty;
        }
    }
}
