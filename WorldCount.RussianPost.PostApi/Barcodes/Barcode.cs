using System;
using WorldCount.RussianPost.PostApi.Barcodes.Interfaces;

namespace WorldCount.RussianPost.PostApi.Barcodes
{
    public class Barcode : IBarcode
    {

        #region Публичные свойства

        /// <summary>Тип ШПИ</summary>
        public BarcodeType Type { get; set; } = BarcodeType.Unknown;

        // Internal Barcode
        /// <summary>Номер отделения связи</summary>
        public int Ops { get; set; }
        /// <summary>Месяц ШПИ</summary>
        public int Month { get; set; }
        /// <summary>Номер ШПИ</summary>
        public int Num { get; set; }

        // External Barcode
        /// <summary>Код отправления для МЖД ШПИ</summary>
        public string Code { get; set; }
        /// <summary>Страна отправления для МЖД ШПИ</summary>
        public string Land { get; set; }

        #endregion


        #region Конструкторы

        /// <summary>
        /// Объект ШПИ
        /// </summary>
        public Barcode()
        {
        }

        /// <summary>
        /// Объект ШПИ
        /// </summary>
        /// <param name="barcodeString">Строка со ШПИ</param>
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

        #endregion


        #region Публичные методы

        /// <summary>
        /// Парсинг строки со ШПИ
        /// </summary>
        /// <param name="barcodeString">Строка с номером ШПИ</param>
        /// <returns>Парсинг выполнен: bool</returns>
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

        /// <summary>
        /// Проверка типа отправления Внутреннее\Внешнее
        /// </summary>
        /// <param name="barcodeString">Строка с номером ШПИ</param>
        public BarcodeType CheckType(string barcodeString)
        {
            if (string.IsNullOrEmpty(barcodeString)) throw new ArgumentNullException(nameof(barcodeString));

            if (Const.Nums.Contains(barcodeString[0])) return BarcodeType.Internal;
            return BarcodeType.External;
        }

        /// <summary>
        /// Уменьшает номер отправления на указанное число
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Уменьшение выполнено: bool</returns>
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

        /// <summary>
        /// Увеличивает номер отправления на указанное число
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Увеличение выполнено: bool</returns>
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

        /// <summary>
        /// Уменьшает номер месяца на указанное число
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Уменьшение выполнено: bool</returns>
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

        /// <summary>
        /// Увеличивает номер месяца на указанное число
        /// </summary>
        /// <param name="num">Число</param>
        /// <returns>Увеличение выполнено: bool</returns>
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

        /// <summary>
        /// Это внешнее отправление
        /// </summary>
        /// <returns>bool</returns>
        public bool IsExternal()
        {
            return Type == BarcodeType.External;
        }

        /// <summary>
        /// Это внутреннее отправление
        /// </summary>
        /// <returns>bool</returns>
        public bool IsInteral()
        {
            return Type == BarcodeType.Internal;
        }

        /// <summary>
        /// Это неопределенное отправление
        /// </summary>
        /// <returns>bool</returns>
        public bool IsUnknown()
        {
            return Type == BarcodeType.Unknown;
        }

        /// <summary>
        /// Возвращает дату по номеру месяца отправления
        /// </summary>
        /// <returns>Дата месяца: DateTime</returns>
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

        /// <summary>
        /// Устанавливает номер месяца по дате
        /// </summary>
        /// <param name="date">Дата месяца</param>
        /// <returns>Установка удалась: bool</returns>
        public bool SetMonthByDate(DateTime date)
        {
            DateTime startDate = new DateTime(2000, 1, 1);

            if (date < startDate)
                return false;

            int month = (((date.Year - startDate.Year) * 12) + date.Month - startDate.Month + 1) % 99;
            Month = month == 0 ? 99 : month;

            return true;
        }

        /// <summary>
        /// Возвращает ШПИ с контрольным разрядом
        /// </summary>
        /// <returns>ШПИ полностью: string</returns>
        public override string ToString()
        {
            if (Type == BarcodeType.Internal)
                return BarcodeGenerator.GetInternalBarcode(this);
            if (Type == BarcodeType.External)
                return BarcodeGenerator.GetExternalBarcode(this);
            return "";
        }

        #endregion

    }
}
