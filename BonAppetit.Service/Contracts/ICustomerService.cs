
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BonAppetit.Service.Contracts
{
    public interface ICustomerService
    {
        Task<CustomerUser> GetUserById(string id);
    }
}
