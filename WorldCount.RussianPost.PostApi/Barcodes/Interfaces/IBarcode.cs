namespace WorldCount.RussianPost.PostApi.Barcodes.Interfaces
{
    public interface IBarcode
    {
        BarcodeType Type { get; set; }

        // Internal Barcode
        int Ops { get; set; }
        int Month { get; set; }
        int Num { get; set; }

        // External Barcode
        string Code { get; set; }
        string Land { get; set; }

        BarcodeType CheckType(string barcodeString);

        bool Parse(string barcodeString);

        bool DecrementNum(int num);

        bool IncrementNum(int num);

        bool DecrementMonth(int num);

        bool IncrementMonth(int num);

        bool IsExternal();

        bool IsInteral();

        bool IsUnknown();

        string ToString();
    }
}
