﻿using IsHoroshiki.DAO.DaoEntities.Editable;
using IsHoroshiki.DAO.Repositories.Editable.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace IsHoroshiki.DAO.Repositories.Editable
{
    /// <summary>
    /// Репозиторий Площадка
    /// </summary>
    public class PlatformRepository : BaseRepository<Platform>, IPlatformRepository
    {
        #region Конструктор

        /// <summary>  
        /// Конструктор  
        /// </summary>  
        /// <param name="context">Контекст выполнения БД</param>  
        public PlatformRepository(ApplicationDbContext context)
            : base(context)
        {

        }

        #endregion

        #region IPlatformRepository

        /// <summary>
        /// Получить все площадки для подразделения
        /// </summary>
        /// <param name="subDivisionId">Id подразделения</param>
        public IEnumerable<Platform> GetAllBySubDivision(int subDivisionId)
        {
            return DbSet.Where(p => p.SubDivisionId == subDivisionId).ToList();
        }

        /// <summary>
        /// true - если существует площака, для которой указан пользователь с Id
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public bool IsExistForUser(int userId)
        {
            return DbSet.Any(p => p.UserId == userId);
        }

        /// <summary>
        /// true - если существует площадка, для которой указано подразделение с Id
        /// </summary>
        /// <param name="subDivisionId">Id подразделения</param>
        public bool IsExistForSubDivision(int subDivisionId)
        {
            return DbSet.Any(p => p.SubDivisionId == subDivisionId);
        }

        #endregion

        #region override

        /// <summary>
        /// Действие с сущностью перед добавлением в БД
        /// </summary>
        /// <param name="entity"></param>
        protected override void BeforeInsert(Platform entity)
        {
            SetChildEntity(entity);
        }

        /// <summary>
        /// Действие с сущностью перед обновлением в БД
        /// </summary>
        /// <param name="entity"></param>
        protected override void BeforeUpdate(Platform entity)
        {
            SetChildEntity(entity);
        }

        /// <summary>
        /// Действие с сущностью перед добавлением в БД
        /// </summary>
        /// <param name="entity"></param>
        protected override void LoadChildEntities(Platform entity)
        {
            if (entity == null)
            {
                return;
            }

            Context.Entry(entity).Reference(p => p.PlatformStatus).Load();
            Context.Entry(entity).Reference(p => p.SubDivision).Load();
            Context.Entry(entity).Reference(p => p.User).Load();
            Context.Entry(entity).Collection(p => p.BuyProcesses).Load();
        }

        #endregion

        #region private

        /// <summary>
        /// Действие с сущностью перед обновлением в БД
        /// </summary>
        /// <param name="entity"></param>
        private void SetChildEntity(Platform entity)
        {
            //if (entity.UserId > 0)
            //{
            //    entity.User = Context.Users.Find(entity.UserId);
            //}

            //entity.PlatformStatus = Context.PlatformStatuses.Find(entity.PlatformStatusId);
            //entity.SubDivision = Context.SubDivisions.Find(entity.SubDivisionId);

            //var list = new List<BuyProcess>();
            //if (entity.BuyProcesses != null)
            //{
            //    foreach (var buyProcess in entity.BuyProcesses)
            //    {
            //        if (buyProcess == null)
            //        {
            //            continue;
            //        }

            //        var daoByProccess = Context.BuyProcesses.Find(buyProcess.Id);
            //        if (!list.Contains(daoByProccess))
            //        {
            //            daoByProccess.Platforms = null;
            //            list.Add(daoByProccess);
            //        }
            //    }
            //}
            //entity.BuyProcesses = list;

            //var list = new List<BuyProcess>();
            //if (entity.BuyProcesses != null)
            //{
            //    foreach (var buyProcess in entity.BuyProcesses)
            //    {
            //        if (buyProcess == null)
            //        {
            //            continue;
            //        }

            //        var daoByProccess = Context.BuyProcesses.Find(buyProcess.Id);
            //        if (!list.Contains(daoByProccess))
            //        {
            //            list.Add(daoByProccess);
            //        }
            //    }
            //}
            //entity.BuyProcesses = list;
        }

        #endregion
    }
}
