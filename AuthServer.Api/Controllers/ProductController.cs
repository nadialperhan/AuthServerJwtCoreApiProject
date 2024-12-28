using AuthServer.Core.DTOs;
using AuthServer.Core.Entity;
using AuthServer.Core.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : CustomBaseController
    {
        private readonly IServiceGeneric<Product,ProductDto> _serviceGeneric;

        public ProductController(IServiceGeneric<Product, ProductDto> serviceGeneric)
        {
            _serviceGeneric = serviceGeneric;
        }

        [HttpGet]

        public async Task<IActionResult> GetProduct()
        {
            return ActionResultInstance(await _serviceGeneric.GetAllAsync());
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _serviceGeneric.AddAsync(productDto));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto productDto)
        {
            return ActionResultInstance(await _serviceGeneric.Update(productDto,productDto.Id));
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            return ActionResultInstance(await _serviceGeneric.Remove(id));
        }
    }
}
