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

using NGit;
using NGit.Api;
using Sharpen;

namespace GitTasks
{
    /// <summary>
    /// The NGit implementation of the IGitFacade. Uses NGit to perform common Git tasks.
    /// </summary>
    public class NGit : IGitFacade
    {
        /// <summary>
        /// Clone a Git repository.
        /// </summary>
        /// <param name="repositoryToClone">The repository to clone.</param>
        /// <param name="targetDirectory">The target directory where you want to place the clone.</param>
        public void Clone(string repositoryToClone, string targetDirectory)
        {
            Git.CloneRepository().
                SetURI(repositoryToClone).
                SetDirectory(new FilePath(targetDirectory)).
                SetBare(false).
                SetCloneAllBranches(true).
                Call();
        }

        /// <summary>
        /// Checkout a branch or SHA.
        /// </summary>
        /// <param name="localRepository">The local Git repository.</param>
        /// <param name="branch">The branch or SHA you want to check out.</param>
        public void CheckoutBranch(string localRepository, string branch)
        {
            Git.Open(localRepository).Checkout().SetName(branch).Call();
        }

        /// <summary>
        /// Gets the latest SHA from a local Git repository.
        /// </summary>
        /// <param name="localRepository">The local Git repository.</param>
        /// <returns>The SHA of the latest commit</returns>
        public string GetLatestSha(string localRepository)
        {
            ObjectId latestCommit = Git.Open(localRepository).
                                        Log().
                                        GetRepository().
                                        Resolve(Constants.HEAD);

            return latestCommit.Name;
        }
    }
}
