using System.Collections.Generic;

namespace zsq.MvcCookieAuth.ViewModels
{
    public class ConsentViewModel : InputConsentViewModel
    {
        public string ClientId { get; set; }

        public string ClientName { get; set; }

        public string ClientUrl { get; set; }

        public string ClientLogoUrl { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }

        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}