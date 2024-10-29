namespace ChattingClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Client client = new Client();

            client.Connect();

            while (client.GetIsConnected())
            {
                Task.Run(() => client.SendMessage());

                Task.Run(() => client.ReceiveMessage());
            }
        }
    }
}