﻿using System.Web.Http;
using IsHoroshiki.BusinessEntities.Account.Interfaces;
using IsHoroshiki.BusinessEntities.Editable.Interfaces;
using IsHoroshiki.WebApi.Handlers;
using IsHoroshiki.BusinessEntities.NotEditable.Interfaces;
using IsHoroshiki.BusinessEntities.Editable.SalePlan;
using IsHoroshiki.BusinessEntities.Editable.SalePlans;
using IsHoroshiki.BusinessEntities.Editable.ShiftPersonals;

namespace IsHoroshiki.WebApi
{
    /// <summary>
    /// Конфигурация контроллеров
    /// </summary>
    public static class ControllerConfig
    {
        /// <summary>
        /// Регистрация биндинга интерфйса на сущность  для контроллера
        /// </summary>
        public static void Register()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IApplicationUserModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<ISubDivisionModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IPlatformModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IEmployeeReasonDismissalModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IDepartmentModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IDeliveryZoneModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<ISalePlanModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<ISalePlanDayModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IShiftPersonalModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IShiftPersonalTimePartModel>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IocCustomCreationConverter<IMonthObjectiveModel>());
        }
    }
}
