using System;

namespace OXXGame.Models
{
    public class TestModel
    {
        public SingleTestResult singleTestResult { get; set; }
        public Models.Task task { get; set; }
        public string code { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
