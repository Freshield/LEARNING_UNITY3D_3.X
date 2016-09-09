using UnityEngine;
using System.Collections;

public class test_hit : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray;
        RaycastHit hit;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject != null)
            {
                HighlightableObject ho = hit.collider.transform.GetComponentInChildren<HighlightableObject>();
                if (ho != null)
                {
                    ho.On(Color.red);
                    //ho.OccluderOn();
                }
            }
        }

    }
}
