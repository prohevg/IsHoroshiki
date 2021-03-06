﻿using System;
using System.Collections.Generic;
using IsHoroshiki.DAO.DaoEntities.Accounts;
using IsHoroshiki.DAO.DaoEntities.NotEditable;

namespace IsHoroshiki.DAO.DaoEntities.Editable
{
    /// <summary>
    /// Площадка
    /// </summary>
    public class Platform : BaseDaoEntity
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public Platform()
        {
            this.BuyProcesses = new HashSet<BuyProcess>();
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Подразделение
        /// </summary>
        public int SubDivisionId
        {
            get;
            set;
        }

        /// <summary>
        /// Подразделение
        /// </summary>
        public SubDivision SubDivision
        {
            get;
            set;
        }

        /// <summary>
        /// Пользователь - управляющий
        /// </summary>
        public int? UserId
        {
            get;
            set;
        }

        /// <summary>
        ///  Пользователь - управляющий
        /// </summary>
        public ApplicationUser User
        {
            get;
            set;
        }

        /// <summary>
        /// Статус площадки
        /// </summary>
        public int PlatformStatusId
        {
            get;
            set;
        }

        /// <summary>
        /// Статус площадки
        /// </summary>
        public PlatformStatus PlatformStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Способы покупки
        /// </summary>
        public ICollection<BuyProcess> BuyProcesses
        {
            get;
            set;
        }

        /// <summary>
        /// Яндекс-карта координаты
        /// </summary>
        public string YandexMap
        {
            get;
            set;
        }

        /// <summary>
        /// Яндекс-карта координаты
        /// </summary>
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// Начало работы
        /// </summary>
        public TimeSpan TimeStart
        {
            get;
            set;
        }

        /// <summary>
        /// Окончание работы
        /// </summary>
        public TimeSpan TimeEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Время начала приема заказов
        /// </summary>
        public TimeSpan OrderTimeStart
        {
            get;
            set;
        }

        /// <summary>
        /// Время окончания приема заказов
        /// </summary>
        public TimeSpan OrderTimeEnd
        {
            get;
            set;
        }

        /// <summary>
        /// Минимальный чек
        /// </summary>
        public decimal MinCheck
        {
            get;
            set;
        }

        /// <summary>
        /// Признак круглосуточного режима работы
        /// </summary>
        public bool IsAroundClock
        {
            get;
            set;
        }
    }
}
