﻿using Microsoft.AspNet.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IsHoroshiki.BusinessServices.Account.Interfaces
{
    /// <summary>
    ///  Сервис аккаунтов
    /// </summary>
    public interface IAccountService : IDisposable
    {
        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        Task<IdentityResult> RegisterAsync(string userName, string password);

        /// <summary>
        /// Найти пользоватея
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        Task<IUser<int>> FindAsync(string userName, string password);

        /// <summary>
        /// Создание Identity пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationType">Тип аутентификации</param>
        /// <returns></returns>
        Task<ClaimsIdentity> GenerateUserIdentityAsync(IUser<int> user, string authenticationType);

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword);

        /// <summary>
        /// Установить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        Task<IdentityResult> AddPasswordAsync(int userId, string newPassword);

        /// <summary>
        /// Удалить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        Task<IdentityResult> RemovePasswordAsync(int userId);

        /// <summary>
        /// Удалить логин
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <returns></returns>
        Task<IdentityResult> RemoveLoginAsync(int userId, string loginProvider, string providerKey);
    }
}