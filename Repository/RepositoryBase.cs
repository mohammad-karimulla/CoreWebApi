﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using WebAPI.Models.DataFirst;

namespace WebAPI.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected OrgDataFirstContext RepositoryContext { get; set; }

        public RepositoryBase(OrgDataFirstContext repositoryContext)
        {
            this.RepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTracking();
        }

        public int GetTotalRecords()
        {
            return this.RepositoryContext.Set<T>().Count();
        }
    }
}
