using NetArchTest.Rules;

namespace TaskFlow.ArchitectureTests;

public class ArchitectureTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Projects()
    {
        var result = Types.InAssembly(typeof(TaskFlow.Tasks.Domain.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "TaskFlow.Tasks.Application",
                "TaskFlow.Tasks.Infrastructure",
                "TaskFlow.Tasks.Api"
            )
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(TaskFlow.Tasks.Application.AssemblyReference).Assembly).ShouldNot().HaveDependencyOn("TaskFlow.Tasks.Infrastructure").GetResult();
        Assert.True(result.IsSuccessful);
    }


    [Fact]
    public void Api_Should_Not_Depend_On_Domain_Directly()
    {
        var result = Types.InAssembly(typeof(TaskFlow.Tasks.Api.AssemblyReference).Assembly)
            .ShouldNot()
            .HaveDependencyOn("TaskFlow.Tasks.Domain")
            .GetResult();

        Assert.True(result.IsSuccessful);
    }

}
