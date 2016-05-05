using UnityEngine;
using System.Collections;

public static class Haversine {

    public static double calculate(float lat1, float lon1, float lat2, float lon2) {
        float R = 6372.8f; // In kilometers
        float dLat = (float) toRadians(lat2 - lat1);
        float dLon = (float)toRadians(lon2 - lon1);
        lat1 = (float) toRadians(lat1);
        lat2 = (float) toRadians(lat2);

        var a = Mathf.Sin(dLat / 2f) * Mathf.Sin(dLat / 2f) + Mathf.Sin(dLon / 2f) * Mathf.Sin(dLon / 2f) * Mathf.Cos(lat1) * Mathf.Cos(lat2);
        var c = 2 * Mathf.Asin(Mathf.Sqrt(a));
        return R * 2 * Mathf.Asin(Mathf.Sqrt(a));
    }

    public static double toRadians(double angle) {
        return Mathf.PI * angle / 180.0;
    }
}



// Returns: The distance between coordinates 36.12,-86.67 and 33.94,-118.4 is: 2887.25995060711
