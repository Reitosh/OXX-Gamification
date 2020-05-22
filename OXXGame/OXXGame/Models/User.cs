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
        public int userId { get; set; }
        public bool isAdmin { get; set; }
        public int loginCounter { get; set; }
        public byte[] _pwdHash;
        public byte[] pwdHash
        {
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

        // Metode som lager hash til lagring av brukerpassord
        public static byte[] createHash(string s)
        {
            var alg = System.Security.Cryptography.SHA256.Create();
            byte[] pwd = System.Text.Encoding.ASCII.GetBytes(s);
            return alg.ComputeHash(pwd);
        }

        [Required(ErrorMessage = "Vennligst skriv inn et passord")]
        [RegularExpression(@"^(?=.*[A-ZÆØÅÉ])(?=.*\d)[a-zA-ZæøöåäéÆØÖÅÄÉ .,:;!?@#$£&%*+-=~^<>(){}\d]{8,255}$",
            ErrorMessage = "Passordet må bestå av minst 8 tegn, en stor bokstav og ett tall")]
        public string password { get; set; }

        [Compare("password", ErrorMessage = "Stemmer ikke overens med angitt passord")]
        public string passwordRepeat { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn ditt fornavn")]
        [RegularExpression("^[a-zA-ZæøöåäéÆØÖÅÄÉ '-]{2,40}$",
            ErrorMessage = "Fornavnet må være mellom 2 og 40 tegn langt og kun inneholde bokstaver og mellomrom")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn ditt etternavn")]
        [RegularExpression("^[a-zA-ZæøöåäéÆØÖÅÄÉ '-]{2,40}$",
            ErrorMessage = "Etternavnet må være mellom 2 og 40 tegn langt og kun inneholde bokstaver og mellomrom")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn din e-postadresse")]
        [EmailAddress(ErrorMessage = "Ugyldig e-postadresse")]
        [StringLength(255, ErrorMessage = "E-postaddressen du har angitt er altfor lang")]
        public string email { get; set; }

        [Required(ErrorMessage = "Vennligst skriv inn ditt telefonnummer")]
        [RegularExpression(@"^((0047)?|(\+47)?)\d{8}$", ErrorMessage = "Ugyldig norsk telefonnummer")]
        public string tlf { get; set; }

        public List<CategoryLvl> categoryLvls { get; set; }

        // Klasse som knytter nivå til kategori ved brukerregistrering. Lagres omsider til ResultsPerCategory
        public class CategoryLvl
        {
            public bool lvl { get; set; }
            public string category { get; set; }
        }
    }
}
