using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TesteRecuperacao.Data;
using TesteRecuperacao.Models;

namespace TesteRecuperacao.Controllers
{
    public class ToolsController : Controller
    {
        private readonly TesteRecuperacaoContext _context;

        public ToolsController(TesteRecuperacaoContext context)
        {
            _context = context;
        }

        // GET: Tools
        // GET: Tools
    public async Task<IActionResult> Index(string toolCategory, string searchString)
    {
        try 
        {
            if (_context.Tools == null)
            {
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            // Consulta LINQ para obter lista de categorias.
            IQueryable<string> categoryQuery = from t in _context.Tools
                                            orderby t.Category
                                            select t.Category;

            var tools = from t in _context.Tools
                        select t;

            // Filtrar por nome, se a pesquisa for fornecida.
            if (!string.IsNullOrEmpty(searchString))
            {
                tools = tools.Where(t => t.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            // Filtrar por categoria, se selecionada.
            if (!string.IsNullOrEmpty(toolCategory))
            {
                tools = tools.Where(t => t.Category == toolCategory);
            }

            // Criar ViewModel para passar os dados à view.
            var toolCategoryVM = new ToolCategoryViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Tools = await tools.ToListAsync()
            };

            return View(toolCategoryVM);
        }
        catch (Exception)
        {
            // Retornar a view de erro, passando uma mensagem de erro, caso o banco de dados falhe
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }



        // GET: Tools/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
        
            var model = await _context.Tools
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        // GET: Tools/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tools/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Quantity,Category")] Tools tools)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tools);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tools);
        }

        // GET: Tools/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tools = await _context.Tools.FindAsync(id);
            if (tools == null)
            {
                return NotFound();
            }
            return View(tools);
        }

        // POST: Tools/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Quantity,Category")] Tools tools)
        {
            if (id != tools.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tools);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ToolsExists(tools.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tools);
        }

        // GET: Tools/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tools = await _context.Tools
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tools == null)
            {
                return NotFound();
            }

            return View(tools);
        }

        // POST: Tools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tools = await _context.Tools.FindAsync(id);
            if (tools != null)
            {
                _context.Tools.Remove(tools);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ToolsExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
}
