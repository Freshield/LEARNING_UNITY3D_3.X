#pragma strict

var TranslateSpeed = 20;

var RotateSpeed = 1000;

function OnGUI()
{
    Gui.backgroundColor = Color.red;

    if (GUI.Button(Rect(10, 10, 70, 30), "turn left")) 
    {
        transform.Rotate(Vector3.up * Time.deltaTime * (-RotateSpeed));
    }

    if (GUI.Button(Rect(90, 10, 70, 30), "move forward")) 
    {
        transform.Translate(Vector3.forward * Time.deltaTime * TranslateSpeed);
    }

    if (GUI.Button(Rect(170, 10, 70, 30), "turn right")) 
    {
        transform.Rotate(Vector3.up * Time.deltaTime * RotateSpeed);
    }

    if (GUI.Button(Rect(90, 50, 70, 30), "move backward")) 
    {
        transform.Translate(Vector3.forward * Time.deltaTime * (-TranslateSpeed));
    }
    
    if (GUI.Button(Rect(10, 50, 70, 30), "move left")) 
    {
        transform.Translate(Vector3.right * Time.deltaTime * (-TranslateSpeed));
    }
    
    if (GUI.Button(Rect(170, 50, 70, 30), "move right")) 
    {
        transform.Translate(Vector3.right * Time.deltaTime * TranslateSpeed);
    }

    GUI.Label(Rect(250,10,20,30),"model position" + transform.position);

    GUI.Label(Rect(250,50,200,30),"model rotation" + transform.rotation);
}