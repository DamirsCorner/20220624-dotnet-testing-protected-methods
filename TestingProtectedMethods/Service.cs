namespace TestingProtectedMethods;

public class Service
{
    private readonly IDependency dependency;

    public Service(IDependency dependency)
    {
        this.dependency = dependency;
    }

    public void WrapperMethod()
    {
        var original = this.dependency.GetValue();
        var processed = this.ImportantMethod(original);
        this.dependency.SendValue(processed);
    }

    protected string ImportantMethod(string input)
    {
        return input;
    }
}
