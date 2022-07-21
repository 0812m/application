using PH.Blog.Contract.IServices;
using PH.Blog.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PH.Core.IOC.Attributes;
using PH.DatabaseAccessor.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;

namespace PH.Blog.Application.ServiceImpl
{
    [AutoInjection]
    public class ConfigurtionSvc : IConfigurtionSvc
    {
        private readonly IRepository<Configurtion> _configRepo;

        public ConfigurtionSvc(IRepository<Configurtion> configRepo)
        {
            _configRepo = configRepo;
        }

        public async Task<Configurtion> GetAsync(string key)
        {
           return await _configRepo.FirstOrDefaultAsync(x => x.Key == key);
        }

        public async Task<IEnumerable<Configurtion>> GetByTypeAsync(string type)
        {
            return await _configRepo.Where(x => x.Type == type).ToListAsync();
        }
    }
}
