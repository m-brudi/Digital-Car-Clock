using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public int myPosIndex;
    public Transform mySpot;
    public bool isAtPos;
    public bool isAtHome;
    public Vector3 mySpotOutside;
    public Digit myDigit;
    public bool go;
    public float speed = 1f;
    Vector3 target;
    Vector3 startPoint;
    public Material frontLightsMat;
    public Material stopLightsMat;
    public Material offMat;
    public MeshRenderer bodyRenderer;
    float time;
    private void Start() {
        Controller.Instance.LightsOn += LightsOn;
        Controller.Instance.LightsOff += LightsOff;
    }

    public virtual void GoToStartPos() {
        startPoint = transform.position;
        target = mySpotOutside;
        time = 0;
        go = true;
    }
    public virtual void GoToPos(Vector3 pos) {
        startPoint = transform.position;
        target = pos;
        time = 0;
        go = true;
    }

    public void LightsOn() {
        Material[] mats = bodyRenderer.materials;
        mats[2] = frontLightsMat;
        mats[4] = stopLightsMat;
        bodyRenderer.materials = mats;
    }

    public void LightsOff() {
        Material[] mats = bodyRenderer.materials;
        mats[2] = offMat;
        mats[4] = offMat;
        bodyRenderer.materials = mats;
    }

    private void Update() {
        if (go) {
            transform.localPosition = Vector3.Lerp(startPoint, target, time * speed);
            time += Time.deltaTime;
            if (transform.position == target) go = false;
        }
    }
}
