using DL.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Octokit;
using Service;

namespace CV_Site.CachServices
{
    public class CachedGitHubService : IGitHubService
    {
        private readonly IGitHubService _gitHubService;
        private readonly IMemoryCache _memoryCache;
        private const string userPortfolioKey = "userPortfolioKey";

        public CachedGitHubService(IGitHubService gitHubService,IMemoryCache  memoryCache)
        {
            _gitHubService = gitHubService;
            _memoryCache = memoryCache;
        }

        public async Task<List<RepositoryDetailsDto>> GetPortfolioAsync()
        {
            if (_memoryCache.TryGetValue(userPortfolioKey, out List<RepositoryDetailsDto> repositoryDetailsDto))
                return repositoryDetailsDto;
            var catchOption = new MemoryCacheEntryOptions().
                SetAbsoluteExpiration(TimeSpan.FromSeconds(30)).
                SetSlidingExpiration(TimeSpan.FromSeconds(10));
            
            repositoryDetailsDto = await _gitHubService.GetPortfolioAsync();
            _memoryCache.Set(userPortfolioKey, repositoryDetailsDto,catchOption);
            return repositoryDetailsDto;
        }

        public Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            return _gitHubService.GetRepositoriesAsync();
        }

        public Task<Repository> GetRepositoryAsync(string owner, string repoName)
        {
            return _gitHubService.GetRepositoryAsync(owner,repoName);
        }

        public Task<IReadOnlyList<Repository>> GetStarredRepositoriesAsync(string username)
        {
            return _gitHubService.GetStarredRepositoriesAsync(username);
        }

        public Task<int> GetUserFollowersAsync(string userName)
        {
            return _gitHubService.GetUserFollowersAsync(userName);
        }

        public Task<List<RepositorySearchDto>> SearchRepositoriesAsync(string? repoName = null, string? language = null, string? userName = null)
        {
            return _gitHubService.SearchRepositoriesAsync(repoName , language,  userName);
        }

        public Task<List<Repository>> SearchRepositoriesInCSharpAsync(string repoName)
        {
            return _gitHubService.SearchRepositoriesInCSharpAsync(repoName);
        }
    }
}
