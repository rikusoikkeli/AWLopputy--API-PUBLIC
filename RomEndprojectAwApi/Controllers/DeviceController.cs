using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RomEndprojectAwApiRikun.Models;
using System;
using System.Linq;
using static RomEndprojectAwApiRikun.HashUtil;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RomEndprojectAwApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly romdatabaseawContext _dbContext;
        private readonly IConfiguration _configuration;
        static string secret;
        public DeviceController(romdatabaseawContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = context;
            secret = _configuration.GetSection("Secret").Value;
        }

        /// <summary>
        /// Jos tietokannasta löytyy laite DeviceID, jonka salasana on Password, palauttaa true.
        /// Muussa tapauksessa palauttaa false.
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Get(string DeviceID, string Password)
        {
            var passHash = Hash(Password, secret);
            var deviceExists = _dbContext.Devices.Where(d => d.DeviceId == DeviceID && d.Password == passHash).FirstOrDefault();
            if (deviceExists != null)
            {
                return new OkObjectResult("true");
            }
            return new OkObjectResult("false");
        }

        // POST api/<DeviceController>
        [HttpPost]
        public IActionResult Post([FromBody] Device value)
        {
            try
            {
                value.Password = Hash(value.Password, secret);
                _dbContext.Devices.Add(value);
                _dbContext.SaveChanges();
                return new OkObjectResult("true");
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult("false");
            }
        }

        // DELETE api/<DeviceController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(string id, string password)
        {
            try
            {
                var passHash = Hash(password, secret);
                var device = _dbContext.Devices.Where(d => d.DeviceId == id && d.Password == passHash).FirstOrDefault();
                _dbContext.Devices.Remove(device);
                _dbContext.SaveChanges();
                return new OkObjectResult("true");
            }
            catch(Exception e)
            {
                return new OkObjectResult("false");
            }
        }
    }
}

