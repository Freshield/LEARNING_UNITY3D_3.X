using UnityEngine;
using System.Collections;

public class calculate : MonoBehaviour {

	// Use this for initialization
	void Start () {

        float latitude = 45.495061f;
        float longtitude = -73.578007f;
        float ratio = Mathf.Cos(Mathf.Deg2Rad * latitude);
        float onesecond = ((360 * 3600) / (512 * 2 * Mathf.Pow(2, 13)));
        float halfimagedegree = (onesecond * 256 / 3600);
        float fullimagedegree = halfimagedegree * 2;
        float halfla = halfimagedegree * ratio;
        float fullla = halfla * 2;

        Debug.Log("2`13 = " + Mathf.Pow(2,13));
        Debug.Log("one second per pixed = " + onesecond);
        Debug.Log("half image = " + (onesecond * 256));
        Debug.Log("half image degree = " + halfimagedegree);
        Debug.Log("full image degree = " + fullimagedegree);
        Debug.Log("ratio = " + ratio);
        Debug.Log("half latitude degree = " + halfla);
        Debug.Log("full latitude degree = " + fullla);


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
