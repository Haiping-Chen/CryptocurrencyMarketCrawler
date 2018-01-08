using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Crawler
{
    public class DbAgent
    {
        public static Database InitDc()
        {
            var dc = new Database();

            string db = Database.Configuration.GetSection("Database:Default").Value;
            string connectionString = Database.Configuration.GetSection("Database:ConnectionStrings")[db];

            dc.BindDbContext<IDbRecord, DbContext4SqlServer>(new DatabaseBind
            {
                MasterConnection = new SqlConnection(connectionString),
                CreateDbIfNotExist = true
            });

            return dc;
        }
    }
}
