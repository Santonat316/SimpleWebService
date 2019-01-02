using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Data;
using MyWebApplication.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace MyWebApplication.Controllers
{
    /// <summary>
    ///     Class ContentController.
    ///     Implements the <see cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiController]
    [Route("api")]
    public class ContentController : ControllerBase
    {
        [HttpGet]
        [Route("/content/{name}", Name = "GetUserByName")]
        [SwaggerOperation(OperationId = "GetUserByName")]
        [SwaggerResponse(200, Description = "Returns the user with the specified name")]
        [SwaggerResponse(404, Description = "User with specified name could not be found")]
        [Produces("text/plain")]
        public IActionResult GetUser(
            [FromRoute] string name)
        {
            EnsureArg.IsNotNullOrEmpty(name);

            if (!NameRepository.NameCollection.Contains(name)) return NotFound(name+" not found");


            return Ok(name);
        }

        [HttpPost]
        [Route("/add", Name = "AddUser")]
        [SwaggerOperation(OperationId = "AddUser")]
        [SwaggerResponse(201, Description = "Adds a user")]
        [SwaggerResponse(409, Description = "The user already exists")]
        public IActionResult AddUser([FromBody] NameApiModel nameToAdd)
        {
            EnsureArg.IsNotNull(nameToAdd);

            if (NameRepository.NameCollection.Contains(nameToAdd.Name)) return Conflict();

            NameRepository.NameCollection.Add(nameToAdd.Name);


            return CreatedAtRoute("GetUserByName", new {nameToAdd.Name}, nameToAdd.Name);
        }
    }
}