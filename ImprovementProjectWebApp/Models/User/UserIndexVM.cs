using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImprovementProjectWebApp.Models.User
{
    public class UserIndexVM
    {
        public List<ApplicationUser> applicationUsers { get; set; }
        public int totalPage { get; set; }
        public int curPage { get; set; }
        public bool ShowDelete { get; set; }
        public int FilterStatus { get; set; }
    }
}
