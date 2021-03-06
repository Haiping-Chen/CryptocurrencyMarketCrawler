﻿using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quartz;
using Quickflow.Core;
using Quickflow.Core.Entities;
using Quickflow.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class CurrencyCrawler : ScheduleJobBase, IScheduleJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            var dc = DbAgent.InitDc();

            var wf = new WorkflowEngine
            {
                WorkflowId = "f0d6bf5d-359a-4903-97ad-2fa9bd8b2997",
                TransactionId = Guid.NewGuid().ToString()
            };

            dc.DbTran(async () => await wf.Run(dc, new { }));

            return Task.CompletedTask;
        }
    }
}
