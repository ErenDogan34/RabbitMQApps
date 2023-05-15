using RabbitMQ.Client;
using Shared;
//using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace DenemeRabbitMQ.publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info = 4
    }
    class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory();
            ////rabbitmq cloud sitesindeki link
            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection=factory.CreateConnection();

            //var channel = connection.CreateModel();
            ////isim,devamlı kalmasi icin,hangi kanal uzerinden baglanti saglanacak,son subscriber kalinca silinip silinmeyecegi
            ///kuyruk olusturma
            //channel.QueueDeclare("Hello-Queue",true,false,false);
            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{
            //    string message = $"Message{x}";

            //    //turkce karakteerde sorun yasanmamasi icin
            //    var messagebody = Encoding.UTF8.GetBytes(message);

            //    channel.BasicPublish(string.Empty, "Hello-Queue", null, messagebody);

            //    Console.WriteLine($"Mesaj gönderildi{message}");
            //});

            //Console.ReadLine();



            //var factory = new ConnectionFactory();

            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();
            ////durable:fiziksle kaydetme
            //channel.ExchangeDeclare("logs-fanout",durable:true,type:ExchangeType.Fanout);

            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{
            //    string message = $"logs{x}";

            //    //turkce karakteerde sorun yasanmamasi icin
            //    var messagebody = Encoding.UTF8.GetBytes(message);

            //    channel.BasicPublish("logs-fanout", "", null, messagebody);

            //    Console.WriteLine($"Mesaj gönderildi{message}");
            //});

            //Console.ReadLine();


            //var factory = new ConnectionFactory();

            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();
            ////durable:fiziksle kaydetme
            //channel.ExchangeDeclare("logs-direct", durable: true, type: ExchangeType.Direct);

            //Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
            //{
            //    var routeKey = $"route-{x}";
            //    var queueName = $"direct-queeu-{x}";
            //    channel.QueueDeclare(queueName,true,false,false);
            //    channel.QueueBind(queueName,"logs-direct",routeKey,null);
            //});


            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{
            //    LogNames log = (LogNames) new Random().Next(1,5);
            //    string message = $"log-type{log}";

            //    //turkce karakteerde sorun yasanmamasi icin
            //    var messagebody = Encoding.UTF8.GetBytes(message);
            //    var routeKey = $"route-{log}";
            //    channel.BasicPublish("logs-direct", routeKey, null, messagebody);

            //    Console.WriteLine($"Log gönderildi{message}");
            //});

            //Console.ReadLine();



            //var factory = new ConnectionFactory();

            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();
            ////durable:fiziksle kaydetme
            //channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);
            //Random rnd = new Random();
            //Enumerable.Range(1, 50).ToList().ForEach(x =>
            //{ 
            //    LogNames log = (LogNames)new Random().Next(1, 5);

            //    LogNames log1 = (LogNames)rnd.Next(1, 5);
            //    LogNames log2 = (LogNames)rnd.Next(1, 5);
            //    LogNames log3 = (LogNames)rnd.Next(1, 5);
            //    var routeKey = $"{log1}.{log2}.{log3}";

            //    string message = $"log-type{log1}-{log2}-{log3}";
            //    var messagebody = Encoding.UTF8.GetBytes(message);


            //    channel.BasicPublish("logs-topic", routeKey, null, messagebody);

            //    Console.WriteLine($"Log gönderildi{message}");
            //});

            //Console.ReadLine();



            var factory = new ConnectionFactory();

            factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            //durable:fiziksle kaydetme
            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);
           
            Dictionary<string,object> headers = new Dictionary<string, object>();
            headers.Add("format", "pdf");
            headers.Add("shape", "a4");
            var properties = channel.CreateBasicProperties();
            properties.Headers=headers;
            //kalici hale getirir.
            properties.Persistent = true;
            var product = new Product { Id = 1, Name = "Kalem", Price = 100, Stock = 1 };
            var productjsonstring=JsonSerializer.Serialize(product);
          
            channel.BasicPublish("header-exchange",string.Empty,properties, Encoding.UTF8.GetBytes(productjsonstring));
            Console.WriteLine("Header olustu");
            Console.ReadLine();


        }
    }
}
