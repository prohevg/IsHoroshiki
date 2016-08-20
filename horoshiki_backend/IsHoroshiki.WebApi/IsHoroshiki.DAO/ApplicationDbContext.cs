﻿using System.Data.Entity;
using IsHoroshiki.DAO.DaoEntities.NotEditableDictionaries;
using IsHoroshiki.DAO.DaoEntityConfigurations.NotEditableDictionaries;

namespace IsHoroshiki.DAO
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        #region поля и свойства

        /// <summary>
        /// Список сущностей БД Способы покупки
        /// </summary>
        public DbSet<BuyProcess> BuyProcesses
        {
            get;
            set;
        }

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        #endregion

        #region методы

        /// <summary>
        /// Переопредeленный метод создания БД
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new BuyProcessConfiguration());
        }

        #endregion
    }
}