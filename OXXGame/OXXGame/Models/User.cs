using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Web;

namespace OXXGame.Models
{

    public class User
    {
        public int userId { get; set; }

        [Required(ErrorMessage ="Vennligst skriv inn passord")]
        public string password { get; set; }
        public int loginCounter { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        [Required(ErrorMessage ="Vennligst skriv inn epostadresse")]
        public string email { get; set; }
        /*public Boolean isAdmin { get; set; }
        public Boolean knowHtml { get; set; }
        public Boolean knowCss { get; set; }
        public Boolean knowJavascript { get; set; }
        public Boolean knowCsharp { get; set; }
        public Boolean knowMvc { get; set; }
        public Boolean knowNetframework { get; set; }
        public Boolean knowTypescript { get; set; }
        public Boolean knowVue { get; set; }
        public Boolean knowReact { get; set; }
        public Boolean knowAngular { get; set; }*/
    }

}