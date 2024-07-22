namespace Server.Repositories;

public class EventRepository : Repository<Event>, IEventRepository
{
    public EventRepository(SchoolContext context) : base(context)
    {
    }


}