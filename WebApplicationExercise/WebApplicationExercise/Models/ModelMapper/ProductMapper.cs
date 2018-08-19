using Unity;
using WebApplicationExercise.Models.ContractModels;
using WebApplicationExercise.Models.EFModels;

namespace WebApplicationExercise.Models.ModelMapper
{
    public class ProductMapper : BaseMapper<DbProduct, Product>
    {
        public ProductMapper(IUnityContainer container)
            : base(container)
        {
        }
    }
}