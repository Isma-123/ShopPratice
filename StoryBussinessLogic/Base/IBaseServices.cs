

using System.Linq.Expressions;

namespace StoryBussinessLogic.Base
{
    public interface IBaseServices<TBaseResponse, TSaveDto,  TGetDto, TUpdatedDto>
    {
        Task<TBaseResponse> Add(TSaveDto saveDto);
        Task<TBaseResponse> GetById(int id);
        Task<TBaseResponse> GetAll();
        Task<TBaseResponse> Update(TUpdatedDto updatedDto);

    }
}
