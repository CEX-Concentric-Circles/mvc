using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Models;

[ApiController]
[Route("[controller]")]
public class ProductController : Controller
{
    private readonly WmsContext _context;

    public ProductController(WmsContext context)
    {
        _context = context;
    }

    // GET: Products
    [HttpGet("/product")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Products.ToListAsync());
    }

    // GET: Product/Details/5
    [HttpGet("/product/detail/{id}")]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var product = await _context.Products
            .FirstOrDefaultAsync(m => m.Id == id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    // POST: Product/Create
    [HttpPost("/product/create")]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }

    // PATCH: Product/Edit/5
    [HttpPatch("/product/edit/{id}")]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Quantity")] Product product)
    {
        if (id != product.Id)
        {
            return NotFound();
        }
    
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
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
        return View(product);
    }

    // DELETE: Product/Delete/5
    [HttpDelete("/product/delete/{id}"), ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}
