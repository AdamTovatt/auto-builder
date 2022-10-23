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
        public IActionResult Build(string applicationName, string apiKey)
        {
            try
            {
                if (EnvironmentHelper.GetEnvironmentVariable("APIKEY") != apiKey)
                    return new ApiResponse("Invalid apiKey provided in query parameter", System.Net.HttpStatusCode.BadRequest);

                ApplicationBuilder builder = ApplicationBuilder.Load();

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
        public IActionResult GetLog(string applicationName)
        {
            try
            {
                ApplicationBuilder builder = ApplicationBuilder.Load();

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
        public IActionResult GetStatus(string apiKey)
        {
            try
            {
                if (EnvironmentHelper.GetEnvironmentVariable("APIKEY") != apiKey)
                    return new ApiResponse("Invalid apiKey provided in query parameter", System.Net.HttpStatusCode.BadRequest);

                ApplicationBuilder builder = ApplicationBuilder.Load();

                return new ApiResponse(new { applicationCount = builder.Applications.Count, configPath = ApplicationBuilder.ConfigurationFilePath, builder.Applications }, System.Net.HttpStatusCode.OK);
            }
            catch (ApiException exception)
            {
                return new ApiResponse(exception);
            }
        }
    }
}
