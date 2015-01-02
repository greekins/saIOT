using Saiot.Bll.Dto;
using Saiot.Core.Storage;
using Saiot.Dal.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Saiot.Bll.Services
{
    public class EventService
    {
        internal EventRepository EventRepository { get; set; }

        internal IStorageProvider StorageProvider { get; set; }

        public void SaveEvent(EventDto eventDto)
        {
            EventRepository.Insert(eventDto.ToEntity());
        }
        
        public IEnumerable<EventDto> GetRecentEvents(string TenantId)
        {
            return (from row in EventRepository.FindEvents(TenantId, 10)
                          select new EventDto(row)).ToList();
        }


    }
}
