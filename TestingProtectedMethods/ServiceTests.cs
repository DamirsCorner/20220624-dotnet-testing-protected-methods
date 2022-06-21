using FluentAssertions;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using ReflectionMagic;

namespace TestingProtectedMethods;

public class ServiceTests
{
    [Test]
    [TestCase("a", "a")]
    public void TestThroughInteraction(string input, string expectedOutput)
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<Service>();
        
        var dependencyMock = mocker.GetMock<IDependency>();
        dependencyMock.Setup(s => s.GetValue()).Returns(input);
        dependencyMock.Setup(s => s.SendValue(It.Is<string>(output => output == expectedOutput)));

        service.WrapperMethod();

        mocker.VerifyAll();
    }

    private class DerivedService : Service
    {
        public DerivedService(IDependency dependency)
            : base(dependency)
        {
        }

        public string ImportantMethodWrapper(string input)
        {
            return base.ImportantMethod(input);
        }
    }

    [Test]
    [TestCase("a", "a")]
    public void TestUsingDerivedClass(string input, string expectedOutput)
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<DerivedService>();

        var result = service.ImportantMethodWrapper(input);
        result.Should().Be(expectedOutput);
    }

    [Test]
    [TestCase("a", "a")]
    public void TestUsingReflection(string input, string expectedOutput)
    {
        var mocker = new AutoMocker();
        var service = mocker.CreateInstance<Service>();

        string result = service.AsDynamic().ImportantMethod(input);
        result.Should().Be(expectedOutput);
    }
}