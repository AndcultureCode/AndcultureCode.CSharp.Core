using System;
using System.Collections.Generic;
using AndcultureCode.CSharp.Core.Models.Entities.Jobs;
using AndcultureCode.CSharp.Testing.Tests;
using Newtonsoft.Json;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace AndcultureCode.CSharp.Core.Tests.Models.Entities.Jobs
{
    public class JobTest : BaseUnitTest
    {
        #region Setup

        public JobTest(ITestOutputHelper output) : base(output) { }

        #endregion

        #region DeserializeArgs

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeserializeArgs_Job_With_Null_Or_Empty_String_WorkerArgs_Returns_Empty_List(string workerArgs)
        {
            // Arrange
            var job = new Job { WorkerArgs = workerArgs };

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            argList.ShouldSatisfyAllConditions(
                () => argList.ShouldBeOfType<List<object>>(),
                () => argList.ShouldBeEmpty()
            );
        }

        [Fact]
        public void DeserializeArgs_Job_With_Malformed_Json_WorkerArgs_Throws_Error()
        {
            // Arrange
            var job = new Job { WorkerArgs = "\\ Manformed Json \\" };

            // Act & Assert
            Should.Throw<Exception>(() =>
            {
                job.GetAndDeserializeWorkerArgs();
            });
        }

        #endregion

        #region SerializeArgs

        [Fact]
        public void SerializeArgs_With_Null_WorkerArgs_Persists_Null()
        {
            // Arrange
            var job = new Job();

            // Act & Assert
            Should.NotThrow(() =>
            {
                job.SerializeAndSetWorkerArgs(null);
            });

            job.WorkerArgs.ShouldBeNull();
        }

        [Fact]
        public void SerializeArgs_Overwrites_Existing_Worker_Args_When_Passed_Null()
        {
            // Arrange
            var job = new Job
            {
                WorkerArgs = JsonConvert.SerializeObject("Test String")
            };

            // Act & Assert
            job.SerializeAndSetWorkerArgs(null);

            // Assert
            job.WorkerArgs.ShouldBeNull();
        }

        #endregion SerializeArgs

        #region SerializeArgs and DeserializeArgs

        [Fact]
        public void SerializeArgs_And_DeserializeArgs_Job_With_Empty_WorkerArgs_List_Returns_Null_In_List()
        {
            // Arrange
            var args = new List<object> { };
            var job = new Job();
            job.SerializeAndSetWorkerArgs(args);

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            argList.ForEach((e) => e.ShouldBeNull());
        }

        [Fact]
        public void SerializeArgs_Job_Includes_Null_Values()
        {
            // Arrange
            var args = new List<object> { null, null };
            var job = new Job();
            job.SerializeAndSetWorkerArgs(args);

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            argList.ForEach((e) =>
            {
                e.ShouldBeNull();
            });
        }

        [Fact]
        public void SerializeArgs_And_DeserializeArgs_Job_With_Int_WorkerArgs_Returns_Arg_List_With_Int()
        {
            // Arrange
            var args = new List<object> { 1 };
            var job = new Job();
            job.SerializeAndSetWorkerArgs(args);

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            argList.ShouldContain(args[0]);
            argList[0].ShouldBeOfType(args[0].GetType());
        }

        [Fact]
        public void SerializeArgs_And_DeserializeArgs_Job_With_Long_WorkerArgs_Returns_Arg_List_With_Long()
        {
            // Arrange
            var args = new List<object> { 1L };
            var job = new Job();
            job.SerializeAndSetWorkerArgs(args);

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            argList.ShouldContain(args[0]);
            argList[0].ShouldBeOfType(args[0].GetType());
        }

        [Fact]
        public void SerializeArgs_And_DeserializeArgs_Job_With_WorkerArgs_Returns_Arg_List_With_Matching_Types()
        {
            // Arrange
            var args = new List<object> { 1, 1L, "string" };
            var job = new Job();
            job.SerializeAndSetWorkerArgs(args);

            // Act
            var argList = job.GetAndDeserializeWorkerArgs();

            // Assert
            args.ForEach((e) =>
            {
                argList.ShouldContain(e);
            });
        }

        [Fact]
        public void SerializeArgs_And_DeserializeArgs_Job_With_Non_Built_In_Types_WorkerArgs_Throws_Exception()
        {
            // Arrange
            var data = new { Id = 1 };
            var args = new List<object> { data };
            var job = new Job();

            // Act & Assert
            Should.Throw<ArgumentException>(() =>
            {
                job.SerializeAndSetWorkerArgs(args);
            });
        }

        #endregion SerializeArgs and DeserializeArgs
    }
}
