﻿using System.Collections.Generic;
using System.Linq;
using IsHoroshiki.BusinessEntities.NotEditable.Interfaces;
using IsHoroshiki.DAO.DaoEntities.NotEditable;

namespace IsHoroshiki.BusinessEntities.NotEditable.MappingDao
{
    /// <summary>
    /// Меппинг полей сущности DAO на бизнес-сущность
    /// </summary>
    public static class EmployeeStatusModelMappingDao
    {
        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static EmployeeStatus ToDaoEntity(this IEmployeeStatusModel model)
        {
            return new EmployeeStatus()
            {
                Id = model.Id,
                Guid = model.Guid,
                Value = model.Value
            };
        }

        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<EmployeeStatus> ToDaoEntityList(this IEnumerable<IEmployeeStatusModel> models)
        {
            return models.Select(model => model.ToDaoEntity());
        }

        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IEmployeeStatusModel ToModelEntity(this EmployeeStatus model)
        {
            return new EmployeeStatusModel()
            {
                Id = model.Id,
                Guid = model.Guid,
                Value = model.Value
            };
        }

        /// <summary>
        /// DAO в модель
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static IEnumerable<IEmployeeStatusModel> ToModelEntityList(this IEnumerable<EmployeeStatus> models)
        {
            return models.Select(model => model.ToModelEntity());
        }
    }
}
