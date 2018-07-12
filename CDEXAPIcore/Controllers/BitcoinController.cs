using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using CDEXAPIcore.lib;

namespace CDEXAPIcore.Controllers
{

    [Authorize(AuthenticationSchemes = "Identity.Application," + JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class BitcoinController : Controller
    {
        private readonly IBitcoinService _bitcoinservice;

        //refactor controller for service dependancy 
        //scope will pass in a resolved IBitcoinService specified in startup
        public BitcoinController(IBitcoinService bitcoinservice)
        {
            _bitcoinservice = bitcoinservice;
        }

        [HttpGet("List")]
        public IActionResult GetList(int id)
        {
            return Ok( new {

                list = _bitcoinservice.GetBitCoin()
            });
        }
    }
}