using System;
using System.Collections.Generic;
using System.Linq;

namespace Interview.Cash
{


    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("Attempted to withdraw more funds than are present.")
        {

        }
    }

    public class InsufficientBillsException : Exception
    {
        public InsufficientBillsException() : base("Funds are sufficient but the delineations required to service the request are lacking.")
        {

        }
    }

    public class CashRegister
    {
        private Dictionary<int, List<ICashBill>> bills = new Dictionary<int, List<ICashBill>>();

        public CashRegister()
        {

        }

        /// <summary>
        /// Current value of the register
        /// </summary>
        public int Value
        {
            get
            {
                var runningBalance = 0;
                foreach (var key in bills.Keys)
                {
                    runningBalance += key * bills[key].Count();
                }
                return runningBalance;
            }
        }

        /// <summary>
        /// Add a single bill
        /// </summary>
        /// <param name="bill">Bill to add</param>
        public void AddBill(ICashBill bill)
        {
            if (bills.ContainsKey(bill.Value))
            {
                bills[bill.Value].Add(bill);
                return;
            }

            bills.Add(bill.Value, new List<ICashBill>() { bill });
        }

        /// <summary>
        /// Add a list of bills
        /// </summary>
        /// <param name="bills">List of bills</param>
        public void AddBills(List<ICashBill> bills)
        {
            foreach (var bill in bills)
            {
                AddBill(bill);
            }
        }

        /// <summary>
        /// Count bills. If a delineation is provided, count instances of just that delineation
        /// </summary>
        /// <param name="delineation">Delineation/value of bill. Default=0/all bills</param>
        /// <returns>Number of instances of bills</returns>
        public int CountBills(int delineation = 0)
        {
            var countOfBills = 0;
            foreach (var key in bills.Keys.Where(v => delineation == 0 ? true : v == delineation))
            {
                countOfBills += bills[key].Count();
            }

            return countOfBills;
        }

        /// <summary>
        /// Withdraw amount minimizing count of bills returned
        /// </summary>
        /// <param name="amount">Amount to remove</param>
        /// <returns>List of bills</returns>
        public List<ICashBill> Withdraw(int amount)
        {
            var returnBills = new List<ICashBill>();
            var remainder = amount;
            var billDelineations = bills.Keys.OrderByDescending(x => x);
            foreach (var delineation in billDelineations)
            {
                var countOfThisDelineationNeed = remainder / delineation;
                var countOfThisDelineationHave = bills[delineation].Count();

                if (countOfThisDelineationHave >= countOfThisDelineationNeed)
                {
                    TransferFromRegisterToList(delineation, countOfThisDelineationNeed, returnBills);
                    remainder -= countOfThisDelineationNeed * delineation;
                }
                else
                {
                    TransferFromRegisterToList(delineation, countOfThisDelineationHave, returnBills);
                    remainder -= countOfThisDelineationHave * delineation;
                }
            }

            if (remainder > 0 && Value > 0) throw new InsufficientBillsException();
            if (remainder > 0 && Value == 0) throw new InsufficientFundsException();

            return returnBills;
        }

        /// <summary>
        /// Exchange funds (e.g. Pull 2x $20s and replace with 4x $10s
        /// </summary>
        /// <param name="insert">List of bills to insert</param>
        /// <param name="remove">Map indicating what to remove. Key=delineation, Value=count</param>
        /// <returns>List containing removed bills</returns>
        public List<ICashBill> Swap(List<ICashBill> insert, Dictionary<int, int> remove)
        {
            var returnBills = new List<ICashBill>();

            try
            {
                // pull out bills
                foreach (var delineation in remove.Keys)
                {
                    var countOfThisDelineationNeed = remove[delineation];
                    var countOfThisDelineationHave = bills[delineation].Count();

                    if (countOfThisDelineationHave < countOfThisDelineationNeed)
                        throw new InvalidOperationException($"Cannot remove {countOfThisDelineationNeed} bills of value {delineation}. {countOfThisDelineationHave} available.");

                    TransferFromRegisterToList(delineation, countOfThisDelineationNeed, returnBills);
                }

                // check equality
                var removeBillsValue = returnBills.Sum(b => b.Value);
                var insertBillsValue = insert.Sum(b => b.Value);
                if (removeBillsValue != insertBillsValue)
                    throw new InvalidOperationException("Value to bills to remove does not equal value of bills to insert");

                // insert the exchange funds
                AddBills(insert);

                return returnBills;
            }
            catch (InvalidOperationException)
            {
                // Return the bills already pulled out
                AddBills(returnBills);
                throw;
            }
        }

        private void TransferFromRegisterToList(int delineation, int count, List<ICashBill> destination)
        {
            var taken = bills[delineation].Take(count);
            var remaining = bills[delineation].Skip(count);

            destination.AddRange(taken);
            bills[delineation] = remaining.ToList();
        }
    }
}