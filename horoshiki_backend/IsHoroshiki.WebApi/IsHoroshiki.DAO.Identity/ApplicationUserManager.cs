﻿using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using IsHoroshiki.WebApi.Models;

namespace IsHoroshiki.WebApi
{
    /// <summary>
    /// Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser, int>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser, int> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(
            IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(
                new CustomUserStore(context.Get<ApplicationDbContext>()));
            
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}