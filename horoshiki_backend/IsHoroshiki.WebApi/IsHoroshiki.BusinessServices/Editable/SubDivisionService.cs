﻿using System.Collections.Generic;
using System.Threading.Tasks;
using IsHoroshiki.BusinessEntities.Editable.Interfaces;
using IsHoroshiki.BusinessEntities.Editable.MappingDao;
using IsHoroshiki.BusinessServices.Editable.Interfaces;
using IsHoroshiki.BusinessServices.Validators;
using IsHoroshiki.BusinessServices.Validators.Editable.Interfaces;
using IsHoroshiki.DAO.DaoEntities.Editable;
using IsHoroshiki.DAO.UnitOfWorks;
using IsHoroshiki.BusinessEntities.Paging;
using IsHoroshiki.BusinessServices.Errors.Enums;
using IsHoroshiki.DAO.Repositories.Editable.Interfaces;

namespace IsHoroshiki.BusinessServices.Editable
{
    /// <summary>
    /// Сервис Подразделения
    /// </summary>
    public class SubDivisionService : BaseEditableService<ISubDivisionModel, ISubDivisionValidator, SubDivision, ISubDivisionRepository>, ISubDivisionService
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork</param>
        /// <param name="validator">Валидатор</param>
        public SubDivisionService(UnitOfWork unitOfWork, ISubDivisionValidator validator)
            : base(unitOfWork, unitOfWork.SubDivisionRepository, validator)
        {
            
        }

        #endregion

        #region protected override

        /// <summary>
        /// Получить всех
        /// </summary>
        /// <param name="pageNo">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        public override async Task<PagedResult<ISubDivisionModel>> GetAllAsync(int pageNo = 1, int pageSize = 50, string sortField = "", bool isAscending = true)
        {
            if (string.Equals(sortField, "PriceType"))
            {
                sortField += "Id";
            }

            return await base.GetAllAsync(pageNo, pageSize, sortField, isAscending);
        }

        /// <summary>
        /// Валидация сущности
        /// </summary>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        protected override async Task<ValidationResult> ValidationInternal(ISubDivisionModel model)
        {
            if (model.PriceType != null)
            {
                var daoPriceType = await _unitOfWork.PriceTypeRepository.GetByIdAsync(model.PriceType.Id);
                if (daoPriceType == null)
                {
                    return new ValidationResult(SubDivisionErrors.PriceTypeNotFound, model.PriceType.Id);
                }
            }

            return new ValidationResult();
        }

        /// <summary>
        /// Валидация сущности при удалении
        /// </summary>
        /// <param name="daoModel">Сущность</param>
        /// <returns></returns>
        protected override ValidationResult CanDeleteInternal(SubDivision daoModel)
        {
            bool result = _unitOfWork.PlatformRepository.IsExistForSubDivision(daoModel.Id);
            if (result)
            {
                return new ValidationResult(SubDivisionErrors.CanNotDeleteExistPlatform);
            }

            return new ValidationResult();
        }

        /// <summary>
        /// Метод конвертации Dao объектa в бизнес-модель 
        /// </summary>
        /// <param name="daoEntity"></param>
        /// <returns></returns>
        protected override ISubDivisionModel ConvertTo(SubDivision daoEntity)
        {
            return daoEntity.ToModelEntity();
        }

        /// <summary>
        /// Метод конвертации коллекции Dao объектов в коллекцию бизнес-модели 
        /// </summary>
        /// <param name="collection">коллекции Dao объектов</param>
        /// <returns></returns>
        protected override IEnumerable<ISubDivisionModel> ConvertTo(IEnumerable<SubDivision> collection)
        {
            return collection.ToModelEntityList();
        }

        /// <summary>
        /// Создание DAO сущности
        /// </summary>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        public override SubDivision CreateInternal(ISubDivisionModel model)
        {
            return model.ToDaoEntity();
        }

        /// <summary>
        /// Обновление сущности
        /// </summary>
        /// <param name="daoEntity">dao Сущность</param>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        public override SubDivision UpdateDaoInternal(SubDivision daoEntity, ISubDivisionModel model)
        {
            var result = daoEntity.Update(model);
            result.PriceType = null;
            return result;
        }

        #endregion
    }
}
