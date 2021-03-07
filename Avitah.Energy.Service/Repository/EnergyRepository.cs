using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avitah.Energy.Service.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Avitah.Energy.Service.Repository
{
    public interface IEnergyRepository
    {
        Task<IEnumerable<MappedData>> GetMappedData(TimeSeriesRequest timeSeriesRequest);
        Task<IEnumerable<MappedDataGrouping>> GetMappedDataGrouping(TimeSeriesRequest timeSeriesRequest);
    }
    public class EnergyRepository : IEnergyRepository
    {
        private readonly IMongoDatabase _database;
        public EnergyRepository()
        {
            var client = new MongoClient();
            _database = client.GetDatabase("energy");
        }
        public async Task<IEnumerable<MappedData>> GetMappedData(TimeSeriesRequest timeSeriesRequest)
        {
            var dataSourceCollection = _database.GetCollection<DataSource>("dataSources");
            var mappingCollection = _database.GetCollection<Mapping>("mappings");
            var historicalCollection = _database.GetCollection<Historical>("historicals");

            var fromDatePred = !timeSeriesRequest.FromDate.HasValue;
            var fromDateTime = fromDatePred ? (DateTime?) null : BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(timeSeriesRequest.FromDate.Value);
            var toDatePred = !timeSeriesRequest.ToDate.HasValue;
            var toDateTime = toDatePred ? (DateTime?) null : BsonUtils.ToDateTimeFromMillisecondsSinceEpoch(timeSeriesRequest.ToDate.Value);

            var query = from mapping in mappingCollection.AsQueryable()
                join historical in historicalCollection.AsQueryable() on mapping.Key equals historical.Key
                join dataSource in dataSourceCollection.AsQueryable() on historical.Key equals dataSource.Key
                where (fromDatePred || historical.DateTime > fromDateTime)
                      && (toDatePred || historical.DateTime < toDateTime)
                      && (string.IsNullOrEmpty(timeSeriesRequest.Country) ||
                          mapping.Country == timeSeriesRequest.Country)
                      && (string.IsNullOrEmpty(timeSeriesRequest.Quantity) ||
                          mapping.Quantity == timeSeriesRequest.Quantity)
                      && (string.IsNullOrEmpty(timeSeriesRequest.Type) || mapping.Type == timeSeriesRequest.Type)
                      && (string.IsNullOrEmpty(timeSeriesRequest.Category) ||
                          mapping.Category == timeSeriesRequest.Category)
                select new MappedData
                {
                    Key = new MappedDataKey
                    {
                        Country = mapping.Country, Quantity = mapping.Quantity,
                        Category = mapping.Category, Type = mapping.Type, Unit = dataSource.Unit
                    },
                    DateTime = historical.DateTime,
                    Value = mapping.Field == "value" ? historical.Data.Value : (double?) null
                };
                // into mappedData
                // group mappedData by mappedData.Key into grp
                //         select grp;
                //select new MappedDataGrouping { Key = grp.Key, MappedDataTimeSeries = grp };

                return await query.ToListAsync();
        }

        public async Task<IEnumerable<MappedDataGrouping>> GetMappedDataGrouping(TimeSeriesRequest timeSeriesRequest)
        {
            return (await this.GetMappedData(timeSeriesRequest))
                .GroupBy(_ => _.Key)
                .Select(g =>
                    new MappedDataGrouping
                    {
                        Key = g.Key,
                        MappedDataTimeSeries = g.ToList()
                    })
                .ToList();
        }
    }
}
