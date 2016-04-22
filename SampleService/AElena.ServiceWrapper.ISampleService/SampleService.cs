using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AElena.ServiceWrapper.SampleService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class SampleService : ISampleService
    {

        public string UpdateStatus ( string myID, int myStatus )
        {
            return String.Format ( "Entity '{0}' has been updated to status {1}", myID, myStatus );
        }


        // --------------------------------------------------------------------------------------------------


        public Client GetClient ( int id )
        {
            return new Client ( id % 2 == 0 ? "John" : "Paul",
                id,
                id % 2 == 0 ? "The master werkers" : "the Crazy Guild",
                new Random ( DateTime.UtcNow.Millisecond ).NextDouble () * 10000 );

        }
    }
}
