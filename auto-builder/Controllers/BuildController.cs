using AutoBuilder.Managers;
using AutoBuilder.Models;
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
        public IActionResult Build(string applicationName)
        {
            try
            {
                ApplicationBuilder builder = ApplicationBuilder.Load();

                Application application = builder.GetApplication(applicationName);

                if (application == null)
                    return new ApiResponse("No application with name: \"" + applicationName + "\" could be found", System.Net.HttpStatusCode.BadRequest);

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
                    return new ApiResponse("No application with name: \"" + applicationName + "\" could be found", System.Net.HttpStatusCode.BadRequest);

                return new ApiResponse(new { log = application.BuildLog }, System.Net.HttpStatusCode.OK);
            }
            catch (ApiException exception)
            {
                return new ApiResponse(exception);
            }
        }
    }
}
