#pragma strict

function Start () {

}

function Update () {
    transform.Translate(Vector3.forward * Time.deltaTime * 2);
    transform.Rotate(Vector3.up * Time.deltaTime * 20);
}