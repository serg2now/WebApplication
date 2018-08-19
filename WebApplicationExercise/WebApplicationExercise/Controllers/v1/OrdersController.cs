using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplicationExercise.Core;
using System.Web.Http.Tracing;
using WebApplicationExercise.Converters;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.EFModels;
using WebApplicationExercise.Repositories;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http.Results;
using WebApplicationExercise.Models.ModelMapper;

namespace WebApplicationExercise.Controllers.v1
{
    /// <summary>
    /// API Controller for Orders 
    /// </summary>
    [RoutePrefix("api/v1/orders")]
    public class OrdersController : ApiController
    {
        private IRepository<DbOrder> _ordersRepository;
        private CustomerManager _customerManager;
        private ITraceWriter _nLogger;
        private ICurrencyConverter _currencyConverter;
        private IMapper<DbOrder, Order> _mapper;

        #region Constructor for Controller class

        /// <summary>
        /// default constructor for Controller class
        /// </summary>
        public OrdersController(
            IRepository<DbOrder> ordersRepository, 
            CustomerManager customerManager, 
            ITraceWriter logger,
            ICurrencyConverter currencyConverter,
            IMapper<DbOrder, Order> mapper)
        {
            _ordersRepository = ordersRepository;
            _customerManager = customerManager;
            _nLogger = logger;
            _currencyConverter = currencyConverter;
            _mapper = mapper;
        }

        #endregion

        #region Api entrypoint methods

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="orderId">Guid of the order</param>
        /// <param name="currency">currency of price in order (default USD)</param>
        /// <returns>200</returns>
        [HttpGet]
        [Route("{orderId}", Name = "GetOrderById")]
        public async Task<IHttpActionResult> GetOrder(Guid orderId, string currency = "USD")
        {
            IHttpActionResult result = await Task.Run(() =>
            {
                var dbOrder = _ordersRepository.GetItem(orderId);

                IHttpActionResult getResult;

                if (dbOrder != null)
                {
                    currency = currency.ToUpper();

                    var error = (!currency.Equals("USD")) ? _currencyConverter.ConvertPrices(dbOrder.Products, currency) : string.Empty;

                    getResult = !string.IsNullOrEmpty(error)
                        ? BuildHttpResultAndLogError(error)
                        : Ok(_mapper.Map(dbOrder));
                }
                else
                {
                    getResult = BuildHttpResultAndLogError($"Order with Id {orderId} does not exist in database!!");
                }
                          
                return getResult;
            }); 
            
            return result;
        }

        /// <summary>
        /// Get all orders
        /// </summary>
        /// <param name="from">Utc Date from</param>
        /// <param name="to">Utc Date to</param>
        /// <param name="customer">customer name</param>
        /// <param name="currency">currency of product prices (default USD)</param>
        /// <param name="page">page number</param>
        /// <param name="pageSize">size of page</param>
        /// <param name="sort">sorting order</param>
        /// <remarks>example of sorting parametr: sort=-Customer, CreatedDate (will sort by Customer in desc order, by created date in asc order)</remarks>
        /// <returns>200</returns>
        [HttpGet]
        [Route]
        public async Task<IHttpActionResult> GetOrders(
            DateTime? from = null, 
            DateTime? to = null, 
            string customer = null, 
            string currency = "USD", 
            int page = 0, 
            int pageSize = 10,
            string sort = "Customer,CreatedDate")
        {
            IHttpActionResult result = await Task.Run(() =>
            {
                IHttpActionResult getResult;

                try
                {
                    var validateResult = ValidateSortParameters(sort);

                    if (!string.IsNullOrEmpty(validateResult))
                    {
                        throw new ArgumentException(validateResult);
                    }
                        
                    List<DbOrder> dbOrders = _ordersRepository.GetItems(
                                o => (customer == null || customer == o.Customer)
                                     && o.Customer != _customerManager.ManagerName
                                     && ((from == null || o.CreatedDate >= from.Value) && (to == null || o.CreatedDate < to.Value)),
                                     page * pageSize,
                                     pageSize,
                                     sort);

                    currency = currency.ToUpper();

                    var error = (!currency.Equals("USD")) ? _currencyConverter.ConvertPrices(dbOrders, currency) : string.Empty;

                    getResult = !string.IsNullOrEmpty(error)
                        ? BuildHttpResultAndLogError(error)
                        : Ok(_mapper.Map(dbOrders));
                }
                catch (Exception e)
                {
                    getResult = BuildHttpResultAndLogError(e.Message);
                }

                return getResult;
            });

            return result;
        }

