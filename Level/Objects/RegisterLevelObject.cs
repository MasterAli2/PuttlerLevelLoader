[AttributeUsage(AttributeTargets.Class)]
public class RegisterLevelObject : Attribute
{
    public string Name { get; }

    public RegisterLevelObject(string name)
    {
        Name = name;
    }
}