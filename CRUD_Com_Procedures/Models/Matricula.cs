using DocumentFormat.OpenXml.Wordprocessing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CRUD_Com_Procedures.Models
{
    public class Matricula
    {
        [Display(Name = "Id")]
        [Column("Id")]
        public int? Id { get; set; }

       
        [Display(Name = "Nome do aluno")]
        [Column("Nome")]
        public string? Nome { get; set; }

        
        [Display(Name = "Data de Matricula")]
        [Column("DataMatricula")]
        public DateTime? DataMatricula { get; set; }

        
        [Display(Name = "Nome do Curso")]
        [Column("NomeCurso")]
        public string? NomeCurso { get; set; }

        [NotMapped]
        [Display(Name = "Id do aluno")]        
        public int? IdAluno { get; set; }

        [NotMapped]
        [Display(Name = "Selecione o nome do curso")]       
        public int? IdCurso { get; set; }

        [NotMapped]
        public List<SelectListItem>? ContatosSelectListNome { get; set; }

        [NotMapped]
        public List<SelectListItem>? ContatosSelectListCPF { get; set; }

        [NotMapped]
        public List<SelectListItem>? ContatosSelectListCurso { get; set; }
    }
}
