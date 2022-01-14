using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking.Core.Persistence
{
    public class MongoContext : IMongoContext
    {
        public MongoContext(IOptions<ContextOptions> options)
        {
            _options = options.Value;

            _commands = new List<Func<Task>>();
        }

        public void AddCommand(Func<Task> func)
        {
            _commands.Add(func);
        }

        public void Dispose()
        {
            Session?.Dispose();
            GC.SuppressFinalize(this);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            ConfigureMongo();

            return _database.GetCollection<T>(name);
        }

        public async Task<int> SaveChanges()
        {
            var count = 0;

            try
            {
                ConfigureMongo();

                var commandTasks = _commands.Select(c => c());

                await Task.WhenAll(commandTasks);

                count = _commands.Count;

                _commands.Clear();
            }
            catch (Exception)
            {
                throw;
            }

            return count;
        }

        private void ConfigureMongo()
        {
            if (MongoClient != null) return;

            MongoClient = new MongoClient(_options.VehicleTrackingDb);

            _database = MongoClient.GetDatabase(_options.DatabaseName);
        }

        public MongoClient MongoClient { get; set; }
        public IClientSessionHandle Session { get; set; }

        private readonly List<Func<Task>> _commands;
        private readonly ContextOptions _options;
        private IMongoDatabase _database;
    }
}
