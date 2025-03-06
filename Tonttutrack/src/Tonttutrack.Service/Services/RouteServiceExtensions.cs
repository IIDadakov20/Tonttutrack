using CoordinateSharp;
using Tonttutrack.DataAccess.Data.Models;

namespace Tonttutrack.Service.Services;

internal static class RouteServiceExtensions
{
    internal static double CalculateTotalDistance(this RouteService routeService, Route route)
    {
        double totalDistance = 0;

        var sortedRoutePoints = route.RoutePoints.OrderBy(rp => rp.RecordedAt).ToList();

        for (int i = 1; i < sortedRoutePoints.Count; i++)
        {
            var startPoint = sortedRoutePoints[i - 1];
            var endPoint = sortedRoutePoints[i];

            var coordOne = new Coordinate((double)startPoint.Latitude, (double)startPoint.Longitude);
            var coordTwo = new Coordinate((double)endPoint.Latitude, (double)endPoint.Longitude);

            Distance distance = new Distance(coordOne, coordTwo, Shape.Ellipsoid);

            totalDistance += distance.Kilometers;
        }

        return Math.Round(totalDistance, 3);
    }
}
