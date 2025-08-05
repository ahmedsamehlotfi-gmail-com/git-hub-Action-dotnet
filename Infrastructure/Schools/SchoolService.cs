using Application.Features.Schools;
using Domain.Entities;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Schools
{
    public class SchoolService : ISchoolService
    {
        private readonly ApplicationDbContext _context;

        public SchoolService(ApplicationDbContext context)
        {
           _context = context;
        }
        public async Task<int> CreateSchoolAsync(School school)
        {
           await _context.AddAsync(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }


        public async Task<int> DeleteSchoolAsync(School school)
        {
            _context.Remove(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }

        public async Task<List<School>> GetSchoolAsync()
        {
          return await _context.schools.ToListAsync();
        }

        public async Task<School> GetSchoolByIdAsync(int schoolId)
        {
            var schoolInDb = await _context.schools.Where(s=>s.Id == schoolId).FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<School> GetSchoolByNameAsync(string name)
        {
            var schoolInDb = await _context.schools.Where(s => s.Name == name).FirstOrDefaultAsync();
            return schoolInDb;
        }

        public async Task<int> UpdateSchoolAsync(School school)
        {
            _context.schools.Update(school);
            await _context.SaveChangesAsync();
            return school.Id;
        }
    }
}
