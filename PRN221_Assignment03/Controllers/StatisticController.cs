using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN221_Assignment03.Models;

namespace PRN221_Asm02.Areas.Admin.Controllers
{
    public class StatisticController : Controller
    {
        private readonly Prn221Asm03Context _context;

        public StatisticController(Prn221Asm03Context context)
        {
            _context = context;
        }

        // GET: Statistic
        public ActionResult Index()
        {
            return View();
        }

        // GET: Statistic/Details/5
        public async Task<IActionResult> ThongKe(DateTime startDate, DateTime endDate)
        {
            string startDate1 = startDate.ToString();
            string endDate1 = endDate.ToString();
            var orders = await _context.Posts.ToListAsync();
            HttpContext.Session.SetString("StartDate", startDate1);
            HttpContext.Session.SetString("EndDate", endDate1);

            return View(orders);
        }
    }
}
