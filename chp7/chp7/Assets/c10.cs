using UnityEngine;
using System.Collections;

public class c10 : MonoBehaviour {

    public Transform target;

    public float distance = 20.0f;

    float x;
    float y;

    const float yMinLimit = 0.0f;
    const float yMaxLimit = 80.0f;

    const float xSpeed = 250.0f;
    const float ySpeed = 120.0f;

	// Use this for initialization
	void Start () {

        Vector2 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        Debug.Log("fist x y " + x + "," + y);

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        //distance = Vector3.Distance(target.position,transform.position);
        distance = (transform.position - target.position).magnitude;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (Input.anyKey)
        {
            if (target)
            {
                x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                Debug.Log("mouse x " + Input.GetAxis("Mouse X"));
                y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
                Debug.Log("mouse y " + Input.GetAxis("Mouse Y"));
                y = ClampAngle(y, yMinLimit, yMaxLimit);
                Quaternion rotation = Quaternion.Euler(y, x, 0);
                Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

                transform.rotation = rotation;
                transform.position = position;
            }
        }
        
	
	}

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
        {
            angle += 360;
        }
        if (angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);
    }
}
