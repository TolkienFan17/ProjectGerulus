using Ardalis.Specification;

namespace Gerulus.Core;

public interface IRepository<TRoot> : IRepositoryBase<TRoot>
        where TRoot : AggregateRoot
{
}