﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsHoroshiki.BusinessServices.Errors.Enums
{
    /// <summary>
    /// Список ошибок для площадки
    /// </summary>
    public enum PlatformErrors
    {
        /// <summary>
        /// Необходимо указать наименование!
        /// </summary>
        NameIsNull,

        /// <summary>
        /// Необходимо указать подразделение!
        /// </summary>
        SubDivisionIsNullModel,

        /// <summary>
        /// Необходмио указать способы покупки!
        /// </summary>
        BuyProcessesIsNullModel,

        /// <summary>
        /// Необходимо указать статус площадки!
        /// </summary>
        PlatformStatusIsNullModel,

        /// <summary>
        /// Необходимо указать время начала работы!
        /// </summary>
        TimeStartIsNullModel,

        /// <summary>
        /// Необходимо указать время окончания работы!
        /// </summary>
        TimeEndIsNullModel,

        /// <summary>
        /// Минимальный чек должен быть больше нуля!
        /// </summary>
        MinChecksIsNull,

        /// <summary>
        /// Статуса площадки для указанного ID={0} не существует!
        /// </summary>
        PlatformStatusNotFound,

        /// <summary>
        /// Подразделение для указанного ID={0} не существует!
        /// </summary>
        SubDivisionNotFound,

        /// <summary>
        /// Пользователя для указанного ID={0} не существует!
        /// </summary>
        UserNotFound,

        /// <summary>
        /// Способа покупки для указанного ID={0} не существует!
        /// </summary>
        BuyProcessNotFound,

        /// <summary>
        /// Невозможно удалить площадку, т.к. она привязана к пользователю!
        /// </summary>
        CanNotDeleteExistUser,

        /// <summary>
        /// Необходимо указать координаты площадки!
        /// </summary>
        YandexMapIsNull,

        /// <summary>
        /// Неверно задано время начала работы для круглосуточного режима
        /// </summary>
        WrongTimeStartForAroundClock,

        /// <summary>
        /// Неверно задано время окончания работы для круглосуточного режима
        /// </summary>
        WrongTimeEndForAroundClock,

        /// <summary>
        /// Неверно задано время начала приема заказов для круглосуточного режима
        /// </summary>
        WrongOrderTimeStartForAroundClock,

        /// <summary>
        /// Неверно задано время окончания приема заказов для круглосуточного режима
        /// </summary>
        WrongOrderTimeEndForAroundClock,
    }
}
