using UnityEngine;
using System.Collections;

public class test_value : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Values x = new Values(0);
        Debug.Log(x.value);
        changeValue(x);
        Debug.Log(x.value);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void changeValue(Values x)
    {
        x.value = 10;
    }
}

public class Values
{
    public int value;
    public Values(int value)
    {
        this.value = value;
    }
}
