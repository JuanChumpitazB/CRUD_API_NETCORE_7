using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Productos.Cliente.Models;
using System.Text;
using System.Text.Json.Serialization;

namespace Productos.Cliente.Controllers
{
    public class ProductoController : Controller
    {
        private readonly HttpClient _httpClient;
        public ProductoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7161/swagger/api");
        }
        public async Task<IActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/Productos/Lista");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var productos = JsonConvert.DeserializeObject<IEnumerable<ProductoViewModel>>(content);
                return View("Index", productos);
            }

            return View(new List<ProductoViewModel>());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductoViewModel producto)
        {
            if(ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/api/Productos/Crear", content);

                if(response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,"Error al crear el producto.");
                }
            }
            return View(producto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Productos/Ver?id={id}");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(content);

                return View(producto);
            }
            else
            {
                return RedirectToAction("Details");
            }

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductoViewModel producto)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(producto);
                var content = new StringContent(json,Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"/api/Productos/Editar?id={id}", content);

                if (response.IsSuccessStatusCode) 
                {
                    return RedirectToAction("Index", new {id});
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error al actualizar el producto.");
                }
            }
            return View(producto);
        }

        public async Task<IActionResult> Details(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Productos/Ver?id={id}");

            if(response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var producto = JsonConvert.DeserializeObject<ProductoViewModel>(content) ;
                
                return View(producto);
            }
            else
            {
                return RedirectToAction("Details");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Productos/Eliminar?id={id}");
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Error"] = "Error al eliminar producto.";
                return RedirectToAction("Index");
            }
        }
    }
}
