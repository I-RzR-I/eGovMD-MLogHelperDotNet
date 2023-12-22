using System;
using MLogHelperDotNet.Models;
using MLogHelperDotNet.Services;

namespace TestConsoleNet45
{
    class Program
    {
        static void Main(string[] args)
        {
            var logInstance = new MLogRestClientService();

            var opt = new RemoteMLogOptions()
            {
                ServiceTimeoutInMinute = "1",
                ServiceCertificatePassword = "pass",
                ServiceClientAddress = "https://lg.lg",
                ServiceCertificatePath = "temp/app"
            };
            var logInstance1 = new MLogRestClientService(ref opt);

            var log = logInstance.RegisterEvent(new MLogEventDto()
            {
                EventCorrelation = Guid.NewGuid().ToString(),
                EventID = Guid.NewGuid().ToString(),
                EventTime = DateTime.Now,
                Subject = "test"
            });

            Console.WriteLine(log.Response);
            Console.ReadKey();
        }
    }
}
