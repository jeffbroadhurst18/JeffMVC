using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JeffMVC.Extensions
{
	public static class SelectListItemsExtensions
	{
		//public static IEnumerable<SelectListItem> ToSelectListItems(
		//	this IDictionary<string, string> dict, string selectedId) =>
		//	dict.Select(item => new SelectListItem
		//	{
		//		Selected = item.Key == selectedId,
		//		Text = item.Value,
		//		Value = item.Key.ToString()
		//	});

		public static IEnumerable<SelectListItem> ToSelectListItems(
			this IDictionary<string, string> dict, string selectedId)
		{
			List<SelectListItem> selectListItems = new List<SelectListItem>(); //Durham
			foreach (var item in dict)
			{
				selectListItems.Add(new SelectListItem
				{
					Selected = item.Key == selectedId,
					Text = item.Value,
					Value = item.Key
				});
			}
			return selectListItems;
		}
	}
}
