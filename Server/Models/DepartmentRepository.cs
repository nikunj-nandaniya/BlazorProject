using BlazorProject.Shared;
using Microsoft.EntityFrameworkCore;

namespace BlazorProject.Server.Models
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private AppDbContext appDbContext;
        public DepartmentRepository(AppDbContext appDbContext) 
        { 
            this.appDbContext = appDbContext;
        }

        public async Task<Department> GetDepartment(int departmentId)
        {
            return await appDbContext.departments
                        .FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
        }

        public async Task<IEnumerable<Department>> GetDepartments()
        {
            return await appDbContext.departments.ToListAsync();
        }
    }
}
