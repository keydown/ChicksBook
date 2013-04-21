using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BYServices
{
    public static class GeoLocationHelper
    {
        public static List<T> FindNearbyLocations<T>(double userLatitude, double userLongitude, double radiusInMiles, List<T> locations)
        {
            List<T> nearbyLocations = new List<T>();
            double distanceFromUser = 0;

            foreach (dynamic location in locations)
            {
                distanceFromUser = CalculateDistanceInMiles(userLatitude, userLongitude, location.Latitude, location.Longitude);
                if (radiusInMiles >= distanceFromUser)
                {
                    location.DistanceFromUser = distanceFromUser;
                    nearbyLocations.Add(location);
                }
            }

            return nearbyLocations;
        }

        //public static List<PetShop> FindNearbyPetShops(double userLatitude, double userLongitude, double radiusInMiles, List<PetShop> petShops)
        //{
        //    List<PetShop> nearbyPetShops = new List<PetShop>();

        //    foreach (PetShop petShop in petShops)
        //    {
        //        if (radiusInMiles >= CalculateDistanceInMiles(userLatitude, userLongitude, (double)petShop.Latitude, (double)petShop.Longitude))
        //        {
        //            nearbyPetShops.Add(petShop);
        //        }
        //    }

        //    return nearbyPetShops;
        //}

        //public static List<Vet> FindNearbyVets(double userLatitude, double userLongitude, double radiusInMiles, List<Vet> vets)
        //{
        //    List<Vet> nearbyVets = new List<Vet>();

        //    foreach (Vet vet in vets)
        //    {
        //        if (radiusInMiles >= CalculateDistanceInMiles(userLatitude, userLongitude, (double)vet.Latitude, (double)vet.Longitude))
        //        {
        //            nearbyVets.Add(vet);
        //        }
        //    }

        //    return nearbyVets;
        //}

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
    }
}