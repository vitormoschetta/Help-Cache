using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using App01.Data;
using App01.Models;
using App01.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace App01.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        public OrderController(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public IActionResult Orders()
        {
            var orders = _context.Order.Include(x => x.OrderProducts).ThenInclude(x => x.Product);
            var itensPorPagina = 5;
            PaginatedList<Order> listaPaginada = PaginatedList<Order>.Create(orders, 1, itensPorPagina);
            return View(listaPaginada);
        }

        public async Task<IActionResult> OrdersInCache()
        {
            PaginatedList<Order> listaPaginada;
            var itensPorPagina = 5;

            string ordersCache = await _cache.GetStringAsync("Orders");
            if (ordersCache != null)
            {                
                var listOrders = JsonConvert.DeserializeObject<List<Order>>(ordersCache);
                listaPaginada = PaginatedList<Order>.Create(listOrders, 1, itensPorPagina);
                return View(listaPaginada);
            }                 

            List<Order> orders = await _context.Order
                .Include(x => x.OrderProducts)
                .ThenInclude(x => x.Product)
                .ToListAsync();

            string stringOrders = JsonConvert.SerializeObject(orders);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromSeconds(20),
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync("Orders", stringOrders, options);

            listaPaginada = PaginatedList<Order>.Create(orders, 1, itensPorPagina);
            return View(listaPaginada);
        }

    }
}