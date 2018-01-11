using EntityFrameworkCore.BootKit;
using Quartz;
using Quickflow.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    public class CurrencyMarketCrawler : ScheduleJobBase, IScheduleJob
    {
        public override Task Execute(IJobExecutionContext context)
        {
            var dc = DbAgent.InitDc();

            var wf = new WorkflowEngine
            {
                WorkflowId = "6240e1f7-b90b-4e60-8b2c-36130b043390",
                TransactionId = Guid.NewGuid().ToString()
            };

            dc.DbTran(async () => await wf.Run(dc, new { }));

            return Task.CompletedTask;
        }
    }
}
