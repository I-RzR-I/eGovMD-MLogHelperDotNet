using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AggregatedGenericResultMessage.Web;
using MLogHelperDotNet.Abstractions;
using MLogHelperDotNet.Enums;
using MLogHelperDotNet.Models;

namespace WebApiNet5.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ResultBaseApiController
    {
        /// <summary>
        ///     Client factory
        /// </summary>
        private readonly Func<EndpointType, IMLogEndpointClientService> _clientFactory;

        public WeatherForecastController(Func<EndpointType, IMLogEndpointClientService> clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost]
        public IActionResult SendSoap()
        {
            var endpoint = _clientFactory(EndpointType.SOAP);
            var result = endpoint.RegisterEvent("Test");

            return JsonResult(result);
        }

        [HttpPost]
        public IActionResult SendRest()
        {
            var endpoint = _clientFactory(EndpointType.REST);
            var result = endpoint.RegisterEvent("Test");

            return JsonResult(result);
        }

        [HttpPost]
        public IActionResult SendRest1()
        {
            var endpoint = _clientFactory(EndpointType.REST);
            var result = endpoint.RegisterEvent(new MLogEventDto()
            {
                User = "testUser",
                EventCorrelation = Guid.Empty.ToString(),
                EventID = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now,
                EventType = "Info",
                EventDetails = new List<int>() { 1, 3, 5 },
                Object = new Dictionary<string, decimal>()
                {
                    {"1.1", 0.2M}, {"1.2", 0.3M}
                }
            });

            return JsonResult(result);
        }
    }
}
