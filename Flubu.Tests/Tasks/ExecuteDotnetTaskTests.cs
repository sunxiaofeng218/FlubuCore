﻿using FlubuCore.Context;
using FlubuCore.Tasks.NetCore;
using Xunit;

namespace Flubu.Tests.Tasks
{
    [Collection(nameof(TaskTestCollection))]
    public class ExecuteDotnetTaskTests : TaskTestBase
    {
        private readonly TaskTestFixture _fixture;

        public ExecuteDotnetTaskTests(TaskTestFixture fixture)
            : base(fixture.LoggerFactory)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ExecuteNonExistentCommand()
        {
            ExecuteDotnetTask task = Context
                .CreateTask<ExecuteDotnetTask>("nonexist")
                .DotnetExecutable("C:/Program Files/dotnet/dotnet.exe");

            TaskExecutionException e = Assert.Throws<TaskExecutionException>(() => task.Execute(Context));
        }

        [Fact]
        public void ExecuteWrongArgsCommand()
        {
            ExecuteDotnetTask task = new ExecuteDotnetTask("build")
                .DotnetExecutable("C:/Program Files/dotnet/dotnet.exe")
                .WithArguments("Flubu.NonExtstProj");

            TaskExecutionException e = Assert.Throws<TaskExecutionException>(() => task.Execute(Context));

            Assert.Equal(1, e.ErrorCode);
        }

        [Fact]
        public void ExecuteCommandDotnetNotSet()
        {
            ExecuteDotnetTask task = new ExecuteDotnetTask("help");

            var e = Assert.Throws<TaskExecutionException>(() => task.Execute(Context));
            Assert.Equal(-1, e.ErrorCode);
        }

        [Fact]
        public void ExecuteCommand()
        {
            ExecuteDotnetTask task = Context
                .CreateTask<ExecuteDotnetTask>("help")
                .DotnetExecutable("C:/Program Files/dotnet/dotnet.exe");

            int res = task.Execute(Context);
            Assert.Equal(0, res);
        }

        [Fact]
        public void ExecuteCommandTargetTreeCreate()
        {
            ExecuteDotnetTask task = Dotnet.Restore();
            task.DotnetExecutable("C:/Program Files/dotnet/dotnet.exe");

            int res = task.Execute(Context);
            Assert.Equal(0, res);
        }
    }
}
