using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Cors;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberAlbumsController : ControllerBase
    {
        private readonly LocationContext _context;

        public MemberAlbumsController(LocationContext context)
        {
            _context = context;
        }

        // GET: api/MemberAlbums
        [HttpGet]
		[EnableCors("AnyGET")]
		public IEnumerable<MemberAlbum> GetMemberAlbums()
        {
			return _context.MemberAlbums.Include(a => a.Album).Include(b => b.Member);
        }

        // GET: api/MemberAlbums/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemberAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var memberAlbum = await _context.MemberAlbums.FindAsync(id);

            if (memberAlbum == null)
            {
                return NotFound();
            }

            return Ok(memberAlbum);
        }

        // PUT: api/MemberAlbums/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMemberAlbum([FromRoute] int id, [FromBody] MemberAlbum memberAlbum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != memberAlbum.Id)
            {
                return BadRequest();
            }

            _context.Entry(memberAlbum).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberAlbumExists(id))
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

        // POST: api/MemberAlbums
        [HttpPost]
        public async Task<IActionResult> PostMemberAlbum([FromBody] MemberAlbum memberAlbum)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.MemberAlbums.Add(memberAlbum);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMemberAlbum", new { id = memberAlbum.Id }, memberAlbum);
        }

        // DELETE: api/MemberAlbums/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemberAlbum([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var memberAlbum = await _context.MemberAlbums.FindAsync(id);
            if (memberAlbum == null)
            {
                return NotFound();
            }

            _context.MemberAlbums.Remove(memberAlbum);
            await _context.SaveChangesAsync();

            return Ok(memberAlbum);
        }

        private bool MemberAlbumExists(int id)
        {
            return _context.MemberAlbums.Any(e => e.Id == id);
        }
    }
}