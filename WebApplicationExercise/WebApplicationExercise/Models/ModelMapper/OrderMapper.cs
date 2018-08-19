using AutoMapper;
using System;
using Unity;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Models.ModelMapper
{
    public class OrderMapper : BaseMapper<DbOrder, Order>
    {
        public OrderMapper(IUnityContainer container)
            : base(container)
        {
        }

        protected override IMappingExpression<DbOrder, Order> ConfigureDirectMapping(IMapperConfigurationExpression configurationExpression)
        {
            var mappingExpresssion = base.ConfigureDirectMapping(configurationExpression);

            mappingExpresssion.ForMember(
                dest => dest.CreatedDate, opt => opt.MapFrom(
                    src => DateTime.SpecifyKind(src.CreatedDate, DateTimeKind.Utc)));
                        
            return mappingExpresssion;
        }

        protected override IMappingExpression<Order, DbOrder> ConfigureBackMapping(IMapperConfigurationExpression configurationExpression)
        {
            var mappingExpression = base.ConfigureBackMapping(configurationExpression);

            mappingExpression.ForMember(
                dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToUniversalTime()));
                       
            return mappingExpression;
        }
    }
}