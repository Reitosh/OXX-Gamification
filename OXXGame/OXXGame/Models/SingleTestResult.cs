using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OXXGame.Models
{
    public class SingleTestResult
    {
        public bool passed { get; set; }
        public int tries { get; set; }
        public string timeSpent { get; set; }
        public int userId { get; set; }
        public int testId { get; set; }
        public bool submitted { get; set; }
    }
}