using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Aelena.ServiceWrapper
{
    public sealed class ServiceWrapper<T>
    {

        public static TReturn Use<TReturn> ( Func<T, TReturn> code, IClientChannel channel )
        {

            if ( channel == null )
                throw new ArgumentException ( "channel instance cannot be null" );

            var proxy = ( IClientChannel ) channel;
            bool success = false;

            try
            {
                var _t = code ( ( T ) proxy );
                proxy.Close ();
                success = true;
                return _t;
            }
            finally
            {
                if ( !success )
                {
                    proxy.Abort ();
                }
            }

        }


        // --------------------------------------------------------------------------------------------------


        public static TReturn Use<TReturn> ( Func<T, TReturn> code, ChannelFactory<T> channelFactory, string url )
        {

            if ( channelFactory == null )
                throw new ArgumentException ( "ChannelFactory instance cannot be null" );



            var proxy = ( IClientChannel ) channelFactory.CreateChannel ( new EndpointAddress ( new Uri ( url ) ) );
            bool success = false;

            try
            {
                var _t = code ( ( T ) proxy );
                proxy.Close ();
                success = true;
                return _t;
            }
            finally
            {
                if ( !success )
                {
                    proxy.Abort ();
                }
            }

        }


        // --------------------------------------------------------------------------------------------------


        public static TReturn Use<TReturn> ( Func<T, TReturn> code, string endpointName )
        {
            // instantiate with the name of the endpoint in the .config file
            var _channelFactory = new ChannelFactory<T> ( endpointName );
            IClientChannel proxy = ( IClientChannel ) _channelFactory.CreateChannel ();
            bool success = false;

            try
            {
                var _t = code ( ( T ) proxy );
                proxy.Close ();
                success = true;
                return _t;
            }
            finally
            {
                if ( !success )
                {
                    proxy.Abort ();
                }
            }

        }



        // --------------------------------------------------------------------------------------------------


    }
}
