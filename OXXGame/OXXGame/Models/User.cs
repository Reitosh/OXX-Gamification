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

        [StringLength(255)]
        public string firstname { get; set; }

        [StringLength(255)]
        public string lastname { get; set; }

        [EmailAddress]
        [StringLength(255)]
        [Required(ErrorMessage ="Vennligst skriv inn epostadresse")]
        public string email { get; set; }
        public bool isAdmin { get; set; }

        public List<CategoryLvl> categoryLvls { get; set; }

        private byte[] createHash(string s)
        {
            var alg = System.Security.Cryptography.SHA256.Create();
            byte[] pwd = System.Text.Encoding.ASCII.GetBytes(s);
            return alg.ComputeHash(pwd);
        }

        public class CategoryLvl
        {
            public bool lvl { get; set; }
            public string category { get; set; }
        }
    }
}