﻿using EntityFrameworkCore.BootKit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Crawler.Tables
{
    public class CurrencyMarket : DbRecord, IDbRecord
    {
        [Required]
        [MaxLength(16)]
        public String Symbol { get; set; }

        public Decimal? Price { get; set; }

        public Decimal? Volume24h { get; set; }

        public Decimal? MaxSupply { get; set; }

        public Decimal? AvailableSupply { get; set; }

        public Decimal? TotalSupply { get; set; }
    }
}
