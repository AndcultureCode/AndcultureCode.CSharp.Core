using Shouldly;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using AndcultureCode.CSharp.Testing;
using AndcultureCode.CSharp.Testing.Extensions;
using AndcultureCode.CSharp.Core.Models;
using AndcultureCode.CSharp.Core.Extensions;
using AndcultureCode.CSharp.Core.Interfaces;
using AndcultureCode.CSharp.Core.Enumerations;
using AndcultureCode.CSharp.Core.Utilities.Hosting;
using AndcultureCode.CSharp.Testing.Tests;

namespace AndcultureCode.CSharp.Core.Tests.Unit.Utilities.Configuration
{
    public class AndcultureCodeWebHostTest : CoreUnitTest
    {
        #region Setup

        public AndcultureCodeWebHostTest(ITestOutputHelper output) : base(output) { }

        #endregion Setup


        #region Preload

        [Fact]
        public void Preload_When_Args_Null_Returns_Builder()
        {
            AndcultureCodeWebHost.Preload(args: null).ShouldNotBeNull();
        }

        [Fact]
        public void Preload_When_Args_Empty_Array_Returns_Builder()
        {
            AndcultureCodeWebHost.Preload(args: new string[] { }).ShouldNotBeNull();
        }

        [Fact]
        public void Preload_When_Args_Array_Returns_Builder()
        {
            AndcultureCodeWebHost.Preload(args: new string[] { Random.String() }).ShouldNotBeNull();
        }

        #endregion Preload
    }
}
