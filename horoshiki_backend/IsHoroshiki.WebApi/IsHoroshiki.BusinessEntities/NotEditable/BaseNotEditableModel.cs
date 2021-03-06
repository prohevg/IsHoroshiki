﻿using System;

namespace IsHoroshiki.BusinessEntities.NotEditable
{
    /// <summary>
    /// Базовый нередактируемый тип справочника
    /// </summary>
    public abstract class BaseNotEditableModel : BaseBusninessModel
    {
        /// <summary>
        /// Уникальный идентификатор объекта
        /// </summary>
        public Guid Guid
        {
            get;
            set;
        }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
