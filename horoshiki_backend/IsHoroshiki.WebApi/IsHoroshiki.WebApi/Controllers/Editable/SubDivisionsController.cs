﻿using System.Web.Http;
using IsHoroshiki.BusinessEntities.Editable.Interfaces;
using IsHoroshiki.BusinessServices.Editable.Interfaces;

namespace IsHoroshiki.WebApi.Controllers.Editable
{
    /// <summary>
    /// Контроллер Подразделения
    /// </summary>
    [Authorize]
    [RoutePrefix("api/SubDivisions")]
    public class SubDivisionsController : BaseEditableController<ISubDivisionModel>
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="service">Cервис Подразделения</param>
        public SubDivisionsController(ISubDivisionService service)
            : base(service)
        {
            
        }

        #endregion
    }
}
