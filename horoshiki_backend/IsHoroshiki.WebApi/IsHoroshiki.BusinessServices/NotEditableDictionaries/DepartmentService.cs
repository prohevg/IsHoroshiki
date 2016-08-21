﻿using System.Collections.Generic;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.Interfaces;
using IsHoroshiki.BusinessEntities.NotEditableDictionaries.MappingDao;
using IsHoroshiki.BusinessServices.NotEditableDictionaries.Interfaces;
using IsHoroshiki.DAO.UnitOfWorks;

namespace IsHoroshiki.BusinessServices.NotEditableDictionaries
{
    /// <summary>
    /// Сервис Отделы
    /// </summary>
    public class DepartmentService : BaseNotEditableDictionaryService<IDepartmentModel>, IDepartmentService
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
        public DepartmentService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IDepartmentService

        /// <summary>
        /// Получить все сущности
        /// </summary>
        public override IEnumerable<IDepartmentModel> GetAll()
        {
            return _unitOfWork.DepartmentRepository.GetAll().ToModelEntityList();
        }

        #endregion
    }
}
