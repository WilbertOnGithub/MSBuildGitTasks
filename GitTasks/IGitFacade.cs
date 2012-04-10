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
