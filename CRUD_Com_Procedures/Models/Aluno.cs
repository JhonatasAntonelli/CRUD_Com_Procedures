using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Com_Procedures.Models
{
    public class Aluno
    {
        [Display(Name = "Id")]
        [Column ("Id")]
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Column("Nome")]
        public string Nome { get; set; }

        [Display(Name = "CPF")]
        [Column("Cpf")]
        public string Cpf { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Column("DataNascimento")]
        public DateTime DataNascimento { get; set; }

        [Display(Name = "Senha")]
        [Column("Senha")]
        public string Senha { get; set; }

        
        [Display(Name = "Administrador")]
        [Column("IsAdm")]
        public bool? IsAdm {get; set; }
    }
}
