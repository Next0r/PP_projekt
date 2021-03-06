﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BedAndBreakfast.Data;
using BedAndBreakfast.Models;
using BedAndBreakfast.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BedAndBreakfast.Controllers
{
	/// <summary>
	/// This class should be used to call methods related to help pages like searching and displaying help page.
	/// </summary>
    public class HelpController : Controller
    {
		private AppDbContext context;

        public HelpController(AppDbContext context) {
			this.context = context;
		}

		/// <summary>
		/// Displays search page. And passes default number of first help pages in database
        /// or less if there is not enough of them. If administrator uses this action
        /// he will also see locked pages.
		/// </summary>
		/// <returns></returns>
		public IActionResult Browse(string message) {
            var helpPages = SearchEngine.FindTopPages(context);
            ViewData["helpPages"] = helpPages;

            // Display message received message
            ViewBag.Message = message;
            return View();
		}

        /// <summary>
        /// Performs search for help pages in database. If administrator 
        /// uses this action he will also see locked pages.
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [HttpPost]
		public IActionResult Search(string query, bool isLocked) {

            // Remember browsed query.
            ViewData["query"] = query;
            ViewData["isLocked"] = isLocked;

            List<HelpPage> pagesByScore = SearchEngine.FindPagesByQueryTags(query, context, isLocked);

            if (pagesByScore == null)
            {
                return RedirectToAction("Browse");
            }

            // Pass found pages to view.
            ViewData["helpPages"] = pagesByScore;

            return View("Browse");
		}

        /// <summary>
        /// Redirects to view with specified help page based on help page reference.
        /// </summary>
        /// <param name="pageID"></param>
        /// <returns></returns>
        public IActionResult Display(int hPage) {
            var helpPage = context.HelpPages.Find(hPage);
            ViewData["helpPage"] = helpPage;
			return View();
		}

		

    }
}