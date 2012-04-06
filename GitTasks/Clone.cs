using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NGit;
using NGit.Api;
using NGit.Revwalk;
using Sharpen;

namespace GitTasks
{
	public class Clone : Task
	{
		private string _SHA;

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
			get { return _SHA; }
		}

		/// <summary>
		/// Gets or sets the branch to switch to.
		/// </summary>
		/// <value>
		/// The branch to switch to.
		/// </value>
		public string BranchToSwitchTo { get; set; }

		public override bool Execute()
		{
			try
			{
				Log.LogMessage(MessageImportance.Normal, string.Format("Cloning {0} to {1}", RepositoryToClone, TargetDirectory));

				Git clone = Git.CloneRepository().
					SetURI(RepositoryToClone).
					SetDirectory(new FilePath(TargetDirectory)).
					SetBare(false).
					SetCloneAllBranches(true).
					Call();
				
				if (!string.IsNullOrEmpty(BranchToSwitchTo) && BranchToSwitchTo.ToLower() != "master")
				{
					Log.LogMessage(MessageImportance.Normal, string.Format("Checking out branch/SHA '{0}'", BranchToSwitchTo));

					clone.Checkout().SetName(BranchToSwitchTo).Call();
				}

				ObjectId latestCommit = clone.Log().GetRepository().Resolve(Constants.HEAD);
				_SHA = latestCommit.Name;

				Log.LogMessage(MessageImportance.Normal, string.Format("Latest commit is '{0}'", latestCommit.Name));
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
