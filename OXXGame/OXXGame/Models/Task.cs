using System;
using System.ComponentModel.DataAnnotations;

namespace OXXGame.Models
{
    public class Task
    {
        public int testId { get; set; }
        public string test { get; set; }
        public int difficulty { get; set; }
        public string category { get; set; }
        public string template { get; set; }
    }
}