using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BYFarmerConsoleServices
{
    static class GeoLocationDistanceCalculator
    {
        public static List<T> FindNearbyLocations<T>(double userLatitude, double userLongitude, double radiusInMiles, List<T> locations)
        {
            List<T> nearbyLocations = new List<T>();

            foreach (dynamic location in locations)
            {
                if (radiusInMiles >= CalculateDistanceInMiles(userLatitude, userLongitude, location.Latitude, location.Longitude))
                {
                    nearbyLocations.Add(location);
                }
            }

            return nearbyLocations;
        }

        private static double CalculateDistanceInMiles(double latitudeA, double longitudeA, double latitudeB, double longitudeB)
        {
            const double EarthRadius = 3958.76;
            double latitudeARadians = latitudeA * (Math.PI / 180.0);
            double longitudeARadians = longitudeA * (Math.PI / 180.0);
            double latitudeBRadians = latitudeB * (Math.PI / 180.0);
            double longitudeBRadians = longitudeB * (Math.PI / 180.0);

            double dLongitude = longitudeBRadians - longitudeARadians;
            double dLatitude = latitudeBRadians - latitudeARadians;

            double tmpResult = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
                          Math.Cos(latitudeARadians) * Math.Cos(latitudeBRadians) *
                          Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);


            double distance = EarthRadius * 2.0 * Math.Atan2(Math.Sqrt(tmpResult), Math.Sqrt(1.0 - tmpResult));

            return distance;
        }

        public static void LocationServiceTest()
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            List<Vet> vets = db.Vets.ToList<Vet>();
            List<Vet> nearbyVets = FindNearbyLocations(47.290418, -122.313312, 10, vets);

            foreach (Vet vet in nearbyVets)
            {
                Console.WriteLine("{0}; {1}; {2}", vet.Name, vet.Latitude, vet.Longitude);
            }
        }

        public static string GetStateByGeoLocation(double latitude, double longitude)
        {
            keydowno_backyard_farmerEntities db = new keydowno_backyard_farmerEntities();

            List<ZipCode> zipData = db.ZipCodes.ToList<ZipCode>();

            double geoCoordsDifference = Math.Abs((double)zipData[0].Latitude - latitude) + Math.Abs((double)zipData[0].Longitude - longitude);
            double newGeoCoordsDifference;
            ZipCode closestLocation = zipData[0];

            foreach (ZipCode zip in zipData)
            {
                newGeoCoordsDifference = Math.Abs((double)zip.Latitude - latitude) + Math.Abs((double)zip.Longitude - longitude);
                if (geoCoordsDifference > newGeoCoordsDifference)
                {
                    closestLocation = zip;
                    geoCoordsDifference = newGeoCoordsDifference;
                }
            }

            return closestLocation.State;
        }

        public static void GetStateTest()
        {
            Console.WriteLine(GetStateByGeoLocation(40, -122));
        }
    }
}
