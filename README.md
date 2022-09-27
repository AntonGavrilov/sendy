# Sendy
Sendy is small and lightweight solution for CQRS. 

Fast synchronous and asynchronous handlers of commands and queries.

### Installing Sendy

Command line interface:

    dotnet add package Sendy
    
..or download directly from nuget [Sendy](https://www.nuget.org/packages/Sendy):     

### Configuration

First, you need to configure Sendy in your Startup.cs：

```cs
public void ConfigureServices(IServiceCollection services)
{
    //......

    services.AddSendy(typeof(Program).Assembly);
}
```
### Add command or query class

Add command or query class derving for 'IRequest' or 'IAsyncRequest'. If you want create command use 'IRequest'('IAsyncRequest') interface or if you want query use 'IRequest<>'('IAsyncRequest<>')

Sync example:
```cs
public class TestResult
{
   ... properties
}
public class TestSyncCommandWithResult : IRequest<TestResult>
{
   ... properties
}

public class TestSyncCommand : IRequest
{
   ... properties
}


```
Async example:

```cs
public class TestResult
{
   ... properties
}
public class TestAsyncCommandWithResult : IAsyncRequest<TestResult>
{
   ... properties
}

public class TestAsyncCommand : IAsyncRequest
{
   ... properties
}


```
### Add handler for created classes
Add command handlers or query handlers class deriving form 'IHandler' or 'IAsyncHandler'. For command handler use 'IRequest'('IAsyncHandler') interface or if you want query use 'IHandler<>'('IAsyncRequest<>')

Sync example:
```cs
public class TestSyncCommandWithResultHandler : IHandler<TestSyncCommandWithResult, SearchResultDto>
{
   private readonly ILogger<TestSyncCommandHandler> _logger;
   public TestSyncCommandWithResultHandler(ILogger<TestSyncCommandHandler> logger)
   {
      _logger = logger;
   }
   public SearchResultDto Handle(TestSyncCommandWithResult request)
   {
      _logger.LogInformation("TestSyncCommandWithResult");
      return new SearchResultDto();
   }
}

public class TestSyncCommandHandler : IHandler<TestSyncCommand>
{
   private readonly ILogger<TestSyncCommandHandler> _logger;

   public TestSyncCommandHandler(ILogger<TestSyncCommandHandler> logger)
   {
      _logger = logger;
   }

   public void Handle(TestSyncCommand request)
   {
      _logger.LogInformation("TestSyncCommand");
   }
}


```
Async example:

```cs
public class TestResult
{
   ... properties
}
public class TestAsyncCommandWithResult : IAsyncRequest<TestResult>
{
   ... properties
}

public class TestAsyncCommand : IAsyncRequest
{
   ... properties
}
