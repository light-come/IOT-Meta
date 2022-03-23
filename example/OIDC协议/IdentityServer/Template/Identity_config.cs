using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityServer.Template
{
    public static class Identity_config
    {
        //public static List<ApiResource> ApiResources()
        //{
        //    return new List<ApiResource> {

        //        new ApiResource("invoice", "Invoice API")
        //        {
        //            Scopes = { "invoice.read", "ApiScope" }
        //        }
        //    };
        //}
        public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("invoice", "Test API")
            {
                Scopes = { "ApiScope", "invoice.read" }
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources()
        {
            return new List<IdentityResource> {
              new IdentityResources.OpenId(),
              new IdentityResources.Profile(),
              new IdentityResources.Email()
            };
        }
        /// <summary>
        /// Authorization Server保护了哪些 API Scope（作用域）
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope> {
                new ApiScope("ApiScope", "API Scope"),
                new ApiScope(name: "invoice.read",   displayName: "Reads your invoices.")
            };
        }
        /// <summary>
        /// 哪些客户端 Client（应用） 可以使用这个 Authorization Server
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client()
                {
                    ClientId="ClientId", ///客户端的标识，要是惟一的
                    //ClientName = "MVC Client",
                    ClientSecrets=new []{new Secret("ClientSecrets".Sha256())}, ////客户端密码，进行了加密
                    AllowedGrantTypes= GrantTypes.ClientCredentials, ////授权方式，这里采用的是客户端认证模式，只要ClientId，以及ClientSecrets正确即可访问对应的AllowedScopes里面的api资源

                     AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "ApiScope"// <- ApiName for IdentityServer authorization
                    } //定义这个客户端可以访问的APi资源数组，上面只有一个api

                },
                new Client()
                {
                    ClientId="123", ///客户端的标识，要是惟一的
                    //ClientName = "MVC Client",
                    ClientSecrets=new []{new Secret("123".Sha256())}, ////客户端密码，进行了加密
                    AllowedGrantTypes= GrantTypes.ClientCredentials, ////授权方式，这里采用的是客户端认证模式，只要ClientId，以及ClientSecrets正确即可访问对应的AllowedScopes里面的api资源

                     AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        "invoice.read"
                    } //定义这个客户端可以访问的APi资源数组，上面只有一个api

                }
            };
        }
        /// <summary>
        /// 哪些User可以被这个AuthorizationServer识别并授权
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TestUser> GetTestUsers()
        {
            return new[]
            {
               new TestUser
               {
                   SubjectId="001",
                   Username="i3yuan",
                   Password="123456"
               }
           };
        }

    }
}
