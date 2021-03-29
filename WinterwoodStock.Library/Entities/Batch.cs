﻿using System;

namespace WinterwoodStock.Library.Entities
{
    public class Batch
    {
        public int BatchId { get; set; }
        public string FruitType { get; set; }
        public string VarietyType { get; set; }
        public int Quantity { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
        public DateTime CreatedDateTime { get; set; }
    }
}
