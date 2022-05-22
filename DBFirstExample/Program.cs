using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ReadExample();
            //AddExample();
            //UpdateExample();
            //DeleteExample();
            //LinqSimpleQuery1();
            //LinqSimpleQuery2();
            //LinqSimpleQuery3();
            //LinqSimpleQuery4();
            //LinqJoinQuery();
            //LinqGroupByQuery();
            //LinqGroupByJoinQuery();
            //LinqSingleSingleOrDefaultFirstFirstOrDefault();

            using (var context = new NorthwindEntities2())
            {
                var supplier = context.Suppliers
                    .Include("Products").Include("Products.Categories")
                    .FirstOrDefault(p => p.SupplierID == 7);
                //var product = context.Products.Where(p => p.SupplierID == 7);
                var products = supplier.Products;
                Console.WriteLine("Supplier: " + supplier.CompanyName);

                foreach (var product in products)
                {
                    Console.WriteLine("-ProductName: " + product.ProductName);
                    Console.WriteLine("-CategoryName: " + product.Categories.CategoryName);

                }

            }

            Console.Read();

        }

        private static void LinqSingleSingleOrDefaultFirstFirstOrDefault()
        {
            using (var context = new NorthwindEntities2())
            {
                //var result = context.Products.Where(p => p.ProductID == 1).Single(); //sadece tek değeri alır

                //var result = context.Products.Where(p => p.CategoryID == 1).SingleOrDefault(); //dönen değeri alır birden fazla değer dönmez exception fırlatır hiç değer dönemzse hata vermez

                //var result = context.Products.Where(p => p.CategoryID == 1).First();// birden fazla değer gelirse ilk değeri alır

                var result = context.Products.Where(p => p.CategoryID == 0).FirstOrDefault(); //değer gelmese bile exception fırlatmaz

                Console.WriteLine(result.ProductID + " " + result.ProductName);
            }
        }

        private static void LinqGroupByJoinQuery()
        {
            using (var context = new NorthwindEntities2())
            {
                var result = from prod in context.Products
                             join cat in context.Categories
                                on prod.CategoryID equals cat.CategoryID
                             group new { prod, cat }
                                by new { cat.CategoryName, prod.Discontinued }
                             into g
                             select new
                             {
                                 CategoryName = g.Key.CategoryName,
                                 Discontinued = g.Key.Discontinued,
                                 UnitPrice = g.Sum(x => x.prod.UnitPrice)
                             };

                foreach (var item in result)
                {
                    Console.WriteLine(" CategoryName: " + item.CategoryName +
                        "Is Discontinued: " + item.Discontinued +
                        " UnitPrice: " + item.UnitPrice);
                }

            }
        }

        private static void LinqGroupByQuery()
        {
            using (var context = new NorthwindEntities2())
            {
                var result = from prod in context.Products
                             group prod by prod.CategoryID into g
                             select new
                             {
                                 CategoryId = g.Key,
                                 UnitsInStock = g.Count()
                             };
                foreach (var item in result)
                {
                    Console.WriteLine("CategoryId: " + item.CategoryId + " " +
                        "UnitsInStock: " + item.UnitsInStock);
                }

            }
        }

        private static void LinqJoinQuery()
        {
            using (var context = new NorthwindEntities2())
            {
                var result = from prod in context.Products
                             join cat in context.Categories
                             on prod.CategoryID equals cat.CategoryID
                             where prod.UnitPrice > 75
                             select new
                             {
                                 CategoryName = cat.CategoryName,
                                 ProductId = prod.ProductID,
                                 ProductName = prod.ProductName,
                                 UnitPrice = prod.UnitPrice
                             };


                foreach (var item in result)
                {
                    Console.WriteLine("ProductName: " + item.ProductName + " " +
                       "CategoryName: " + item.CategoryName + " " +
                       "ProductId: " + item.ProductId + " " +
                       "UnitPrice: " + item.UnitPrice);
                }

            }
        }

        private static void NewMethod1()
        {
            using (var context = new NorthwindEntities2())
            {
                var products = context.Products
                    .Where(p => p.ProductName
                    .Contains("Chef") || p.Discontinued != false);

                foreach (var product in products)
                {
                    Console.WriteLine("ProductName: " + product.ProductName + " " +
                       "Discontinued: " + product.Discontinued);
                }

            }
        }

        private static void LinqSimpleQuery3()
        {
            using (var context = new NorthwindEntities2())
            {
                var products = context.Products
                    .Where(p => p.UnitsInStock > 100 && p.QuantityPerUnit
                                    .Contains("bottles"));

                foreach (var product in products)
                {
                    Console.WriteLine("QuantityPerUnit: " + product.QuantityPerUnit + " " +
                       "UnitsInStock: " + product.UnitsInStock);
                }
            }
        }

        private static void NewMethod()
        {
            using (var context = new NorthwindEntities2())
            {
                //ef extension
                //var products = context.Products
                //    .Select(p => new
                //    {
                //        ProductName = p.ProductName,
                //        UnitPrice = p.UnitPrice
                //    });

                //Linq sorgu
                var products = from prod in context.Products
                               select new
                               {
                                   ProductName = prod.ProductName,
                                   UnitPrice = prod.UnitPrice
                               };

                foreach (var item in products)
                {
                    Console.WriteLine("ProductName: " + item.ProductName + " " +
                        "UnitPrice: " + item.UnitPrice);
                }

            }
        }

        private static void LinqSimpleQuery()
        {
            using (var context = new NorthwindEntities2())
            {
                //var products = contex.Products; sorgu çalışmaz

                var products = context.Products.ToList(); // sorgu çalışır


                foreach (var item in products)
                {
                    Console.WriteLine("ProductId: " + item.ProductID + " " +
                        "ProductName: " + item.ProductName);
                }
            }
        }

        private static void DeleteExample()
        {
            using (var context = new NorthwindEntities2())
            {
                var category = context.Categories.Find(9);
                context.Categories.Remove(category);
                context.SaveChanges();
            }

        }

        private static void UpdateExample()
        {
            using (var context = new NorthwindEntities2())
            {
                //find pk ile çalışır(pk'ya göre satırın değeri gelir)
                var category = context.Categories.Find(9);
                category.CategoryName = "Sport";
                category.Description = "This is Sport category";

                context.SaveChanges();
            }
        }

        private static void AddExample()
        {
            using (var context = new NorthwindEntities2())
            {
                Categories category = new Categories()
                { CategoryName = "Information", Description = "This is an IT category" };
                context.Categories.Add(category);
                context.SaveChanges();
            }
        }

        private static void ReadExample()
        {
            NorthwindEntities2 context = new NorthwindEntities2();

            var categories = context.Categories.ToList();

            foreach (var item in categories)
            {
                Console.WriteLine(item.CategoryName);
            }
        }
    }
}
