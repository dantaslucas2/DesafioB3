namespace DesafioB3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string asset;
            decimal targetToSell;
            decimal targetToBuy;

            if (args.Length >= 3)
            {
                asset = args[0];
                targetToSell = args[1];
                targetToBuy = args[2];
            }

        }
    }
}
