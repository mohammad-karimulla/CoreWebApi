using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.DataFirst;
using WebAPI.Models.Pagination;

namespace WebAPI.Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(OrgDataFirstContext repositoryContext) 
            : base(repositoryContext)
        {
        }

        public Task<PagedList<Employee>> GetEmployees(PagingParameters pagingParameters)
        {
            return Task.FromResult(PagedList<Employee>.GetPagedList(FindAll().OrderBy(s => s.EmployeeID), pagingParameters.PageNumber, pagingParameters.PageSize));
        }

        public Task<int> GetEmployeesCount()
        {
            return Task.FromResult(GetTotalRecords());
        }
    }
}
