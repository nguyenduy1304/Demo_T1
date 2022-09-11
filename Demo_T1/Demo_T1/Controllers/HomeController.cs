using Demo_T1.Models;
using Demo_T1.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.Json;

namespace Demo_T1.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly PositionOptions _options;

        private readonly IConfiguration Configuration;

        private readonly ILogger _logger;

        #region DI
        ITransientService _transientService1;
        ITransientService _transientService2;

        IScopedService _scopedService1;
        IScopedService _scopedService2;

        ISingletonService _singletonService1;
        ISingletonService _singletonService2;

        #endregion

        #region Access the exception
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string? ExceptionMessage { get; set; }

        //public void OnGet()
        //{
        //    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        //    var exceptionHandlerPathFeature =
        //        HttpContext.Features.Get<IExceptionHandlerPathFeature>();

        //    if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        //    {
        //        ExceptionMessage = "The file was not found.";
        //    }

        //    if (exceptionHandlerPathFeature?.Path == "/Home/Privacy")
        //    {
        //        ExceptionMessage ??= string.Empty;
        //        ExceptionMessage += " Page: Home/.";
        //    }
        //}

        #endregion

        public HomeController(ITransientService transientService1,
                              ITransientService transientService2,

                                IScopedService scopedService1,
                                IScopedService scopedService2,

                                ISingletonService singletonService1,
                                ISingletonService singletonService2,
                                ILogger<HomeController> logger,

                                IOptions<PositionOptions> options,

                                IConfiguration configuration)
        {
            _logger = logger;
            _options = options.Value;
            Configuration = configuration;

            _transientService1 = transientService1;
            _transientService2 = transientService2;

            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;

            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;

            

        }
        //public void OnGet()
        //{
        //    _logger.LogInformation("Xin chào nhá!!! {DT}",
        //        DateTime.UtcNow.ToLongTimeString());
        //}
        [Route("home/index")]
        public IActionResult Index()
        {
            #region ViewBag

            ViewBag.message1 = "First Instance " + _transientService1.GetID().ToString();
            ViewBag.message2 = "Second Instance " + _transientService2.GetID().ToString();

            ViewBag.message3 = "First Instance " + _scopedService1.GetID().ToString();
            ViewBag.message4 = "Second Instance " + _scopedService2.GetID().ToString();

            ViewBag.message5 = "First Instance " + _singletonService1.GetID().ToString();
            ViewBag.message6 = "Second Instance " + _singletonService2.GetID().ToString();

            #endregion

            #region Logging
            _logger.LogInformation("Xin chào LogInformation nhá !!! {DT}",
                DateTime.UtcNow.ToLongTimeString());

            _logger.LogError("Xin chào LogError nhá !!! {DT}",
                DateTime.UtcNow.ToLongTimeString());

            _logger.LogWarning("Xin chào LogWarning nhá !!! {DT}",
               DateTime.UtcNow.ToLongTimeString());
            #endregion

            #region Options
            ViewBag.message7 = _options;
            #endregion

            #region Configuration
            var myKeyValue = Configuration["MyKey"];
            var title = Configuration["Position:Title"];
            var name = Configuration["Position:Name"];
            var defaultLogLevel = Configuration["Logging:LogLevel:Default"];


            //return Content($"MyKey value: {myKeyValue} \n" +
            //               $"Title: {title} \n" +
            //               $"Name: {name} \n" +
            //               $"Default Log Level: {defaultLogLevel}");
            #endregion


            GetAPI();
            return View();
        }
        public async Task GetAPI()
        {
            var url = "https://catfact.ninja/fact";
            using var client = new HttpClient();

            var result = await client.GetAsync(url);
            var content =await result.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<API>(content);
            Console.WriteLine("fact: " + apiResult.Fact);
            Console.WriteLine("lenght: " + apiResult.Length);
            

        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}