using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AElena.ServiceWrapper.SampleService
{

    [DataContract]
    public sealed class Client
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public double PurchasesLastYear { get; set; }

        public Client ()
        { }

        public Client ( string name, int id, string company, double purchasesLastYear )
        {
            this.Name = name;
            this.Id = id;
            this.Company = company;
            this.PurchasesLastYear = purchasesLastYear;
        }

    }
}
