using Microsoft.AspNetCore.Mvc;
using RomEndprojectAwApiRikun.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RomEndprojectAwApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowsingStartController : ControllerBase
    {
        private readonly romdatabaseawContext _dbContext;

        public BrowsingStartController(romdatabaseawContext context)
        {
            _dbContext = context;
        }

        // GET: api/<BrowserStartController>
        [HttpGet]
        public List<BrowsingStart> Get()
        {
            //LopputyöContext db = new LopputyöContext();
            return _dbContext.BrowsingStarts.ToList();
        }

        // POST api/<BrowserStartController>
        [HttpPost]
        public IActionResult Post([FromBody] BrowsingStart value)
        {
            try
            {
                //LopputyöContext db = new LopputyöContext();
                _dbContext.BrowsingStarts.Add(value);
                _dbContext.SaveChanges();
                return new OkObjectResult("Added successfully");
            }
            catch(Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }

        }
    }
}
