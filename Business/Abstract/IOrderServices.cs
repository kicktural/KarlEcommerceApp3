using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concreate.ErrorResult;
using Entities.DTO.OrderDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IOrderServices
    {
        IResult CheckProducyQuantity(List<OrderCreateDTO> orderList);
        IResult CheckProductQuantityLimit(List<OrderCreateDTO> orderCreateDTOs);
        IResult CreateOrder(List<OrderCreateDTO> orderCreateDTOs);
    }
}
