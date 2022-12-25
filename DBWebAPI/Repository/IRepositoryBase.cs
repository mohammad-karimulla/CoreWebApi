using System.Linq;

namespace DBWebAPI.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();

        int GetTotalRecords();
    }
}
