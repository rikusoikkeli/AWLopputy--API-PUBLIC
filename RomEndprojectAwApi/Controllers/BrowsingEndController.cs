using Microsoft.AspNetCore.Mvc;
using RomEndprojectAwApiRikun.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RomEndprojectAwApiApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrowsingEndController : ControllerBase
    {
        private readonly romdatabaseawContext _dbContext;

        public BrowsingEndController(romdatabaseawContext context)
        {
            _dbContext = context;
        }

        // GET: api/<BrowsingEndController>
        [HttpGet]
        public List<BrowsingEnd> Get()
        {
            //LopputyöContext db = new LopputyöContext();
            return _dbContext.BrowsingEnds.ToList();
        }

        // GET api/<BrowsingEndController>/5
        [HttpGet("{id}")]
        public BrowsingEnd Get(string id)
        {
                //LopputyöContext db = new();
                return _dbContext.BrowsingEnds.Where(b => b.SessionId == id).First();
        }

        // POST api/<BrowsingEndController>
        [HttpPost]
        public IActionResult Post([FromBody] BrowsingEnd value)
        {
            try
            {
                //LopputyöContext db = new LopputyöContext();
                _dbContext.BrowsingEnds.Add(value);
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
