﻿using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class BasketListModel
    {
        public IEnumerable<BasketProduct> ProductsInBasket { get; set; }
        public Dictionary<int, Image> MainImages { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public decimal SummaryPrice { get; set; }
    }
}