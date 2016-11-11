﻿using IsHoroshiki.BusinessEntities.Editable.ShiftPersonalSchedules;
using IsHoroshiki.BusinessEntities.Editable.ShiftPersonalSchedules.Tables;
using IsHoroshiki.DAO.Helpers;
using IsHoroshiki.DAO.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using IsHoroshiki.DAO.DaoEntities.Editable;
using IsHoroshiki.BusinessEntities.NotEditable;
using IsHoroshiki.BusinessEntities.Account;

namespace IsHoroshiki.BusinessServices.Editable.ShiftPersonalSchedules
{
    /// <summary>
    /// Построение таблиц графика (расписания) смен сотрудников
    /// </summary>
    public class ShiftPersonalScheduleHelper : IShiftPersonalScheduleHelper
    {
        #region поля и свойства

        /// <summary>
        /// UnitOfWork
        /// </summary>
        private readonly UnitOfWork _unitOfWork;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork</param>
        public ShiftPersonalScheduleHelper(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region IShiftPersonalScheduleHelper

        /// <summary>
        /// График (расписание) смен сотрудников
        /// </summary>
        public async Task<IShiftPersonalScheduleReportModel> GetReport(IShiftPersonalScheduleDataModel model)
        {
            model.Platform.ThrowIfNull();
            model.DateStart.ThrowIfNull();
            model.DateEnd.ThrowIfNull();

            List<int> departaments = null;
            if (model.Departaments != null)
            {
                departaments = model.Departaments.Select(d => d.Id).ToList();
            }

            List<int> subDepartaments = null;
            if (model.SubDepartaments != null)
            {
                subDepartaments = model.SubDepartaments.Select(d => d.Id).ToList();
            }

            var result = new ShiftPersonalScheduleReportModel();

            result.HeaderScheduleColumns = GetHeaderScheduleColumns(model);
            result.SalePlanCountColumns = GetSalePlanCountColumns(model, result);

            var schedulerShiftPersonals = _unitOfWork.ShiftPersonalScheduleRepository.GetScheduleShiftPersonal(departaments, subDepartaments, model.Platform.Id, model.DateStart, model.DateEnd);

            result.DepartamentScheduleRows = GetDepartamentScheduleRows(model, result, schedulerShiftPersonals);
          
            return result;
        }

        #endregion

        #region private

        /// <summary>
        /// Заголовок таблицы
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private List<IHeaderScheduleColumnModel> GetHeaderScheduleColumns(IShiftPersonalScheduleDataModel model)
        {
            var result = new List<IHeaderScheduleColumnModel>();

            for (var currentDate = model.DateStart; currentDate <= model.DateEnd; currentDate = currentDate.AddDays(1))
            {
                var column = new HeaderScheduleColumnModel()
                {
                    Date = currentDate,
                    DayOfWeekDescr = currentDate.ToString("ddd", new CultureInfo("ru-Ru"))
                };
                result.Add(column);
            }
                        
            return result;
        }

        /// <summary>
        /// Кол-во чеков за период
        /// </summary>
        /// <param name="model">Данные с фронта</param>
        /// <param name="report">Отчет</param>
        /// <returns></returns>
        private List<ISalePlanCountColumnModel> GetSalePlanCountColumns(IShiftPersonalScheduleDataModel model, IShiftPersonalScheduleReportModel report)
        {
            var result = new List<ISalePlanCountColumnModel>();

            Dictionary<DateTime, int> periods = _unitOfWork.SalePlanDayRepository.GetByCountPeriod(model.Platform.Id, (int)model.PlanType, model.DateStart, model.DateEnd);

            foreach (var headerColumn in report.HeaderScheduleColumns)
            {
                var column = new SalePlanCountColumnModel()
                {
                    Date = headerColumn.Date,
                };

                if (periods.ContainsKey(headerColumn.Date))
                {
                    column.Count = periods[headerColumn.Date];
                }

                result.Add(column);
            }

            return result;
        }

        /// <summary>
        /// Отделы\сотрудники
        /// </summary>
        /// <param name="model">Данные с фронта</param>
        /// <param name="report">Отчет</param>
        /// <returns></returns>
        private List<IDepartamentScheduleRowModel> GetDepartamentScheduleRows(IShiftPersonalScheduleDataModel model,
            IShiftPersonalScheduleReportModel report, 
            List<ScheduleShiftPersonalResult> scheduleShiftPersonalResults)
        {
            var result = new List<IDepartamentScheduleRowModel>();

            foreach (var scheduleResult in scheduleShiftPersonalResults)
            {
                var rowDepartment = result.FirstOrDefault(r => r.Department.Id == scheduleResult.DepartmentId);
                if (rowDepartment == null)
                {
                    rowDepartment = new DepartamentScheduleRowModel()
                    {
                        Department = new DepartmentModel()
                        {
                            Id = scheduleResult.DepartmentId.Value,
                            Value = scheduleResult.DepartmentName
                        },
                        SubDepartment = new List<ISubDepartamentScheduleRowModel>()
                    };

                    result.Add(rowDepartment);
                }

                var subDepartamentRow = scheduleResult.SubDepartmentId.HasValue
                    ? rowDepartment.SubDepartment.FirstOrDefault(sd => sd.SubDepartment != null && sd.SubDepartment.Id == scheduleResult.SubDepartmentId.Value)
                    : rowDepartment.SubDepartment.FirstOrDefault(sd => sd.Position != null && sd.Position.Id == scheduleResult.PositionId.Value);

                if (subDepartamentRow == null)
                {
                    subDepartamentRow = new SubDepartamentScheduleRowModel();

                    if (scheduleResult.SubDepartmentId.HasValue)
                    {
                        subDepartamentRow.SubDepartment = new SubDepartmentModel()
                        {
                            Id = scheduleResult.SubDepartmentId.Value,
                            Value = scheduleResult.SubDepartmentName
                        };
                    }
                    else if (scheduleResult.PositionId.HasValue)
                    {
                        subDepartamentRow.Position = new PositionModel()
                        {
                            Id = scheduleResult.PositionId.Value,
                            Value = scheduleResult.PositionName
                        };
                    }

                    subDepartamentRow.UserRows = new List<IApplicationUserScheduleRowModel>();

                    rowDepartment.SubDepartment.Add(subDepartamentRow);
                }

                if (scheduleResult.UserId.HasValue)
                {
                    var userRow = subDepartamentRow.UserRows.FirstOrDefault(ur => ur.User.Id == scheduleResult.UserId.Value);
                    if (userRow == null)
                    {
                        userRow = new ApplicationUserScheduleRowModel()
                        {
                            Date = scheduleResult.DateDoc,
                        };

                        userRow.User = new ApplicationUserSmallModel()
                        {
                            Id = scheduleResult.UserId.Value,
                            UserName = scheduleResult.UserName
                        };
                        
                        subDepartamentRow.UserRows.Add(userRow);
                    }

                    if (scheduleResult.ShiftPersonalScheduleId.HasValue)
                    {
                        userRow.ShiftPersonalSchedule = new ShiftPersonalScheduleModel
                        {
                            Id = scheduleResult.ShiftPersonalScheduleId.Value,
                            ShiftPersonalSchedulePeriods = new List<IShiftPersonalSchedulePeriodModel>()
                        };
                    }

                }
            }

            return result;
        }

        #endregion
    }
}
