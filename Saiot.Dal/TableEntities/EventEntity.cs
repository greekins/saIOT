using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;


namespace Saiot.Dal.TableEntities 
{
    public class EventEntity : TableEntity
    {
        public EventEntity(string PK, string RK)
        {
            this.PartitionKey = PK;
            this.RowKey = RK;
        }

        public EventEntity()
        {
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public string PriorityLevel { get; set; }
       
    }
}
