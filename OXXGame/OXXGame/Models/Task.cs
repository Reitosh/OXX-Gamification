using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OXXGame.Models
{

    public class Task
    {
        public int testId { get; set; }

        public string test { get; set; }

        public string difficulty { get; set; }

        public string category { get; set; }
    }

}