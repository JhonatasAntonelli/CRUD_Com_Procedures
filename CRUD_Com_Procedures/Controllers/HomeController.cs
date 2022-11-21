using CRUD_Com_Procedures.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


namespace CRUD_Com_Procedures.Controllers
{    
    public class HomeController : Controller
    {    
        ValidaCpf validaCPF = new();

        private readonly Contexto _context;

        public HomeController(Contexto context)
        {
            _context = context;
        }    
    
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PaginaAluno()
        {
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login login)
        {
            var query = string.Format("EXEC VerificaLogin '{0}', '{1}'", 
                login.LoginProvider, login.ProviderKey);

            var aluno = _context.Aluno.FromSqlRaw(query).ToList().FirstOrDefault();

            if (aluno != null)
            {
                var query_2 = string.Format("EXEC Usuario '{0}'", aluno.Cpf);
                _context.Aluno.FromSqlRaw(query_2).ToListAsync();

                if (aluno.IsAdm.Equals(true))
                {
                    return View("PaginaAdm");
                }
                else
                {
                    return View("PaginaAluno");
                }                
            }

            else
            {   
                TempData["MessagemErrorLogin"] = "Login ou senha incorreto";             
                return RedirectToAction(nameof(Index));
            }
        }        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Aluno aluno)
        {

            var query2 = string.Format("EXEC VerificaCPF '{0}'", aluno.Cpf);

            var aluno1 = _context.Aluno.FromSqlRaw(query2).ToList().FirstOrDefault();


            if (ModelState.IsValid & validaCPF.IsCpf(aluno.Cpf) & aluno1 == null)
            {
                var query = string.Format("EXEC AddAluno '{0}', '{1}', '{2}', '{3}', 0",
                    aluno.Nome, aluno.Cpf, aluno.DataNascimento, aluno.Senha);

                _context.Aluno.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoAddAluno"] = "Aluno adicionado com sucesso!! Faça o Login e aproveite seus estudos";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["MessagemErrorCPFAluno"] = "CPF invalido ou já cadastrado!!";
                return RedirectToAction(nameof(Create));
            }
        }
        public IActionResult Edit()
        {
            var usuario = _context.Aluno.FromSqlRaw("EXEC CpfUsuario ").ToList().FirstOrDefault();

            var aluno = _context.Aluno.FromSqlRaw("EXEC DetalheAluno {0}", usuario.Id)
                .ToList().FirstOrDefault();

            return View(aluno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Aluno aluno)
        {
            ValidaCpf validaCPF = new();

            if (ModelState.IsValid & validaCPF.IsCpf(aluno.Cpf))
            {
                var query = string.Format("EXEC EditarAluno {0}, '{1}', '{2}', '{3}', '{4}', 0",
                    id, aluno.Nome, aluno.Cpf, aluno.DataNascimento, aluno.Senha);

                _context.Aluno.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoEditALuno"] = "Informações editadas com sucesso!!";

                return RedirectToAction(nameof(PaginaAluno));

            }

            TempData["MessagemErrorEditAluno"] = "Não foi possível editar, veridfique as informações!!";
            return View(aluno);
        }

        public IActionResult Details()
        {
            var usuario = _context.Aluno.FromSqlRaw("EXEC CpfUsuario ").ToList().FirstOrDefault();

            var query = string.Format("EXEC ConsultaMatricula '{0}'", usuario.Cpf);

            var matriculas = _context.Matricula.FromSqlRaw(query).ToList();

            if (matriculas == null)
            {
                TempData["MessagemDetailsMatricula"] = "O aluno não está Matriculado em nenhum curso!!";
                return View();
            }
            else
            {
                return View(matriculas);
            }
            
        }
        public IActionResult Matricular()
        {
            {
                Matricula matriculas = new()
                {                    
                    ContatosSelectListCurso = new List<SelectListItem>()
                };
                
                foreach (var curso in _context.Curso)
                {
                    matriculas.ContatosSelectListCurso.Add(new SelectListItem
                    {
                        Text = curso.NomeCurso,
                        Value = curso.Id.ToString()
                    });
                }
                return View(matriculas);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Matricular(Matricula matricula)
        {
            var usuario = _context.Aluno.FromSqlRaw("EXEC CpfUsuario ").ToList().FirstOrDefault();

            var query2 = string.Format("EXEC VerificaMatricula '{0}', {1}", usuario.Cpf, matricula.IdCurso);

            var aluno1 = _context.Aluno.FromSqlRaw(query2).ToList().FirstOrDefault();


            if (ModelState.IsValid & aluno1 == null)
            {
                var query = string.Format("EXEC CriarMatriculaAluno '{0}', '{1}';",
                    usuario.Cpf, matricula.IdCurso);

                _context.Matricula.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoMatriculaAluno"] = "Matricula realizada com sucesso!!";

                return RedirectToAction(nameof(Details));
            }
            else
            {
                TempData["MessagemErrorCPFAluno"] = "Aluno já matriculado nesse curso cadastrado!!";
                return View("PaginaAluno");
            }
        }
        public IActionResult DeleteMatricula(int id)
        {
            var matricula = _context.Matricula.FromSqlRaw("EXEC DetalheMatricula " + id)
                 .ToList().FirstOrDefault();

            return View(matricula);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]       
        public IActionResult DeleteMatriculaConfirmed(int id)
        {   
            _context.Matricula.FromSqlRaw("EXEC DeleteMatricula {0}", id).ToListAsync();

            _context.SaveChangesAsync();

            TempData["MessagemSucessoDelMatriculaALuno"] = "Matricula Removida com sucesso!!";
            return RedirectToAction(nameof(Details));

        }
        public IActionResult DetailsAluno()
        {
            var usuario = _context.Aluno.FromSqlRaw("EXEC CpfUsuario ").ToList().FirstOrDefault();

            var aluno = _context.Aluno.FromSqlRaw("EXEC DetalheAluno " + usuario.Id).ToList().FirstOrDefault();

            return View(aluno);
        }
      
        [HttpPost, ActionName("DeleteAluno")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAlunoConfirmed(int id)
        {
           var matriculado = _context.Aluno.FromSqlRaw("EXEC VerificaMatriculaAluno {0}", id)
                .ToList().FirstOrDefault();

            if (matriculado == null)
            {
                _context.Matricula.FromSqlRaw("EXEC DeleteAluno {0}", id).ToListAsync();

                _context.SaveChangesAsync();

                TempData["MessagemSucessoDelAlunoAluno"] = "Aluno Removida com sucesso!!";
                return RedirectToAction(nameof(Index));
            }

            else
            {
                TempData["MessagemErrorDelAlunoAluno"] = "Aluno não pode ser removido, verifique se consta como matriculado!!";

                return View("PaginaAluno");
            }

        }
    }
}