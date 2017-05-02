using UnityEngine;
using System.Collections;

public class New_change_alpha : MonoBehaviour {

    IEnumerator change_alpha_down(GameObject obj)
    {
        for(float i = 1.0f; i >= 0.0f; i = i - 0.02f)
        {
            yield return new WaitForSeconds(0.01f);

            Color color = obj.GetComponent<TextMesh>().color;
            color.a = i;
            obj.GetComponent<TextMesh>().color = color;
        }
        
    }

    IEnumerator change_alpha_up(GameObject obj)
    {
        for (float i = 0.0f; i <= 1.0f; i = i + 0.02f)
        {
            yield return new WaitForSeconds(0.01f);

            Color color = obj.GetComponent<TextMesh>().color;
            color.a = i;
            obj.GetComponent<TextMesh>().color = color;
        }

    }

}
