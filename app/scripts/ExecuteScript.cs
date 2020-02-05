using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace scripting.Scripts
{
    public class Global
    {
        private readonly IServiceProvider serviceProvider;

        public Global(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public T GetService<T>()
        {
            return serviceProvider.GetRequiredService<T>();
        }
    }
    public class Log: IRequest
    {
        public string Message { get; set; }
    }


    public class LogHandler : IRequestHandler<Log>
    {
        private readonly ILogger<LogHandler> logger;

        public LogHandler(ILogger<LogHandler> logger)
        {
            this.logger = logger;
        }
        public Task<Unit> Handle(Log request, CancellationToken cancellationToken)
        {
            logger.LogInformation(request.Message);
            return Unit.Task;
        }
    }

    public class ExecuteScript : IRequest<object>
    {
        public string Script { get; set; }
    }

    public class ExecuteScriptHandler : IRequestHandler<ExecuteScript, object>
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMediator mediator;

        public ExecuteScriptHandler(IServiceProvider serviceProvider, IMediator mediator)
        {
            this.serviceProvider = serviceProvider;
            this.mediator = mediator;
        }
        //@"
        //using Microsoft.ApplicationInsights.Extensibility;
        //using Microsoft.AspNetCore;
        //using Microsoft.AspNetCore.Hosting;
        //using Microsoft.Azure.KeyVault;
        //using Microsoft.Azure.Services.AppAuthentication;
        //using Microsoft.CodeAnalysis.CSharp.Scripting;
        //using Microsoft.CodeAnalysis.Scripting;
        //using Microsoft.EntityFrameworkCore;
        //using Microsoft.EntityFrameworkCore.Infrastructure;
        //using Microsoft.EntityFrameworkCore.Migrations;
        //using Microsoft.Extensions.Configuration;
        //using Microsoft.Extensions.Configuration.AzureKeyVault;
        //using Microsoft.Extensions.DependencyInjection;
        //using Microsoft.Extensions.Logging;
        //using Serilog;
        //using Serilog.Core;
        //using System;
        //using System.Collections.Generic;
        //using System.IO;
        //using System.Linq;
        //using System.Threading.Tasks;
        //using Microsoft.Extensions.DependencyInjection;

        //Database.Migrate();
        //"
        public async Task<object> Handle(ExecuteScript request, CancellationToken cancellationToken)
        {
        
            object result = await CSharpScript.EvaluateAsync(request.Script
                , ScriptOptions.Default
                .WithReferences(
                    typeof(ServiceProviderServiceExtensions).Assembly,
                    typeof(IHostingStartup).Assembly,
                    typeof(Program).Assembly,
                    typeof(Microsoft.Extensions.Logging.ILogger).Assembly,
                    typeof(IServiceScope).Assembly
                ).WithImports(
                    "MediatR",
                    "Microsoft.Extensions.DependencyInjection",
                    "scripting.Scripts"),
                globals: new Global(serviceProvider));
   
            return result;
        }
    }
}
