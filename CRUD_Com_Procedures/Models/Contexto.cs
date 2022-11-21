using Microsoft.EntityFrameworkCore;
using CRUD_Com_Procedures.Models;

namespace CRUD_Com_Procedures.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        public DbSet<Aluno> Aluno { get; set; }
        public DbSet<Curso> Curso { get; set; }
        public DbSet<Matricula> Matricula { get; set; }
        public DbSet<CRUD_Com_Procedures.Models.Login> Login { get; set; }
    }
}
