using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace CRUD_Com_Procedures.Models
{
    public class Curso
    {
        [Display(Name = "Id")]
        [Column("Id")]
        public int Id { get; set; }

        [Display(Name = "Nome do Curso")]
        [Column("NomeCurso")]
        public string? NomeCurso {get; set; } 

        [Display(Name = "Tempo do Curso")]
        [Column("Tempo")]
        public int? Horas { get; set; }

        [Display(Name = "Descrição do Curso")]
        [Column("Descricao")]
        public string? Descricao { get; set; }
    }
}
