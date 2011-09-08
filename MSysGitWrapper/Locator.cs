using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Win32;
using Microsoft.Build.Utilities;

namespace MsysgitWrapper
{
    /// <summary>
    /// Helper class to locate the installed version of msysgit.
    /// </summary>
    internal class Locator
    {
        /// <summary>
        /// Returns the fully qualified path to the executable file.
        /// </summary>
        /// <param name="log"></param>
        /// <returns>
        /// The fully qualified path to the executable file.
        /// </returns>
        public static string GenerateFullPathToTool(TaskLoggingHelper log)
        {
            // Use the registry to search for the uninstall information of the msysgit installer.
            string regKey = @"software\microsoft\windows\currentversion\uninstall\git_is1";

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(regKey))
            {
                if (key != null)
                {
                    string version = key.GetValue("DisplayName").ToString();
                    if (version != null)
                    {
                        log.LogMessage(MessageImportance.Low, string.Format("Found an installed msysgit. Version: {0}", version));
                    }

                    string partialPath = key.GetValue("InstallLocation").ToString();
                    if (version == null)
                    {
                        log.LogError(string.Format("Cannot read the value 'InstallLocation' using the registry key: HKEY_LOCAL_MACHINE\\{0}", regKey));
                        return null;
                    }

                    return Path.Combine(partialPath, "bin", ToolName);
                }
                else
                {
                    log.LogError(String.Format("Cannot locate the install location of msysgit using the registry key: HKEY_LOCAL_MACHINE\\{0}", regKey));
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the name of the executable file to run.
        /// </summary>
        /// <returns>The name of the executable file to run.</returns>
        public static string ToolName
        {
            get { return "git.exe"; }
        }
    }
}
