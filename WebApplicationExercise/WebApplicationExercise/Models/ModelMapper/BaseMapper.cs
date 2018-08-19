using System.Collections.Generic;
using AutoMapper;
using Unity;

namespace WebApplicationExercise.Models.ModelMapper
{
    public abstract class BaseMapper<DbType, ExtType> : IMapper<DbType, ExtType>
    {
        private IUnityContainer _container;
        private IMapper _mapper;

        protected BaseMapper(IUnityContainer container)
        {
            _container = container;
        }

        public void ConfigureMapper(IMapperConfigurationExpression configurationExpression)
        {
            ConfigureDirectMapping(configurationExpression);
            ConfigureBackMapping(configurationExpression);
        }

        public ExtType Map(DbType sourceObject)
        {
            _mapper = _mapper ?? _container.Resolve<IMapper>();

            return _mapper.Map<DbType, ExtType>(sourceObject);
        }

        public List<ExtType> Map(List<DbType> sourceList)
        {
            _mapper = _mapper ?? _container.Resolve<IMapper>();

            return _mapper.Map<List<DbType>, List<ExtType>>(sourceList);
        }

        public DbType Map(ExtType destObject)
        {
            _mapper = _mapper ?? _container.Resolve<IMapper>();

            return _mapper.Map<ExtType, DbType>(destObject);
        }

        protected virtual IMappingExpression<DbType, ExtType> ConfigureDirectMapping(IMapperConfigurationExpression configurationExpression)
        {
            return configurationExpression.CreateMap<DbType, ExtType>();
        }

        protected virtual IMappingExpression<ExtType, DbType> ConfigureBackMapping(IMapperConfigurationExpression configurationExpression)
        {
            return configurationExpression.CreateMap<ExtType, DbType>();
        }
    }
}