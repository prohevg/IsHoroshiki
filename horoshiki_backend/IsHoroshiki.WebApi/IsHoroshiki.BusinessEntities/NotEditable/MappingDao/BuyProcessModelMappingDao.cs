﻿using System.Collections.Generic;
using System.Linq;
using IsHoroshiki.BusinessEntities.NotEditable.Interfaces;
using IsHoroshiki.DAO.DaoEntities.NotEditable;

namespace IsHoroshiki.BusinessEntities.NotEditable.MappingDao
{
    /// <summary>
    /// Меппинг полей сущности DAO на бизнес-сущность
    /// </summary>
    public static class BuyProcessModelMappingDao
    {
        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static BuyProcess ToDaoEntity(this IBuyProcessModel model)
        {
            return new BuyProcess()
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
        public static IEnumerable<BuyProcess> ToDaoEntityList(this IEnumerable<IBuyProcessModel> models)
        {
            return models.Select(model => model.ToDaoEntity());
        }

        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static ICollection<BuyProcess> ToDaoEntityList(this ICollection<IBuyProcessModel> models)
        {
            return models.Select(model => model.ToDaoEntity()).ToList();
        }

        /// <summary>
        /// Модель в DAO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IBuyProcessModel ToModelEntity(this BuyProcess model)
        {
            return new BuyProcessModel()
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
        public static IEnumerable<IBuyProcessModel> ToModelEntityList(this IEnumerable<BuyProcess> models)
        {
            return models.Select(model => model.ToModelEntity());
        }

        /// <summary>
        /// DAO в модель
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        public static ICollection<IBuyProcessModel> ToModelEntityList(this ICollection<BuyProcess> models)
        {
            return models.Select(model => model.ToModelEntity()).ToList();
        }
    }
}
