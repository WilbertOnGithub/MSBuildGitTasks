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

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace GitTasks
{
	public class Clone : Task
	{
		private readonly IGitFacade _gitFacade;
		private string _sha;

		/// <summary>
		/// Initializes a new instance of the <see cref="Clone"/> class.
		/// </summary>
		/// <param name="facade">IGitFacade interface</param>
		/// <remarks>Added to be able to unit test this class using a mock</remarks>
		public Clone (IGitFacade facade)
		{
			_gitFacade = facade;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Clone"/> class.
		/// </summary>
		/// <remarks>When no parameter is used in the constructor, instantiate the default</remarks>
		public Clone()
		{
			_gitFacade = new NGit();
		}

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
		/// Gets the SHA of the latest commit in the given repository.
		/// </summary>
		[Output]
		public string SHA
		{
			get { return _sha; }
		}

		/// <summary>
		/// Gets or sets the branch to switch to.
		/// </summary>
		/// <value>
		/// The branch to switch to.
		/// </value>
		public string BranchToSwitchTo { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>
		/// true if the task successfully executed; otherwise, false.
		/// </returns>
		public override bool Execute()
		{
			try
			{
				_gitFacade.Clone(RepositoryToClone, TargetDirectory);
				Log.LogMessage(MessageImportance.Normal, string.Format("Cloning {0} to {1}", RepositoryToClone, TargetDirectory));
				
				if (!string.IsNullOrEmpty(BranchToSwitchTo) && BranchToSwitchTo.ToLower() != "master")
				{
					_gitFacade.CheckoutBranch(TargetDirectory, BranchToSwitchTo);
					Log.LogMessage(MessageImportance.Normal, string.Format("Checking out branch/SHA '{0}'", BranchToSwitchTo));
				}

				_sha = _gitFacade.GetLatestSha(TargetDirectory);
				Log.LogMessage(MessageImportance.Normal, string.Format("Latest commit is '{0}'", _sha));
			}
			catch (Exception ex)
			{
				Log.LogErrorFromException(ex);
				return false;
			}

			return true;
		}

	}
}
