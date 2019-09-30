using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace Serilog.Sinks.Memory
{
    public class MemorySink : ILogEventSink
    {
        readonly object _syncRoot = new object();
        private readonly Queue<string> _messagesQueue;
        private readonly ITextFormatter _textFormatter;
        private readonly int _keepLimit;
        private readonly StringWriter _textWriter;
        private readonly StringBuilder _stringBuilder;

        public MemorySink(Queue<string> messagesQueue, ITextFormatter textFormatter, int messagesCountLimit)
        {
            _messagesQueue = messagesQueue ?? throw new ArgumentNullException(nameof(messagesQueue));
            _textFormatter = textFormatter ?? throw new ArgumentNullException(nameof(textFormatter));
            _keepLimit = messagesCountLimit > 0 ? messagesCountLimit : 10;
            _textWriter = new StringWriter();
            _stringBuilder = _textWriter.GetStringBuilder();
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent == null) throw new ArgumentNullException(nameof(logEvent));
            lock (_syncRoot)
            {
                _textFormatter.Format(logEvent, _textWriter);
                var message = _textWriter.ToString();
                _stringBuilder.Clear();

                if (string.IsNullOrWhiteSpace(message))
                {
                    return;
                }

                _messagesQueue.Enqueue(message);
                if (_messagesQueue.Count > _keepLimit)
                {
                    _messagesQueue.Dequeue();
                }
            }
        }
    }
}
