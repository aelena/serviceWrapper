using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace AElena.ServiceWrapper.SampleService
{


    [ServiceContract ( Namespace = ConfigurationConstants.ServiceNamespace )]
    public interface ISampleService
    {

        [OperationContract]
        string UpdateStatus ( string myID, int myStatus );

    }
}
