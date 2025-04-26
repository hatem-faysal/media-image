using image.Data;
using image.Models;
using image.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace image.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataDbContext _context;
        private readonly IWebHostEnvironment _webHost;
        public ProductController(DataDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            var Products = _context.Products.ToList();
            return View(Products);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            await ImageServices.StoreImage(product, product.Image, _webHost, _context);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(x => x.Id == product.Id);
            existingProduct.Name = product.Name;
            if (product.Image != null)
            {
                await ImageServices.EditImage(existingProduct, product.Image, _webHost, _context);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
            await ImageServices.DeleteImage(product, _webHost, _context);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
