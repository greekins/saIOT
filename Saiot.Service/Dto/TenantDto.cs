using Saiot.Dal.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll.Dto
{
    public class TenantDto
    {
        public TenantDto() { }
        internal TenantDto(TenantEntity tenantEntity)
        {
            TenantId = tenantEntity.PartitionKey;
            Username = tenantEntity.RowKey;
        }

        public string TenantId { get; set; }
        public string Username { get; set; }

        internal TenantEntity ToEntity()
        {
            return new TenantEntity
            {
                PartitionKey = TenantId,
                RowKey = Username
            };
        }
    }
}
