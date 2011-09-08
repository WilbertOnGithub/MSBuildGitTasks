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
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Win32;

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
