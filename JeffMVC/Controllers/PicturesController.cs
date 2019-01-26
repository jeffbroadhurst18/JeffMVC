using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace JeffMVC.Controllers
{
	public class PicturesController : Controller
	{
		public IActionResult NewYork()
		{
			return View(GetPictures(0));
		}

		public IActionResult Roma()
		{
			return View(GetPictures(1));
		}

		public IActionResult Budapest()
		{
			return View(GetPictures(2));
		}

		private Picture GetPictures(int id)
		{
			Picture pictures;
			string[] photos;

			switch (id)
			{
				case 0:
					photos = new string[] { "~/images/newyork/newyork1.jpg", "~/images/newyork/newyork2.jpg", "~/images/newyork/newyork3.jpg", "~/images/newyork/newyork4.jpg" };
					pictures = new Picture
					{
						Name = "New York",
						Year = 2018,
						Path = photos.ToList()
					};
					break;
				case 1:
					photos = new string[] { "~/images/roma/roma1.jpg", "~/images/roma/roma2.jpg", "~/images/roma/roma3.jpg", "~/images/roma/roma4.jpg" };
					pictures = new Picture
					{
						Name = "Rome",
						Year = 2018,
						Path = photos.ToList()
					};
					break;
				case 2:
					photos = new string[] { "~/images/budapest/budapest1.jpg", "~/images/budapest/budapest2.jpg", "~/images/budapest/budapest3.jpg", "~/images/budapest/budapest4.jpg" };
					pictures = new Picture
					{
						Name = "Budapest",
						Year = 2017,
						Path = photos.ToList()
					};
					break;
				default:
					photos = new string[] { "~/images/newyork/newyork1.jpg", "~/images/newyork/newyork2.jpg", "~/images/newyork/newyork3.jpg", "~/images/newyork/newyork4.jpg" };
					pictures = new Picture
					{
						Name = "New York",
						Year = 2018,
						Path = photos.ToList()
					};
					break;  
			}
			return getTwoPictures(pictures);
		}

		private Picture getTwoPictures(Picture pictures)
		{
			string temp;
			Random rnd = new Random();
			pictures.Path.RemoveAt(rnd.Next(0, 4));
			pictures.Path.RemoveAt(rnd.Next(0, 3));
			if (rnd.Next(0,2) == 0)
			{
				temp = pictures.Path[0];
				pictures.Path[0] = pictures.Path[1];
				pictures.Path[1] = temp;
			}
			return pictures;			
		}

	}

	internal class Array<T>
	{
	}
}