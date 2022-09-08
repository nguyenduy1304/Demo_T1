using Demo_T1.Models;
using Demo_T1.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Demo_T1.Controllers
{
    public class HomeController : Controller
    {
        ITransientService _transientService1;
        ITransientService _transientService2;

        IScopedService _scopedService1;
        IScopedService _scopedService2;

        ISingletonService _singletonService1;
        ISingletonService _singletonService2;

        private readonly ILogger _logger;
        public HomeController(ITransientService transientService1,
                              ITransientService transientService2,

                                IScopedService scopedService1,
                                IScopedService scopedService2,

                                ISingletonService singletonService1,
                                ISingletonService singletonService2,
                                ILogger<ISingletonService> logger)
        {
            _logger = logger;

            _transientService1 = transientService1;
            _transientService2 = transientService2;

            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;

            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;
           

        }

        public IActionResult Index()
        {

            ViewBag.message1 = "First Instance " + _transientService1.GetID().ToString();
            ViewBag.message2 = "Second Instance " + _transientService2.GetID().ToString();

            ViewBag.message3 = "First Instance " + _scopedService1.GetID().ToString();
            ViewBag.message4 = "Second Instance " + _scopedService2.GetID().ToString();

            ViewBag.message5 = "First Instance " + _singletonService1.GetID().ToString();
            ViewBag.message6 = "Second Instance " + _singletonService2.GetID().ToString();
            _logger.LogInformation("About page visited at {DT}",
           DateTime.UtcNow.ToLongTimeString());
            return View();
        }
    }
}