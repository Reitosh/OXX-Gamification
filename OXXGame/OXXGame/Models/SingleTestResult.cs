using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OXXGame.Models
{
    public class SingleTestResult
    {
        public const string PASSED = "Godkjent";
        public const string NOT_PASSED = "Ikke godkjent";
        public const string UNDEFINED = "Til godkjenning";

        public string passed { get; set; }
        public int tries { get; set; }
        public string timeSpent { get; set; }
        public int userId { get; set; }
        public int testId { get; set; }
        public string codeLink { get; set; }
    }
}