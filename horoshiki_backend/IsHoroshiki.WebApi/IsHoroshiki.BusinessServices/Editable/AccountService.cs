﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IsHoroshiki.BusinessEntities.Account.Interfaces;
using IsHoroshiki.BusinessEntities.Account.MappingDao;
using IsHoroshiki.BusinessEntities.Paging;
using IsHoroshiki.BusinessServices.Editable.Interfaces;
using IsHoroshiki.BusinessServices.Errors.Enums;
using IsHoroshiki.BusinessServices.Errors.ErrorDatas;
using IsHoroshiki.BusinessServices.Validators;
using IsHoroshiki.BusinessServices.Validators.Editable.Interfaces;
using IsHoroshiki.DAO.DaoEntities.Accounts;
using IsHoroshiki.DAO.Repositories.Accounts.Interfaces;
using IsHoroshiki.DAO.UnitOfWorks;
using Microsoft.AspNet.Identity;
using IsHoroshiki.DAO;

namespace IsHoroshiki.BusinessServices.Editable
{
    /// <summary>
    /// Сервис аккаунтов
    /// </summary>
    public class AccountService : BaseEditableService<IApplicationUserModel, IAccountValidator, ApplicationUser, IAccountRepository>, IAccountService
    {
        #region Конструктор

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="unitOfWork">UnitOfWork</param>
        /// <param name="validator">Валидатор</param>
        public AccountService(UnitOfWork unitOfWork, IAccountValidator validator)
            : base(unitOfWork, unitOfWork.AccountRepository, validator)
        {

        }

        #endregion
       
