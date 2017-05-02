using UnityEngine;
using System.Collections;

public class Change_font : MonoBehaviour {

    IEnumerator change_font_size(GameObject obj)
    {
        for (int i = 40; i <= 60; i++)
        {
            yield return new WaitForSeconds(0.04f);
            
            obj.GetComponent<TextMesh>().fontSize = i;
        }

    }
}