        /// <summary>
        /// Add new order
        /// </summary>
        /// <param name="order">Order object</param>
        /// <returns>200</returns>
        [HttpPost]
        [Route]
        public async Task<IHttpActionResult> SaveOrder([FromBody]Order order)
        {
            IHttpActionResult result = await Task.Run(() =>
            {
                IHttpActionResult saveResult;

                try
                {
                    if (order == null)
                    {
                        throw new ArgumentNullException(nameof(order));
                    }
                        
                    var dbOrder = _mapper.Map(order);

                    var resultMessage = _ordersRepository.SaveItem(dbOrder);

                    saveResult = (resultMessage == string.Empty)
                        ? BuildCreatedResult(dbOrder) 
                        : BuildHttpResultAndLogError(resultMessage);
                }
                catch (Exception e)
                {
                    saveResult = BuildHttpResultAndLogError(e.Message);
                }

                return saveResult;
            });

            return result;
        }

        /// <summary>
        /// Update existing order
        /// </summary>
        /// <param name="orderId">Guid of the order</param>
        /// <param name="order">Order object</param>
        /// <returns>200</returns>
        [HttpPut]
        [Route("{orderId}")]
        public async Task<IHttpActionResult> UpdateOrder(Guid orderId, [FromBody]Order order)
        {
            IHttpActionResult result = await Task.Run(() =>
            {
                IHttpActionResult updateResult;

                try
                {
                    if (order == null)
                    {
                        throw new ArgumentNullException(nameof(order));
                    }
                        
                    if (orderId != order.Id)
                    {
                        throw new ArgumentException("orderId is not equal id of passed order object!");
                    }               

                    var dbOrder = _mapper.Map(order);

                    var resultMessage = _ordersRepository.UpdateItem(dbOrder);

                    if (resultMessage == string.Empty)
                    {
                        if (orderId != dbOrder.Id)
                        {
                            updateResult = BuildCreatedResult(dbOrder);
                        }
                        else
                        {
                            updateResult = StatusCode(HttpStatusCode.NoContent);
                        }
                    }
                    else
                    {
                        updateResult = BuildHttpResultAndLogError(resultMessage);
                    }
                }
                catch (Exception e)
                {
                    updateResult = BuildHttpResultAndLogError(e.Message);
                }

                return updateResult;
            });

            return result;
        }

        /// <summary>
        /// Delete order by id
        /// </summary>
        /// <param name="orderId">Guid of the order</param>
        /// <returns>200</returns>
        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IHttpActionResult> DeleteOrder(Guid orderId)
        {
            IHttpActionResult result = await Task.Run(() =>
            {
                IHttpActionResult deleteResult;

                try
                {
                    var order = _ordersRepository.GetItem(orderId, true);

                    if (order != null)
                    {
                        var resultMessage = _ordersRepository.DeleteItem(order);

                        deleteResult = (resultMessage == string.Empty)
                            ? StatusCode(HttpStatusCode.NoContent)
                            : BuildHttpResultAndLogError(resultMessage); 
                    }
                    else
                    {
                        deleteResult = BuildHttpResultAndLogError($"Order with Id {orderId} does not exist in database!!");
                    }
                }
                catch (Exception e)
                {
                    deleteResult = BuildHttpResultAndLogError(e.Message);
                }

                return deleteResult;
            });

            return result;
        }

        #endregion

        #region Help methods

        private IHttpActionResult BuildHttpResultAndLogError(string errorMessage)
        {
            _nLogger.Error(Request, null, errorMessage);

            return BadRequest(errorMessage);
        }

        private CreatedNegotiatedContentResult<Order> BuildCreatedResult(DbOrder order)
        {
            var location = Url.Link("GetOrderById", new { orderId = order.Id });

            return Created(location, _mapper.Map(order));
        }

        private string ValidateSortParameters(string sortString)
        {
            var parameters = Regex.Replace(sortString, "[- ]", string.Empty).Split(',');

            var columns = typeof(DbOrder).GetProperties().Select(p => p.Name);

            var wrongParameters = string.Join(", ", parameters.Except(columns));

            return (!string.IsNullOrEmpty(wrongParameters)) ? $"Columns: '{wrongParameters}' do not exist in Orders table!" : string.Empty;
        }

        #endregion

        #region IDisposable implementation

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ordersRepository.Dispose();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}