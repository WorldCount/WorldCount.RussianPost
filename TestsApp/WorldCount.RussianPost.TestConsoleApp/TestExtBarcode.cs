namespace WorldCount.RussianPost.TestConsoleApp
{
    class TestExtBarcode : ITestBarcode
    {
        public string Name { get; set; } = "TestExtBarcode";

        public string Land { get; set; } = "RU";

        public void Gen()
        {
            return;
        }
    }
}
