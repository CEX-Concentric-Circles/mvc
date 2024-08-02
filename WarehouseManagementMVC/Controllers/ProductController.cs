// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using WarehouseManagementMVC.Models;
//
// [ApiController]
// [Route("[controller]")]
// public class ProductController : Controller
// {
//     private readonly WmsContext _context;
//
//     public ProductController(WmsContext context)
//     {
//         _context = context;
//     }
//
//     // GET: Products
//     [HttpGet("/product")]
//     public async Task<IActionResult> Index()
//     {
//         return Ok(await _context.Products.ToListAsync());
//     }
//
//     // GET: Product/Details/5
//     [HttpGet("/product/detail/{id}")]
//     public async Task<IActionResult> Details(int? id)
//     {
//         if (id == null)
//         {
//             return NotFound();
//         }
//
//         var product = await _context.Products
//             .FirstOrDefaultAsync(m => m.Id == id);
//         if (product == null)
//         {
//             return NotFound();
//         }
//
//         return Ok(product);
//     }
//
//     // POST: Product/Create
//     [HttpPost("/product/create")]
//     public async Task<IActionResult> Create([Bind("Id,Name,Description,Quantity")] Product product)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Add(product);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//         return Ok(product);
//     }
//
//     // DELETE: Product/Delete/5
//     [HttpDelete("/product/delete/{id}"), ActionName("Delete")]
//     public async Task<IActionResult> DeleteConfirmed(int id)
//     {
//         var product = await _context.Products.FindAsync(id);
//         _context.Products.Remove(product);
//         await _context.SaveChangesAsync();
//         return Ok(nameof(Index));
//     }
// }
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseManagementMVC.Data;
using WarehouseManagementMVC.Models;

namespace WarehouseManagementMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly WmsContext _context;

        public ProductsController(WmsContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductDto product)
        {
            // Check if the id matches the product id
            if (id == null)
            {
                return BadRequest();
            }

            // Find the existing product
            var findProduct = await _context.Products.FindAsync(id);

            if (findProduct == null)
            {
                return NotFound();
            }

            // Update product properties with the new values
            findProduct.Name = product.Name;
            findProduct.Description = product.Description;

            _context.Entry(findProduct).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Add inventory entry for the new product if it does not exist
            var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.ProductId == product.Id);
            if (inventory == null)
            {
                _context.Inventories.Add(new Inventory
                {
                    ProductId = product.Id
                });
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Remove inventory entry for the deleted product
            var inventory = await _context.Inventories.SingleOrDefaultAsync(i => i.ProductId == id);
            if (inventory != null)
            {
                _context.Inventories.Remove(inventory);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
