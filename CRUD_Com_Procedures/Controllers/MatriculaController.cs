using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CRUD_Com_Procedures.Models;


namespace CRUD_Com_Procedures.Controllers
{
    public class MatriculaController : Controller
    {
        private readonly Contexto _context;
        public MatriculaController(Contexto context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View (_context.Matricula.FromSqlRaw("EXEC DetalheMatriculaIndice ").ToList());
        }
        public IActionResult Details(int id)
        {            
            var matricula = _context.Matricula.FromSqlRaw("EXEC DetalheMatricula " + id).
                ToList().FirstOrDefault();

            return View(matricula);            
        }
        public IActionResult Create()
        {
            Matricula matriculas = new()
            {
                ContatosSelectListNome = new List<SelectListItem>(),
                ContatosSelectListCurso = new List<SelectListItem>()
            };

            foreach (var aluno in _context.Aluno)
            {      
                matriculas.ContatosSelectListNome.Add(new SelectListItem { 
                    Text = aluno.Nome, Value = aluno.Id.ToString()});
            }
            foreach (var curso in _context.Curso)
            {
                matriculas.ContatosSelectListCurso.Add(new SelectListItem { 
                    Text = curso.NomeCurso, Value = curso.Id.ToString()});
            }
            return View(matriculas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                var query = string.Format("EXEC CriarMatricula '{0}', '{1}';",
                    matricula.IdAluno, matricula.IdCurso);

                _context.Matricula.FromSqlRaw(query).ToListAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(matricula);
        }
        public IActionResult Edit()
        {
            Matricula matriculas = new()
            {
                ContatosSelectListNome = new List<SelectListItem>(),
                ContatosSelectListCurso = new List<SelectListItem>()
            };

            foreach (var aluno in _context.Aluno)
            {
                matriculas.ContatosSelectListNome.Add(new SelectListItem { 
                    Text = aluno.Nome, Value = aluno.Id.ToString() });
            }
            foreach (var curso in _context.Curso)
            {
                matriculas.ContatosSelectListCurso.Add(new SelectListItem { 
                    Text = curso.NomeCurso, Value = curso.Id.ToString() });
            }
            return View(matriculas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Matricula matricula)
        {
            if (ModelState.IsValid)
            {
                var query = string.Format("EXEC EditarMatricula {0}, {1}, {2};",
                    id, matricula.IdAluno, matricula.IdCurso);

                _context.Matricula.FromSqlRaw(query).ToListAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(matricula);
        }
        public IActionResult Delete(int id)
        {
            var matricula = _context.Matricula.FromSqlRaw("EXEC DetalheMatricula " + id).
                ToList().FirstOrDefault();

            return View(matricula);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.Matricula.FromSqlRaw("EXEC DeleteMatricula " + id).ToListAsync();
          
            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
