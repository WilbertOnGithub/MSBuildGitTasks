/*
 * Copyright 2011 Wilbert van Dolleweerd (wilbert@arentheym.com)
 * This file is part of the MSBuildGitTasks project
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
 * along with MSBuildGitTasks.  If not, see <http://www.gnu.org/licenses/>.
 */

namespace GitTasks
{
    /// <summary>
    /// Facade for NGit
    /// </summary>
    public interface IGitFacade
    {
        void Clone(string repositoryToClone, string targetDirectory);
        void CheckoutBranch(string localRepository, string branch);
        string GetLatestSha(string localRepository);
    }
}
