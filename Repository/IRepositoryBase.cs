using System.Linq;

namespace WebAPI.Repository
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        int GetTotalRecords();
    }
}
