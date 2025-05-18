using DL.DTOs;
using Octokit;

namespace Service
{
    public interface IGitHubService
    {
       

        Task<IReadOnlyList<Repository>> GetRepositoriesAsync();
        Task<Repository> GetRepositoryAsync(string owner, string repoName);
        Task<IReadOnlyList<Repository>> GetStarredRepositoriesAsync(string username);
        Task<int> GetUserFollowersAsync(string userName);
        Task<List<Repository>> SearchRepositoriesInCSharpAsync(string repoName);
        Task<List<RepositoryDetailsDto>> GetPortfolioAsync();
        Task<List<RepositorySearchDto>> SearchRepositoriesAsync(string? repoName = null, string? language = null, string? userName = null);
    }
}
