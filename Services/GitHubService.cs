


using DL.DTOs;
using Microsoft.Extensions.Options;
using Microsoft.Kiota.Abstractions.Extensions;
using Octokit;
using Service;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{


    public class GitHubService : IGitHubService
    {
        private readonly GitHubClient _client;
        private readonly GitHubIntegrationOptions _options;

        public GitHubService(IOptions<GitHubIntegrationOptions> options)
        {
            _options = options.Value;

            _client = new GitHubClient(new ProductHeaderValue("CVSiteApp"));

            if (!string.IsNullOrEmpty(_options.Token))
            {
                _client.Credentials = new Credentials(options.Value.Token);
            }


        }


        public async Task<IReadOnlyList<Repository>> GetRepositoriesAsync()
        {
            Console.WriteLine($"[INFO] התחלת שליפת ריפוזיטוריז עבור המשתמש: {_options.UserName}");
            try
            {
                var repos = await _client.Repository.GetAllForUser(_options.UserName);
                Console.WriteLine($"[INFO] נמצאו {repos.Count} ריפוזיטוריז עבור המשתמש: {_options.UserName}");
                return repos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] שגיאה בשליפת ריפוזיטוריז עבור המשתמש: {_options.UserName}. הודעת שגיאה: {ex.Message}");
                return Array.Empty<Repository>();
            }
        }

        public async Task<Repository> GetRepositoryAsync(string owner, string repoName)
        {
            return await _client.Repository.Get(owner, repoName);
        }

        public async Task<IReadOnlyList<Repository>> GetStarredRepositoriesAsync(string username)
        {
            return await _client.Activity.Starring.GetAllForUser(username);
        }

        public async Task<int> GetUserFollowersAsync(string userName)
        {
            var user = await _client.User.Get(userName);
            return user.Followers;
        }

        public async Task<List<Repository>> SearchRepositoriesInCSharpAsync(string repoName)
        {
            var searchRequest = new SearchRepositoriesRequest(repoName) { Language = Language.CSharp };
            var searchResult = await _client.Search.SearchRepo(searchRequest);
            return searchResult.Items.ToList();
        }

        public async Task<List<RepositoryDetailsDto>> GetPortfolioAsync()
        {
            var repositories = await _client.Repository.GetAllForUser(_options.UserName);
            var portfolio = new List<RepositoryDetailsDto>();
            foreach (var repo in repositories)
            {
                var languages = await _client.Repository.GetAllLanguages(repo.Id);
                var pulls = await _client.PullRequest.GetAllForRepository(repo.Id, new PullRequestRequest { State = ItemStateFilter.Open });
                var commits = await _client.Repository.Commit.GetAll(repo.Id);

                portfolio.Add(new RepositoryDetailsDto
                {
                    Name = repo.Name,
                    Description = repo.Description,
                    HtmlUrl = repo.HtmlUrl,
                    Languages = languages.Select(l => l.Name).ToList(),
                    Stars = repo.StargazersCount,
                    PullRequests = pulls.Count,
                    LastCommitDate = commits.FirstOrDefault()?.Commit.Committer.Date.DateTime
                });
            }
            return portfolio;
        }

        public async Task<List<RepositorySearchDto>> SearchRepositoriesAsync(string? repoName = null, string? language = null, string? userName = null)
        {
            var searchTerm = string.Empty;

            if (!string.IsNullOrWhiteSpace(repoName))
            {
                searchTerm += repoName + " ";
            }

            if (!string.IsNullOrWhiteSpace(language))
            {
                searchTerm += $"language:{language} ";
            }

            if (!string.IsNullOrWhiteSpace(userName))
            {
                searchTerm += $"user:{userName} ";
            }

            searchTerm = searchTerm.Trim();

            // דרישה של Octokit: term לא יכול להיות ריק
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = "stars:>1000"; // ברירת מחדל הגיונית
            }

            var request = new SearchRepositoriesRequest(searchTerm)
            {
                SortField = RepoSearchSort.Stars,
                Order = SortDirection.Descending
            };

            var result = await _client.Search.SearchRepo(request);

            return result.Items.Select(repo => new RepositorySearchDto
            {
                Name = repo.Name,
                Description = repo.Description,
                HtmlUrl = repo.HtmlUrl,
                Language = repo.Language,
                Stars = repo.StargazersCount
            }).ToList();
        }
    }

}

