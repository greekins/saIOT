﻿using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saiot.Dal.TableEntities
{
    public class TenantEntity : TableEntity
    {
        public string Identifier { get; set; }
    }
}
