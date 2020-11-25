using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Contracts;
using BonAppetit.Service.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BonAppetit.Service.Implementations
{
    public class CustomerService: ICustomerService
    {
        private readonly ICustomerRepository _userRepository;
        public CustomerService(ICustomerRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<CustomerUser> GetUserById(string id)
        {
            return _userRepository.GetUserById(id);
        }
    }
}
