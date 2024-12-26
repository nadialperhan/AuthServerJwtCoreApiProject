using AuthServer.Core.Repositories;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data
{
    public class UnitofWork : IUnitOfWork
    {
        private readonly DbContext _context;
        public UnitofWork(AppDbContext context)
        {
            _context = context;
        }
        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
           await _context.SaveChangesAsync();
        }
        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>((AppDbContext)_context);
        }
    }
}
