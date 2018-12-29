using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using zsq.MvcCookieAuth.ViewModels;

namespace zsq.MvcCookieAuth.Services
{
    public class ConsentService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl, InputConsentViewModel model = null)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            //Todo: 判断client存不存在
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            var consentViwModel = CreateConsentViewModel(request, client, resources, model);
            consentViwModel.ReturnUrl = returnUrl;
            return consentViwModel;
        }

        private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request, Client client, Resources resources, InputConsentViewModel model)
        {
            var rememberConsent = model?.RememberConsent ?? true;
            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();

            var cvm = new ConsentViewModel();
            cvm.ClientName = client.ClientName;
            cvm.ClientLogoUrl = client.LogoUri;
            cvm.ClientUrl = client.ClientUri;
            cvm.RememberConsent = rememberConsent;//client.AllowRememberConsent;

            cvm.IdentityScopes = resources.IdentityResources.Select(i => CreateScopeViewModel(i, selectedScopes.Contains(i.Name) || model == null));
            //使用select返回的是IEnumerable<ICollection<scope>>
            //cvm.ResourceScopes = resources.ApiResources.Select(a => a.Scopes).Select(s => CreateScopeViewModel(s));
            //使用selectMany返回的是IEnumerable<scope>
            cvm.ResourceScopes = resources.ApiResources.SelectMany(a => a.Scopes).Select(s => CreateScopeViewModel(s, selectedScopes.Contains(s.Name) || model == null));

            return cvm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource, bool check)
        {
            return new ScopeViewModel
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Required = identityResource.Required,
                Checked = check || identityResource.Required,
                Emphasize = identityResource.Emphasize,
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Required = scope.Required,
                Checked = check || scope.Required,
                Emphasize = scope.Emphasize,
            };
        }

        public async Task<ProcessConsentResult> ProcessConsent(InputConsentViewModel model)
        {
            var result = new ProcessConsentResult();
            ConsentResponse consentResponse = null;
            if (model.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (model.Button == "yes")
            {
                if (model.ScopesConsented != null && model.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse
                    {
                        RememberConsent = model.RememberConsent,
                        ScopesConsented = model.ScopesConsented
                    };
                }

                result.ValidationError = "最少选中一个权限";
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(model.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                result.ReturnUrl = model.ReturnUrl;
            }

            var consentViewModel = await BuildConsentViewModelAsync(model.ReturnUrl, model);
            result.ViewModel = consentViewModel;

            return result;
        }
    }
}