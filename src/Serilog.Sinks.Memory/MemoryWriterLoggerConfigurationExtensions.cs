// Copyright 2013-2016 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Collections.Generic;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Sinks.Memory;

// ReSharper disable once CheckNamespace
namespace Serilog
{
    /// <summary>
    /// Adds the WriteTo.MemoryWriter() extension method to <see cref="LoggerConfiguration"/>.
    /// </summary>
    public static class MemoryWriterLoggerConfigurationExtensions
    {
        const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}";

        /// <summary>
        /// Write log events to the provided <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="messagesQueue">The queue to write log events to.</param>
        /// <param name="messagesCountLimit">The maximum of number of messages to keep</param>
        /// <param name="restrictedToMinimumLevel">The minimum level for
        ///     events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="outputTemplate">Message template describing the output format.</param>
        /// <param name="formatProvider">Supplies culture-specific formatting information, or null.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        ///     to be changed at runtime.</param>
        /// <returns>Configuration object allowing method chaining.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration MemoryWriter(this LoggerSinkConfiguration sinkConfiguration,
            Queue<string> messagesQueue,
            int messagesCountLimit,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            IFormatProvider formatProvider = null,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (messagesQueue == null) throw new ArgumentNullException(nameof(messagesQueue));
            if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

            var formatter = new MessageTemplateTextFormatter(outputTemplate, formatProvider);
            var sink = new MemorySink(messagesQueue, formatter, messagesCountLimit);
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }

        /// <summary>
        /// Write log events to the provided <see cref="Queue{T}"/>.
        /// </summary>
        /// <param name="sinkConfiguration">Logger sink configuration.</param>
        /// <param name="messagesQueue">The queue to write log events to.</param>
        /// <param name="messagesCountLimit">The maximum of number of messages to keep</param>
        /// <param name="formatter">Text formatter used by sink.</param>
        /// /// <param name="restrictedToMinimumLevel">The minimum level for
        /// events passed through the sink. Ignored when <paramref name="levelSwitch"/> is specified.</param>
        /// <param name="levelSwitch">A switch allowing the pass-through minimum level
        /// to be changed at runtime.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static LoggerConfiguration MemoryWriter(
            this LoggerSinkConfiguration sinkConfiguration,
            ITextFormatter formatter,
            Queue<string> messagesQueue,
            int messagesCountLimit,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            LoggingLevelSwitch levelSwitch = null)
        {
            if (messagesQueue == null) throw new ArgumentNullException(nameof(messagesQueue));
            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            var sink = new MemorySink(messagesQueue, formatter, messagesCountLimit);
            return sinkConfiguration.Sink(sink, restrictedToMinimumLevel, levelSwitch);
        }
    }
}
