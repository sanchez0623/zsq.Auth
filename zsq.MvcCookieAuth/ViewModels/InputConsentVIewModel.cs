using System.Collections.Generic;

namespace zsq.MvcCookieAuth.ViewModels
{
    public class InputConsentViewModel
    {
        public string Button { get; set; }

        public IEnumerable<string> ScopesConsented { get; set; }

        public bool RememberConsent { get; set; }

        public string ReturnUrl { get; set; }
    }
}