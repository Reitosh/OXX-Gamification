using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OXXGame.Models
{
    public class ResultPerCategory
    {
        public int userId { get; set; }
        public string category { get; set; }
        public int lvl { get; set; }
        public int counter { get; set; }
    }
}
