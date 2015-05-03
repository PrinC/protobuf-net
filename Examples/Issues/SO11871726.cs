﻿using Xunit;
using ProtoBuf.Meta;
using System;
using System.Runtime.Serialization;

namespace Examples.Issues
{
    
    public class SO11871726
    {
        [Fact]
        public void ExecuteWithoutAutoAddProtoContractTypesOnlyShouldWork()
        {
            var model = TypeModel.Create();
            Assert.IsType(typeof(Foo), model.DeepClone(new Foo()));
        }
        [Fact]
        public void ExecuteWithAutoAddProtoContractTypesOnlyShouldFail()
        {
            var msg = Assert.Throws<InvalidOperationException>(() =>
            {
                var model = TypeModel.Create();
                model.AutoAddProtoContractTypesOnly = true;
                Assert.IsType(typeof(Foo), model.DeepClone(new Foo()));
            }).Message;
            Assert.Equal("Type is not expected, and no contract can be inferred: Examples.Issues.SO11871726+Foo", msg);
        }

        [DataContract]
        public class Foo { }
    }
}
