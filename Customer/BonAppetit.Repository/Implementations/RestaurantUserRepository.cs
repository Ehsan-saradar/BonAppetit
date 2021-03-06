﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonAppetit.Model;
using BonAppetit.Model.Entities;
using BonAppetit.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BonAppetit.Repository.Implementations
{
    public class RestaurantUserRepository : BaseRepository, IRestaurantUserRepository
    {
        public Task<RestaurantUser> GetUserById(string id) =>
           _context
               .RestaurantUsers
               .SingleOrDefaultAsync(m => m.Id == id);

        public RestaurantUserRepository(BonAppetitDbContext context) : base(context)
        {
        }
        public IEnumerable<MenuItem> GetMenu(string id)
        {
            return _context.MenuItems.Where(item => item.RestaurantUserId == id);
        }
        public bool AddMenu(MenuItem menuItem)
        {
            _context.MenuItems.Add(menuItem);
            return _context.SaveChanges() >= 0;
        }
        public bool EditMenuCount(MenuItem menuItem)
        {
            _context.MenuItems.Update(menuItem);
            return _context.SaveChanges() >= 0;
        }
        public bool DeleteMenu(long menuId)
        {
            _context.MenuItems.Remove(_context.MenuItems.Single(item => item.Id == menuId));
            return _context.SaveChanges() >= 0;
        }
        public bool ChangeOpen(string id,bool isopen)
        {
            var rest = _context.RestaurantUsers.Single(item => item.Id == id);
            rest.IsOpen = isopen;
            return _context.SaveChanges()>=0;
        }
        public bool Delete(string id)
        {
            _context.RestaurantUsers.Remove(_context.RestaurantUsers.Single(item=>item.Id == id));
            return _context.SaveChanges()>=0;
        }
        public IEnumerable<RestaurantUser> List()
        {
            return _context.RestaurantUsers.ToList();
        }
        public IEnumerable<Order> ListOrders(string id)
        {
            return _context.OrderItems.Where(item => item.MenuItem.RestaurantUserId == id).Select(item => item.Order).Distinct().ToList();
        }
        public bool EditOrder(long id, bool? approve, string status)
        {
            var order = _context.Orders.Single(item => item.Id == id);
            if (approve.HasValue)
            {
                order.approved = approve;
            }
            if (!string.IsNullOrEmpty(status))
            {
                order.Status = status;
            }
            _context.Orders.Update(order);
            return _context.SaveChanges() >= 0;

        }
        public bool AddOrder(Order order)
        {
            _context.Orders.Add(order);
            return _context.SaveChanges() >= 0;
        }
    }
}
