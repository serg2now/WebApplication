using System.Collections.Generic;
using AutoMapper;

namespace WebApplicationExercise.Models.ModelMapper
{
    /// <summary>
    /// Interface for model mapper class
    /// </summary>
    /// <typeparam name="DbType">Internal type for EF</typeparam>
    /// <typeparam name="ExtType">Type for data contract</typeparam>
    public interface IMapper<DbType, ExtType>
    {
        void ConfigureMapper(IMapperConfigurationExpression configurationExpression);

        ExtType Map(DbType sourceObject);

        List<ExtType> Map(List<DbType> sourceList);

        DbType Map(ExtType destObject);
    }
}
