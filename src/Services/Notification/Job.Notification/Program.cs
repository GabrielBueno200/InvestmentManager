﻿using Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Job.Notification.Jobs;
using InvestmentManager.Shared.Utilities.Abstractions;
using Job.Notification.Entities;
using InvestmentManager.Shared.Configurations.Extensions;
using InvestmentManager.Shared.Configurations.Settings;
using Microsoft.Extensions.Configuration;
using Job.Notification.Settings;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostContext, configuration) =>
    {
        hostContext.HostingEnvironment.EnvironmentName = environment!;

        configuration.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        services.AddBindedSettings<DatabaseSettings>()
            .AddBindedSettings<EmailSettings>()
            .AddScoped<IRepository<User>, Repository<User>>()
            .AddScoped<IRepository<Product>, Repository<Product>>();

        var dailyCronExpression = context.Configuration["CronExpression"]!;

        services.AddQuartz(configurator =>
        {
            configurator.ScheduleJob<SendEmailNotificationJob>(trigger => trigger
                .WithIdentity("emailJobTrigger", "emailJobGroup")
                .StartNow()
                .WithCronSchedule(dailyCronExpression));
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);
    })
    .RunConsoleAsync();
