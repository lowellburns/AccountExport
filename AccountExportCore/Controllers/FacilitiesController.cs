using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AccountExportCore2.Models;

namespace AccountExportCore2.Controllers
{
    [Produces("application/json")]
    [Route("api/Facilities")]
    public class FacilitiesController : Controller
    {
        private readonly AccountExportContext _context;

        public FacilitiesController(AccountExportContext context)
        {
            _context = context;
        }

        // GET: api/Facilities
        [HttpGet]
        public IEnumerable<Facility> GetFacility()
        {
            return _context.Facility;
        }

        // GET: api/Facilities/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFacility([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facility = await _context.Facility.SingleOrDefaultAsync(m => m.Id == id);

            if (facility == null)
            {
                return NotFound();
            }

            return Ok(facility);
        }

        // PUT: api/Facilities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFacility([FromRoute] int id, [FromBody] Facility facility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != facility.Id)
            {
                return BadRequest();
            }

            _context.Entry(facility).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FacilityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Facilities
        [HttpPost]
        public async Task<IActionResult> PostFacility([FromBody] Facility facility)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Facility.Add(facility);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFacility", new { id = facility.Id }, facility);
        }

        // DELETE: api/Facilities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFacility([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var facility = await _context.Facility.SingleOrDefaultAsync(m => m.Id == id);
            if (facility == null)
            {
                return NotFound();
            }

            _context.Facility.Remove(facility);
            await _context.SaveChangesAsync();

            return Ok(facility);
        }

        private bool FacilityExists(int id)
        {
            return _context.Facility.Any(e => e.Id == id);
        }
    }
}