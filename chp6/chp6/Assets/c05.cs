using UnityEngine;
using System.Collections;

public class c05 : MonoBehaviour {

    public Texture texture;

    public string info;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            info = "hit";
        }
        else
        {
            info = "not hit";
        }
	
	}

    void OnGUI()
    {
        Rect rect = new Rect(Input.mousePosition.x - (texture.width >> 1), Screen.height - Input.mousePosition.y - (texture.height >> 1), texture.width, texture.height);

        GUI.DrawTexture(rect, texture);

        GUILayout.Label(info + ",position is: " + Input.mousePosition);
    }
}
