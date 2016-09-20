using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SplashLoading : MonoBehaviour {

    GameObject image;
    GameObject text;
    int token = 0;
    Tweener tweener;
    float alphaValue = 0;
    public float fadeInTime = 3;
    public float fadeOutTime = 3;

	// Use this for initialization
	void Start () {
        image = GameObject.Find("Image");
        text = GameObject.Find("Text");
	}
	
	// Update is called once per frame
	void Update () {

        switch (token)
        {
            case 0:
                tweener = DOTween.To(x => alphaValue = x, 0, 1, fadeInTime).SetAutoKill(false).SetEase(Ease.Linear);
                token = 1;
                break;
            case 1:
                if (tweener.IsComplete())
                {
                    tweener.Kill();
                    tweener = DOTween.To(x => alphaValue = x, 1, 0, fadeOutTime).SetAutoKill(false).SetEase(Ease.Linear);
                    token = 2;
                }
                else
                {
                    image.GetComponent<Image>().color = new Color(1, 1, 1, alphaValue);
                    text.GetComponent<Text>().color = new Color(1, 1, 1, alphaValue);
                }
                break;
            case 2:
                if (tweener.IsComplete())
                {
                    tweener.Kill();
                    Destroy(image);
                    SceneManager.LoadScene("scene1");
                    token = 3;
                }
                else
                {
                    image.GetComponent<Image>().color = new Color(1, 1, 1, alphaValue);
                    text.GetComponent<Text>().color = new Color(1, 1, 1, alphaValue);
                }
                break;
            default:
                break;
        }

    }
}
