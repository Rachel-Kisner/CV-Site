

namespace DL.DTOs
{
    public class RepositoryDetailsDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? HtmlUrl { get; set; }
        public IEnumerable<string>? Languages { get; set; }
        public int Stars { get; set; }
        public int PullRequests { get; set; }
        public DateTime? LastCommitDate { get; set; }
    }
}
