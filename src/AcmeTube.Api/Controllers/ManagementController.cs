using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcmeTube.Api.Controllers
{
    [Route("management")]
    public class ManagementController : ControllerBase
    {
        /// <summary>
        /// Retrieve current version this application
        /// </summary>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("version")]
        public IActionResult Version()
        {
            return Ok(new
            {
                ApplicationVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
            });
        }
    }
}
