using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class CurveCar : Car
{
    public SplineContainer goHomeCurve;
    public SplineContainer goToPlaceCurve;
    SplineContainer activeSpline;
    float distancePercs = 0;
    float splineLength;
    bool canGo;
    private void Start() {
        Controller.Instance.LightsOn += base.LightsOn;
        Controller.Instance.LightsOff += base.LightsOff;
        
        isAtPos = true;
    }

    public override void GoToPos(Vector3 pos) {
        if (!isAtPos) {
            StartCoroutine(Delay(() => {
            activeSpline = goToPlaceCurve;
                isAtHome = false;
                speed = 5f;
                splineLength = activeSpline.CalculateLength();
                distancePercs = 0;
                canGo = true;
            }));
         }
    }

    public override void GoToStartPos() {
        if (!isAtHome) {
            StartCoroutine(Delay(() => {
                isAtPos = false;
                activeSpline = goHomeCurve;
                distancePercs = 0;
                speed = 5f;
                splineLength = activeSpline.CalculateLength();
                canGo = true;
            }));
        }
    }

    IEnumerator Delay(System.Action callback) {
        yield return new WaitForSeconds(Random.Range(0, .5f));
        callback();
    }

    private void Update() {
        if (canGo) {
            if(activeSpline != null) {
                distancePercs += speed * Time.deltaTime / splineLength;
                Vector3 currPos = activeSpline.EvaluatePosition(distancePercs);
                transform.position = currPos;

                if (distancePercs > 1) {
                    canGo = false;
                    if (activeSpline == goToPlaceCurve) isAtPos = true;
                    if (activeSpline == goHomeCurve) isAtHome = true;
                } else {
                    Vector3 nextPos = activeSpline.EvaluatePosition(distancePercs + 0.1f);
                    Vector3 dir = nextPos - currPos;
                    transform.rotation = Quaternion.LookRotation(dir, transform.up);
                }
            }
        }
    }
}
