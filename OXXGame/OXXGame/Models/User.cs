using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 
using System.Linq;
using System.Web;
using System.Diagnostics;

namespace OXXGame.Models
{

    public class User
    {
        public byte[] _pwdHash;

        public int userId { get; set; }

        [Required(ErrorMessage ="Vennligst skriv inn passord")]
        public string password { get; set; }
        public byte[] pwdHash {
            get 
            {

                if (password == null)
                {
                    return _pwdHash;
                }
                else
                {
                    return createHash(password);
                }
            }

            set 
            {
                _pwdHash = value;
            } 
        }
        public int loginCounter { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

        [Required(ErrorMessage ="Vennligst skriv inn epostadresse")]
        public string email { get; set; }
        public bool isAdmin { get; set; }
        public bool knowHtml { get; set; }
        public bool knowCss { get; set; }
        public bool knowJavascript { get; set; }
        public bool knowCsharp { get; set; }
        public bool knowMvc { get; set; }
        public bool knowNetframework { get; set; }
        public bool knowTypescript { get; set; }
        public bool knowVue { get; set; }
        public bool knowReact { get; set; }
        public bool knowAngular { get; set; }

        private byte[] createHash(string s)
        {
            var alg = System.Security.Cryptography.SHA256.Create();
            byte[] pwd = System.Text.Encoding.ASCII.GetBytes(s);
            return alg.ComputeHash(pwd);
        }
    }
}