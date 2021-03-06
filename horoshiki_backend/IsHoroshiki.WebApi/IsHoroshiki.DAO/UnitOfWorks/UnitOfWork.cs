﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using IsHoroshiki.DAO.Repositories.Accounts;
using IsHoroshiki.DAO.Repositories.Accounts.Interfaces;
using IsHoroshiki.DAO.Repositories.Editable;
using IsHoroshiki.DAO.Repositories.Editable.Interfaces;
using IsHoroshiki.DAO.Repositories.NotEditable;
using IsHoroshiki.DAO.Repositories.NotEditable.Interfaces;
using IsHoroshiki.DAO.Repositories.Integrations;

namespace IsHoroshiki.DAO.UnitOfWorks
{
    /// <summary>  
    /// Unit of Work
    /// </summary>  
    public class UnitOfWork : IDisposable
    {
        #region поля и свойства

        /// <summary>
        /// true - если был вызван Dispose
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Контекст выполнения БД
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Репозиторий Способы покупки
        /// </summary>
        private IBuyProcessRepository _buyProcessPepository;

        /// <summary>
        /// Репозиторий Статус площадки
        /// </summary>
        private IPlatformStatusRepository _platformStatusRepository;

        /// <summary>
        /// Репозиторий Должности
        /// </summary>
        private IPositionRepository _positionRepository;

        /// <summary>
        /// Репозиторий Статус сотрудника
        /// </summary>
        private IEmployeeStatusRepository _employeeStatusRepository;

        /// <summary>
        /// Репозиторий Причины увольнения сотрудника
        /// </summary>
        private EmployeeReasonDismissalRepository _employeeReasonDismissalRepository;

        /// <summary>
        /// Репозиторий Отделы
        /// </summary>
        private IDepartmentRepository _departmentRepository;

        /// <summary>
        /// Репозиторий Подoтделы
        /// </summary>
        private ISubDepartmentRepository _subDepartmentRepository;

        /// <summary>
        /// Репозиторий Статус заказа
        /// </summary>
        private IOrderStatusRepository _orderStatusRepository;

        /// <summary>
        /// Репозиторий Оплата заказа
        /// </summary>
        private IOrderPayRepository _orderPayRepository;

        /// <summary>
        /// Репозиторий Подразделения
        /// </summary>
        private ISubDivisionRepository _subDivisionRepository;

        /// <summary>
        /// Репозиторий Типы цен
        /// </summary>
        private IPriceTypeRepository _priceTypeRepository;

        /// <summary>
        /// Репозиторий зон доставки
        /// </summary>
        private IDeliveryZoneRepository _deliveryZoneRepository;

        /// <summary>
        /// Репозиторий Типы зон доставки
        /// </summary>
        private IDeliveryZoneTypeRepository _deliveryZoneTypeRepository;

        /// <summary>
        /// Репозиторий Время доставки
        /// </summary>
        private IDeliveryTimeRepository _deliveryTimeRepository;

        /// <summary>
        /// Репозитарий авторизации
        /// </summary>
        private IAccountRepository _accountRepository;

        /// <summary>
        /// Репозиторий Площадка
        /// </summary>
        private IPlatformRepository _platformRepository;

        /// <summary>
        /// Репозиторий План продаж
        /// </summary>
        private ISalePlanRepository _salePlanRepository;

        /// <summary>
        /// Репозиторий День плана продаж
        /// </summary>
        private ISalePlanDayRepository _salePlanDayRepository;

        /// <summary>
        /// Репозиторий План продаж
        /// </summary>
        private IIntegrationCheckRepository _integrationCheckRepository;

        /// <summary>
        /// Репозиторий Чек продаж
        /// </summary>
        private ISaleCheckRepository _saleCheckRepository;

        /// <summary>
        /// Репозиторий Тип смены
        /// </summary>
        private IShiftTypeRepository _shiftTypeRepository;

        /// <summary>
        /// Репозиторий Смена
        /// </summary>
        private IShiftPersonalRepository _shiftPersonalRepository;

        /// <summary>
        /// Репозиторий Цель на месяц по показателям
        /// </summary>
        private IMonthObjectiveRepository _monthObjectiveRepository;

