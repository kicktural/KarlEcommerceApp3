using AutoMapper;
using Business.Abstract;
using Core.Utilities.Business;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Core.Utilities.Results.Concreate.SuccessResult;
using DataAccess.Abstract;
using Entities.Concreate;
using Entities.DTO.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concreate
{
    public class OrderManager : IOrderServices
    {

        private readonly IOrderDAL _orderDAL;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        public OrderManager(IOrderDAL orderDAL, IMapper mapper, IProductService productService)
        {
            _orderDAL = orderDAL;
            _mapper = mapper;
            _productService = productService;
        }

        public IResult CreateOrder(List<OrderCreateDTO> orderCreateDTOs)
        {
            try
            {
                var result = BusinessRules.Check(CheckProducyQuantity(orderCreateDTOs));
                if (!result.Success)
                {
                    return new ErrorResult("There was a mistake when ordering!");
                }
                var map = _mapper.Map<List<Order>>(orderCreateDTOs);
                _orderDAL.OrderAddRange(map);
                return new SuccessResult("Successfully Order");
            }
            catch (Exception ex)
            {

                return new ErrorResult(message: ex.Message);
            }
        }


        public IResult CheckProducyQuantity(List<OrderCreateDTO> orderCreateDTOs)
        {
            try
            {
                foreach (var item in orderCreateDTOs)
                {
                    var result = _productService.GetProductByIdQuantity(item.ProductId);
                    if (result.Data == 0)
                    {
                        return new ErrorResult("There is no such product");
                    }
                }
                return new SuccessResult("There is such a product");
            }
            catch (Exception ex)
            {

                return new ErrorResult(message: ex.Message);
            }
        }


        public IResult CheckProductQuantityLimit(List<OrderCreateDTO> orderCreateDTOs)
        {
            foreach (var item in orderCreateDTOs)
            {
                if (item.ProductQuantity > 10)
                {
                    return new ErrorResult();
                }
            }
            return new SuccessResult();
        }

        IResult IOrderServices.CheckProducyQuantity(List<OrderCreateDTO> orderList)
        {
            try
            {
                foreach (var item in orderList)
                {
                    var result = _productService.GetProductByIdQuantity(item.ProductId);
                    if (result.Data == 0)
                    {
                        return new ErrorResult("There is no such product");
                    }
                }
                return new SuccessResult("There is such a product");
            }
            catch (Exception ex)
            {

                return new ErrorResult(message: ex.Message);
            }
        }
    }
}

