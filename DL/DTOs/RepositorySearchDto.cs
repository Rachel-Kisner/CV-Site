using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DL.DTOs
{
    public class RepositorySearchDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string HtmlUrl { get; set; }
        public string Language { get; set; }
        public int Stars { get; set; }
    }
}
