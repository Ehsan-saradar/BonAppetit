using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace BonAppetit.Repository.Contracts
{
    public interface ICustomerRepository
    {
        Task<CustomerUser> GetUserById(string id);
    }
}
