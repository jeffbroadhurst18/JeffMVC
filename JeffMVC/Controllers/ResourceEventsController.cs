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
	public class ResourceEventsController : Controller
	{
		private ResourceContext _resourceContext;

		public ResourceEventsController(ResourceContext resourceContext)
		{
			_resourceContext = resourceContext;

		}

		public IActionResult Index(int? monthid = null)
		{
			int selectedMonthId = monthid ?? DateTime.Now.Month;

			ViewBag.monthlist = GetMonths(selectedMonthId);

			var currentYear = DateTime.Now.Year;
			var lastDay = DateTime.DaysInMonth(currentYear, (int)selectedMonthId);
			var startDate = new DateTime(currentYear, (int)selectedMonthId, 1);
			var lastDate = new DateTime(currentYear, (int)selectedMonthId, lastDay);

			IEnumerable<ResourceEvent> resourceEvents = _resourceContext.ResourceEvents
				.Where(x => x.EventDate >= startDate && x.EventDate <= lastDate)
				.Include(h => h.Person).Include(z => z.Resource).OrderBy(d => d.EventDate);

			List<ResourceEvent> monthEvents = new List<ResourceEvent>();
			DateTime displayedDate = startDate;
			do
			{
				var dayEvents = resourceEvents.Where(d => d.EventDate == displayedDate);

				if (dayEvents.Count() > 0)
				{
					foreach (var dayEvent in dayEvents)
					{
						monthEvents.Add(dayEvent);
					}
				}
				else
				{
					monthEvents.Add(new ResourceEvent()
					{
						EventDate = displayedDate
					});
				}

				displayedDate = displayedDate.AddDays(1);
			} while (displayedDate <= lastDate);

			return View(monthEvents);
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

		private IEnumerable<SelectListItem> GetMonths(int id)
		{
			var dictionary = new Dictionary<int, string>();
			var monthIndex = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
			var monthName = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

			for (int i = 0; i < 12; i++)
			{
				dictionary.Add(monthIndex[i], monthName[i]);
			}

			var items = dictionary.ToSelectListItems(id);
			return items;
		}

		private bool ResourceEventExists(int id)
		{
			return _resourceContext.ResourceEvents.Any(e => e.Id == id);
		}
	}
}