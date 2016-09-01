using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Locator : MonoBehaviour {
    public Position center;
    public float fullLat;
    public float fullLon;
    public float radius;
    string name;

    public Locator(Position center, float fullLat, float fullLon, string name)
    {
        this.center = center;
        this.fullLat = fullLat;
        this.fullLon = fullLon;
        this.name = name;
    }


    //using world position to locate
    public GameObject worldLocate(GameObject prefab, List<VecTime> locations)
    {
        radius = prefab.transform.localScale.x / 2;

        List<GameObject> objs = new List<GameObject>();

        GameObject parent = new GameObject(name);

        for (int i = 0; i < locations.Count; i++)
        {
            GameObject obj = Instantiate(prefab);
            objs.Add(obj);
            obj.transform.position = locations[i].worldPosition;
            obj.transform.parent = parent.transform;
        }

        return parent;
    }

    //using true position to locate
    public GameObject locateObject(GameObject prefab, List<Position> locations)
    {
        radius = prefab.transform.localScale.x / 2;

        List<GameObject> objs = new List<GameObject>();

        GameObject parent = new GameObject(name);

        for (int i = 0; i < locations.Count; i++)
        {
            GameObject obj = Instantiate(prefab);
            objs.Add(obj);
            locate(obj, parent, locations[i]);
        }
        
        return parent;
    }
    
    public void locate(GameObject ball, GameObject parent, Position position)
    {
        float x = (position.lontitute - center.lontitute) * 10 / fullLon;
        float y = (position.latitute - center.latitute) * 10 / fullLat;
        ball.transform.position = new Vector3(x, y, -radius);
        ball.transform.parent = parent.transform;
    }
}
