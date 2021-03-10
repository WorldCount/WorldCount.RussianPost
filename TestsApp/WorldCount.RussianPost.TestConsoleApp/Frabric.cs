namespace WorldCount.RussianPost.TestConsoleApp
{
    public abstract class Frabric
    {
        public abstract object Select();
    }

    public class FabricExt : Frabric
    {
        public override object Select()
        {
            return new TestExtBarcode();
        }
    }

    public class FabricInt : Frabric
    {
        public override object Select()
        {
            return new TestIntBarcode();
        }
    }
}
