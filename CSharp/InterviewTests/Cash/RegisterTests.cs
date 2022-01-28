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

        }

        [Test]
        public void Register_Withdraw_InsufficientFundsThrows()
        {

        }

        [Test]
        public void Register_Withdraw_InsufficientBillsThrows()
        {

        }
    }
}
