public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(SchoolContext context) : base(context)
    {
    }


}