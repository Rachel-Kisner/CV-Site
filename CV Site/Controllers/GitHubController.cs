using DL.DTOs;
using Microsoft.AspNetCore.Mvc;
using Service;
using Services;
using System;
using System.Threading.Tasks;

namespace CV_Site.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet("repo")]
        public async Task<IActionResult> GetRepositoriesAsync()
        {
            try
            {
                var repositories = await _gitHubService.GetRepositoriesAsync();
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching repositories: {ex.Message}");
            }
        }

        [HttpGet("repo/{owner}/{repoName}")]
        public async Task<IActionResult> GetAsync(string owner, string repoName)
        {
            try
            {
                var repository = await _gitHubService.GetRepositoryAsync(owner, repoName);
                return Ok(repository);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching repository: {ex.Message}");
            }
        }

        [HttpGet("starred/{userName}")]
        public async Task<IActionResult> GetStarredRepositoriesAsync(string userName)
        {
            try
            {
                var starredRepositories = await _gitHubService.GetStarredRepositoriesAsync(userName);
                return Ok(starredRepositories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching starred repositories: {ex.Message}");
            }
        }

        [HttpGet("followers/{userName}")]
        public async Task<IActionResult> GetUserFollowersAsync(string userName)
        {
            try
            {
                var followers = await _gitHubService.GetUserFollowersAsync(userName);
                return Ok(followers);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching followers: {ex.Message}");
            }
        }

        [HttpGet("searchRepositoriesInCSharp")]
        public async Task<IActionResult> SearchRepositoriesInCSharpAsync([FromQuery] string repoName)
        {
            try
            {
                var repositories = await _gitHubService.SearchRepositoriesInCSharpAsync(repoName);
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error searching repositories: {ex.Message}");
            }
        }

        [HttpGet("portfolio")]
        public async Task<ActionResult<List<RepositoryDetailsDto>>> GetPortfolioAsync()
        {
            try
            {
                var portfolio = await _gitHubService.GetPortfolioAsync();
                return Ok(portfolio);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching portfolio: {ex.Message}");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<RepositorySearchDto>>> SearchRepositoriesAsync([FromQuery] string? repoName = null, [FromQuery] string? language = null, [FromQuery] string? userName = null)
        {
            try
            {
                var repositories = await _gitHubService.SearchRepositoriesAsync(repoName, language, userName);
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error searching repositories: {ex.Message}");
            }

        }
    }
}

