﻿using System.Collections.Generic;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.Interfaces;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.MappingDao;
using IsHoroshiki.BusinessServices.NotEditableDictionaries.Interfaces;
using IsHoroshiki.DAO.UnitOfWorks;

namespace IsHoroshiki.BusinessServices.NotEditableDictionaries
{
    /// <summary>
    /// Сервис Настройки заказа
    /// </summary>
    public class OrderSettingService : BaseNotEditableDictionaryService<IOrderSettingModel>, IOrderSettingService
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
        public OrderSettingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IOrderSettingService

        /// <summary>
        /// Получить все сущности
        /// </summary>
        public override IEnumerable<IOrderSettingModel> GetAll()
        {
            return _unitOfWork.OrderSettingRepository.GetAll().ToModelEntityList();
        }

        #endregion
    }
}
