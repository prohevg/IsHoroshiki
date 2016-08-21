﻿using System.Collections.Generic;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.Interfaces;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.MappingDao;
using IsHoroshiki.BusinessServices.NotEditableDictionaries.Interfaces;
using IsHoroshiki.DAO.UnitOfWorks;

namespace IsHoroshiki.BusinessServices.NotEditableDictionaries
{
    /// <summary>
    /// Сервис Типы цен
    /// </summary>
    public class PriceTypeService : BaseNotEditableDictionaryService<IPriceTypeModel>, IPriceTypeService
    {
        #region поля и свойства

        /// <summary>
        /// UnitOfWork
        /// </summary>
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork</param>
        public PriceTypeService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IPriceTypeService

        /// <summary>
        /// Получить все сущности
        /// </summary>
        public override IEnumerable<IPriceTypeModel> GetAll()
        {
            return _unitOfWork.PriceTypeRepository.GetAll().ToModelEntityList();
        }

        #endregion
    }
}