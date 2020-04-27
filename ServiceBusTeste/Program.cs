using Data;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusTeste
{
    class Program
    {

        const string ServiceBusConnectionString = "ConnectionString";
        const string QueueName = "QueueName";
        static IQueueClient queueClient;
        public static async Task Main(string[] args)
        {
            const int numberOfMessages = 10;
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);

            
            await SendMessagesAsync(numberOfMessages);

           

            await queueClient.CloseAsync();
        }


        static async Task SendMessagesAsync(int numberOfMessagesToSend)
        {
            try
            {

                Random r = new Random();
                string[] Descricao = { "Prato Feito", "Pizza", "Sobremessa", "Churrasco" };
                
                
                for (int i = 0; i < numberOfMessagesToSend; i++)
                {
                    Pedido pedido = new Pedido();
                    pedido.IdPedido = r.Next(0, 100);
                    pedido.Descricao = Descricao[r.Next(0, 3)];


                    var message =  new Message((Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pedido))))
                    {
                        ContentType = "application/json",
                        Label = "Restaurante",
                        TimeToLive = TimeSpan.FromMinutes(15)
                    };
                    await queueClient.SendAsync(message);
                }

              }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }
    }
}
