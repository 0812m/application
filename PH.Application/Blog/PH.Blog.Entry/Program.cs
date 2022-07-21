await WebApplication
   .CreateBuilder()
   .InitApplication(args)
   .UseDefaultSerilog()
   .Build()
   .RunAsync();