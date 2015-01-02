using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Dto
{
    public class CommandCorrelationDto
    {
        public CommandCorrelationDto() { }
        internal CommandCorrelationDto(CommandCorrelationEntity correlationEntitiy)
        {
            nestedEntity = correlationEntitiy;
            CorrelationId = nestedEntity.RowKey;
            TenantId = nestedEntity.PartitionKey;
            Cmd = nestedEntity.Cmd;
            Rpl = nestedEntity.Rpl;
        }

        private CommandCorrelationEntity nestedEntity;

        public string CorrelationId { get; set; }
        public string TenantId { get; set; }
        public string Cmd { get; set; }
        public string Rpl { get; set; }

        internal CommandCorrelationEntity ToEntity()
        {
            return new CommandCorrelationEntity()
            {
                RowKey = CorrelationId,
                PartitionKey = TenantId,
                Cmd = Cmd,
                Rpl = Rpl
            };
        }

    }
}
