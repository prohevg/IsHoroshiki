﻿using IsHoroshiki.BusinessEntities.NotEditable.Interfaces;
using System.Collections.Generic;

namespace IsHoroshiki.BusinessEntities.Editable.ShiftPersonalSchedules.Tables
{
    /// <summary>
    /// Должности в графике смен (строка в таблице)
    /// </summary>
    public interface IPositionScheduleRowModel
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Должность
        /// </summary>
        IPositionModel Position
        {
            get;
            set;
        }

        /// <summary>
        /// Всего кол-во сотрудников в смене на этот день (по всем сменам)
        /// </summary>
        List<IShiftCountResultColumn> ShiftCountResultColumns
        {
            get;
            set;
        }

        /// <summary>
        /// Строка в таблице - сотрудники
        /// </summary>
        List<IUserRowModel> UserRows
        {
            get;
            set;
        }
    }
}