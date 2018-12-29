using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using zsq.MvcCookieAuth.Services;
using zsq.MvcCookieAuth.ViewModels;

namespace zsq.MvcCookieAuth
{
    public class ConsentController : Controller
    {
        private readonly ConsentService _consentService;

        public ConsentController(ConsentService consentService)
        {
            _consentService = consentService;
        }

        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consentService.BuildConsentViewModelAsync(returnUrl);

            if (model == null)
            {

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            var result = await _consentService.ProcessConsent(viewModel);
            if (result.IsRedirect)
            {
                return Redirect(result.ReturnUrl);
            }

            if (!string.IsNullOrEmpty(result.ValidationError))
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            return View(result.ViewModel);
        }
    }
}