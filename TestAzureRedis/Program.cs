using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;
using System.Threading;

namespace TestAzureRedis
{
    class Program
    {
        static string ConnectionString = @"password@host?ssl=true&db=1";

        static void Main(string[] args)
        {
            do
            {
                DoSomeCaching();
                Thread.Sleep(TimeSpan.FromSeconds(1));
            } while (true);
        }

        static void DoSomeCaching()
        {
            try
            {
                var redisRepo = new RedisRepo(ConnectionString);
                redisRepo.DoSomething();
                Console.WriteLine("Good: Caching all done successfully");
            }
            catch (Exception e)
            {
                Console.WriteLine("Bad: Error whilst doing caching");
            }
        }
    }

    public class RedisRepo
    {
        private string ConnectionString;

        public RedisRepo(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        public RedisClient Client
        {
            get
            {
                return new RedisClient(this.ConnectionString);
            }
        }

        public void DoSomething()
        {
            var key = "key1";
            var val = "This is life!";

            using (var client = Client)
            {
                client.Set<string>(key, val);
            }

            using (var client = Client)
            {
                var retrievedVal = client.Get<string>(key);
            }
        }
    }
}
