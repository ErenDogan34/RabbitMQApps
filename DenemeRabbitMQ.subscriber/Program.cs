using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace DenemeRabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var factory = new ConnectionFactory();
            ////rabbitmq cloud sitesindeki link
            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();

            ////mesaj bolmeyi sagliyor. true olsaydi mesela 4 tane ise 2 2 boler. false olsaydi tek seferde iki prosesede ikinci degere yazilan kadar gonderir.
            //channel.BasicQos(0, 1, false);

            ////isim,devamlı kalmasi icin,hangi kanal uzerinden baglanti saglanacak,son subscriber kalinca silinip silinmeyecegi
            ////channel.QueueDeclare("Hello-Queue", true, false, false);

            //var consumer = new EventingBasicConsumer(channel);

            //channel.BasicConsume("Hello-Queue", false, consumer);

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());
            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen mesaj" + message);

            //    channel.BasicAck(e.DeliveryTag,false);

            //};

            ////Console.ReadLine();
            //var factory = new ConnectionFactory();
            ////rabbitmq cloud sitesindeki link
            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();

            //var randomQueueName = channel.QueueDeclare().QueueName;

            ////kuyruk silinmeyeceğii icin sabit bir kuyruk olusturuldu.
            ////var randomQueueName = "log-database-save-queue";
            ////true,false,false=1,2,3 : 1:Fiziksel kayit yapilsin 2:baska kanallardan kuyruga baglanirsin 3:otomatik silinmesin
            ////kutruk silinmeyecek.
            ////channel.QueueDeclare(randomQueueName,true,false,false);


            //channel.QueueBind(randomQueueName, "logs-fanout", "", null);


            //channel.BasicQos(0, 1, false);



            //var consumer = new EventingBasicConsumer(channel);

            //channel.BasicConsume(randomQueueName, false, consumer);

            //Console.WriteLine("Loglar dinleniyor");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());
            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen mesaj" + message);

            //    channel.BasicAck(e.DeliveryTag, false);

            //};

            //Console.ReadLine();

            //----------------------------------------------------------------------------------------------
            //var factory = new ConnectionFactory();
            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();


            //channel.BasicQos(0, 1, false);
            //var consumer = new EventingBasicConsumer(channel);

            //var queueName = "direct-queeu-Critical";
            //channel.BasicConsume(queueName, false, consumer);

            //Console.WriteLine("Logları dinleniyor...");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());

            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen Mesaj:" + message);

            //     File.AppendAllText("log-critical.txt", message+ "\n");

            //    channel.BasicAck(e.DeliveryTag, false);
            //};

            //Console.ReadLine();

//----------------------------------------------------------------------------------------------------------------------


            //var factory = new ConnectionFactory();
            //factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            //using var connection = factory.CreateConnection();

            //var channel = connection.CreateModel();


            //channel.BasicQos(0, 1, false);
            //var consumer = new EventingBasicConsumer(channel);

            //var queueName = channel.QueueDeclare().QueueName;
            //var routeKey = "*.Error.*";
            //channel.QueueBind(queueName, "logs-topic", routeKey);
            //channel.BasicConsume(queueName, false, consumer);

            //Console.WriteLine("Logları dinleniyor...");

            //consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            //{
            //    var message = Encoding.UTF8.GetString(e.Body.ToArray());

            //    Thread.Sleep(1500);
            //    Console.WriteLine("Gelen Mesaj:" + message);

            //    File.AppendAllText("log-critical.txt", message + "\n");

            //    channel.BasicAck(e.DeliveryTag, false);
            //};

            //Console.ReadLine();

            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://aecdcjws:aWAenT6o5_Y1oUgTwmLCKMemeIM-EONi@toad.rmq.cloudamqp.com/aecdcjws ");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();
            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;

            Dictionary<string,object> headers = new Dictionary<string, object>();

            headers.Add("format","pdf");
            headers.Add("shape","a4");
            headers.Add("x-match","all");


            channel.QueueBind(queueName, "header-exchange",string.Empty,headers);
            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Logları dinleniyor...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());
                Product product = JsonSerializer.Deserialize<Product>(message);
                Thread.Sleep(1500);
                Console.WriteLine($"Gelen Mesaj: { product.Id}-{product.Name}-{product.Price}-{product.Stock}");

                File.AppendAllText("log-critical.txt", message + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };
            Console.ReadLine();
        }
    }
}
