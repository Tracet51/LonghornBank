using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LonghornBank.Models
{
    public class SearchViewModel
    {
        public String SearchDescription { get; set; }

        public BankingTranactionType SelectedType { get; set; }

        public String SearchAmountBegin { get; set; }

        public String SearchAmountEnd { get; set; }

        public Int32 SearchAmountRange { get; set; }

        public String SearchTransactionNumber { get; set; }

        public String BeginSearchDate { get; set; }

        public String EndSearchDate { get; set; }

        public Int32 DateRange { get; set; }

        public SortingOption SortType { get; set; }
    }


}