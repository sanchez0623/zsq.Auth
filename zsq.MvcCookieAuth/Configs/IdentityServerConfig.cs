using System.Collections.Generic;
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
                    AllowedGrantTypes= { GrantType.Implicit},
                    ClientSecrets={ new Secret("secret".Sha256())},
                    AllowedScopes={"api"}//这个client允许访问的apiResource（对应apiResource的名称）
                }
            };
        }

        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>{
                new TestUser
                {
                    SubjectId="1",
                    Username="sanchez",
                    Password="123456"
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