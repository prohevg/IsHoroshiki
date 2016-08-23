﻿using IsHoroshiki.DAO.Accounts.Interfaces;
using Microsoft.AspNet.Identity;
using IsHoroshiki.DAO.Identities;
using System.Threading.Tasks;
using System.Security.Claims;
using IsHoroshiki.DAO.DaoEntities.Accounts;

namespace IsHoroshiki.DAO.Repositories.Accounts
{
    /// <summary>
    /// Репозитарий авторизации
    /// </summary>
    public class AccountRepository : IAccountRepository
    {
        #region поля и свойства

        /// <summary>
        /// Контекст БД
        /// </summary>
        private ApplicationDbContext _ctx;

        /// <summary>
        /// Конфигурация хранилища пользователя
        /// </summary>
        private ApplicationUserManager _userManager;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public AccountRepository(ApplicationDbContext ctx)
        {
            _ctx = ctx;
            var userStore = new CustomUserStore(_ctx);
            _userManager = new ApplicationUserManager(userStore);
        }

        #endregion

        #region методы

        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public Task<IdentityResult> RegisterAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
            };

            return _userManager.CreateAsync(user, password);
        }

        /// <summary>
        /// Найти пользоватея
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public async Task<IUser<int>> FindAsync(string userName, string password)
        {
            return await _userManager.FindAsync(userName, password);
        }

        /// <summary>
        /// Создание Identity пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationType">Тип аутентификации</param>
        /// <returns></returns>
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(IUser<int> user, string authenticationType)
        {
            var applicationUser = _userManager.FindById(user.Id);

            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await _userManager.CreateIdentityAsync(applicationUser, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        /// <summary>
        /// Установить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        public async Task<IdentityResult> AddPasswordAsync(int userId, string newPassword)
        {
            return await _userManager.AddPasswordAsync(userId, newPassword);
        }

        /// <summary>
        /// Удалить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public async Task<IdentityResult> RemovePasswordAsync(int userId)
        {
            return await _userManager.RemovePasswordAsync(userId);
        }

        /// <summary>
        /// Удалить логин
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        public async Task<IdentityResult> RemoveLoginAsync(int userId, string loginProvider, string providerKey)
        {
            return await _userManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// IDisposable
        /// </summary>
        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
        }

        #endregion
    }
}