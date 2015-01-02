using Newtonsoft.Json;
using Saiot.Bll.Dto;
using Saiot.Dal.Repositories;
using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Services
{
    public class ActorService
    {
        internal ActorRepository actorRepository { get; set; }

        public AbstractActorDto RegisterActor(AbstractActorDto dto)
        {
            var actor = actorRepository.FindAbstractActorByName(dto.TenantId, dto.Name).FirstOrDefault();
            if (actor == null)
            {
                actor = new AbstractActorEntity
                {
                    RowKey = Guid.NewGuid().ToString(),
                    Config = string.Empty
                };
            }
            actor.PartitionKey = dto.TenantId;
            actor.Location = dto.Location;
            actor.Name = dto.Name;
            actor.Type = dto.Type;

            actorRepository.InsertOrUpdateAbstractActor(actor);

            dto.ActorId = actor.RowKey;
            dto.Config = actor.Config;
            return dto;
        }

        public void UnregisterActor(string tenantId, string actorName)
        {
            var actor = actorRepository.FindAbstractActorByName(tenantId, actorName).FirstOrDefault();
            actorRepository.DeleteAbstractActor(actor);
        }

        public void UpdateConfig(string tenantId, string actorName, string config)
        {
            var actor = actorRepository.FindAbstractActorByName(tenantId, actorName).First();
            actor.Config = config;
            actorRepository.InsertOrUpdateAbstractActor(actor);
        }

        public string GetConfig(string tenantId, string actorName)
        {
            var actor = actorRepository.FindAbstractActorByName(tenantId, actorName).First();
            return actor.Config;
        }

        public void UpdateLocation(string tenantId, string actorName, string location)
        {
            var actor = actorRepository.FindAbstractActorByName(tenantId, actorName).First();
            actor.Location = location;
            actorRepository.InsertOrUpdateAbstractActor(actor);
        }

        public AbstractActorDto GetActor(string tenantId, string actorId)
        {
            var entity = actorRepository.FindAbstractActorById(tenantId, actorId);
            return new AbstractActorDto(entity);
        }

        public IEnumerable<AbstractActorDto> GetAllActors(string tenantId)
        {
            return (from row in actorRepository.FindAllActors(tenantId)
                    select new AbstractActorDto(row)).ToList(); 
        }
    }
}
