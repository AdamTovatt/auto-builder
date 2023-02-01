using AutoBuilder.Helpers;
using AutoBuilder.Managers;
using AutoBuilder.Models;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sakur.WebApiUtilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoBuilder.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BuildController : ControllerBase
    {
        [HttpPost("/start")]
        public async Task<IActionResult> Build(string applicationName, string apiKey)
        {
            try
            {
                if (EnvironmentHelper.GetEnvironmentVariable("APIKEY") != apiKey)
                    return new ApiResponse("Invalid apiKey provided in query parameter", System.Net.HttpStatusCode.BadRequest);

                ApplicationBuilder builder = await ApplicationBuilder.LoadAsync();

                Application application = builder.GetApplication(applicationName);

                if (application == null)
                    return new ApiResponse(new { message = "No application with name: \"" + applicationName + "\" could be found", applicationCount = builder.Applications.Count }, System.Net.HttpStatusCode.BadRequest);

                builder.Build(application);
                return new ApiResponse("Build started");
            }
            catch (ApiException exception)
            {
                return new ApiResponse(exception);
            }
        }

        [HttpGet("/log")]
        public async Task<IActionResult> GetLog(string applicationName)
        {
            try
            {
                ApplicationBuilder builder = await ApplicationBuilder.LoadAsync();

                Application application = builder.GetApplication(applicationName);

                if (application == null)
                    return new ApiResponse(new { message = "No application with name: \"" + applicationName + "\" could be found", applicationCount = builder.Applications.Count }, System.Net.HttpStatusCode.BadRequest);

                return new ApiResponse(new { log = application.BuildLog }, System.Net.HttpStatusCode.OK);
            }
            catch (ApiException exception)
            {
                return new ApiResponse(exception);
            }
        }

        [HttpGet("/status")]
        public async Task<IActionResult> GetStatus(string apiKey)
        {
            try
            {
                if (EnvironmentHelper.GetEnvironmentVariable("APIKEY") != apiKey)
                    return new ApiResponse("Invalid apiKey provided in query parameter", System.Net.HttpStatusCode.BadRequest);

                try
                {
                    ApplicationBuilder builder = await ApplicationBuilder.LoadAsync();

                    Command temperatureCommand = new Command("vcgencmd measure_temp", WorkingDirectory.Default);
                    Command cpuUsageCommand = new Command("top -n 1 -b -u pi", WorkingDirectory.Default);

                    return new ApiResponse(new
                    {
                        temperature = await temperatureCommand.RunAsync(),
                        cpuUsage = await cpuUsageCommand.RunAsync(),
                        applicationCount = builder.Applications.Count,
                        configPath = ApplicationBuilder.ConfigurationFilePath,
                        builder.Applications
                    }, System.Net.HttpStatusCode.OK);
                }
                catch (Exception exception)
                {
                    return new ApiResponse(new { configurationFilePath = ApplicationBuilder.ConfigurationFilePath, errorMessage = exception.Message }, System.Net.HttpStatusCode.InternalServerError);
                }
            }
            catch (ApiException exception)
            {
                return new ApiResponse(exception);
            }
        }
    }
}
