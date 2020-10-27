using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BonAppetit.Service.Contracts
{
    public interface IResturantService
    {
        Task<RestaurantUser> GetResturantById(string id);
        IEnumerable<MenuItem> GetMenu(string id);
        bool AddMenu(MenuItem menuItem);
        bool DeleteMenu(long menuId);
        bool EditMenuCount(MenuItem menuItem);
        bool ChangeOpen(string id, bool isopen);
        bool Delete(string id);
        IEnumerable<RestaurantUser> List();
        IEnumerable<Order> ListOrders(string id);
        bool EditOrder(long id,bool? approve,string status);
        bool AddOrder(Order order);
    }
}
