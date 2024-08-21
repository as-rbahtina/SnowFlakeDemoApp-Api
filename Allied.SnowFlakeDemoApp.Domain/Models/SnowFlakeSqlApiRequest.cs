using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Allied.SnowFlakeDemoApp.Domain.Models
{
    public class SnowFlakeSqlApiRequest
    {
        public string? statement { get; set; }
        public string? database { get; set; }
        public string? schema { get; set; }
        public string? warehouse { get; set; }
        public string? role { get; set; }

    }
}
