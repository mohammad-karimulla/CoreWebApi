using System.Threading.Tasks;
using DBWebAPI.Models.DataFirst;
using DBWebAPI.Models.Pagination;

namespace DBWebAPI.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<Employees>
    {
        Task<PagedList<Employees>> GetEmployees(PagingParameters pagingParameters);

        Task<int> GetEmployeesCount();
    }
}
