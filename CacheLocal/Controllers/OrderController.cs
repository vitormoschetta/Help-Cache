using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CacheLocal.Data;
using CacheLocal.Models;
using CacheLocal.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace CacheLocal.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;
        public OrderController(AppDbContext context, IMemoryCache cache)
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
            List<Order> orders;
            PaginatedList<Order> listaPaginada;
            var itensPorPagina = 5;

            var cache = _cache.TryGetValue("Products", out orders);
            if (cache)
            {
                listaPaginada = PaginatedList<Order>.Create(orders, 1, itensPorPagina);
                return View(listaPaginada);
            }

            orders = await _context.Order.Include(x => x.OrderProducts).ThenInclude(x => x.Product).ToListAsync();

            var expiration = DateTime.Now.AddMinutes(30);
            _cache.Set("Products", orders, expiration);

            listaPaginada = PaginatedList<Order>.Create(orders, 1, itensPorPagina);
            return View(listaPaginada);
        }

    }
}