﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using ProtoBuf;
using System.IO;

namespace Examples
{
    [ProtoContract]
    public class TraceErrorData
    {
        [ProtoMember(1)]
        public int Foo { get; set; }

        [ProtoMember(2)]
        public string Bar { get; set; }

    }

    
    public class TraceError
    {
        [Fact]
        public void TestTrace()
        {
            TraceErrorData ed = new TraceErrorData {Foo = 12, Bar = "abcdefghijklmnopqrstuvwxyz"};
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, ed);
            byte[] buffer = ms.ToArray();
            Assert.Equal(30, ms.Length);
            MemoryStream ms2 = new MemoryStream();
            ms2.Write(buffer, 0, (int)ms.Length - 5);
            ms2.Position = 0;
            try
            {
                Serializer.Deserialize<TraceErrorData>(ms2);
                Assert.Equal("##fail##", "Should have errored");
            } catch(EndOfStreamException ex)
            {
                Assert.True(ex.Data.Contains("protoSource"), "Missing protoSource");
                Assert.Equal("tag=2; wire-type=String; offset=4; depth=0", ex.Data["protoSource"]);
            } catch(Exception ex)
            {
                Assert.Equal("##fail##", "Unexpected exception: " + ex);
            }
        }
    }
}
