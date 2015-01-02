
using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Dto
{
    public class AbstractActorDto 
    {
        public AbstractActorDto() { }
        internal AbstractActorDto(AbstractActorEntity entity)
        {
            TenantId = entity.PartitionKey;
            ActorId = entity.RowKey;
            Type = entity.Type;
            Location = entity.Location;
            Name = entity.Name;
            Config = entity.Config;
        }
        public string TenantId { get; set; }
        public string ActorId { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Config { get; set; }

    }
}
