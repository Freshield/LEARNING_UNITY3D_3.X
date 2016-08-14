using UnityEngine;
using System.Collections;

public class c04 : MonoBehaviour {

    public Texture imageUp;
    public Texture imageDown;
    public Texture imageLeft;
    public Texture imageRight;
    public Texture imageSuccess;

    public const int KEY_UP = 0;
    public const int KEY_DOWN = 1;
    public const int KEY_LEFT = 2;
    public const int KEY_RIGHT = 3;
    public const int KEY_FIRT = 4;
    public const int FRAME_COUNT = 100;
    public const int SAMPLE_SIZE = 3;
    public const int SAMPLE_COUNT = 5;

    int[,] Sample =
    {
        { KEY_DOWN,KEY_RIGHT,KEY_DOWN,KEY_RIGHT,KEY_FIRT },
        { KEY_DOWN,KEY_RIGHT,KEY_DOWN,KEY_LEFT,KEY_FIRT },
        { KEY_DOWN,KEY_LEFT,KEY_DOWN,KEY_LEFT,KEY_FIRT },
    };

    int currentKeyCode = 0;
    bool startFrame = false;
    int currentFrame = 0;
    ArrayList playerSample = new ArrayList();
    bool isSuccess = false;


	// Use this for initialization
	void Start () {

        playerSample = new ArrayList();
	
	}

    void OnGUI()
    {
        int size = playerSample.Count;

        for (int i = 0; i < size; i++)
        {
            int key = (int)playerSample[i];
            Texture temp = null;

            switch (key)
            {
                case KEY_UP:
                    temp = imageUp;
                    break;

                case KEY_DOWN:
                    temp = imageDown;
                    break;

                case KEY_LEFT:
                    temp = imageLeft;
                    break;

                case KEY_RIGHT:
                    temp = imageRight;
                    break;
                default:
                    break;
            }

            if (temp != null)
            {
                GUILayout.Label(temp);
            }
        }

        if (isSuccess)
        {
            GUILayout.Label(imageSuccess);
        }

        GUILayout.Label("suit1: down, right, down, right, fight");
        GUILayout.Label("suit2: down, right, down, left, fight");
        GUILayout.Label("suit3: down, left, down, left, fight");
    }
	
	// Update is called once per frame
	void Update () {

        UpdateKey();

        if (Input.anyKeyDown)
        {
            if (isSuccess)
            {
                isSuccess = false;

                Reset();
            }

            if (!startFrame)
            {
                startFrame = true;
            }

            playerSample.Add(currentKeyCode);

            int size = playerSample.Count;

            if (size == SAMPLE_COUNT)
            {
                for (int i = 0; i < SAMPLE_SIZE; i++)
                {
                    int SuccessCount = 0;
                    for (int j = 0; j < SAMPLE_COUNT; j++)
                    {
                        int temp = (int)playerSample[j];
                        if (temp == Sample[i,j])
                        {
                            SuccessCount++;
                        }
                    }
                    if (SuccessCount == SAMPLE_COUNT)
                    {
                        isSuccess = true;
                        break;
                    }
                }
            }

        }

        if (startFrame)
        {
            currentFrame++;
        }

        if (currentFrame >= FRAME_COUNT)
        {
            if (!isSuccess)
            {
                Reset();
            }
        }
	
	}

    void Reset()
    {
        currentFrame = 0;
        startFrame = false;
        playerSample.Clear();
    }

    void UpdateKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentKeyCode = KEY_UP;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            currentKeyCode = KEY_DOWN;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            currentKeyCode = KEY_LEFT;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            currentKeyCode = KEY_RIGHT;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentKeyCode = KEY_FIRT;
        }
    }
}
