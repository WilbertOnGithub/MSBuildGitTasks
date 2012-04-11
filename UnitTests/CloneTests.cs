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
using NUnit.Framework;
using Moq;

namespace GitTasks.Tests
{
    [TestFixture]
    public class CloneTests
    {
        /// <summary>
        /// Happy path. Execute should return true when no errors occur.
        /// </summary>
        [Test]
        public void ShouldReturnTrueWhenCalled()
        {
            // Arrange
            var msbuildCloneTask = new Clone(new Mock<IGitFacade>().Object)
            {
                // Mock away IBuildEngine cause we are not interested in the logging functionality.
                BuildEngine = new Mock<IBuildEngine>().Object,
                RepositoryToClone = string.Empty,
                TargetDirectory = string.Empty
            };

            // Act
            bool result = msbuildCloneTask.Execute();

            // Assert
            Assert.IsTrue(result);
        }


        /// <summary>
        /// Tests if Clone and GetLatestSHA are called.
        /// </summary>
        [Test]
        public void CloneRepository()
        {
            // Arrange
            var mock = new Mock<IGitFacade>();
            var msbuildCloneTask = new Clone(mock.Object) 
                                        {
                                            // Mock away IBuildEngine cause we are not interested in the logging functionality.
                                            BuildEngine = new Mock<IBuildEngine>().Object, 
                                            RepositoryToClone = string.Empty,
                                            TargetDirectory = string.Empty
                                        };

            // Act
            Assert.IsTrue(msbuildCloneTask.Execute());
            
            // Assert
            mock.Verify(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory), Times.Once());
            mock.Verify(git => git.GetLatestSha(msbuildCloneTask.TargetDirectory), Times.Once());
        }

        /// <summary>
        /// Tests if Clone, CheckoutBranch and GetLatestSHA is called.
        /// </summary>
        [Test]
        public void CloneRepositoryCheckoutBranch()
        {
            // Arrange
            var mock = new Mock<IGitFacade>();
            var msbuildCloneTask = new Clone(mock.Object)
                                        {
                                            // Mock away IBuildEngine cause we are not interested in the logging functionality.
                                            BuildEngine = new Mock<IBuildEngine>().Object,
                                            RepositoryToClone = string.Empty,
                                            TargetDirectory = string.Empty,
                                            BranchToSwitchTo = "somebranchtoswitchto"
                                        };

            // Act
            msbuildCloneTask.Execute();

            // Assert
            mock.Verify(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory), Times.Once());
            mock.Verify(git => git.CheckoutBranch(msbuildCloneTask.TargetDirectory, msbuildCloneTask.BranchToSwitchTo), Times.Once());
            mock.Verify(git => git.GetLatestSha(msbuildCloneTask.TargetDirectory), Times.Once());
        }

        /// <summary>
        /// Execute should return false when an exception occurs in the task.
        /// </summary>
        [Test]
        public void ShouldReturnFalseWhenExceptionOccurs()
        {
            // Arrange
            var gitMock = new Mock<IGitFacade>();
            var msbuildCloneTask = new Clone(gitMock.Object)
                                    {
                                        BuildEngine = new Mock<IBuildEngine>().Object,
                                        RepositoryToClone = string.Empty,
                                        TargetDirectory = string.Empty
                                    };

            gitMock.Setup(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory)).Throws<InvalidOperationException>();

            // Act 
            bool result = msbuildCloneTask.Execute();

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// MSBuild should log an error when an exception occurs in the task.
        /// </summary>
        [Test]
        public void ShouldLogErrorWhenExceptionErrors()
        {
            // Arrange
            var gitMock = new Mock<IGitFacade>();
            var buildengineMock = new Mock<IBuildEngine>();
            var msbuildCloneTask = new Clone(gitMock.Object)
            {
                BuildEngine = buildengineMock.Object,
                RepositoryToClone = string.Empty,
                TargetDirectory = string.Empty
            };
            gitMock.Setup(git => git.Clone(msbuildCloneTask.RepositoryToClone, msbuildCloneTask.TargetDirectory)).Throws<InvalidOperationException>();

            // Act 
            msbuildCloneTask.Execute();

            // Assert
            buildengineMock.Verify(msbuild => msbuild.LogErrorEvent(It.IsAny<BuildErrorEventArgs>()));			
        }
    }
}