        #region IAccountService

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <param name="pageNo">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        public override async Task<PagedResult<IApplicationUserModel>> GetAllAsync(int pageNo = 1, int pageSize = 50, string sortField = "", bool isAscending = true)
        {
            sortField = GetSortField(sortField);
            return await base.GetAllAsync(pageNo, pageSize, sortField, isAscending);
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <param name="pageNo">Номер страницы</param>
        /// <param name="pageSize">Размер страницы</param>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <param name="filterLastName">Фильтр по фамилии</param>
        /// <param name="filterIsAccess">Фильтр Доступ в систему</param>
        /// <param name="filterEmployeeStatusId">Фильтр Статус сотрудника</param>
        /// <param name="filterPositionId">Фильтр Должности</param>
        /// <param name="filterDepartmentId">Фильтр Отдел</param>
        /// <param name="filterPlatformId">Фильтр Площадка</param>
        /// <param name="filterIsHaveMedicalBook">Фильтр Наличие мед книжки</param>
        /// <returns></returns>
        public async Task<PagedResult<IApplicationUserModel>> GetAllAsync(int pageNo = 1, int pageSize = 50, string sortField = "", bool isAscending = true,
            string filterLastName = "", bool? filterIsAccess = null, int filterEmployeeStatusId = 0, int filterPositionId = 0, int filterDepartmentId = 0,
            int filterPlatformId = 0, bool? filterIsHaveMedicalBook = null)
        {
            sortField = GetSortField(sortField);

            var list = await _repository.GetAllAsync(pageNo, pageSize, sortField, isAscending, true, filterLastName, filterIsAccess, filterEmployeeStatusId, filterPositionId,
                filterDepartmentId, filterPlatformId, filterIsHaveMedicalBook);
            var count = list.Count();
            var result = ConvertTo(list);
            return new PagedResult<IApplicationUserModel>(result, pageNo, pageSize, count);
        }

        /// <summary>
        /// Получить все записи
        /// </summary>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        public async Task<IEnumerable<IApplicationUserSmallModel>> GetAllSmall(string sortField = "", bool isAscending = true)
        {
            sortField = GetSortField(sortField);
            var list = await _repository.GetAllAsync(1, Int32.MaxValue, sortField, isAscending, false);
            return list.ToUserSmallModelEntityList();
        }

        /// <summary>
        /// Получить всех управляющих
        /// </summary>
        /// <param name="sortField">Поле для сортировки</param>
        /// <param name="isAscending">true - сортировать по возрастанию</param>
        /// <returns></returns>
        public async Task<IEnumerable<IApplicationUserSmallModel>> GetAllSmallManager(string sortField = "", bool isAscending = true)
        {
            sortField = GetSortField(sortField);
            var list = await _repository.GetAllSmallManager(sortField, isAscending);
            return list.ToUserSmallModelEntityList();
        }

        /// <summary>
        /// Зарегистрировать пользователя
        /// </summary>
        /// <param name="model">пользователь</param>
        /// <returns></returns>
        public override async Task<ModelEntityModifyResult> AddAsync(IApplicationUserModel model)
        {
            try
            {
                if (model == null)
                {
                    return new ModelEntityModifyResult(CommonErrors.EntityAddIsNull);
                }

                var validateResult = await _validator.ValidateAsync(model);
                if (!validateResult.IsSucceeded)
                {
                    return new ModelEntityModifyResult(validateResult.Errors);
                }

                var customValidateResult = await ValidationInternal(model);
                if (!customValidateResult.IsSucceeded)
                {
                    return new ModelEntityModifyResult(customValidateResult.Errors);
                }

                var daoEntity = CreateInternal(model);

                var result = await _repository.InsertAsync(daoEntity, model.Password);
                if (!result.Succeeded)
                {
                    var errorData = new ErrorData(AccountErrors.AddException, result.Errors.FirstOrDefault());
                    return new ModelEntityModifyResult(errorData);
                }

                _unitOfWork.Save();

                return new ModelEntityModifyResult(daoEntity.Id);
            }
            catch (Exception e)
            {
                var errorData = new ErrorData(CommonErrors.Exception, e.Message);
                return new ModelEntityModifyResult(errorData);
            }
        }

        /// <summary>
        /// ОБновить в БД
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns></returns>
        public override async Task<ModelEntityModifyResult> UpdateAsync(IApplicationUserModel model)
        {
            try
            {
                if (model == null)
                {
                    return new ModelEntityModifyResult(CommonErrors.Exception);
                }

                var validateResult = await _validator.ValidateWithoutPasswordAsync(model);
                if (!validateResult.IsSucceeded)
                {
                    return new ModelEntityModifyResult(validateResult.Errors);
                }

                var customValidateResult = await ValidationInternal(model);
                if (!customValidateResult.IsSucceeded)
                {
                    return new ModelEntityModifyResult(customValidateResult.Errors);
                }

                var daoEntity = await _repository.GetByIdAsync(model.Id);
                if (daoEntity == null)
                {
                    var errorData = new ErrorData(CommonErrors.EntityUpdateNotFound, parameters: new object[] { model.Id });
                    return new ModelEntityModifyResult(errorData);
                }

                UpdateDaoInternal(daoEntity, model);

                _repository.Update(daoEntity);
                _unitOfWork.Save();

                return new ModelEntityModifyResult();
            }
            catch (Exception e)
            {
                var errorData = new ErrorData(CommonErrors.Exception, e.Message);
                return new ModelEntityModifyResult(errorData);
            }
        }

        /// <summary>
        /// Проверка существования логина для пользователя
        /// </summary>
        /// <param name="userName">Логин пользователя</param>
        /// <returns></returns>
        public async Task<bool> IsExistUserName(string userName)
        {
            var user = await _unitOfWork.AccountRepository.FindByNameAsync(userName);
            return user != null;
        }

        /// <summary>
        /// Найти пользоватея
        /// </summary>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="password">Пароль</param>
        /// <returns></returns>
        public Task<IUser<int>> FindAsync(string userName, string password)
        {
            return _unitOfWork.AccountRepository.FindAsync(userName, password);
        }

        /// <summary>
        /// Создание Identity пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticationType">Тип аутентификации</param>
        /// <returns></returns>
        public Task<ClaimsIdentity> GenerateUserIdentityAsync(IUser<int> user, string authenticationType)
        {
            return _unitOfWork.AccountRepository.GenerateUserIdentityAsync(user, authenticationType);
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="oldPassword">Старый пароль</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        public Task<IdentityResult> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            return _unitOfWork.AccountRepository.ChangePasswordAsync(userId, oldPassword, newPassword);
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <param name="confirmPassword">Подтверждение пароля</param>
        /// <returns></returns>
        public async Task<IdentityResult> ChangePasswordUserAsync(int userId, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                return new IdentityResult(ResourceBusinessServices.AccountsController_PasswordIsNull);
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                return new IdentityResult(ResourceBusinessServices.AccountsController_ConfirmPasswordIsNull);
            }

            if (!string.Equals(newPassword, confirmPassword))
            {
                return new IdentityResult(ResourceBusinessServices.AccountsController_PasswordNotEquals);
            }

            var daoUser = await _unitOfWork.AccountRepository.GetByIdAsync(userId);
            if (daoUser == null)
            {
                return new IdentityResult(ResourceBusinessServices.AccountsController_UserNotFound);
            }

            return await _unitOfWork.AccountRepository.ChangePasswordAsync(daoUser.Id, newPassword);
        }

        /// <summary>
        /// Изменить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <param name="newPassword">Новый пароль</param>
        /// <returns></returns>
        public Task<IdentityResult> AddPasswordAsync(int userId, string newPassword)
        {
            return _unitOfWork.AccountRepository.AddPasswordAsync(userId, newPassword);
        }

        /// <summary>
        /// Удалить пароль
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public Task<IdentityResult> RemovePasswordAsync(int userId)
        {
            return _unitOfWork.AccountRepository.RemovePasswordAsync(userId);
        }

        /// <summary>
        /// Получить текущего пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public async Task<IApplicationUserModel> GetCurrent(int userId)
        {
            var result = await _repository.GetByIdAsync(userId);
            if (result == null)
            {
                return null;
            }

            if (result.Platform != null)
            {
                result.Platform.SubDivision =  await _unitOfWork.SubDivisionRepository.GetByIdAsync(result.Platform.SubDivisionId);
            }

            return result.ToModelEntity();
        }



        #endregion

        #region protected override


        /// <summary>
        /// Валидация сущности при удалении
        /// </summary>
        /// <param name="daoEntity">Сущность</param>
        /// <returns></returns>
        protected override ValidationResult CanDeleteInternal(ApplicationUser daoEntity)
        {
            bool result = _unitOfWork.PlatformRepository.IsExistForUser(daoEntity.Id);
            if (result)
            {
                return new ValidationResult(AccountErrors.CanNotDeleteExistPlatform);
            }

            return new ValidationResult();
        }

        /// <summary>
        /// Валидация сущности
        /// </summary>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        protected override async Task<ValidationResult> ValidationInternal(IApplicationUserModel model)
        {
            var position = await _unitOfWork.PositionRepository.GetByIdAsync(model.Position.Id);
            if (position == null)
            {
                return new ValidationResult(AccountErrors.PositionRepositoryIsNull, model.Position.Id);
            }

            var employeeStatus = await _unitOfWork.EmployeeStatusRepository.GetByIdAsync(model.EmployeeStatus.Id);
            if (employeeStatus == null)
            {
                return new ValidationResult(AccountErrors.EmployeeStatusRepositoryIsNull, model.EmployeeStatus.Id);
            }

            if (model.Platform != null && model.Platform.Id > 0)
            {
                var platform = await _unitOfWork.PlatformRepository.GetByIdAsync(model.Platform.Id);
                if (platform == null)
                {
                    return new ValidationResult(AccountErrors.PlatformRepositoryIsNull, model.Platform.Id);
                }
            }

            if (model.Department != null && model.Department.Id > 0)
            {
                var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(model.Department.Id);
                if (department == null)
                {
                    return new ValidationResult(AccountErrors.DepartmentRepositoryIsNull, model.Department.Id);
                }
            }

            if (model.EmployeeReasonDismissal != null && model.EmployeeReasonDismissal.Id > 0)
            {
                var employeeReasonDismissal = await _unitOfWork.EmployeeReasonDismissalRepository.GetByIdAsync(model.EmployeeReasonDismissal.Id);
                if (employeeReasonDismissal == null)
                {
                    return new ValidationResult(AccountErrors.EmployeeReasonDismissalRepositoryIsNull, model.EmployeeReasonDismissal.Id);
                }
            }

            return new ValidationResult();
        }

        /// <summary>
        /// Метод конвертации Dao объектa в бизнес-модель 
        /// </summary>
        /// <param name="daoEntity"></param>
        /// <returns></returns>
        protected override IApplicationUserModel ConvertTo(ApplicationUser daoEntity)
        {
            return daoEntity.ToModelEntity();
        }

        /// <summary>
        /// Метод конвертации коллекции Dao объектов в коллекцию бизнес-модели 
        /// </summary>
        /// <param name="collection">коллекции Dao объектов</param>
        /// <returns></returns>
        protected override IEnumerable<IApplicationUserModel> ConvertTo(IEnumerable<ApplicationUser> collection)
        {
            return collection.ToModelEntityList();
        }

        /// <summary>
        /// Создание DAO сущности
        /// </summary>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        public override ApplicationUser CreateInternal(IApplicationUserModel model)
        {
            return model.ToDaoEntity();
        }

        /// <summary>
        /// Обновление сущности
        /// </summary>
        /// <param name="daoEntity">dao Сущность</param>
        /// <param name="model">Сущность</param>
        /// <returns></returns>
        public override ApplicationUser UpdateDaoInternal(ApplicationUser daoEntity, IApplicationUserModel model)
        {
            daoEntity.Update(model);

            if (daoEntity.EmployeeStatusId > 0)
            {
                var task = _unitOfWork.EmployeeStatusRepository.GetByIdAsync(daoEntity.EmployeeStatusId);
                task.Wait();
                var status = task.Result;

                if (status != null && status.Guid != DatabaseConstant.EmployeeStatusDismissal)
                {
                    daoEntity.DateEnd = null;
                    daoEntity.EmployeeReasonDismissalId = null;
                }
            }

            return daoEntity;
        }

        #endregion

        #region private 

        /// <summary>
        /// Добавить сортировку по Id
        /// </summary>
        /// <param name="sortField"></param>
        /// <returns></returns>
        private string GetSortField(string sortField)
        {
            if (string.IsNullOrEmpty(sortField))
            {
                return string.Empty;
            }

            if (string.Equals(sortField, "EmployeeStatus") 
                || string.Equals(sortField, "Position")
                || string.Equals(sortField, "Platform")
                || string.Equals(sortField, "EmployeeReasonDismissal")
                || string.Equals(sortField, "Department"))
            {
                sortField += "Id";
            }

            return sortField;
        }

        #endregion
    }
}
