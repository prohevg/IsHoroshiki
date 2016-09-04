﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.ModelBinding;
using IsHoroshiki.BusinessEntities.Account;
using IsHoroshiki.BusinessEntities.Paging;
using IsHoroshiki.BusinessServices.Editable.Interfaces;
using IsHoroshiki.BusinessServices.Helpers;
using IsHoroshiki.WebApi.Handlers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;

namespace IsHoroshiki.WebApi.Controllers.Editable
{
    /// <summary>
    /// Контроллер авторизации
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Accounts")]
    public class AccountsController : ApiController
    {
        #region поля и свойства

        /// <summary>
        /// 
        /// </summary>
        private const string LocalLoginProvider = "Local";

        /// <summary>
        /// Сервис аккаунтов
        /// </summary>
        private readonly IAccountService _service;

        /// <summary>
        /// Менеджер аутентификации
        /// </summary>
        private IAuthenticationManager Authentication
        {
            get
            {
                return Request.GetOwinContext().Authentication;
            }
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Сервис аккаунтов</param>
        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        #endregion

        #region методы контроллера

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <param name="pageNo">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        [ResponseType(typeof(PagedResult<ApplicationUserModel>))]
        public async Task<IHttpActionResult> Get(int pageNo = 1, int pageSize = 50, string sortField = "", bool isAscending = true)
        {
            try
            {
                var list = await _service.GetAll(pageNo, pageSize, sortField, isAscending);
                return Ok(list);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Создать пользователя
        /// </summary>
        /// <param name="model">Данные</param>
        [Route("Add")]
        public async Task<IHttpActionResult> Add(ApplicationUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetErrorResult(ModelState);
            }

            try
            {
                IdentityResult result = await _service.RegisterAsync(model);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception e)
            {
               return new ErrorMessageResult(e);
            }
                      
            return Ok();
        }

        /// <summary>
        /// Получить пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(int id)
        {
            try
            {
                var user = await this._service.GetByIdAsync(id);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return Ok(ResourceControllers.AccountsController_UserNotFound);
                }

            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Обновить пользователя
        /// </summary>
        /// <param name="model">Данные</param>
        [Route("Update")]
        public async Task<IHttpActionResult> Update(ApplicationUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return GetErrorResult(ModelState);
            }

            try
            {
                IdentityResult result = await _service.UpdateAsync(model);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }

            return Ok();
        }

        /// <summary>
        /// Удалить пользователя по Id
        /// </summary>
        /// <param name="id">Id пользователя</param>
        [Route("{id}")]
        public async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                IdentityResult result = await _service.DeleteAsync(id);
                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }

            return Ok();
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        /// <returns></returns>
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="model">Данные</param>
        /// <returns></returns>
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _service.ChangePasswordAsync(GetUserId(), model.OldPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="model">Данные</param>
        /// <returns></returns>
        [Route("ChangePasswordUser")]
        public async Task<IHttpActionResult> ChangePasswordUser(ChangePasswordUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _service.ChangePasswordUserAsync(model.UserId, model.Password, model.ConfirmPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Установить пароль
        /// </summary>
        /// <param name="model">Данные</param>
        /// <returns></returns>
        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _service.AddPasswordAsync(GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Удалить пользователя
        /// </summary>
        /// <param name="model">Данные</param>
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;
            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await _service.RemovePasswordAsync(GetUserId());
            }
            else
            {
                result = await _service.RemoveLoginAsync(GetUserId(), model.LoginProvider, model.ProviderKey);
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        /// <summary>
        /// Получение текущего пользователя
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Current")]
        [ResponseType(typeof(ApplicationUserModel))]
        public async Task<IHttpActionResult> GetCurrent()
        {
            try
            {
                var userId = User.Identity.GetUserId();
                return await Get(Int32.Parse(userId));
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Проверка существования логина для пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("IsExist/{userName}")]
        [ResponseType(typeof(bool))]
        public async Task<IHttpActionResult> IsExistUserName(string userName)
        {
            try
            {
                var result = await _service.IsExistUserName(userName);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        #endregion

        #region методы

        /// <summary>
        /// ДОбавление ошибок
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        return new ErrorMessageResult(error);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        /// <summary>
        /// Вернуть первую ошибку
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        private IHttpActionResult GetErrorResult(ModelStateDictionary modelState)
        {
            if (modelState == null)
            {
                return InternalServerError();
            }

            if (!modelState.IsValid)
            {
                var bulider = new StringBuilder();
                foreach (var modelStateError in modelState.Values)
                {
                    foreach (var error in modelStateError.Errors)
                    {
                        return BadRequest(error.ErrorMessage);
                    }
                }
                
                return BadRequest(ModelState);
            }

            return null;
        }

        /// <summary>
        /// Найти Id пользователя
        /// </summary>
        /// <returns></returns>
        private int GetUserId()
        {
            return User.Identity.GetUserId<int>();
        }

        /// <summary>
        /// Очистить ресурсы
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && _service != null)
            {
                _service.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
