using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LonghornBank.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using System.Diagnostics;
using System.Security.Principal;

namespace LonghornBank.Utility
{
    public static class SearchTransactions
    {
        public static List<BankingTransaction> Search(AppDbContext db, SearchViewModel TheSearch, Int32 AccountChoice, Int32 AccountID)
        {
            List<BankingTransaction> Transactions = new List<BankingTransaction>();

            if (AccountChoice == 1)
            {
                var AccountQuery = from a in db.BankingTransaction
                                   from b in a.CheckingAccount
                                   where b.CheckingID == AccountID
                                   select a;

                Transactions.AddRange(AccountQuery.ToList());
            }
            else if (AccountChoice == 2)
            {
                var AccountQuery = from st in db.BankingTransaction
                                   from sa in st.SavingsAccount
                                   where sa.SavingID == AccountID
                                   select st;

                Transactions.AddRange(AccountQuery.ToList());
            }

            else if (AccountChoice == 3)
            {
                var AccountQuery = from it in db.BankingTransaction
                                   from ia in it.IRAAccount
                                   where ia.IRAID == AccountID
                                   select it;

                Transactions.AddRange(AccountQuery.ToList());
            }
            else if (AccountChoice == 4)
            {
                var AccountQuery = from st in db.BankingTransaction
                                   where st.StockAccount.StockAccountID == AccountID
                                   select st;

                Transactions.AddRange(AccountQuery.ToList());
            }

            else
            {
                List<BankingTransaction> All = db.BankingTransaction.ToList();

                Transactions.AddRange(All);
            }

            
            if (TheSearch.SearchDescription != null)
            {
                Transactions = Transactions.Where(c => c.Description.Contains(TheSearch.SearchDescription)).ToList();

            }

            if (TheSearch.SelectedType != BankingTranactionType.None)
            {
                //Search for transactions with type deposit
                if (TheSearch.SelectedType == BankingTranactionType.Deposit)
                {

                    Transactions = Transactions.Where(c => c.BankingTransactionType == BankingTranactionType.Deposit).ToList();
                }

                //Search for transactions with type withdrawl
                if (TheSearch.SelectedType == BankingTranactionType.Withdrawl)
                {
                    Transactions = Transactions.Where(c => c.BankingTransactionType == BankingTranactionType.Withdrawl).ToList();
                }

                //Search for transactions with type transfer
                if (TheSearch.SelectedType == BankingTranactionType.Transfer)
                {
                    Transactions = Transactions.Where(c => c.BankingTransactionType == BankingTranactionType.Transfer).ToList();
                }

                //Search for transactions with type fees
                if (TheSearch.SelectedType == BankingTranactionType.Fee)
                {
                    Transactions = Transactions.Where(c => c.BankingTransactionType == BankingTranactionType.Fee).ToList();
                }

                if (TheSearch.SelectedType == BankingTranactionType.BillPayment)
                {
                    Transactions = Transactions.Where(c => c.BankingTransactionType == BankingTranactionType.BillPayment).ToList();
                }


            }

            // Convert them to a string
            if (!String.IsNullOrEmpty(TheSearch.SearchAmountBegin) && !String.IsNullOrEmpty(TheSearch.SearchAmountEnd) && (TheSearch.SearchAmountRange == 0 || TheSearch.SearchAmountRange == 5))
            {
                Decimal AmountBegin = Convert.ToDecimal(TheSearch.SearchAmountBegin);
                Decimal AmountEnd = Convert.ToDecimal(TheSearch.SearchAmountEnd);

                if (AmountBegin >= 0 && AmountEnd > AmountBegin && (TheSearch.SearchAmountRange == 0 || TheSearch.SearchAmountRange == 5))
                {
                    Transactions = Transactions.Where(c => c.Amount >= AmountBegin && c.Amount <= AmountEnd).ToList();
                }
            }

            if (TheSearch.SearchAmountRange != 0 && TheSearch.SearchAmountRange != 5)
            {
                //For 0 to 100
                if (TheSearch.SearchAmountRange == 1)
                {
                    Transactions = Transactions.Where(c => c.Amount >= 0 && c.Amount <= 100).ToList();
                }

                //For 100 to 200
                if (TheSearch.SearchAmountRange == 2)
                {
                    Transactions = Transactions.Where(c => c.Amount >= 100 && c.Amount <= 200).ToList();
                }

                //For 200 to 300
                if (TheSearch.SearchAmountRange == 3)
                {
                    Transactions = Transactions.Where(c => c.Amount >= 200 && c.Amount <= 300).ToList();
                }

                //For 300+
                if (TheSearch.SearchAmountRange == 4)
                {
                    Transactions = Transactions.Where(c => c.Amount >= 300).ToList();
                }
            }

            if (!String.IsNullOrEmpty(TheSearch.BeginSearchDate) && !String.IsNullOrEmpty(TheSearch.EndSearchDate) && (TheSearch.DateRange == 0 || TheSearch.DateRange == 4))
            {
                DateTime BeginDate;
                DateTime EndDate;
                try
                {
                    // Convert to Datetime 
                    BeginDate = Convert.ToDateTime(TheSearch.BeginSearchDate);
                    EndDate = Convert.ToDateTime(TheSearch.EndSearchDate);

                    if (BeginDate < DateTime.Today && EndDate > BeginDate && (TheSearch.DateRange == 0 || TheSearch.DateRange == 4))
                    {
                        Transactions = Transactions.Where(c => c.TransactionDate >= BeginDate && c.TransactionDate <= EndDate).ToList();
                    }
                }
                catch (Exception)
                {
                }
            }

            //0 should indicate searching for all dates
            if (TheSearch.DateRange != 0)
            {
                //Last 15 days
                if (TheSearch.DateRange == 1)
                {
                    Transactions = Transactions.Where(c => c.TransactionDate >= DateTime.Today.AddDays(-15)).ToList();
                }

                //Last 30 days
                if (TheSearch.DateRange == 2)
                {
                    Transactions = Transactions.Where(c => c.TransactionDate >= DateTime.Today.AddDays(-30)).ToList();
                }
                //Last 60 days
                if (TheSearch.DateRange == 3)
                {
                    Transactions = Transactions.Where(c => c.TransactionDate >= DateTime.Today.AddDays(-60)).ToList();
                }

            }

            if (TheSearch.SearchTransactionNumber != null)
            {
                Transactions = Transactions.Where(c => c.BankingTransactionID == Convert.ToInt32(TheSearch.SearchTransactionNumber)).ToList();
            }

            //Order Trans ID ascending
            if (TheSearch.SortType != SortingOption.TransIDAsc)
            {
                //query = query.OrderBy(c => c.BankingTransactionID);
                //Order Trans ID descending
                if (TheSearch.SortType == SortingOption.TransIDDec)
                {

                    Transactions = Transactions.OrderByDescending(c => c.BankingTransactionID).ToList();
                }

                //Order by type ascending
                if (TheSearch.SortType == SortingOption.TransTypeAsc)
                {
                    Transactions = Transactions.OrderBy(c => c.BankingTransactionType).ToList();
                }

                //Order by type descending
                if (TheSearch.SortType == SortingOption.TransTypeDec)
                {
                    Transactions = Transactions.OrderByDescending(c => c.BankingTransactionType).ToList();
                }

                //Order by Description Ascending
                if (TheSearch.SortType == SortingOption.TransDescriptionAsc)
                {
                    Transactions = Transactions.OrderBy(c => c.Description).ToList();
                }

                //Order by description descending
                if (TheSearch.SortType == SortingOption.TransDescriptionDec)
                {
                    Transactions = Transactions.OrderByDescending(c => c.Description).ToList();
                }
                //Order by amount ascending
                if (TheSearch.SortType == SortingOption.TransAmountAsc)
                {
                    Transactions = Transactions.OrderBy(c => c.Amount).ToList();
                }

                //Order by amount descending
                if (TheSearch.SortType == SortingOption.TransAmountDec)
                {
                    Transactions = Transactions.OrderByDescending(c => c.Amount).ToList();
                }

                if (TheSearch.SortType == SortingOption.TransDateAsc)
                {
                    Transactions = Transactions.OrderBy(C => C.TransactionDate).ToList();
                }

                if (TheSearch.SortType == SortingOption.TransDateDec)
                {

                    Transactions = Transactions.OrderByDescending(c => c.TransactionDate).ToList();
                }

            }

            // return the list
            return Transactions;
        }

