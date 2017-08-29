using DNTCaptcha.Core;
using DNTCaptcha.Core.Providers;
using DNTCaptcha.TestWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DNTCaptcha.TestWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter the security code as a number.",
                            IsNumericErrorMessage = "The input value should be a number.",
                            CaptchaGeneratorLanguage = Language.Chinese)]
        public IActionResult Index([FromForm]AccountViewModel data)
        {
            if (ModelState.IsValid)
            {
                //TODO: Save data
                return RedirectToAction(nameof(Thanks), new { name = data.Username });
            }
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        [ValidateDNTCaptcha(ErrorMessage = "Please enter the security code as a number.",
                            IsNumericErrorMessage = "The input value should be a number.",
                            CaptchaGeneratorLanguage = Language.Chinese)]
        public IActionResult Login2([FromForm]AccountViewModel data)
        {
            if (ModelState.IsValid)
            {
                //TODO: Save data
                return RedirectToAction(nameof(Thanks), new { name = data.Username });
            }
            return View(nameof(Index));
        }


        public IActionResult Thanks(string name)
        {
            return View(nameof(Thanks), name);
        }
    }
}