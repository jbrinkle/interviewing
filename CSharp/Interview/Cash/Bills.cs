using System;

namespace Interview.Cash
{
    /* Possible Tech Assessment Questions:
     *
     * - What are the pros/cons of this design?
     * - What kind of unit tests would you write for UsdBill?
     * - What would need to be tested if the Govt introduced a $500 or reintroduced $2
     */

    public interface ICashBill
    {
        int Value { get; }
        string CurrencyLabel { get; }
        string Character { get; }
    }

    public enum UsdDelineation
    {
        One = 1,
        Two = 2,
        Five = 5,
        Ten = 10,
        Twenty = 20,
        Fifty = 50,
        Hundred = 100
    }

    public class UsdBill : ICashBill
    {
        public int Value { get; private set; }

        public string CurrencyLabel => "USD";

        public UsdDelineation Delineation { get; private set; }

        public string Character
        {
            get
            {
                switch (Delineation)
                {
                    case UsdDelineation.One:
                        return "Washington";
                    case UsdDelineation.Two:
                        return "Jefferson";
                    case UsdDelineation.Five:
                        return "Lincoln";
                    case UsdDelineation.Ten:
                        return "Hamilton";
                    case UsdDelineation.Twenty:
                        return "Jackson";
                    case UsdDelineation.Fifty:
                        return "Grant";
                    case UsdDelineation.Hundred:
                        return "Franklin";
                    default:
                        throw new FormatException();
                }
            }
        }

        public UsdBill(UsdDelineation value)
        {
            Value = (int)value;
            Delineation = value;
        }
    }
}