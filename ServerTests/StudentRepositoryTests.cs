using Microsoft.EntityFrameworkCore;
using Server.Repositories;
using ServerTests;

namespace ServerTest;

public class StudentRepositoryTest
{
    //inmem mock

    private readonly SchoolContext _context;
    private DbContextOptions<TestDbContext> _dbContextOptions;

    private readonly StudentRepository _studentRepository;



    public StudentRepositoryTest() {

        _dbContextOptions = new DbContextOptionsBuilder<TestDbContext>()
         .UseInMemoryDatabase(databaseName: "TestDatabase")
         .Options;

        _studentRepository = new StudentRepository(_context);
    }

}