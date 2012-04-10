using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using NUnit.Framework;
using Moq;
using GitTasks;

namespace GitTasks.Tests
{
	[TestFixture]
	public class CloneTests
	{
		/// <summary>
		/// Tests if Clone and GetLatestSHA are called.
		/// </summary>
		[Test]
		public void CloneRepository()
		{
			var mock = new Mock<IGitFacade>();
			var msbuildCloneTask = new Clone(mock.Object) 
										{
											// Mock away IBuildEngine cause we are not interested in the logging functionality.
											BuildEngine = new Mock<IBuildEngine>().Object, 
											RepositoryToClone = "", 
											TargetDirectory = ""
										};

			msbuildCloneTask.Execute();
			mock.Verify(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory), Times.Once());
			mock.Verify(git => git.GetLatestSHA(msbuildCloneTask.TargetDirectory), Times.Once());
		}

		/// <summary>
		/// Tests if Clone, CheckoutBranch and GetLatestSHA is called.
		/// </summary>
		[Test]
		public void CloneRepositoryCheckoutBranch()
		{
			var mock = new Mock<IGitFacade>();
			var msbuildCloneTask = new Clone(mock.Object)
										{
											// Mock away IBuildEngine cause we are not interested in the logging functionality.
											BuildEngine = new Mock<IBuildEngine>().Object,
											RepositoryToClone = string.Empty,
											TargetDirectory = string.Empty,
											BranchToSwitchTo = "somebranchtoswitchto"
										};

			msbuildCloneTask.Execute();
			mock.Verify(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory), Times.Once());
			mock.Verify(git => git.CheckoutBranch(msbuildCloneTask.TargetDirectory, msbuildCloneTask.BranchToSwitchTo), Times.Once());
			mock.Verify(git => git.GetLatestSHA(msbuildCloneTask.TargetDirectory), Times.Once());
		}
	}
}