        public static SelectList AmountRange()
        {
            // Create a list of ranges
            List<Ranges> RangesList = new List<Ranges>();

            Ranges None = new Ranges { Name = "None", RangeID = 0 };
            Ranges _100 = new Ranges { Name = "$0-100", RangeID = 1 };
            Ranges _100_200 = new Ranges { Name = "$100-200", RangeID = 2 };
            Ranges _200_300 = new Ranges { Name = "$200-300", RangeID = 3 };
            Ranges _300 = new Ranges { Name = "$300+", RangeID = 4 };
            Ranges Custom = new Ranges { Name = "Custom", RangeID = 5 };

            // Add to the list
            RangesList.Add(None);
            RangesList.Add(_100);
            RangesList.Add(_100_200);
            RangesList.Add(_200_300);
            RangesList.Add(_300);
            RangesList.Add(Custom);


            // Convert to Select List

            SelectList RangesSelect = new SelectList(RangesList, "RangeID", "Name");

            return RangesSelect;
        }

        public static SelectList DateRanges()
        {
            // Create a list for Dates
            List<RangesDate> RangeDates = new List<RangesDate>();

            RangesDate All = new RangesDate { Name = "All Available", RangeID = 0 };
            RangesDate Last15 = new RangesDate { Name = "Last 15 Days", RangeID = 1 };
            RangesDate Last30 = new RangesDate { Name = "Last 30 Days", RangeID = 2 };
            RangesDate Last60 = new RangesDate { Name = "Last 60 Days", RangeID = 3 };
            RangesDate Custom = new RangesDate { Name = "Custom", RangeID = 4 };

            // add to the list 
            RangeDates.Add(All);
            RangeDates.Add(Last15);
            RangeDates.Add(Last30);
            RangeDates.Add(Last60);
            RangeDates.Add(Custom);

            // create a select list 
            SelectList DateSelect = new SelectList(RangeDates, "RangeID", "Name");

            return DateSelect;
        }
    }

}