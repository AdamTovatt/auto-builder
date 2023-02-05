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

                ApplicationConfiguration application = builder.GetApplication(applicationName);

                if (application == null)
                    return new ApiResponse(new { message = "No application with name: \"" + applicationName + "\" could be found", applicationCount = builder.ApplicationCount }, System.Net.HttpStatusCode.BadRequest);

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

                ApplicationConfiguration application = builder.GetApplication(applicationName);

                if (application == null)
                    return new ApiResponse(new { message = "No application with name: \"" + applicationName + "\" could be found", applicationCount = builder.ApplicationCount }, System.Net.HttpStatusCode.BadRequest);

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
                    TopCommand topCommand = await TopCommand.GetCurrentAsync();

                    List<Application> applications = builder.GetApplications(topCommand, await SystemctlListCommand.GetCurrentAsync());

                    return new ApiResponse(new
                    {
                        temperature = await TemperatureCommand.GetCurrentAsync(),
                        applicationCount = applications.Count,
                        configPath = ApplicationBuilder.ConfigurationFilePath,
                        applications,
                        topCommand
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
