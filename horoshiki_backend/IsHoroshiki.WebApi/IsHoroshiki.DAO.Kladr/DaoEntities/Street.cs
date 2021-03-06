﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsHoroshiki.DAO.Kladr.DaoEntities
{
    /// <summary>
    /// Записи с объектами пятого уровня классификации (улицы городов и населенных пунктов);
    /// </summary>
    public class Street : BaseDaoEntity
    {
        /// <summary>
        /// Старый код
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Сокращенное наименование типа объекта
        /// </summary>
        public string Socr
        {
            get;
            set;
        }

        /// <summary>
        /// Код
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string Index
        {
            get;
            set;
        }

        /// <summary>
        /// Код ИФНС
        /// </summary>
        public string GNINMB
        {
            get;
            set;
        }

        /// <summary>
        /// Код территориального участка ИФНС
        /// </summary>
        public string UNO
        {
            get;
            set;
        }

        /// <summary>
        /// Код ОКАТО 
        /// </summary>
        public string OCATD
        {
            get;
            set;
        }

        /// <summary>
        /// Доп поле. Код быстрого поиска региона
        /// </summary>
        public string CodeRegion
        {
            get;
            set;
        }

        /// <summary>
        /// Доп поле. Код быстрого поиска по городу\населенному пункту
        /// </summary>
        public string CodeQuick
        {
            get;
            set;
        }
    }
}
