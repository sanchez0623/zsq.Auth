using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace zsq.MvcCookieAuth
{
    public class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api","Api Application")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="mvc",
                    ClientName="Mvc",
                    ClientUri="http://localhost:5003",
                    LogoUri="https://timgsa.baidu.com/timg?image&quality=80&size=b9999_10000&sec=1526955882826&di=d53ce0d491bf61a180194fd358641f81&imgtype=0&src=http%3A%2F%2Fimg.mukewang.com%2F5a77b61000013ca502560192.jpg",
                    AllowedGrantTypes= { GrantType.Implicit},
                    ClientSecrets={ new Secret("secret".Sha256())},
                    AllowedScopes=
                    {
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email
                    },
                    RequireConsent=true,
                    RedirectUris={"http://localhost:5003/signin-oidc"},
                    PostLogoutRedirectUris={"http://localhost:5003/signout-callback-oidc"},
                    AllowOfflineAccess=true,
                    AllowAccessTokensViaBrowser=true,
                    AlwaysIncludeUserClaimsInIdToken=true//为啥我不设置为true，客户端就能获取到userinfo！！！？？
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId="1",
                    Username="sanchez",
                    Password="123456",
                    Claims=new List<Claim>
                    {
                        new Claim("name","sanchez"),
                        new Claim("website","www.baidu.com")
                    }
                }
            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResource()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };
        }
    }
}