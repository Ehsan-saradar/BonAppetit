using System;
using System.Collections.Generic;
using System.Text;
using BonAppetit.Model.Entities;

namespace BonAppetit.Repository.Implementations
{
    public class BaseRepository
    {
        internal readonly BonAppetitDbContext _context;
        public BaseRepository(BonAppetitDbContext context)
        {
            _context = context;
        }
    }
}
