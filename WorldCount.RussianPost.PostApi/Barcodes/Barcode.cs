using System;
using WorldCount.RussianPost.PostApi.Barcodes.Interfaces;

namespace WorldCount.RussianPost.PostApi.Barcodes
{
    public class Barcode : IBarcode
    {
        public BarcodeType Type { get; set; } = BarcodeType.Unknown;

        // Internal Barcode
        // ReSharper disable once MemberCanBePrivate.Global
        public int Ops { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public int Month { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public int Num { get; set; }

        // External Barcode
        // ReSharper disable once MemberCanBePrivate.Global
        public string Code { get; set; }
        // ReSharper disable once MemberCanBePrivate.Global
        public string Land { get; set; }

        public Barcode()
        {
        }

        public Barcode(string barcodeString)
        {
            try
            {
                Parse(barcodeString);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public bool Parse(string barcodeString)
        {
            int length = barcodeString.Length;

            if (length < 13 || length > 14)
                throw new ArgumentOutOfRangeException(nameof(barcodeString), "Строка должна быть не короче 13 и не длинее 14 символов");

            Type = CheckType(barcodeString);

            try
            {
                if (Type == BarcodeType.Internal)
                {
                    Ops = int.Parse(barcodeString.Substring(0, 6));
                    Month = int.Parse(barcodeString.Substring(6, 2));
                    Num = int.Parse(barcodeString.Substring(8, 5));
                }

                if (Type == BarcodeType.External)
                {
                    Code = barcodeString.Substring(0, 2);
                    Num = int.Parse(barcodeString.Substring(2, 8));
                    Land = barcodeString.Substring(11, 2);
                }
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }

            return true;
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public BarcodeType CheckType(string barcodeString)
        {
            if(string.IsNullOrEmpty(barcodeString)) throw new ArgumentNullException(nameof(barcodeString));

            if (Const.Nums.Contains(barcodeString[0])) return BarcodeType.Internal;
            return BarcodeType.External;
        }

        public bool DecrementNum(int num)
        {
            if (Type == BarcodeType.Internal)
            {
                if (Num - num <= 0)
                {
                    DecrementMonth(1);
                    Num = 99999;
                    return true;
                }

                Num -= num;
                return true;
            }

            if (Type == BarcodeType.External)
            {
                if (Num - num <= 0)
                    Num = 99999999;
                else
                    Num -= num;

                return true;
            }

            return false;
        }

        public bool IncrementNum(int num)
        {
            if (Type == BarcodeType.Internal)
            {
                if (Num + num > 99999)
                {
                    IncrementMonth(1);
                    Num = 1;
                    return true;
                }

                Num += num;
                return true;
            }

            if (Type == BarcodeType.External)
            {
                if (Num + num > 99999999)
                    Num = 1;
                else
                    Num += num;

                return true;
            }

            return false;
        }

        public bool DecrementMonth(int num)
        {
            if (Type == BarcodeType.External)
                return false;

            if (Month - num <= 0)
                Month = 99;
            else
                Month -= num;

            return true;
        }

        public bool IncrementMonth(int num)
        {
            if (Type == BarcodeType.External)
                return false;

            if (Month + num > 99)
                Month = 1;
            else
                Month += num;

            return true;
        }

        public bool IsExternal()
        {
            return Type == BarcodeType.External;
        }

        public bool IsInteral()
        {
            return Type == BarcodeType.Internal;
        }

        public bool IsUnknown()
        {
            return Type == BarcodeType.Unknown;
        }

        public DateTime? MonthToDate()
        {
            if (Type == BarcodeType.External)
                return null;

            DateTime startDate = new DateTime(2000, 1, 1);
            int currentYear = DateTime.Today.Year;

            DateTime addDate = new DateTime(2000, 1, 1);
            while (true)
            {
                addDate = addDate.AddMonths(99);

                if (addDate.Year < currentYear)
                    startDate = startDate.AddMonths(99);
                else
                    break;
            }

            return startDate.AddMonths(Month - 1);
        }

        public bool SetMonthByDate(DateTime date)
        {
            DateTime startDate = new DateTime(2000, 1, 1);

            if (date < startDate)
                return false;

            int month = (((date.Year - startDate.Year) * 12) + date.Month - startDate.Month + 1) % 99;
            Month = month == 0 ? 99 : month;

            return true;
        }

        public override string ToString()
        {
            if (Type == BarcodeType.Internal)
                return BarcodeGenerator.GetInternalBarcode(this);
            if (Type == BarcodeType.External)
                return BarcodeGenerator.GetExternalBarcode(this);
            return "";
        }
    }
}
