using Microsoft.WindowsAzure.Storage;
using Saiot.Bll.Services;
using Saiot.Core.Security;
using Saiot.Core.Storage;
using Saiot.Dal.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Bll
{
    public class ServiceAssembler
    {

        public ServiceAssembler(IStorageProvider storageProvider, CloudStorageAccount storageAccount)
        {
            StorageProvider = storageProvider;
            StorageAccount = storageAccount;
        }

        public IStorageProvider StorageProvider { get; set; }
        public CloudStorageAccount StorageAccount { get; set; }

        private EventService eventService;
        public EventService GetEventService()
        {
            if (eventService == null)
            {
                eventService = new EventService();
                eventService.EventRepository = getEventRepository();
            }
            return eventService;
        }

        private ActorService actorService;
        public ActorService GetActorService()
        {
            if (actorService == null)
            {
                actorService = new ActorService();
                actorService.actorRepository = getActorRepository();
            }
            return actorService;
        }


        private TenantService tenantService;
        public TenantService GetTenantService()
        {
            if (tenantService == null)
            {
                tenantService = new TenantService();
                tenantService.TenantRepository = getTenantRepository();
                tenantService.CredentialRepository = getCredentialRepository();
                tenantService.SasProducer = new StorageSasProducer(StorageAccount);
            }
            return tenantService;
        }

        private CommandService commandService;
        public CommandService GetCommandService()
        {
            if (commandService == null)
            {
                commandService = new CommandService();
                commandService.CommandRepository = getCommandRepository();
                commandService.CredentialRepository = getCredentialRepository();
            }
            return commandService;
        }


        private EventRepository eventRepository;
        private EventRepository getEventRepository()
        {
            if (eventRepository == null)
            {
                eventRepository = new EventRepository();
                eventRepository.StorageProvider = StorageProvider;
            }
            return eventRepository;
        }

        private ActorRepository actorRepository;
        private ActorRepository getActorRepository()
        {
            if (actorRepository == null) 
            {
                actorRepository = new ActorRepository();
                actorRepository.StorageProvider = StorageProvider;
            }
            return actorRepository;
        }

        private TenantRepository tenantRepository;
        private TenantRepository getTenantRepository()
        {
            if (tenantRepository == null)
            {
                tenantRepository = new TenantRepository();
                var client = StorageAccount.CreateCloudTableClient();
                tenantRepository.Table = client.GetTableReference(TableName.Tenant.ToString());
            }
            return tenantRepository;
        }

        private CredentialRepository credentialRepository;
        private CredentialRepository getCredentialRepository()
        {
            if (credentialRepository == null)
            {
                credentialRepository = new CredentialRepository();
                credentialRepository.StorageProvider = StorageProvider;
            }
            return credentialRepository;
        }

        private CommandRepository commandRepository;
        private CommandRepository getCommandRepository()
        {
            if (commandRepository == null)
            {
                commandRepository = new CommandRepository();
                commandRepository.StorageProvider = StorageProvider;
            }
            return commandRepository;
        }
    }
}
