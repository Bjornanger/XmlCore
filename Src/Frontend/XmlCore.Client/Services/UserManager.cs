using XmlCore.Shared.Entities;

namespace XmlCore.Client.Services;

public class UserManager
{
    public User? User { get; set; }

    public bool Exists { get; private set; }

    public void Set(User user)
    {
        Exists = true;
        User = user;
    }

    public void Clear()
    {
        Exists = false;
        User = null;
    }

    public User Get()
    {
        return User;
    }
}