using System.Threading.Tasks;
using WebAPI.Models.DataFirst;
using WebAPI.Models.Pagination;

namespace WebAPI.Repository
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task<PagedList<Employee>> GetEmployees(PagingParameters pagingParameters);

        Task<int> GetEmployeesCount();
    }
}
