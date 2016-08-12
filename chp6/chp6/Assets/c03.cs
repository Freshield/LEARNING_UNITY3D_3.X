using UnityEngine;
using System.Collections;

public class c03 : MonoBehaviour {

    CharacterController controller = null;

    float moveSpeed = 30.0f;

    float rotateSpeed = 3.0f;

	// Use this for initialization
	void Start () {

        controller = GetComponent<CharacterController>();
	
	}
	
	// Update is called once per frame
	void OnGUI () {

        if (GUILayout.RepeatButton("left rotate"))
        {
            transform.Rotate(0, -rotateSpeed, 0);
        }

        if (GUILayout.RepeatButton("right rotate"))
        {
            transform.Rotate(0, rotateSpeed, 0);
        }

        if (GUILayout.RepeatButton("forward"))
        {
            controller.SimpleMove(Vector3.forward * moveSpeed);
        }

        if (GUILayout.RepeatButton("backward"))
        {
            controller.SimpleMove(Vector3.back * moveSpeed);
        }

        if (GUILayout.RepeatButton("left"))
        {
            controller.SimpleMove(Vector3.left * moveSpeed);
        }

        if (GUILayout.RepeatButton("right"))
        {
            controller.SimpleMove(Vector3.right * moveSpeed);
        }

        if (GUILayout.RepeatButton("up"))
        {
            transform.Translate(0, 1, 0);
        }

        if (GUILayout.RepeatButton("down"))
        {
            transform.Translate(0, -1, 0);
        }

    }
}
