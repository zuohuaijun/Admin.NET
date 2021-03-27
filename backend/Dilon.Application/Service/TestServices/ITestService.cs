using Dilon.Core.Service;
using Furion.DatabaseAccessor;

namespace Dilon.Application
{
    public interface ITestService
        : IBaseService<long, Test, TestEntityOutputDto, TestAddInputDto, TestUpdateInputDto, TestPageListInputDto, MasterDbContextLocator>
    {

    }
}
