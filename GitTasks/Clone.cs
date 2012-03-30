using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using NGit;
using NGit.Api;
using Sharpen;

namespace GitTasks
{
	public class Clone : Task
	{
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

		public override bool Execute()
		{
			try
			{
				Git repo = Git.CloneRepository().
					SetURI(RepositoryToClone).
					SetDirectory(new FilePath(TargetDirectory)).
					SetBare(false).
					SetCloneAllBranches(true).
					Call();
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
