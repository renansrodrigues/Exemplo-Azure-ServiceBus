using Data;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using ServiceBusTeste;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusReceiveMessage
{

    class Program
    {

        const string ServiceBusConnectionString = "<ConnectionString>";
        const string QueueName = "<QueueName>";
        static IQueueClient queueClient;

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
            RegisterOnMessageHandlerAndReceiveMessages();

            await queueClient.CloseAsync();
        }


        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
           
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                
                MaxConcurrentCalls = 1,

               
                AutoComplete = false
            };

            
            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }



        static async Task ProcessMessagesAsync(Message message, System.Threading.CancellationToken token)
        {
          
     

            var pedido = JsonConvert.DeserializeObject<Data.Pedido>(Encoding.UTF8.GetString(message.Body));
            PedidoRepository repository = new PedidoRepository();

           
            var ret = await repository.InseriPedido(pedido);
            if (ret == 1)
            {
                await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            }
            
           

           
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}