        #endregion

        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        public UnitOfWork()
        {
            _context = new ApplicationDbContext();
        }

        #endregion

        #region свойства репозиториев

        /// <summary>  
        /// Репозиторий Способы покупки  
        /// </summary>  
        public IBuyProcessRepository BuyProcessPepository
        {
            get
            {
                if (this._buyProcessPepository == null)
                {
                    this._buyProcessPepository = new BuyProcessRepository(_context);
                }
                return _buyProcessPepository;
            }
        }

        /// <summary>  
        /// Репозиторий Статус площадки  
        /// </summary>  
        public IPlatformStatusRepository PlatformStatusRepository
        {
            get
            {
                if (this._platformStatusRepository == null)
                {
                    this._platformStatusRepository = new PlatformStatusRepository(_context);
                }
                return _platformStatusRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Должности  
        /// </summary>  
        public IPositionRepository PositionRepository
        {
            get
            {
                if (this._positionRepository == null)
                {
                    this._positionRepository = new PositionRepository(_context);
                }
                return _positionRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Статус сотрудника  
        /// </summary>  
        public IEmployeeStatusRepository EmployeeStatusRepository
        {
            get
            {
                if (this._employeeStatusRepository == null)
                {
                    this._employeeStatusRepository = new EmployeeStatusRepository(_context);
                }
                return _employeeStatusRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Причины увольнения сотрудника
        /// </summary>  
        public IEmployeeReasonDismissalRepository EmployeeReasonDismissalRepository
        {
            get
            {
                if (this._employeeReasonDismissalRepository == null)
                {
                    this._employeeReasonDismissalRepository = new EmployeeReasonDismissalRepository(_context);
                }
                return _employeeReasonDismissalRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Отделы  
        /// </summary>  
        public IDepartmentRepository DepartmentRepository
        {
            get
            {
                if (this._departmentRepository == null)
                {
                    this._departmentRepository = new DepartmentRepository(_context);
                }
                return _departmentRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Подoтделы  
        /// </summary>  
        public ISubDepartmentRepository SubDepartmentRepository
        {
            get
            {
                if (this._subDepartmentRepository == null)
                {
                    this._subDepartmentRepository = new SubDepartmentRepository(_context);
                }
                return _subDepartmentRepository;
            }
        }
        
        /// <summary>  
        /// Репозиторий Статус заказа  
        /// </summary>  
        public IOrderStatusRepository OrderStatusRepository
        {
            get
            {
                if (this._orderStatusRepository == null)
                {
                    this._orderStatusRepository = new OrderStatusRepository(_context);
                }
                return _orderStatusRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Оплата заказа  
        /// </summary>  
        public IOrderPayRepository OrderPayRepository
        {
            get
            {
                if (this._orderPayRepository == null)
                {
                    this._orderPayRepository = new OrderPayRepository(_context);
                }
                return _orderPayRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Подразделения  
        /// </summary>  
        public ISubDivisionRepository SubDivisionRepository
        {
            get
            {
                if (this._subDivisionRepository == null)
                {
                    this._subDivisionRepository = new SubDivisionRepository(_context);
                }
                return _subDivisionRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Типы цен  
        /// </summary>  
        public IPriceTypeRepository PriceTypeRepository
        {
            get
            {
                if (this._priceTypeRepository == null)
                {
                    this._priceTypeRepository = new PriceTypeRepository(_context);
                }
                return _priceTypeRepository;
            }
        }

        /// <summary>  
        /// Репозиторий зон доставки  
        /// </summary>  
        public IDeliveryZoneRepository DeliveryZoneRepository
        {
            get
            {
                if (this._deliveryZoneRepository == null)
                {
                    this._deliveryZoneRepository = new DeliveryZoneRepository(_context);
                }
                return _deliveryZoneRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Типы зон доставки  
        /// </summary>  
        public IDeliveryZoneTypeRepository DeliveryZoneTypeRepository
        {
            get
            {
                if (this._deliveryZoneTypeRepository == null)
                {
                    this._deliveryZoneTypeRepository = new DeliveryZoneTypeRepository(_context);
                }
                return _deliveryZoneTypeRepository;
            }
        }

        /// <summary>  
        /// Репозиторий Время доставки  
        /// </summary>  
        public IDeliveryTimeRepository DeliveryTimeRepository
        {
            get
            {
                if (this._deliveryTimeRepository == null)
                {
                    this._deliveryTimeRepository = new DeliveryTimeRepository(_context);
                }
                return _deliveryTimeRepository;
            }
        }

        /// <summary>  
        /// Репозитарий авторизации 
        /// </summary>  
        public IAccountRepository AccountRepository
        {
            get
            {
                if (this._accountRepository == null)
                {
                    this._accountRepository = new AccountRepository(_context);
                }
                return _accountRepository;
            }
        }

        /// <summary>  
        /// Репозитарий Площадка 
        /// </summary>  
        public IPlatformRepository PlatformRepository
        {
            get
            {
                if (this._platformRepository == null)
                {
                    this._platformRepository = new PlatformRepository(_context);
                }
                return _platformRepository;
            }
        }

        /// <summary>  
        /// Репозитарий План продаж 
        /// </summary>  
        public ISalePlanRepository SalePlanRepository
        {
            get
            {
                if (this._salePlanRepository == null)
                {
                    this._salePlanRepository = new SalePlanRepository(_context);
                }
                return _salePlanRepository;
            }
        }

        /// <summary>  
        /// Репозитарий День плана продаж
        /// </summary>  
        public ISalePlanDayRepository SalePlanDayRepository
        {
            get
            {
                if (this._salePlanDayRepository == null)
                {
                    this._salePlanDayRepository = new SalePlanDayRepository(_context);
                }
                return _salePlanDayRepository;
            }
        }

        /// <summary>  
        /// Репозитарий План продаж
        /// </summary>  
        public IIntegrationCheckRepository IntegrationCheckRepository
        {
            get
            {
                if (this._integrationCheckRepository == null)
                {
                    this._integrationCheckRepository = new IntegrationCheckRepository(_context);
                }
                return _integrationCheckRepository;
            }
        }

        /// <summary>  
        /// Репозитарий Чек продаж
        /// </summary>  
        public ISaleCheckRepository SaleCheckRepository
        {
            get
            {
                if (this._saleCheckRepository == null)
                {
                    this._saleCheckRepository = new SaleCheckRepository(_context);
                }
                return _saleCheckRepository;
            }
        }

        /// <summary>  
        /// Репозитарий Тип смены
        /// </summary>  
        public IShiftTypeRepository ShiftTypeRepository
        {
            get
            {
                if (this._shiftTypeRepository == null)
                {
                    this._shiftTypeRepository = new ShiftTypeRepository(_context);
                }
                return _shiftTypeRepository;
            }
        }

        /// <summary>
        /// Репозиторий Смена
        /// </summary>
        public IShiftPersonalRepository ShiftPersonalRepository
        {
            get
            {
                if (this._shiftPersonalRepository == null)
                {
                    this._shiftPersonalRepository = new ShiftPersonalRepository(_context);
                }
                return this._shiftPersonalRepository;
            }
        }

        /// <summary>
        /// Репозиторий Цель на месяц по показателям
        /// </summary>
        public IMonthObjectiveRepository MonthObjectiveRepository
        {
            get
            {
                if (this._monthObjectiveRepository == null)
                {
                    this._monthObjectiveRepository = new MonthObjectiveRepository(_context);
                }
                return this._monthObjectiveRepository;
            }
        }

        #endregion

        #region методы

        /// <summary>  
        /// Сохранить контекст  
        /// </summary>  
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format("{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now, eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    foreach (var ve in eve.ValidationErrors)
                    {
                        outputLines.Add(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                    }
                }
                //System.IO.File.AppendAllLines(@"C:\errors.txt", outputLines);

                throw e;
            }

        }

        #endregion

        #region IDisposable 

        /// <summary>  
        /// IDisposable
        /// </summary>  
        /// <param name="disposing"></param>  
        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                _context.Dispose();
            }
            this._disposed = true;
        }

        /// <summary>  
        /// Dispose  
        /// </summary>  
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
