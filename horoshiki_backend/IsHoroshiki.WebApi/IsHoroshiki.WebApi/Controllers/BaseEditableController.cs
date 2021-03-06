﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using IsHoroshiki.BusinessEntities;
using IsHoroshiki.BusinessServices;
using IsHoroshiki.WebApi.Handlers;

namespace IsHoroshiki.WebApi.Controllers
{
    /// <summary>
    /// Абстрактный класс редактируемого контроллера
    /// </summary>
    public abstract class BaseEditableController<TModelEntity, TModelEntityService> : BaseController<TModelEntity>
        where TModelEntity : class, IBaseBusninessModel
        where TModelEntityService : IBaseEditableService<TModelEntity>
    {
        #region поля и свойства

        /// <summary>
        /// Сервис бизнес-логики
        /// </summary>
        protected readonly TModelEntityService _service;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Сервис бизнес-логики</param>
        protected BaseEditableController(TModelEntityService service)
            : base(service)
        {
            _service = service;
        }

        #endregion

        #region методы

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <param name="pageNo">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get(int pageNo = 1, int pageSize = 50, string sortField = "", bool isAscending = true)
        {
            try
            {
                var list = await _service.GetAllAsync(pageNo, pageSize, sortField, isAscending);
                return Ok(list);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        [Route("Small")]
        public virtual async Task<IHttpActionResult> GetAll(string sortField = "", bool isAscending = true)
        {
            try
            {
                var list = await _service.GetAllAsync(sortField, isAscending);
                return Ok(list);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Добавить в БД
        /// </summary>
        /// <param name="model">Данные</param>
        [Route("Add")]
        public virtual async Task<IHttpActionResult> Add(TModelEntity model)
        {
            if (!ModelState.IsValid)
            {
                return GetErrorResult(ModelState);
            }

            try
            {
                ModelEntityModifyResult result = await _service.AddAsync(model);
                if (!result.IsValidationSucceeded || !result.IsSucceeded)
                {
                    return new ErrorMessageResult(result.ValidationErrors);
                }

                return Ok(result.NewId);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Обновить объект в БД
        /// </summary>
        /// <param name="model">Данные</param>
        [Route("Update")]
        public virtual async Task<IHttpActionResult> Update(TModelEntity model)
        {
            if (!ModelState.IsValid)
            {
                return GetErrorResult(ModelState);
            }

            try
            {
                ModelEntityModifyResult result = await _service.UpdateAsync(model);
                if (!result.IsValidationSucceeded || !result.IsSucceeded)
                {
                    return new ErrorMessageResult(result.ValidationErrors);
                }
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }

            return Ok();
        }

        /// <summary>
        /// true - если можно удалить из БД
        /// </summary>
        /// <param name="id">Id объекта</param>
        [Route("CanDelete/{id}")]
        [HttpGet]
        public virtual async Task<IHttpActionResult> IsCanDelete(int id)
        {
            try
            {
                bool result = await _service.IsCanDeleteAsync(id);
                return Ok(result);
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }
        }

        /// <summary>
        /// Удалить Объект из БД по Id
        /// </summary>
        /// <param name="id">Id объекта</param>
        [Route("{id}")]
        public virtual async Task<IHttpActionResult> Delete(int id)
        {
            try
            {
                ModelEntityModifyResult result = await _service.DeleteAsync(id);
                if (!result.IsValidationSucceeded || !result.IsSucceeded)
                {
                    return new ErrorMessageResult(result.ValidationErrors);
                }
            }
            catch (Exception e)
            {
                return new ErrorMessageResult(e);
            }

            return Ok();
        }

        #endregion
    }
}
