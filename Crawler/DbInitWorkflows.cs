using EntityFrameworkCore.BootKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quickflow.Core.Entities;
using Quickflow.Core.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Crawler
{
    public class DbInitWorkflows
    {
        public static void Init()
        {
            var dc = DbAgent.InitDc();

            if (dc.Table<Workflow>().FirstOrDefault() != null) return;

            Directory.GetFiles(Database.ContentRootPath + "\\App_Data\\DbInitializer", "*.Workflows.json")
                .ToList()
                .ForEach(path =>
                {
                    string json = File.ReadAllText(path);
                    var dbContent = JsonConvert.DeserializeObject<JObject>(json);

                    if (dbContent["workflows"] != null)
                    {
                        dc.DbTran(() => DataInitialization.InitWorkflows(dc, dbContent["workflows"].ToList()));
                    }
                });
        }
    }
}
