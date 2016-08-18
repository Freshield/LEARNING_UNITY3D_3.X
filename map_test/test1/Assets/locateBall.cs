using UnityEngine;
using System.Collections;

public class locateBall : MonoBehaviour {

    
    public Position center;
    public float fullLat;
    public float fullLon;
    public float radius = GameObject.Find("Prefab").transform.localScale.x / 2;
    string name;

    public locateBall(Position center, float fullLat, float fullLon,string name)
    {
        this.center = center;
        this.fullLat = fullLat;
        this.fullLon = fullLon;
        this.name = name;
    }

    public GameObject putBall(ArrayList balls, ArrayList locations)
    {
        GameObject parent = new GameObject(name);

        for (int i = 0; i < balls.Count; i++)
        {
            locate((GameObject)balls[i], parent, (Position)locations[i]);
        }

        return parent;

    }

    public ArrayList addBall(GameObject prefab,int length)
    {
        ArrayList balls = new ArrayList();
        for (int i = 0; i < length; i++)
        {
            GameObject ball = Instantiate(prefab);
            balls.Add(ball);
        }
        return balls;
    }

    public void locate(GameObject ball, GameObject parent, Position position)
    {
        
        float x = (position.lontitute - center.lontitute) * 10 / fullLon;
        float y = (position.latitute - center.latitute) * 10 / fullLat;
        ball.transform.position = new Vector3(x, y, -radius);
        ball.transform.parent = parent.transform;
        
    }
    
}
