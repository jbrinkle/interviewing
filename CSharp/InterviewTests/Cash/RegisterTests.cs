using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Interview.Cash;

namespace InterviewTests
{
    public class RegisterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Register_AddingBillsUpdatesValue()
        {
            // arrange
            var r = new CashRegister();
            Assert.AreEqual(0, r.Value);

            // act
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
            });

            // assert
            Assert.AreEqual(7, r.Value);
        }

        [Test]
        public void Register_CanCountBills()
        {
            // arrange
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
            });

            // act / assert
            Assert.AreEqual(1, r.CountBills(5), "Only 1x Five present");
            Assert.AreEqual(2, r.CountBills(1), "2x Ones present");
            Assert.AreEqual(3, r.CountBills(), "3x bills present");
        }

        [Test]
        public void Register_Withdraw_Works()
        {
            // arrange
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
            });

            // act
            var cash = r.Withdraw(6);

            // assert
            Assert.AreEqual(6, cash.Sum(b => b.Value));
            Assert.AreEqual(1, r.Value);
        }

        [Test]
        public void Register_Withdraw_MinimizesBills()
        {
            // arrange
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Hundred),
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.One)
            });

            // act
            var cash = r.Withdraw(101);

            // assert
            Assert.AreEqual(2, cash.Count());
            Assert.AreEqual(5, r.CountBills(20));
        }

        [Test]
        public void Register_Withdraw_InsufficientFundsThrows()
        {
            // arrange
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Twenty),
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One)
            });

            // act / assert
            Assert.Throws<InsufficientFundsException>(() =>
            {
                var cash = r.Withdraw(27);
            });
        }

        [Test]
        public void Register_Withdraw_InsufficientBillsThrows()
        {
            // arrange
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One)
            });

            // act / assert
            Assert.Throws<InsufficientBillsException>(() =>
            {
                var cash = r.Withdraw(3);
            });
        }

        [Test]
        public void Register_Swap_EqualityTestHappens()
        {
            // arrange
            var expectedCashValue = 17;
            var r = new CashRegister();
            r.AddBills(new List<ICashBill> {
                new UsdBill(UsdDelineation.Ten),
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One)
            });
            Assert.AreEqual(expectedCashValue, r.Value, "Initial value before test");
            // withdrawing ten
            var swapRemoval = new Dictionary<int, int> { { 10, 1 } };
            // only depositing nine
            var swapInsert = new List<ICashBill> {
                new UsdBill(UsdDelineation.Five),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
                new UsdBill(UsdDelineation.One),
            };

            // act / assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                r.Swap(swapInsert, swapRemoval);
            });
            Assert.AreEqual(expectedCashValue, r.Value, "No change in value after throwing exception");

            // act
            // change insert to have value of 10
            swapInsert.Add(new UsdBill(UsdDelineation.One));
            r.Swap(swapInsert, swapRemoval);

            // assert
            Assert.AreEqual(expectedCashValue, r.Value, "No change in value expected");
            Assert.AreEqual(0, r.CountBills(10));
            Assert.AreEqual(2, r.CountBills(5));
            Assert.AreEqual(7, r.CountBills(1));
        }
    }
}
