using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BonAppetit.Repository.Implementations
{
    public class CustomerRepository:BaseRepository , ICustomerRepository
    {
        public Task<CustomerUser> GetUserById(string id) =>
            _context
                .Users
                .SingleOrDefaultAsync(m => m.Id == id);

        public CustomerRepository(BonAppetitDbContext context) : base(context)
        {
        }
    }

}
