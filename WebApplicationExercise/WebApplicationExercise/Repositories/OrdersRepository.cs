using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using LinqKit;
using RefactorThis.GraphDiff;
using WebApplicationExercise.Core;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Repositories
{
    public class OrdersRepository : IRepository<DbOrder>
    {
        private readonly MainDataContext _dataContext;

        public OrdersRepository(MainDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        #region IRepository implementation

        public DbOrder GetItem(Guid orderId, bool loadItemsFromDb = false)
        {
            var order = (!loadItemsFromDb)
                ? _dataContext.Orders.AsNoTracking().Include(o => o.Products).SingleOrDefault(o => o.Id == orderId)
                : _dataContext.Orders.Include(o => o.Products).SingleOrDefault(o => o.Id == orderId);

            return order;
        }

        public List<DbOrder> GetItems(Expression<Func<DbOrder, bool>> predicate, int offset, int count, string sortString)
        {
            return _dataContext.Orders.AsNoTracking()
                .Include(o => o.Products)
                .AsExpandable()
                .Where(predicate)
                .ApplySort(sortString)
                .Skip(() => offset)
                .Take(() => count)
                .ToList();          
        }

        public string SaveItem(DbOrder dbOrder)
        {
            string result = string.Empty;

            try
            {
                _dataContext.Orders.Add(dbOrder);
                _dataContext.SaveChanges();
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string DeleteItem(DbOrder order)
        {
            string result;

            try
            {
                DeleteProductsFromOrder(order.Products);

                order.Products.Clear();
                _dataContext.Entry(order).State = EntityState.Deleted;

                _dataContext.SaveChanges();

                result = string.Empty;
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        public string UpdateItem(DbOrder newItem)
        {
            string result;

            try
            {
                if (_dataContext.Orders.AsNoTracking().SingleOrDefault(o => o.Id == newItem.Id) != null)
                {
                    _dataContext.UpdateGraph(newItem, map => map.OwnedCollection(x => x.Products));
                }
                else
                {
                    _dataContext.Orders.Add(newItem);
                }

                _dataContext.SaveChanges();

                result = string.Empty;
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }

        #endregion

        #region Help methods

        private void DeleteProductsFromOrder(List<DbProduct> products)
        {
            for (int i = products.Count - 1; i >= 0; i--)
            {
                _dataContext.Entry(products[i]).State = EntityState.Deleted;
            }

            _dataContext.SaveChanges();
        }

        #endregion

        #region IDisposable implementation

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        #endregion
    }
}