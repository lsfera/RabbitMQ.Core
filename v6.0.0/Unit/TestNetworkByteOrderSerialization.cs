﻿using NUnit.Framework;
using RabbitMQ.Util;
using System;

namespace RabbitMQ.Client.Unit
{
    [TestFixture]
    class TestNetworkByteOrderSerialization
    {
        public void Check(byte[] actual, byte[] expected)
        {
            try
            {
                Assert.AreEqual(expected, actual);
            }
            catch
            {
                Console.WriteLine();
                Console.WriteLine("EXPECTED ==================================================");
                DebugUtil.Dump(expected, Console.Out);
                Console.WriteLine("ACTUAL ====================================================");
                DebugUtil.Dump(actual, Console.Out);
                Console.WriteLine("===========================================================");
                throw;
            }
        }

        readonly byte[] expectedDoubleBytes = new byte[] { 63, 243, 190, 118, 200, 180, 57, 88 };
        readonly byte[] expectedSingleBytes = new byte[] { 63, 157, 243, 182 };

        [Test]
        public void TestSingleDecoding()
        {
            Assert.AreEqual(1.234f, NetworkOrderDeserializer.ReadSingle(expectedSingleBytes.AsSpan()));
        }

        [Test]
        public void TestSingleEncoding()
        {
            byte[] bytes = new byte[4];
            NetworkOrderSerializer.WriteSingle(bytes, 1.234f);
            Check(bytes, expectedSingleBytes);
        }

        [Test]
        public void TestDoubleDecoding()
        {
            Assert.AreEqual(1.234, NetworkOrderDeserializer.ReadDouble(expectedDoubleBytes.AsSpan()));
        }

        [Test]
        public void TestDoubleEncoding()
        {
            byte[] bytes = new byte[8];
            NetworkOrderSerializer.WriteDouble(bytes, 1.234);
            Check(bytes, expectedDoubleBytes);
        }

        [Test]
        public void TestWriteInt16_positive()
        {
            byte[] bytes = new byte[2];
            NetworkOrderSerializer.WriteInt16(bytes, 0x1234);
            Check(bytes, new byte[] { 0x12, 0x34 });
        }

        [Test]
        public void TestWriteInt16_negative()
        {
            byte[] bytes = new byte[2];
            NetworkOrderSerializer.WriteInt16(bytes, -0x1234);
            Check(bytes, new byte[] { 0xED, 0xCC });
        }

        [Test]
        public void TestWriteUInt16()
        {
            byte[] bytes = new byte[2];
            NetworkOrderSerializer.WriteUInt16(bytes, 0x89AB);
            Check(bytes, new byte[] { 0x89, 0xAB });
        }

        [Test]
        public void TestReadInt16()
        {
            Assert.AreEqual(0x1234, NetworkOrderDeserializer.ReadInt16(new byte[] { 0x12, 0x34 }.AsSpan()));
        }

        [Test]
        public void TestReadUInt16()
        {
            Assert.AreEqual(0x89AB, NetworkOrderDeserializer.ReadUInt16(new byte[] { 0x89, 0xAB }.AsSpan()));
        }

        [Test]
        public void TestWriteInt32_positive()
        {
            byte[] bytes = new byte[4];
            NetworkOrderSerializer.WriteInt32(bytes, 0x12345678);
            Check(bytes, new byte[] { 0x12, 0x34, 0x56, 0x78 });
        }

        [Test]
        public void TestWriteInt32_negative()
        {
            byte[] bytes = new byte[4];
            NetworkOrderSerializer.WriteInt32(bytes, -0x12345678);
            Check(bytes, new byte[] { 0xED, 0xCB, 0xA9, 0x88 });
        }

        [Test]
        public void TestWriteUInt32()
        {
            byte[] bytes = new byte[4];
            NetworkOrderSerializer.WriteUInt32(bytes, 0x89ABCDEF);
            Check(bytes, new byte[] { 0x89, 0xAB, 0xCD, 0xEF });
        }

        [Test]
        public void TestReadInt32()
        {
            Assert.AreEqual(0x12345678, NetworkOrderDeserializer.ReadInt32(new byte[] { 0x12, 0x34, 0x56, 0x78 }.AsSpan()));
        }

        [Test]
        public void TestReadUInt32()
        {
            Assert.AreEqual(0x89ABCDEF, NetworkOrderDeserializer.ReadUInt32(new byte[] { 0x89, 0xAB, 0xCD, 0xEF }.AsSpan()));
        }

        [Test]
        public void TestWriteInt64_positive()
        {
            byte[] bytes = new byte[8];
            NetworkOrderSerializer.WriteInt64(bytes, 0x123456789ABCDEF0);
            Check(bytes, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 });
        }

        [Test]
        public void TestWriteInt64_negative()
        {
            byte[] bytes = new byte[8];
            NetworkOrderSerializer.WriteInt64(bytes, -0x123456789ABCDEF0);
            Check(bytes, new byte[] { 0xED, 0xCB, 0xA9, 0x87, 0x65, 0x43, 0x21, 0x10 });
        }

        [Test]
        public void TestWriteUInt64()
        {
            byte[] bytes = new byte[8];
            NetworkOrderSerializer.WriteUInt64(bytes, 0x89ABCDEF01234567);
            Check(bytes, new byte[] { 0x89, 0xAB, 0xCD, 0xEF, 0x01, 0x23, 0x45, 0x67 });
        }

        [Test]
        public void TestReadInt64()
        {
            Assert.AreEqual(0x123456789ABCDEF0, NetworkOrderDeserializer.ReadInt64(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 }.AsSpan()));
        }

        [Test]
        public void TestReadUInt64()
        {
            Assert.AreEqual(0x89ABCDEF01234567, NetworkOrderDeserializer.ReadUInt64(new byte[] { 0x89, 0xAB, 0xCD, 0xEF, 0x01, 0x23, 0x45, 0x67 }.AsSpan()));
        }
    }
}
