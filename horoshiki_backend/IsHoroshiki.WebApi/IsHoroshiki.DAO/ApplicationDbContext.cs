﻿using System.Data.Entity;
using IsHoroshiki.DAO.DaoEntities.Accounts;
using IsHoroshiki.DAO.DaoEntities.Editable;
using IsHoroshiki.DAO.DaoEntities.NotEditable;
using IsHoroshiki.DAO.DaoEntityConfigurations.Editable;
using IsHoroshiki.DAO.DaoEntityConfigurations.NotEditable;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IsHoroshiki.DAO
{
    /// <summary>
    /// Контекст БД
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>
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

        /// <summary>
        /// Список сущностей БД площадки
        /// </summary>
        public DbSet<Platform> Platform
        {
            get;
            set;
        }


        /// <summary>
        /// Список сущностей БД Статус площадки
        /// </summary>
        public DbSet<PlatformStatus> PlatformStatuses
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Должности
        /// </summary>
        public DbSet<Position> Positions
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Статус сотрудника
        /// </summary>
        public DbSet<EmployeeStatus> EmployeeStatuses
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Отделы
        /// </summary>
        public DbSet<Department> Departments
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД ПодОтделы
        /// </summary>
        public DbSet<SubDepartment> SubDepartments
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Статус заказа
        /// </summary>
        public DbSet<OrderStatus> OrderStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Оплата заказа
        /// </summary>
        public DbSet<OrderPay> OrderPays
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Подразделения
        /// </summary>
        public DbSet<SubDivision> SubDivisions
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Типы цен
        /// </summary>
        public DbSet<PriceType> PriceTypes
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Типы зон доставки
        /// </summary>
        public DbSet<DeliveryZoneType> DeliveryZones
        {
            get;
            set;
        }

        /// <summary>
        /// Список сущностей БД Причины увольнения сотрудника
        /// </summary>
        public DbSet<EmployeeReasonDismissal> EmployeeReasonDismissals
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
            modelBuilder.Configurations.Add(new PlatformStatusConfiguration());
            modelBuilder.Configurations.Add(new PlatformConfiguration());
            modelBuilder.Configurations.Add(new PositionConfiguration());
            modelBuilder.Configurations.Add(new EmployeeStatusConfiguration());
            modelBuilder.Configurations.Add(new EmployeeReasonDismissalConfiguration());
            modelBuilder.Configurations.Add(new DepartmentConfiguration());
            modelBuilder.Configurations.Add(new OrderStatusConfiguration());
            modelBuilder.Configurations.Add(new OrderPayConfiguration());
            modelBuilder.Configurations.Add(new SubDivisionConfiguration());
            modelBuilder.Configurations.Add(new PriceTypeConfiguration());
            modelBuilder.Configurations.Add(new DeliveryZoneConfiguration());
            modelBuilder.Configurations.Add(new DeliveryZoneTypeConfiguration());
            modelBuilder.Configurations.Add(new DeliveryTimeConfiguration());
        }

        #endregion
    }
}