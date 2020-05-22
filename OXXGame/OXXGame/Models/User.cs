using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace OXXGame.Models
{

    public class User
    {
        public byte[] _pwdHash;

        public int userId { get; set; }
        public string tlf { get; set; }

        [Required(ErrorMessage ="Vennligst skriv inn passord")]
        public string password { get; set; }
        public string passwordRepeat { get; set; }
        public byte[] pwdHash {
            get 
            {
                if (password == null) // bruker er hentet fra databasen, og objektet inneholder kun hash
                {
                    return _pwdHash;
                }
                else
                {
                    return createHash(password); // bruker er opprettet fra input, og mangler hash
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

        [EmailAddress(ErrorMessage = "Ikke gyldig")]
        [StringLength(255)]
        [Required(ErrorMessage ="Vennligst skriv inn epostadresse")]
        public string email { get; set; }
        public bool isAdmin { get; set; }

        public List<CategoryLvl> categoryLvls { get; set; }

        // Metode som lager hash til lagring av brukerpassord
        public static byte[] createHash(string s)
        {
            var alg = System.Security.Cryptography.SHA256.Create();
            byte[] pwd = System.Text.Encoding.ASCII.GetBytes(s);
            return alg.ComputeHash(pwd);
        }

        // Klasse som knytter nivå til kategori ved brukerregistrering. Lagres omsider til ResultsPerCategory
        public class CategoryLvl
        {
            public bool lvl { get; set; }
            public string category { get; set; }
        }
    }
}