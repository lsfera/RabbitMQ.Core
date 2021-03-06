﻿using NUnit.Framework;
using System;

namespace RabbitMQ.Client.Unit
{
    internal class TestBasicPublishBatch : IntegrationFixture
    {
        [Test]
        public void TestBasicPublishBatchSend()
        {
            Model.ConfirmSelect();
            Model.QueueDeclare(queue: "test-message-batch-a", durable: false);
            Model.QueueDeclare(queue: "test-message-batch-b", durable: false);
            IBasicPublishBatch batch = Model.CreateBasicPublishBatch();
            batch.Add("", "test-message-batch-a", false, null, new byte[] { });
            batch.Add("", "test-message-batch-b", false, null, new byte[] { });
            batch.Publish();
            Model.WaitForConfirmsOrDie(TimeSpan.FromSeconds(15));
            BasicGetResult resultA = Model.BasicGet("test-message-batch-a", true);
            Assert.NotNull(resultA);
            BasicGetResult resultB = Model.BasicGet("test-message-batch-b", true);
            Assert.NotNull(resultB);
        }
    }
}
