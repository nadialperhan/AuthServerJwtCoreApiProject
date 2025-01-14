﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiniApp1.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        [Authorize(Roles = "admin,admin2",Policy =("AmasyaPolicy"))]
        [Authorize(Policy ="AgePolicy")]
        [HttpGet]
        public IActionResult GetStock()
        {
            var username = HttpContext.User.Identity.Name;
            var useridClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            return Ok($"Stock->Username:{username},UserId:{useridClaim.Value}");
        }
    }
}
