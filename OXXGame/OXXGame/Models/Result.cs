using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OXXGame.Models
{
    public class Result
    {
        public int userId { get; set; }
        public string timeUsed { get; set; }
        public int testsPassed { get; set; }
        public int testsFailed { get; set; }
        public int tests { get; set; }
    }
}