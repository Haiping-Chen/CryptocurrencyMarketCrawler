using EntityFrameworkCore.BootKit;
using Quickflow.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crawler
{
    public class CurrencyMarketCrawler
    {
        public static void Start()
        {
            var dc = DbAgent.InitDc();

            var wf = new WorkflowEngine
            {
                WorkflowId = "6240e1f7-b90b-4e60-8b2c-36130b043390",
                TransactionId = Guid.NewGuid().ToString()
            };

            dc.DbTran(async () => await wf.Run(dc, new { }));
        }
    }
}
