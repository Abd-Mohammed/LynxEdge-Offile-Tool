using Refit;
using SharedKernal.Models.Mapbox;

namespace SharedKernal.Persistence.IRoutingBuilder;

public interface IRoutingBuilder
{
    [Get("/directions/v5/mapbox/driving/{coordinates}?alternatives=true&geometries=geojson&language=en&overview=full&steps=true&access_token=pk.eyJ1IjoiYWJkLW9kZWgwMDciLCJhIjoiY2xzazQ1NTc0MmxzbDJqbm9iaHpwazhncSJ9.n56bne7ZI7yS5YqcS0VfQA")]
    Task<RoutesResponse> GetRoutesFromTo([AliasAs("coordinates")] string coordinates);
}