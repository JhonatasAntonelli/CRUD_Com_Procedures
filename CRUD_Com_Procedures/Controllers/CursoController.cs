using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CRUD_Com_Procedures.Models;


namespace CRUD_Com_Procedures.Controllers
{
    public class CursoController : Controller
    {
        private readonly Contexto _context;

        public CursoController(Contexto context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
              return View(_context.Curso.FromSqlRaw("EXEC DetalheCursoIndice").ToList());
        }
        public IActionResult Details(int id)
        {
            var curso = _context.Curso.FromSqlRaw("EXEC DetalheCurso " + id)
                .ToList().FirstOrDefault();
               
            return View(curso);
        }
                
        public IActionResult Create()
        {
            return View();
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Curso curso)
        {
            if (ModelState.IsValid)
            {
                var query = string.Format("EXEC AddCurso '{0}', {1}, '{2}'",
                    curso.NomeCurso, curso.Horas, curso.Descricao);

                _context.Curso.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoCadastro"] = "Cadastro de Curso realizado com sucesso!!";
                return RedirectToAction(nameof(Index));

            }
            return View(curso);
        }  
        public IActionResult Edit(int id)
        {
            return View( _context.Curso.FromSqlRaw("EXEC DetalheCurso " + id)
                .ToList().FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Curso curso)
        {
            if (ModelState.IsValid)
            {
                var query = string.Format("EXEC EditarCurso {0}, '{1}', {2}, '{3}'",
                    id, curso.NomeCurso, curso.Horas, curso.Descricao);

                _context.Curso.FromSqlRaw(query).ToListAsync();

                TempData["MessagemSucessoEdicao"] = "Curso Editado realizado com sucesso!!";

                return RedirectToAction(nameof(Index));

            }
            return View(curso);
        }
              
        public IActionResult Delete(int id)
        {
            return View(_context.Curso.FromSqlRaw("EXEC DetalheCurso " + id)
               .ToList().FirstOrDefault());
        }
               
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.Curso.FromSqlRaw("EXEC DeleteCurso " + id).ToListAsync();

            TempData["MessagemSucessoDelCurso"] = "Curso Removido com sucesso!!";

            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
