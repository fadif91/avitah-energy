using System;
using System.Linq;
using System.Threading.Tasks;
using Avitah.Energy.Service.Repository;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;

namespace Avitah.Energy.Service.Services
{
    public class EnergyService : Energy.EnergyBase
    {
        private readonly ILogger<EnergyService> _logger;
        private readonly IEnergyRepository _energyRepository;

        public EnergyService(ILogger<EnergyService> logger, IEnergyRepository energyRepository)
        {
            _logger = logger;
            _energyRepository = energyRepository;
        }

        public override async Task<TimeSeriesResponse> GetTimeSeries(TimeSeriesRequest request, ServerCallContext context)
        {
            try
            {
                var mappedData = await _energyRepository.GetMappedDataGrouping(request);

                var timeSeriesResponse = new TimeSeriesResponse();
                timeSeriesResponse.TimeSeriesGrouping.AddRange(
                    mappedData.Select(_ =>
                {
                    var timeSeriesGrouping = new TimeSeriesGrouping
                    {
                        Country = _.Key.Country,
                        Quantity = _.Key.Quantity,
                        Category = _.Key.Category,
                        Type = _.Key.Type,
                        Unit = _.Key.Unit
                    };
                    timeSeriesGrouping.TimeSeries.AddRange(
                        _.MappedDataTimeSeries
                            .Select(ts => new TimeSeries
                            {
                                DateTime = BsonUtils.ToMillisecondsSinceEpoch(ts.DateTime), 
                                Value = ts.Value
                            }));

                    return timeSeriesGrouping;
                }));

                return timeSeriesResponse;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}
