using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace BonAppetit.Repository.Contracts
{
    public interface IRestaurantUserRepository
    {
        Task<RestaurantUser> GetUserById(string id);
        IEnumerable<MenuItem> GetMenu(string id);
        bool AddMenu(MenuItem menuItem);
        bool DeleteMenu(long menuId);
        bool ChangeOpen(string id, bool isopen);
        bool Delete(string id);
        IEnumerable<RestaurantUser> List();
        IEnumerable<Order> ListOrders(string id);
        bool EditOrder(long id, bool? approve, string status);
        bool AddOrder(Order order);
        bool UpdateResturantById(RestaurantUser user);
    }
}
