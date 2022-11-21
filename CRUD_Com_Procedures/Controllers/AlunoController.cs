using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Com_Procedures.Models;

namespace CRUD_Com_Procedures.Controllers
{
    public class AlunoController : Controller
    {
        ValidaCpf validaCPF = new();

        private readonly Contexto _context;

        public AlunoController(Contexto context)
        {
            _context = context;
        }
                
        public IActionResult Index()
        {
            return View(_context.Aluno.FromSqlRaw("EXEC DetalheAlunoIndice").ToList());
        }
                
        public IActionResult Details(int id)
        {            
            var aluno =  _context.Aluno.FromSqlRaw("EXEC DetalheAluno " + id).ToList().FirstOrDefault();
            
            return View(aluno);
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
                var query = string.Format("EXEC AddAluno '{0}', '{1}', '{2}', '{3}', {4}",
                    aluno.Nome, aluno.Cpf, aluno.DataNascimento, aluno.Senha, aluno.IsAdm);

                _context.Aluno.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoAdd"] = "Aluno adicionado com sucesso!!";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["MessagemErrorCPF"] = "CPF invalido ou já cadastrado!!";
                return RedirectToAction(nameof(Create));
            }                
        }
        public IActionResult Edit(int id)
        {
            var aluno = _context.Aluno.FromSqlRaw("EXEC DetalheAluno " + id)
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
                var query = string.Format("EXEC EditarAluno {0}, '{1}', '{2}', '{3}', '{4}', {5}",
                    id, aluno.Nome, aluno.Cpf, aluno.DataNascimento, aluno.Senha, aluno.IsAdm);

                _context.Aluno.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoEdit"] = "Informações editadas com sucesso!!";

                return RedirectToAction(nameof(Index));

            }

            TempData["MessagemErrorEdit"] = "Não foi possível editar, veridfique as informações!!";
            return View(aluno);
        }
                
        public IActionResult Delete(int id)
        {
            var aluno = _context.Aluno.FromSqlRaw("EXEC DetalheAluno " + id)
                 .ToList().FirstOrDefault();

            return View(aluno);
        }
                
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {

            _context.Aluno.FromSqlRaw("EXEC DeleteAluno " + id).ToListAsync();

            _context.SaveChangesAsync();

            TempData["MessagemSucessoDel"] = "Cadastro de aluno removido com sucesso!!";
            return RedirectToAction(nameof(Index));
            
        }  
    }
}
