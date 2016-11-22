﻿using IsHoroshiki.BusinessEntities.NotEditable.Interfaces;
using System.Collections.Generic;

namespace IsHoroshiki.BusinessEntities.Editable.ShiftPersonalSchedules.Tables
{
    /// <summary>
    /// Должности в графике смен (строка в таблице)
    /// </summary>
    public class PositionScheduleRowModel : IPositionScheduleRowModel
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Должность
        /// </summary>
        public IPositionModel Position
        {
            get;
            set;
        }

        /// <summary>
        /// Всего кол-во сотрудников в смене на этот день (по всем сменам)
        /// </summary>
        public List<IShiftCountResultColumn> ShiftCountResultColumns
        {
            get;
            set;
        }

        /// <summary>
        /// Строка в таблице - сотрудники
        /// </summary>
        public List<IUserRowModel> UserRows
        {
            get;
            set;
        }
    }
}