using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Com_Procedures.Models
{
    public class Login
    {
        [Display(Name = "Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "CPF")]
        public string LoginProvider { get; set; }

        [Display(Name = "Senha")]
        public string ProviderKey { get; set; }
    }
}
