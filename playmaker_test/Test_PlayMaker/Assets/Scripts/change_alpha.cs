using UnityEngine;
using System.Collections;

public class change_alpha : MonoBehaviour {
    public GameObject title;
    public GameObject line1;
    public GameObject line2;
    public GameObject line3;
    public GameObject line4;

    public void chg_Title_alpha(float value)
    {
        
        Color color = title.GetComponent<TextMesh>().color;
        color.a = value;
        title.GetComponent<TextMesh>().color = color;
    }

    public void chg_line1_alpha(float value)
    {

        Color color = line1.GetComponent<TextMesh>().color;
        color.a = value;
        line1.GetComponent<TextMesh>().color = color;
    }
    public void chg_line2_alpha(float value)
    {

        Color color = line2.GetComponent<TextMesh>().color;
        color.a = value;
        line2.GetComponent<TextMesh>().color = color;
    }
    public void chg_line3_alpha(float value)
    {

        Color color = line3.GetComponent<TextMesh>().color;
        color.a = value;
        line3.GetComponent<TextMesh>().color = color;
    }
    public void chg_line4_alpha(float value)
    {

        Color color = line4.GetComponent<TextMesh>().color;
        color.a = value;
        line4.GetComponent<TextMesh>().color = color;
    }

    public void display_all(float value)
    {
        Color color = title.GetComponent<TextMesh>().color;
        color.a = value;
        title.GetComponent<TextMesh>().color = color;
        line1.GetComponent<TextMesh>().color = color;
        line2.GetComponent<TextMesh>().color = color;
        line3.GetComponent<TextMesh>().color = color;
        line4.GetComponent<TextMesh>().color = color;
    }
}
