#pragma strict

var TranslateSpeed = 20;

var RotateSpeed = 1000;

function OnGUI()
{
    GUI.backgroundColor = Color.red;


    if (GUI.Button(Rect(10, 10, 300, 200), "turn left")) 
    {
        transform.Rotate(Vector3.up * Time.deltaTime * (-RotateSpeed));
    }

    if (GUI.Button(Rect(410, 10, 300, 200), "move forward")) 
    {
        transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed);
    }

    if (GUI.Button(Rect(820, 10, 300, 200), "turn right")) 
    {
        transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
    }

    if (GUI.Button(Rect(410, 310, 300, 200), "move backward")) 
    {
        transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed));
    }
    
    if (GUI.Button(Rect(10, 310, 300, 200), "move left")) 
    {
        transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed));
    }
    
    if (GUI.Button(Rect(820, 310, 300, 200), "move right")) 
    {
        transform.Translate(Vector3.right * Time.deltaTime * TranslateSpeed);
    }

    GUI.Label(Rect(1250,10,300,200),"model position" + transform.position);

    GUI.Label(Rect(1250,310,300,200),"model rotation" + transform.rotation);
}