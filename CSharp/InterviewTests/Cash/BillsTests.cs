using System;
using NUnit.Framework;
using Interview.Cash;

namespace InterviewTests
{
    public class BillsTests
    {
        /* Possible Tech Assessment questions
         *
         * - What kind of test cases would be useful for scenario x?
         * - Benefits of TestCase attribute over Test attribute
         */ 

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void UsdBill_CanCreate()
        {
            // arrange

            // act
            var usd = new UsdBill(UsdDelineation.One);

            // assert
            Assert.NotNull(usd);
        }

        [TestCase(UsdDelineation.One, 1)]
        [TestCase(UsdDelineation.Two, 2)]
        [TestCase(UsdDelineation.Five, 5)]
        [TestCase(UsdDelineation.Ten, 10)]
        [TestCase(UsdDelineation.Twenty, 20)]
        [TestCase(UsdDelineation.Fifty, 50)]
        [TestCase(UsdDelineation.Hundred, 100)]
        public void UsdBill_ValueMatchesDelineation(UsdDelineation delineation, int expectedValue)
        {
            // arrange

            // act
            var usd = new UsdBill(delineation);

            // assert
            Assert.AreEqual(usd.Value, expectedValue);
        }

        [TestCase(UsdDelineation.One, "Washington")]
        [TestCase(UsdDelineation.Five, "Lincoln")]
        [TestCase(UsdDelineation.Hundred, "Franklin")]
        public void UsdBill_ValueMatchesDelineation(UsdDelineation delineation, string expectedValue)
        {
            // arrange

            // act
            var usd = new UsdBill(delineation);

            // assert
            Assert.AreEqual(usd.Character, expectedValue);
        }

        [Test]
        public void UsdBill_AllDelineationsAreKnown()
        {
            // arrange
            var knownDelineations = new[]
            {
                UsdDelineation.One,
                UsdDelineation.Two,
                UsdDelineation.Five,
                UsdDelineation.Ten,
                UsdDelineation.Twenty,
                UsdDelineation.Fifty,
                UsdDelineation.Hundred
            };

            // act
            foreach (string eVal in Enum.GetNames(typeof(UsdDelineation)))
            {
                var usdDel = Enum.Parse<UsdDelineation>(eVal);

                // assert
                Assert.Contains(usdDel, knownDelineations);
            }
        }
    }
}