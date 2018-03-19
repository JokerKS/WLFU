﻿using JokerKS.WLFU.Entities;
using JokerKS.WLFU.Entities.Helpers;
using JokerKS.WLFU.Entities.Product;
using System.Collections.Generic;

namespace JokerKS.WLFU.Models
{
    public class BasketListModel
    {
        public IEnumerable<BasketProduct> ProductsInBasket { get; set; }
        public Dictionary<int, Image> MainImages { get; set; }
        public decimal SummaryPrice { get; set; }

        public MessageResult DeleteResult { get; set; }

        public List<SelectedProduct> SelectedProducts { get; set; }
    }

    public class SelectedProduct
    {
        public int ProductId { get; set; }
        public bool Checked { get; set; }
        public int Amount { get; set; }
    }
}