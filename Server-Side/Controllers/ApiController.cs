using Domain.Model;
using Domain.ReturnsModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server_Side.Middlewares;
using Server_Side.Services.Classes;
using Server_Side.Services.Interfaces;
using System;
using static System.Collections.Specialized.BitVector32;

namespace Server_Side.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IHandlerService _handler;
        private readonly IJwtService<JWTDataModel> _jwtService;

        public ApiController(IHandlerService handler, IJwtService<JWTDataModel> jwtService)
        {
            _handler = handler;
            _jwtService = jwtService;
        }
        [HttpPost("Test")]
        public IActionResult Login()
        {
            return Ok(new 
            {
                Comment = "ok",
                IsSucceeded = true,
            });
        }
    }
}
