using System;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Devices;
using RomEndprojectAwApiRikun.Models;
using Microsoft.Extensions.Configuration;

namespace RomEndprojectAwApiRikun.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RaspberryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public RaspberryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This Controller amd this post method in specific is for sending messages to
        /// start or stop capturing pictures based on the information from the extension
        /// </summary>
        /// <param name="value"></param>
        // POST api/<RaspberryController>
        [HttpPost]
        public async void Post([FromBody] Capture value)
        {
            string message = JsonConvert.SerializeObject(value);
            // CONNECTIONSTRING AND TARGETDEVICE CANNOT BE HARD CODED HERE!!!!!!!!
            string targetDevice = "RaspberryPi";                                        // The target device
            ServiceClient serviceClient;                                                // Use serviceclient class to establish connetion to azure device
            serviceClient = ServiceClient.CreateFromConnectionString(_configuration.GetConnectionString("IoTHub"));
            var commandMessage = new
             Message(Encoding.ASCII.GetBytes(message));
            await serviceClient.SendAsync(targetDevice, commandMessage);                // Send the message to device
        }



    }
}