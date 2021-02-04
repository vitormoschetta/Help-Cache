using System;
using System.Collections.Generic;
using System.Linq;
using App02.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App02.Data
{
    public class InitializeData
    {
        public static void InitializeProducts(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Product.Any())
                    return;

                var list = new List<Product>();

                for (int i = 1; i < 100; i++)
                {
                    var product = new Product( $"Product {i}", i + i);
                    list.Add(product);
                }

                context.Product.AddRange(list);
                context.SaveChanges();
            }
        }

        public static void InitializeOrders(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Order.Any())
                    return;

                var list = new List<Order>();

                for (int i = 1; i < 50000; i++)
                {
                    var order = new Order("Custommer " + i);
                    list.Add(order);
                }

                context.Order.AddRange(list);
                context.SaveChanges();
            }
        }

        public static void InitializeOrderProducts(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context.Order.Any())
                    return;

                var list = new List<OrderProduct>();
                var product = 1;

                for (int i = 1; i < 10000; i++)
                {                    
                    if (product > 99 )
                        product = 1;

                    var orderProduct = new OrderProduct(i, i);
                    var orderProduct02 = new OrderProduct(i, i > 99 ? 1 : i+1);
                    list.Add(orderProduct);
                    list.Add(orderProduct02);

                    product++;
                }

                context.OrderProduct.AddRange(list);
                context.SaveChanges();
            }
        }
    }
}