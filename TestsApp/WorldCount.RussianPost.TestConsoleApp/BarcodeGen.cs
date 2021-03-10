namespace WorldCount.RussianPost.TestConsoleApp
{
    static class BarcodeGen
    {
        public static T Gen<T>(bool ext = false) where T : new()
        {
            return new T();
        }
    }
}
