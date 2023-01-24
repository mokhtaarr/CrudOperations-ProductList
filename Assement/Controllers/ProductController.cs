using Assement.Dtos;
using Assement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NToastNotify;
using System.Xml.Linq;
using X.PagedList;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Assement.Controllers
{
    public class ProductController : Controller
    {
        private readonly DBContext _context;
        private readonly List<string> _allowedExtentations = new List<string> { ".jpg", ".png" };
        private long _maxAllowPosterSize = 1048576;
        private readonly IToastNotification _toastNotification;


        public ProductController(DBContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;

        }
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 3)
        {
            var prd = await _context.products.ToPagedListAsync(pageIndex, pageSize);
            return View(prd);
        }

        public async Task<IActionResult> Create()
        {
            var prd = new ProductDto
            {
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()
            };

            return View("ProductForm", prd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

                return View("ProductForm", model);
            }

            var files = Request.Form.Files;

            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("poster", "plese select product image");
                return View("ProductForm", model);

            }

            var poster = files.FirstOrDefault();

            if (!_allowedExtentations.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                ModelState.AddModelError("poster", "only jpg or png extention is allowed");
                return View("ProductForm", model);

            }


            if (poster.Length > _maxAllowPosterSize)
            {
                ModelState.AddModelError("poster", "poster can not be more than 1MB !");
                return View("ProductForm", model);

            }

            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);

            var prd = new Product
            { 
                Name = model.Name,
                GenreId= model.GenreId,
                Price = model.Price,
                Description = model.Description,
                poster = dataStream.ToArray()
            };

            _context.products.Add(prd);
            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Product Created Successfully");


            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return BadRequest($"We don`t Found your id : {id}");

            var prd = await _context.products.FindAsync(id);

            if (prd == null)
                return NotFound();

            var prdEdit = new ProductDto
            {
                Id = prd.Id,
                Name = prd.Name,
                Price = prd.Price,
                Description = prd.Description,
                poster = prd.poster,
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync()

            };

            return View("ProductForm", prdEdit);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductDto model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();

                return View("ProductForm", model);
            }

            var prd = await _context.products.FindAsync(model.Id);

            if (prd == null)
                return NotFound();

            var files = Request.Form.Files;

            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var dataStream = new MemoryStream();
                await poster.CopyToAsync(dataStream);

                model.poster = dataStream.ToArray();

                if (!_allowedExtentations.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    ModelState.AddModelError("poster", "only jpg or png extention is allowed");
                    return View("ProductForm", model);

                }

                if (poster.Length > _maxAllowPosterSize)
                {
                    ModelState.AddModelError("poster", "poster can not be more than 1MB !");
                    return View("ProductForm", model);

                }

                prd.poster = model.poster;
            }

            prd.Name = model.Name;
            prd.Price = model.Price;
            prd.Description = model.Description;
            prd.GenreId= model.GenreId;

            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Product Updated Successfully");

            return RedirectToAction(nameof(Index));


        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return BadRequest();

            var prd = await _context.products.Include(m => m.Genre).SingleOrDefaultAsync(m => m.Id == id);

            if (prd == null)
                return NotFound();

            return View(prd);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return BadRequest();

            var prd = await _context.products.FindAsync(id);

            if (prd == null)
                return NotFound();

            _context.products.Remove(prd);
            _context.SaveChanges();

            return Ok();
        }

    }

}
