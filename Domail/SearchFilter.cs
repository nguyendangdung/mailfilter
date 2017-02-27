using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain
{
    public class SearchFilter
    {
        public string FileNameOrId { get; set; }
        public string Content { get; set; }
        public string ViolatedContent { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public bool Violated { get; set; }
        public bool NotViolated { get; set; }
        public bool Error { get; set; }
    }
}
