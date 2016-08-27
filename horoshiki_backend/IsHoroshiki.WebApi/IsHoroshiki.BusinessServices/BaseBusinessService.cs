﻿using IsHoroshiki.BusinessEntities;
using IsHoroshiki.DAO;
using IsHoroshiki.DAO.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IsHoroshiki.BusinessServices.Validators;

namespace IsHoroshiki.BusinessServices
{
    /// <summary>
    /// Базовый класс для всей бизнес-логики сервисов
    /// </summary>
    public abstract class BaseBusinessService<TModelEntity, TDaoEntity> : IBaseBusinessService<TModelEntity> 
        where TModelEntity : class, IBaseBusninessModel
        where TDaoEntity : BaseDaoEntity
    {
        #region поля и свойства 

        /// <summary>
        /// true - если был вызван Dispose
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// Репозитарий сущности
        /// </summary>
        protected IBaseRepository<TDaoEntity> _repository;

        /// <summary>
        /// Репозитарий сущности
        /// </summary>
        protected IValidator<TModelEntity> _validator;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="repository">Репозитарий сущности</param>
        /// <param name="validator">Валидатор сущности</param>
        protected BaseBusinessService(IBaseRepository<TDaoEntity> repository, IValidator<TModelEntity> validator)
        {
            if (repository == null)
            {
                throw new ArgumentNullException();
            }

            _repository = repository;
        }

        #endregion

        #region IBaseBusinessService

        /// <summary>  
        /// Найти по Id 
        /// </summary>  
        /// <param name="id">Id</param>  
        /// <returns></returns>  
        public virtual async Task<TModelEntity> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result != null)
            {
                return ConvertTo(result);
            }
            return null;
        }

        /// <summary>  
        /// Проверка на существование записи
        /// </summary>  
        /// <param name="id">id</param>  
        /// <returns></returns>  
        public virtual async Task<bool> IsExistsAsync(int id)
        {
            return await _repository.IsExistsAsync(id);
        }

        #endregion

        #region protected abstract

        /// <summary>
        /// Метод конвертации Dao объектa в бизнес-модель 
        /// </summary>
        /// <param name="daoEntity"></param>
        /// <returns></returns>
        protected abstract TModelEntity ConvertTo(TDaoEntity daoEntity);

        /// <summary>
        /// Метод конвертации коллекции Dao объектов в коллекцию бизнес-модели 
        /// </summary>
        /// <param name="collection">коллекции Dao объектов</param>
        /// <returns></returns>
        protected abstract IEnumerable<TModelEntity> ConvertTo(IEnumerable<TDaoEntity> collection);

        #endregion

        #region IDisposable 

        /// <summary>  
        /// IDisposable
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                
            }
            this._disposed = true;
        }

        /// <summary>  
        /// Dispose  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
