using UnityEngine;
using System.Collections;

public class obj_text : MonoBehaviour {

    public Camera m_Camera = null;
    public GameObject m_goFollowing = null;
    public Vector3 m_vOffset;

    // Update is called once per frame
    void OnGUI () {

        Vector3 vPosScreen = m_Camera.WorldToScreenPoint(m_goFollowing.transform.position + m_vOffset);

        var color = new Color(0.6f, 0.1f, 0.5f, 1); //文字颜色


        var pcolor = new Color(1, 1, 1, 1); //描边颜色


        var pos = new Rect(vPosScreen.x, Screen.height - vPosScreen.y, 200, 80);


        MakeStroke(pos, "你好", color, pcolor, 1);

    }

    public void MakeStroke(Rect position, string txt, Color txtColor, Color outlineColor, float outlineWidth )
    {


        position.y -= outlineWidth;


        GUI.color = outlineColor;


        GUI.Label(position, txt);


        position.y += outlineWidth * 2;


        GUI.Label(position, txt);


        position.y -= outlineWidth;


        position.x -= outlineWidth;


        GUI.Label(position, txt);


        position.x += outlineWidth * 2;


        GUI.Label(position, txt);


        position.x -= outlineWidth;


        GUI.color = txtColor;


        GUI.Label(position, txt);


    }
}
