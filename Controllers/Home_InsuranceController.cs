using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test0000001.DB;
using test0000001.Models;
using test0000001.Repository.InterfaceClass;

namespace test0000001.Controllers
{
    public class Home_InsuranceController : Controller
    {
        private IHomeInsurance service;
        private IWebHostEnvironment env;
        private DatabaseContext db;

        public Home_InsuranceController(IHomeInsurance _service, IWebHostEnvironment _env, DatabaseContext _db)
        {
            service = _service;
            env = _env;
            db = _db;
        }
        //admin page
        public async Task<IActionResult> Index()
        {
            var model = await service.getAllHomeInsurance();
            return View(model);
        }
        public async Task<IActionResult> Introduce()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            //ViewData["DepId"] = new SelectList(db.Policy, "Id", "Name");
            ViewData["DurationId"] = new SelectList(db.Duration, "Id", "Term");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Home_Insurance newHome_Insurance)
        {
            await service.addHomeInsurance(newHome_Insurance);
            return RedirectToAction("Index", "Home_Insurance");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["DurationId"] = new SelectList(db.Duration, "Id", "Term");
            var model = await service.getHomeInsurance(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Home_Insurance editHome_Insurance)
        {
            await service.editHomeInsurance(editHome_Insurance);
            ViewData["msg"] = "Congratulation !!! Edit Success";
            return RedirectToAction("Index", "Home_Insurance");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var model = await service.deleteHomeInsurance(id);
                ViewData["msg"] = "Delete Home_Insurance Success";
                return RedirectToAction("Index", "Home_Insurance");
            }
            catch (Exception)
            {
                ViewData["fail"] = "Delete Home_Insurance Fail";
                return RedirectToAction("Index", "Home_Insurance");
            }
        }
    }
}
