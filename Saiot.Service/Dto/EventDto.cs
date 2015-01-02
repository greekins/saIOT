using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Dto
{
    public class EventDto
    {
        public EventDto() { }
        internal EventDto(EventEntity eventEntity)
        {
            this.nestedEntity = eventEntity;
            Name = eventEntity.Name;
            Type = eventEntity.Type;
            Sender = eventEntity.Sender;
            Body = eventEntity.Body;
            PriorityLevel = eventEntity.PriorityLevel;
            TenantId = eventEntity.PartitionKey;
            EventId = eventEntity.RowKey;
            Timestamp = eventEntity.Timestamp;
        }

        private EventEntity nestedEntity;
        public string Name { get; set; }
        public string Type { get; set; }
        public string Sender { get; set; }
        public string Body { get; set; }
        public string PriorityLevel { get; set; }
        public string TenantId { get; set; }
        public string EventId { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        internal EventEntity ToEntity()
        {
            return new EventEntity
            {
                PartitionKey = TenantId,
                RowKey = EventId,
                Name = Name,
                Sender = Sender,
                Body = Body,
                Type = Type,
                PriorityLevel = PriorityLevel,
                Timestamp = Timestamp
            };
        }

    }
}
