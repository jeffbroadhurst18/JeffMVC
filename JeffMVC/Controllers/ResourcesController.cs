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
			ViewBag.resourcelist = GetResources();
			ViewBag.personlist = GetPersons();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Resource,Person,EventDate")] ResourceEvent resourceEvent)
		{
			if (ModelState.IsValid)
			{
				_resourceContext.Add(resourceEvent);
				await _resourceContext.SaveChangesAsync();
				
				return RedirectToAction(nameof(Index));
			}
			return View(resourceEvent);
		}

		private IEnumerable<SelectListItem> GetResources()
		{

			var resources = _resourceContext.Resources.OrderBy(o => o.ResourceTitle);

			var dictionary = new Dictionary<int, string>();

			foreach (var resource in resources)
			{
				dictionary.Add(resource.Id, resource.ResourceTitle);
			}

			return dictionary.ToSelectListItems(0);

		}

		private IEnumerable<SelectListItem> GetPersons()
		{

			var persons = _resourceContext.Persons.OrderBy(o => o.Name);

			var dictionary = new Dictionary<int, string>();

			foreach (var person in persons)
			{
				dictionary.Add(person.Id, person.Name);
			}

			return dictionary.ToSelectListItems(0);

		}
	}
}