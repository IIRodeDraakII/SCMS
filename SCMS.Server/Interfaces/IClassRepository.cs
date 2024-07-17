public interface IClassRepository : IRepository<Class>
{
    Task<IEnumerable<Class>> GetClassesByTeacherIdAsync(int teacherId);
}