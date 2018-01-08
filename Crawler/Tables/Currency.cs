using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Crawler.Tables
{
    public class Currency : DbRecord, IDbRecord
    {
        [Required]
        [MaxLength(64)]
        public String Name { get; set; }

        [Required]
        [MaxLength(16)]
        public String Symbol { get; set; }

        [MaxLength(256)]
        public String WebSite { get; set; }

        [MaxLength(256)]
        public String SourceCodeUrl { get; set; }

        public DateTime UpdatedTime { get; set; }
    }
}
