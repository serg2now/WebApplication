using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using WebApplicationExercise.Models;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Core
{
    public class CustomDataBaseInitializer : DropCreateDatabaseAlways<MainDataContext>
    {
        protected override void Seed(MainDataContext context)
        {
            List<DbOrder> orders = new List<DbOrder>
            {
                new DbOrder()
                {
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    Customer = "Andrii.R",
                    Products = new List<DbProduct>
                    {
                        new DbProduct { Name = "Printer", Price = 130 },
                        new DbProduct { Name = "LCD", Price = 200 },
                        new DbProduct { Name = "Mouse", Price = 50 },
                        new DbProduct { Name = "Keyboard", Price = 80 },
                    }
                },
                new DbOrder()
                {
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    Customer = "Sergii.T",
                    Products = new List<DbProduct>
                    {
                        new DbProduct { Name = "Graphic Card", Price = 500 },
                        new DbProduct { Name = "SSD", Price = 150 },
                        new DbProduct { Name = "HDD", Price = 110 },
                        new DbProduct { Name = "Motherboard", Price = 190 },
                    }
                },
                new DbOrder()
                {
                    CreatedDate = DateTime.Now.ToUniversalTime(),
                    Customer = "Leonid.K",
                    Products = new List<DbProduct>
                    {
                        new DbProduct { Name = "Xiaomi Redmi 5", Price = 250 },
                        new DbProduct { Name = "Xiaomi Power bank", Price = 40 },
                        new DbProduct { Name = "headphones beats", Price = 110 },
                        new DbProduct { Name = "Vodafone Unlim 3g", Price = 2 },
                    }
                }
            };

            context.Orders.AddOrUpdate(orders.ToArray());
        }
    }
}