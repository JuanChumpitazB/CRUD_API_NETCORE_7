using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Productos.Server.Models;

namespace Productos.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductosContext _productosContext;
        public ProductosController(ProductosContext productosContext)
        {
            _productosContext = productosContext;
        }

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> CrearProducto(Producto producto)
        {
            await _productosContext.Productos.AddAsync(producto);
            await _productosContext.SaveChangesAsync();

            return Ok();

        }

        [HttpGet]
        [Route("Lista")]
        public async Task<ActionResult<IEnumerable<Producto>>> ListaProductos()
        {
            var productos = await _productosContext.Productos.ToListAsync();
            return Ok(productos);
        }

        [HttpGet]
        [Route("Ver")]
        public async Task<IActionResult> VerProducto(int id)
        {
            var producto = await _productosContext.Productos.FindAsync(id);

            if(producto == null) return NotFound();

            return Ok(producto);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> ActualizarProducto(int id, Producto producto)
        {
            var objProducto = await _productosContext.Productos.FindAsync(id);
            
            if(objProducto == null) return NotFound(); 

            objProducto!.Nombre = producto.Nombre;
            objProducto!.Descripcion = producto.Descripcion;
            objProducto.Precio = producto.Precio;

            await _productosContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete]
        [Route("Eliminar")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var producto = await _productosContext.Productos.FindAsync(id);
            if(producto == null) return NotFound();

            _productosContext.Remove(producto);
            await _productosContext.SaveChangesAsync();

            return Ok();
        }
    }
}
