namespace Server.Repositories;
public class StudentRepository : Repository<Student>, IStudentRepository
{


    public StudentRepository(SchoolContext context) : base(context)
    {
    }


}