using Aelena.ServiceWrapper;
using ServiceWrapper.Nuget.TestConsole.SampleServiceAsSvcRef1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceWrapper.Nuget.TestConsole
{
    class Program
    {

        private const string _endpointNameForUpdateService = "sampleServiceEndpoint";
        private const string _endpointNameForUpdateServiceWithWsdl = "sampleServiceWsdlEndpoint";
        private const string _endpointNameForUpdateServiceWithWsdlAndMessages = "sampleServiceWsdlEndpointWithMessages";


        static void Main ( string [ ] args )
        {

            var proxy = new SampleServiceAsSvcRef1.SampleServiceClient ();

            var proxy2 = new SampleServiceAsSvcRef2.SampleServiceClient ();


            var response1 = ServiceWrapper<SampleServiceAsSvcRef1.ISampleService>.Use<String> ( _ => proxy.UpdateStatus ( "Frank", 5 ), _endpointNameForUpdateServiceWithWsdl );
            Console.WriteLine ( response1 );

            PrintSeparator ();

            var response2 = ServiceWrapper<SampleServiceAsSvcRef1.ISampleService>.Use<Client> ( _ => proxy.GetClient ( 3 ), _endpointNameForUpdateServiceWithWsdl );

            Console.WriteLine ( response2.Id );
            Console.WriteLine ( response2.Name );
            Console.WriteLine ( response2.Company );

            PrintSeparator ();

            var response3 = ServiceWrapper<SampleServiceAsSvcRef2.ISampleService>.Use<SampleServiceAsSvcRef2.UpdateStatusResponse> (
                _ => proxy2.UpdateStatus ( new SampleServiceAsSvcRef2.UpdateStatusRequest ( "John", 15 ) ),
                            _endpointNameForUpdateServiceWithWsdlAndMessages );

            Console.WriteLine ( response3.UpdateStatusResult );

            PrintSeparator ();

            var response4 = ServiceWrapper<SampleServiceAsSvcRef2.ISampleService>.Use<SampleServiceAsSvcRef2.GetClientResponse> (
                _ => proxy2.GetClient ( new SampleServiceAsSvcRef2.GetClientRequest ( 4 ) ),
                            _endpointNameForUpdateServiceWithWsdlAndMessages );

            Console.WriteLine ( response4.GetClientResult.Id );
            Console.WriteLine ( response4.GetClientResult.Name );
            Console.WriteLine ( response4.GetClientResult.Company );

            PrintSeparator ();

            Console.ReadLine ();

        }

        private static void PrintSeparator ()
        {
            Console.WriteLine ( "------------------------------------------------------------------------------" );
        }
    }
}
