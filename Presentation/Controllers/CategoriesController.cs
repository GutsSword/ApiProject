using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly IServiceManager _services;

        public CategoriesController(IServiceManager services)
        {
            _services = services;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync(bool trackChanges)
        {
           return Ok (await _services.CategoryService.GetAllCategoriesAsync(false));
        }
        //[Authorize(Roles = "Admin,User,Editor")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllCategoriesAsync([FromRoute] int id)
        {
            return Ok(await _services.CategoryService.GetOneByIdCategoryAsync(id,false));
        }
    }
}
