using System.Linq;
using System.Threading.Tasks;
using DBWebAPI.Models.DataFirst;
using DBWebAPI.Models.Pagination;

namespace DBWebAPI.Repository
{
    public class EmployeeRepository : RepositoryBase<Employees>, IEmployeeRepository
    {
        public EmployeeRepository(OrgDataFirstContext repositoryContext) 
            : base(repositoryContext)
        {
        }

        public Task<PagedList<Employees>> GetEmployees(PagingParameters pagingParameters)
        {
            return Task.FromResult(PagedList<Employees>.GetPagedList(FindAll().OrderBy(s => s.EmployeeID), pagingParameters.PageNumber, pagingParameters.PageSize));
        }

        public Task<int> GetEmployeesCount()
        {
            return Task.FromResult(GetTotalRecords());
        }
    }
}
