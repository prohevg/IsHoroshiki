﻿using IsHoroshiki.BusinessEntities.Editable.SalePlan;
using System.Collections.Generic;

namespace IsHoroshiki.BusinessEntities.Editable.SalePlans.Result
{
    /// <summary>
    /// Сформированый план продаж
    /// </summary>
    public interface ISalePlanTableModel
    {
        /// <summary>
        /// План продаж
        /// </summary>
        ISalePlanModel SalePlan
        {
            get;
            set;
        }

        /// <summary>
        /// Строка отчета
        /// </summary>
        List<ISalePlanDataRowModel> DataRows
        {
            get;
            set;
        }

        /// <summary>
        /// Итоговая строка отчета
        /// </summary>
        ISalePlanSumRowModel SumRow
        {
            get;
            set;
        }
    }
}
