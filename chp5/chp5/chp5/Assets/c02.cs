using UnityEngine;
using UnityEditor;
using System.Collections;

public class c02 : MonoBehaviour {

    [MenuItem("new menu/clone choiced item")]
    static void ClothObject()
    {
        Instantiate(Selection.activeTransform, Vector3.zero, Quaternion.identity);
    }

    [MenuItem("new menu/clone choiced item",true)]
    static bool NoClothObject()
    {
        return Selection.activeGameObject != null;
    }

    [MenuItem("new menu/delete choiced item")]
    static void RemoveObject()
    {
        DestroyImmediate(Selection.activeGameObject, true);
    }

    [MenuItem("new menu/delete choiced item",true)]
    static bool NoRemoveObject()
    {
        return Selection.activeGameObject != null;
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
