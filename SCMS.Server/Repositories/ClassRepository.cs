using Microsoft.EntityFrameworkCore;
namespace Server.Repositories;
public class ClassRepository : Repository<Class>, IClassRepository
{
    public ClassRepository(SchoolContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Class>> GetClassesByTeacherIdAsync(int teacherId)
    {
        return await _context.Classes
                             .Where(c => c.TeacherId == teacherId)
                             .ToListAsync();
    }
}