using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffMVC.Extensions;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JeffMVC.Controllers
{
    public class ResourcesController : Controller
    {
		private ResourceContext _resourceContext;

		public ResourcesController(ResourceContext resourceContext)
		{
			_resourceContext = resourceContext;
			
		}

		public IActionResult Index()
        {
			 IEnumerable<ResourceEvent> resourceEvents =  _resourceContext.ResourceEvents.Include(h => h.Person).Include(z => z.Resource).OrderBy(d => d.EventDate);
			return View(resourceEvents);
		}

		// GET: Holidays/Create
		public IActionResult Create()
		{
			ViewBag.resourcelist = GetResources(0);
			ViewBag.personlist = GetPersons(0);
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("ResourceId,PersonId,EventDate")] ResourceEvent resourceEvent)
		{
			if (ModelState.IsValid)
			{
				_resourceContext.Add(resourceEvent);
				await _resourceContext.SaveChangesAsync();
				
				return RedirectToAction(nameof(Index));
			}
			return View(resourceEvent);
		}

		// GET: Holidays/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var resourceEvent = await _resourceContext.ResourceEvents.FindAsync(id);
			if (resourceEvent == null)
			{
				return NotFound();
			}

			ViewBag.resourcelist = GetResources(resourceEvent.ResourceId);
			ViewBag.personlist = GetPersons(resourceEvent.PersonId);
			
			return View(resourceEvent);
		}

		// POST: Holidays/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,ResourceId,PersonId,EventDate")] ResourceEvent resourceEvent)
		{
			if (id != resourceEvent.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_resourceContext.Update(resourceEvent);
					await _resourceContext.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ResourceEventExists(resourceEvent.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			return View(resourceEvent);
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var resourceEvent = await _resourceContext.ResourceEvents.Include(r => r.Resource).Include(p => p.Person)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (resourceEvent == null)
			{
				return NotFound();
			}

			return View(resourceEvent);
		}

		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var resourceEvent = await _resourceContext.ResourceEvents.Include(r => r.Resource).Include(p => p.Person)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (resourceEvent == null)
			{
				return NotFound();
			}

			return View(resourceEvent);
		}

		// POST: Holidays/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var resourceEvent = await _resourceContext.ResourceEvents.FindAsync(id);
			_resourceContext.ResourceEvents.Remove(resourceEvent);
			await _resourceContext.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private IEnumerable<SelectListItem> GetResources(int resourceId)
		{

			var resources = _resourceContext.Resources.OrderBy(o => o.ResourceTitle);

			var dictionary = new Dictionary<int, string>();

			foreach (var resource in resources)
			{
				dictionary.Add(resource.Id, resource.ResourceTitle);
			}

			var items = dictionary.ToSelectListItems(resourceId);
			return items;
		}

		private IEnumerable<SelectListItem> GetPersons(int personId)
		{

			var persons = _resourceContext.Persons.OrderBy(o => o.Name);

			var dictionary = new Dictionary<int, string>();

			foreach (var person in persons)
			{
				dictionary.Add(person.Id, person.Name);
			}

			var items = dictionary.ToSelectListItems(personId);
			return items;

		}

		private bool ResourceEventExists(int id)
		{
			return _resourceContext.ResourceEvents.Any(e => e.Id == id);
		}
	}
}