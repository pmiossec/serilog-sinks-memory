# Serilog.Sinks.Memory

The Queue<string> sink for Serilog.


Writes to a specified `System.Collection.Generic.Queue<string>` and by specifying the number of messages kept.

```csharp
var messages = new Queue<string>();

var log = new LoggerConfiguration()
    .WriteTo.MemoryWriter(messages, 100)
    .CreateLogger();
```

* [Documentation](https://github.com/serilog/serilog/wiki)

Copyright &copy; 2019 Serilog Contributors - Provided under the [Apache License, Version 2.0](http://apache.org/licenses/LICENSE-2.0.html).
