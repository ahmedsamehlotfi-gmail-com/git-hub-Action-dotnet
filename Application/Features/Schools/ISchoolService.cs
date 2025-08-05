using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Schools
{
    public interface ISchoolService
    {
        Task<int> CreateSchoolAsync(School school);
        Task<int> UpdateSchoolAsync(School school);
        Task<int> DeleteSchoolAsync(School school);

        Task<School> GetSchoolByIdAsync(int schoolId);
        Task<List<School>> GetSchoolAsync();
        Task<School> GetSchoolByNameAsync(string name);
    }
}
