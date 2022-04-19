using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductApp.Data;
using ProductApp.Models;

namespace ProductApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly DataContext _context;

        public HomeController(DataContext db)
        {
            _context = db;
        }

        [HttpGet("[action]")]
        public List<ProductDataModel> GetProduct()
        {
            return this._context.GetProduct();
        }

        [HttpPost("[action]")]
        public string UpdateProduct(ProductDataModel model)
        {
            return _context.UpdateProduct(model);
        }

        [HttpPost("[action]")]
        public string DeleteProduct(ProductDataModel model)
        {
            return _context.DeleteProduct(model);
        }
    }
}