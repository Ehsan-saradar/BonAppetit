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
    public class ResturantService : IResturantService
    {
        private readonly IRestaurantUserRepository _restaurantUserRepository;
        public ResturantService(IRestaurantUserRepository restaurantUserRepository)
        {
            _restaurantUserRepository = restaurantUserRepository;
        }
        public Task<RestaurantUser> GetResturantById(string id)
        {
            return _restaurantUserRepository.GetUserById(id);
        }
        public bool UpdateResturantById(RestaurantUser user)
        {
            return _restaurantUserRepository.UpdateResturantById(user);
        }
        public bool AddMenu(MenuItem menuItem)
        {
            return _restaurantUserRepository.AddMenu(menuItem);
        }
        public bool DeleteMenu(long menuId)
        {
            return _restaurantUserRepository.DeleteMenu(menuId);
        }
        public bool ChangeOpen(string id, bool isopen)
        {
            return _restaurantUserRepository.ChangeOpen(id,isopen);
        }
        public bool Delete(string id)
        {
            return _restaurantUserRepository.Delete(id);
        }
        public IEnumerable<MenuItem> GetMenu(string id)
        {
            return _restaurantUserRepository.GetMenu(id);
        }
        public IEnumerable<RestaurantUser> List()
        {
            return _restaurantUserRepository.List();
        }
        public IEnumerable<Order> ListOrders(string id)
        {
            return _restaurantUserRepository.ListOrders(id);
        }
        public bool EditOrder(long id, bool? approve, string status)
        {
            return _restaurantUserRepository.EditOrder(id,approve,status);
        }
        public bool AddOrder(Order order)
        {
            return _restaurantUserRepository.AddOrder(order);
        }
    }
}
