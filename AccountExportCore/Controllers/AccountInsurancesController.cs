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
    [Route("api/AccountInsurances")]
    public class AccountInsurancesController : Controller
    {
        private readonly AccountExportContext _context;

        public AccountInsurancesController(AccountExportContext context)
        {
            _context = context;
        }

        // GET: api/AccountInsurances
        [HttpGet]
        public IEnumerable<AccountInsurance> GetAccountInsurance()
        {
            return _context.AccountInsurance;
        }

        // GET: api/AccountInsurances/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccountInsurance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountInsurance = await _context.AccountInsurance.SingleOrDefaultAsync(m => m.Id == id);

            if (accountInsurance == null)
            {
                return NotFound();
            }

            return Ok(accountInsurance);
        }

        // PUT: api/AccountInsurances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccountInsurance([FromRoute] int id, [FromBody] AccountInsurance accountInsurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accountInsurance.Id)
            {
                return BadRequest();
            }

            _context.Entry(accountInsurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountInsuranceExists(id))
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

        // POST: api/AccountInsurances
        [HttpPost]
        public async Task<IActionResult> PostAccountInsurance([FromBody] AccountInsurance accountInsurance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.AccountInsurance.Add(accountInsurance);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccountInsurance", new { id = accountInsurance.Id }, accountInsurance);
        }

        // DELETE: api/AccountInsurances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccountInsurance([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountInsurance = await _context.AccountInsurance.SingleOrDefaultAsync(m => m.Id == id);
            if (accountInsurance == null)
            {
                return NotFound();
            }

            _context.AccountInsurance.Remove(accountInsurance);
            await _context.SaveChangesAsync();

            return Ok(accountInsurance);
        }

        private bool AccountInsuranceExists(int id)
        {
            return _context.AccountInsurance.Any(e => e.Id == id);
        }
    }
}