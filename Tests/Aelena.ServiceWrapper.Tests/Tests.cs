using AElena.ServiceWrapper.SampleService;
using FluentAssertions;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Xunit;
using Xunit.Abstractions;

namespace Aelena.ServiceWrapper.Tests
{
    public class Tests : IDisposable
    {

        /// <summary>
        /// Capture output
        /// <remarks>
        /// https://xunit.github.io/docs/capturing-output.html
        /// </remarks>
        /// </summary>
        private readonly ITestOutputHelper output;

        private const string _endpointNameForUpdateService = "sampleServiceEndpoint";
        private const string _endpointNameForUpdateServiceWithWsdl = "sampleServiceWsdlEndpoint";


        private readonly string testUrl = @"http://localhost:8733/Design_Time_Addresses/AElena.ServiceWrapper.SampleService/SampleService";

        private Process wcfTestClientProcess = null;

        private const string wcfSvcHostPath = @"C:\Program Files (x86)\Microsoft Visual Studio 12.0\Common7\IDE\WcfSvcHost.exe";

        private const string wcfSvcHostPara = @"/service:AElena.ServiceWrapper.SampleService.dll /config:App.Config";


        // --------------------------------------------------------------------------------------------------


        public Tests ( ITestOutputHelper output )
        {
            this.output = output;

            try
            {
                this.output.WriteLine ( "Server started" );
                if ( !Process.GetProcesses ().Any ( p => p.ProcessName == "WcfSvcHost" ) )
                    wcfTestClientProcess = Process.Start ( wcfSvcHostPath, wcfSvcHostPara );
            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex.ToString () );
                this.output.WriteLine ( ex.ToString () );
            }
        }


        // --------------------------------------------------------------------------------------------------


        [Fact]
        public void Should_Call_Service_Using_Factory ()
        {
            using ( var factory = new ChannelFactory<ISampleService> ( "sampleServiceEndpoint" ) )
            {
                var channel = factory.CreateChannel ( new EndpointAddress ( new Uri ( testUrl ) ) );
                var response = channel.UpdateStatus ( "John", 2 );
                response.Should ().Be ( "Entity 'John' has been updated to status 2" );
            }

        }


        // --------------------------------------------------------------------------------------------------


        [Fact]
        public void Should_Call_Service_Using_Wrapper_With_Factory ()
        {
            using ( var factory = new ChannelFactory<ISampleService> ( "sampleServiceEndpoint" ) )
            {
                var response = ServiceWrapper<ISampleService>.Use<String> ( _ =>
                             _.UpdateStatus ( "John", 2 ), factory, testUrl );
                response.Should ().Be ( "Entity 'John' has been updated to status 2" );

            }
        }


        // --------------------------------------------------------------------------------------------------


        [Theory]
        [InlineData ( 1, "Paul", "the Crazy Guild" )]
        [InlineData ( 2, "John", "The master werkers" )]
        [InlineData ( 4, "John", "The master werkers" )]
        public void Should_Call_Service_Returning_Class_Instance_Using_Wrapper_With_Factory ( int id, string expectedName, string expectedCompany )
        {
            using ( var factory = new ChannelFactory<ISampleService> ( "sampleServiceEndpoint" ) )
            {
                var response = ServiceWrapper<ISampleService>.Use<Client> ( _ =>
                             _.GetClient ( id ), factory, testUrl );
                response.Id.Should ().Be ( id );
                response.Name.Should ().Be ( expectedName );
                response.Company.Should ().Be ( expectedCompany );
            }
        }


        // --------------------------------------------------------------------------------------------------


        [Fact]
        public void Should_Call_Service_Via_Wrapper_And_Standard_Svc_Proxy ()
        {
            var proxy = new SampleServiceAsSvcRef1.SampleServiceClient ();
            var response = ServiceWrapper<SampleServiceAsSvcRef1.ISampleService>.Use<String> ( _ => proxy.UpdateStatus ( "Frank", 5 ), Tests._endpointNameForUpdateServiceWithWsdl );
            response.Should ().Be ( "Entity 'Frank' has been updated to status 5" );
        }


        // --------------------------------------------------------------------------------------------------


        [Theory]
        [InlineData ( 1, "Paul", "the Crazy Guild" )]
        [InlineData ( 2, "John", "The master werkers" )]
        [InlineData ( 4, "John", "The master werkers" )]
        public void Should_Call_Service_Via_Wrapper_And_Standard_Svc_Proxy_2 ( int id, string expectedName, string expectedCompany )
        {
            var proxy = new SampleServiceAsSvcRef1.SampleServiceClient ();
            var response = ServiceWrapper<SampleServiceAsSvcRef1.ISampleService>.Use<Client> ( _ => proxy.GetClient ( id ), Tests._endpointNameForUpdateServiceWithWsdl );
            response.Id.Should ().Be ( id );
            response.Name.Should ().Be ( expectedName );
            response.Company.Should ().Be ( expectedCompany );
        }


        // --------------------------------------------------------------------------------------------------

        public void Dispose ()
        {
            try
            {
                if ( wcfTestClientProcess != null )
                    wcfTestClientProcess.Kill ();

                this.output.WriteLine ( "Server stopped" );

            }
            catch ( Exception ex )
            {
                Debug.WriteLine ( ex.ToString () );
                this.output.WriteLine ( ex.ToString () );
            }
        }


        // --------------------------------------------------------------------------------------------------

    }
}
