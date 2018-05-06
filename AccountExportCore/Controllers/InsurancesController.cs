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
    [Route("api/Insurances")]
    public class InsurancesController : Controller
    {
        private readonly AccountExportContext _context;

        public InsurancesController(AccountExportContext context)
        {
            _context = context;
        }

        // GET: api/Insurances
        [HttpGet]
        public IEnumerable<Insurance> GetInsurance()
        {
            return _context.Insurance;
        }

        // GET: api/Insurances/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInsurance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var insurance = await _context.Insurance.SingleOrDefaultAsync(m => m.Id == id);

            if (insurance == null)
            {
                return NotFound();
            }

            return Ok(insurance);
        }

        // PUT: api/Insurances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurance([FromRoute] int id, [FromBody] Insurance insurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != insurance.Id)
            {
                return BadRequest();
            }

            _context.Entry(insurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
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

        // POST: api/Insurances
        [HttpPost]
        public async Task<IActionResult> PostInsurance([FromBody] Insurance insurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Insurance.Add(insurance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInsurance", new { id = insurance.Id }, insurance);
        }

        // DELETE: api/Insurances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var insurance = await _context.Insurance.SingleOrDefaultAsync(m => m.Id == id);
            if (insurance == null)
            {
                return NotFound();
            }

            _context.Insurance.Remove(insurance);
            await _context.SaveChangesAsync();

            return Ok(insurance);
        }

        private bool InsuranceExists(int id)
        {
            return _context.Insurance.Any(e => e.Id == id);
        }
    }
}