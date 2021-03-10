using System;
using System.Collections.Generic;
using WorldCount.RussianPost.PostApi.Barcodes;

namespace WorldCount.RussianPost.TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string barcodeRaw = "19331848010271";
            string barcodeRawExt = "RR073165527KZ";
            string barcodeRawBad = "RR07316";


            Barcode barcode = new Barcode(barcodeRaw);

            Console.WriteLine($"ШПИ: {barcodeRaw}");
            Console.WriteLine($"Тип ШПИ: {barcode.Type}");
            Console.WriteLine($"ОПС: {barcode.Ops}");
            Console.WriteLine($"Месяц: {barcode.Month}");
            Console.WriteLine($"Дата Месяца: {barcode.MonthToDate()}");
            Console.WriteLine($"Номер: {barcode.Num}");
            Console.WriteLine($"Код отправления: {barcode.Code}");
            Console.WriteLine($"Страна: {barcode.Land}");

            barcode.IncrementNum(10);
            Console.WriteLine($"Валидный ШПИ: {barcode}");

            DateTime date = new DateTime(2021, 1, 1);
            barcode.SetMonthByDate(date);
            Console.WriteLine($"Новый Месяц: {barcode.Month}");
            Console.WriteLine($"Дата Нового Месяца: {barcode.MonthToDate()}");
            Console.WriteLine($"Валидный ШПИ: {barcode}");


            Console.WriteLine();

            Barcode barcodeExt = new Barcode(barcodeRawExt);

            Console.WriteLine($"ШПИ: {barcodeRawExt}");
            Console.WriteLine($"Тип ШПИ: {barcodeExt.Type}");
            Console.WriteLine($"ОПС: {barcodeExt.Ops}");
            Console.WriteLine($"Месяц: {barcodeExt.Month}");
            Console.WriteLine($"Дата Месяца: {barcodeExt.MonthToDate()}");
            Console.WriteLine($"Номер: {barcodeExt.Num}");
            Console.WriteLine($"Код отправления: {barcodeExt.Code}");
            Console.WriteLine($"Страна: {barcodeExt.Land}");

            barcodeExt.DecrementNum(10);
            Console.WriteLine($"Валидный ШПИ: {barcodeExt}");

            Console.WriteLine();
            Console.WriteLine($"ШПИ: {barcodeRawBad}");
            try
            {
                Barcode barcodeBad = new Barcode(barcodeRawBad);

                Console.WriteLine($"Тип ШПИ: {barcodeBad.Type}");
                Console.WriteLine($"ОПС: {barcodeBad.Ops}");
                Console.WriteLine($"Месяц: {barcodeBad.Month}");
                Console.WriteLine($"Номер: {barcodeBad.Num}");
                Console.WriteLine($"Код отправления: {barcodeBad.Code}");
                Console.WriteLine($"Страна: {barcodeBad.Land}");
                Console.WriteLine($"Валидный ШПИ: {barcodeBad}");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine();
            Console.WriteLine($"Генерация списка ШПИ с 19331848999901 на 20 шт");
            List<Barcode> barcodes = BarcodeGenerator.GenBarcodes(new Barcode("19331848999901"), 20);
            foreach (Barcode barcode1 in barcodes)
            {
                Console.WriteLine(barcode1);
            }

            Console.WriteLine();
            Console.WriteLine($"Валидация ШПИ с 1933184*9999**");
            List<Barcode> validBarcodes = BarcodeGenerator.GenValidBarcodes("1933184*9999*1");
            foreach (Barcode barcode2 in validBarcodes)
            {
                Console.WriteLine(barcode2);
            }


            Console.ReadKey();
        }
    }
}
