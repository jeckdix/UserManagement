using UserManagement.Models;

namespace UserManagement.Models
{
    public class Session
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set;}
        public List<SessionClass> SessionClasses { get; set; }

        public Session (string name)
        {
            Name = name;
        }

    }
}

public class SessionClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int SessionId { get; set; }
    public Session Session { get; set; }


}


public class SessionDto
{
    public string SessionName { get; set; }
  
    public List<SessionClassDto> SessionClasses { get; set;}
}

public class SessionClassDto
{
    public string ClassName { get; set; }
}

