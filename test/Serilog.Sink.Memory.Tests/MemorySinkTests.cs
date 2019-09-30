using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Serilog.Sinks.Memory.Tests
{
    public class MemorySinkTests
    {
        [Fact]
        public void ShouldKeepEvents()
        {
            var messagesQueue = new Queue<string>();

            var log = new LoggerConfiguration()
                .WriteTo.MemoryWriter(messagesQueue, 2, outputTemplate:"{Message}")
                .CreateLogger();

            log.Information("my test string");

            messagesQueue.ShouldNotBeEmpty();
            messagesQueue.Count.ShouldBe(1);
            messagesQueue.ToArray()[0].ShouldBe("my test string");
        }

        [Fact]
        public void ShouldRespectLimitOfEventsKept()
        {
            var messagesQueue = new Queue<string>();

            var log = new LoggerConfiguration()
                .WriteTo.MemoryWriter(messagesQueue, 2, outputTemplate: "{Message}")
                .CreateLogger();

            log.Information("1");
            log.Warning("2");
            log.Error("3");

            messagesQueue.Count.ShouldBe(2);
            var messages = messagesQueue.ToArray();
            messages[0].ShouldBe("2");
            messages[1].ShouldBe("3");
        }
    }
}
