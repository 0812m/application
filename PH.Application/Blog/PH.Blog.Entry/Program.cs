await WebApplication
   .CreateBuilder(args)
   .InitApplication()
   .UseDefaultSerilog()
   .Build()
   .RunAsync();