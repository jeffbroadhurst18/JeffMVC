using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffShared;
using JeffShared.ViewModel;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JeffAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsCachedController : ControllerBase
    {
		private readonly LocationContext _context;

		public CommentsCachedController(LocationContext context)
		{
			_context = context;
		}

		// GET: api/Comments
		[HttpGet]
		[EnableCors("AnyGET")]
		public IEnumerable<Comment> GetComments()
		{
			return _context.Comments.OrderByDescending(d => d.DateRecorded);
		}

		// GET: api/Comments/5
		[HttpGet("Id")]
		[ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "id" })]
		public async Task<IActionResult> GetComment(int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comment = await _context.Comments.FindAsync(id);

			if (comment == null)
			{
				return NotFound();
			}

			return Ok(comment);
		}

		// PUT: api/Comments/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutComment([FromRoute] int id, [FromBody] Comment comment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != comment.Id)
			{
				return BadRequest();
			}

			_context.Entry(comment).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CommentExists(id))
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

		// POST: api/Comments
		[HttpPost]
		[EnableCors("AnyGET")]
		public async Task<IActionResult> PostComment([FromBody] Comment comment)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Comments.Add(comment);
			await _context.SaveChangesAsync();

			return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
		}

		// DELETE: api/Comments/5
		[HttpDelete("{id}")]
		[EnableCors("AnyGET")]
		public async Task<IActionResult> DeleteComment([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var comment = await _context.Comments.FindAsync(id);
			if (comment == null)
			{
				return NotFound();
			}

			_context.Comments.Remove(comment);
			await _context.SaveChangesAsync();

			return Ok(comment);
		}

		private bool CommentExists(int id)
		{
			return _context.Comments.Any(e => e.Id == id);
		}
	}
}