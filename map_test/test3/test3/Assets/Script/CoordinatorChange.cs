using UnityEngine;

public class CoordinatorChange{

    const double a = 6378245.0;
    const double ee = 0.00669342162296594323;

    //
    // World Geodetic System ==> Mars Geodetic System
    bool outOfChina(Position coordinate)
    {
        if (coordinate.lontitute < 72.004 || coordinate.lontitute > 137.8347)
            return true;
        if (coordinate.latitute < 0.8293 || coordinate.latitute > 55.8271)
            return true;
        return false;
    }

    float transformLat(float x, float y)
    {
        float ret = -100 + 2 * x + 3 * y + 0.2f * y * y + 0.1f * x * y + 0.2f * Mathf.Sqrt(Mathf.Abs(x));
        ret += (20 * Mathf.Sin(6 * x * Mathf.PI) + 20 * Mathf.Sin(2 * x * Mathf.PI)) * 2 / 3;
        ret += (20 * Mathf.Sin(y * Mathf.PI) + 40 * Mathf.Sin(y / 3 * Mathf.PI)) * 2 / 3;
        ret += (160 * Mathf.Sin(y / 12 * Mathf.PI) + 320 * Mathf.Sin(y * Mathf.PI / 30)) * 2 / 3;
        return ret;
    }

    float transformLon(float x, float y)
    {
        float ret = 300 + x + 2 * y + 0.1f * x * x + 0.1f * x * y + 0.1f * Mathf.Sqrt(Mathf.Abs(x));
        ret += (20 * Mathf.Sin(6 * x * Mathf.PI) + 20 * Mathf.Sin(2 * x * Mathf.PI)) * 2 / 3;
        ret += (20 * Mathf.Sin(x * Mathf.PI) + 40 * Mathf.Sin(x / 3 * Mathf.PI)) * 2 / 3;
        ret += (150 * Mathf.Sin(x / 12 * Mathf.PI) + 300 * Mathf.Sin(x / 30 * Mathf.PI)) * 2 / 3;
        return ret;
    }

    // 地球坐标系 (WGS-84) -> 火星坐标系 (GCJ-02)
    public Position wgs2gcj(Position coordinate)
    {
        if (outOfChina(coordinate))
        {
            return coordinate;
        }
        float wgLat = coordinate.latitute;
        float wgLon = coordinate.lontitute;
        float dLat = transformLat(wgLon - 105, wgLat - 35);
        float dLon = transformLon(wgLon - 105, wgLat - 35);
        float radLat = wgLat / 180 * Mathf.PI;
        float magic = Mathf.Sin(radLat);
        magic = 1 - (float)(ee * magic * magic);
        float sqrtMagic = Mathf.Sqrt(magic);
        dLat = (dLat * 180) / (float)((a * (1 - ee)) / (magic * sqrtMagic) * Mathf.PI);
        dLon = (dLon * 180) / (float)(a / sqrtMagic * Mathf.Cos(radLat) * Mathf.PI);
        return new Position((float)(wgLat + dLat), (float)(wgLon + dLon), coordinate.time);
    }
}
