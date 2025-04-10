using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ContentManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ContentManagementSystem.Filters;
using GuardTour;

namespace ContentManagementSystem.Controllers
{
    [AuthenticationFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        db_Utility util = new db_Utility();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ShowSidebar = true; // Enable sidebar for this page
            return View();
        }

        public IActionResult MainDashboard()
        {
            var ds = util.Fill("select sum(case when Status='Assigned' then 1 else 0 end) Assigned,sum(case when Status='UnAssigned' then 1 else 0 end) UnAssigned ,Count(Id) as Total from MaterialItems", util.strElect);
            ViewBag.Assigned = ds.Tables[0].Rows[0][0].ToString();
            ViewBag.UnAssigned = ds.Tables[0].Rows[0][1].ToString();
            ViewBag.Total = ds.Tables[0].Rows[0][2].ToString();
           
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
