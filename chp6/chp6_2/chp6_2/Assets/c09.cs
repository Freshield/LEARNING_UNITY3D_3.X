using UnityEngine;
using System.Collections;

public class c09 : MonoBehaviour {

    private GameObject obj;

    public Texture texture;

	// Use this for initialization
	void Start () {

        obj = GameObject.Find("Sphere");

        Cursor.visible = false;
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "cube")
                {
                    Vector3 direction = hit.transform.position - obj.transform.position;

                    obj.GetComponent<Rigidbody>().AddForceAtPosition(direction, hit.transform.position, ForceMode.Impulse);
                }
            }
        }
	
	}

    void OnGUI()
    {
        Rect rect = new Rect(Input.mousePosition.x - (texture.width >> 1), Screen.height - Input.mousePosition.y - (texture.height >> 1), texture.width, texture.height);

        GUI.DrawTexture(rect, texture);
    }
}
