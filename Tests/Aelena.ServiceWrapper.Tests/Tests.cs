using AElena.ServiceWrapper.SampleService;
using CassiniDev;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace Aelena.ServiceWrapper.Tests
{
    public class Tests : IDisposable
    {

        /// <summary>
        /// CassiniDev, an open source library that allows to run a lightweight ASP.NET web server in process, 
        /// which is especially useful for unit testing scenarios
        /// </summary>
        private readonly CassiniDevServer host;
        /// <summary>
        /// Capture output
        /// <remarks>
        /// https://xunit.github.io/docs/capturing-output.html
        /// </remarks>
        /// </summary>
        private readonly ITestOutputHelper output;

        private const string _endpointNameForUpdateService = "sampleUpdateService";

        private readonly string testUrl = @"http://localhost:8733/Design_Time_Addresses/AElena.ServiceWrapper.SampleService/SampleService";

        private Process wcfTestClientProcess = null;

        private const string wcfSvcHostPath = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\WcfSvcHost.exe";

        // the AElena.ServiceWrapper.SampleService's App.Config "Copy to Output Directory" property
        // must be set set to "Copy always" to ensure both files are available for WcfSvcHost
        private const string wcfSvcHostPara = @"/service:AElena.ServiceWrapper.SampleService.dll /config:App.Config";



        public Tests ( ITestOutputHelper output )
        {
            this.output = output;

            try
            {
                //host = new CassiniDevServer ();
                //host.StartServer ( @"..\..\SampleService\AElena.ServiceWrapper.ISampleService", 8080, "/", "localhost" );
                //this.output.WriteLine ( "Server started" );



                if ( !Process.GetProcesses ().Any ( p => p.ProcessName == "WcfSvcHost" ) )
                    wcfTestClientProcess = Process.Start ( wcfSvcHostPath, wcfSvcHostPara );


            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex.ToString () );
                this.output.WriteLine ( ex.ToString () );
            }
        }



        [Fact]
        public void Should_Call_Service_Using_Wrapper ()
        {
            using ( var factory = new ChannelFactory<ISampleService> ( "sampleServiceEndpoint" ) )
            {
                var channel = factory.CreateChannel ( new EndpointAddress ( new Uri ( testUrl ) ) );
                var response = channel.UpdateStatus ( "John", 2 );
                response.Should ().Be ( "Entity 'John' has been updated to status 2" );
            }

        }





        public void Dispose ()
        {
            try
            {
                if ( host != null )
                    host.StopServer ();
                this.output.WriteLine ( "Server stopped" );

                if ( wcfTestClientProcess != null )
                    wcfTestClientProcess.Kill ();

            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex.ToString () );
                this.output.WriteLine ( ex.ToString () );
            }
        }
    }
}
