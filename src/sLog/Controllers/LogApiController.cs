using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sLog.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace sLog.Controllers
{
    /// <summary>
    ///     An API that supports adding log entries for a given registration.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    /// <devdoc>
    ///     //TODO add annotations to all method, including examples.
    ///     see https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1
    ///     &tabs=visual-studio
    /// </devdoc>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LogApiController : ControllerBase
    {
        private readonly sLogContext _context;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogApiController" /> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public LogApiController(sLogContext context)
        {
            _context = context;
        }

        // GET: api/LogApi
        [HttpGet]
        public IEnumerable<Log> GetLog()
        {
            return _context.Log;
        }

        // GET: api/LogApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLog([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var log = await _context.Log.FindAsync(id);

            if (log == null)
                return NotFound();

            return Ok(log);
        }

        // PUT: api/LogApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLog([FromRoute] Guid id, [FromBody] Log log)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != log.LogId)
                return BadRequest();

            _context.Entry(log).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LogExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        /// <summary>
        ///     Posts a new log entry.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///      POST /LogApi
        ///      {
        ///      "timestamp": "2018-11-19T08:00:21.726Z",
        ///      "data": "Some log data",
        ///      "mimeType": "text/plain",
        ///      "registrationId": 1,
        ///      }
        /// 
        /// </remarks>
        /// <param name="log">The new log entry.</param>
        /// <returns></returns>
        /// <devdoc>
        ///     POST: api/LogApi
        /// </devdoc>
        [HttpPost]
        [SwaggerResponse(201, type: typeof(Log), description: "Created")]
        public async Task<IActionResult> PostLog([FromBody] Log log)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.Log.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLog", new {id = log.LogId}, log);
        }

        // DELETE: api/LogApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var log = await _context.Log.FindAsync(id);
            if (log == null)
                return NotFound();

            _context.Log.Remove(log);
            await _context.SaveChangesAsync();

            return Ok(log);
        }

        private bool LogExists(Guid id)
        {
            return _context.Log.Any(e => e.LogId == id);
        }
    }
}