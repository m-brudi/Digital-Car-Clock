
using UnityEngine;
public class Curve : MonoBehaviour
{
    [SerializeField] Transform[] controlPoints;
    float amount;
    private float splineLength;
    public Transform car;
    float moveAmount;
    public float speed;

    private void Awake() {
        splineLength = GetSplineLength();
    }

    private void Update() {
        moveAmount = (moveAmount + (Time.deltaTime * speed)) % 1;
        car.position = GetPositionAt(moveAmount);
        car.forward = GetForwardAt(moveAmount);
    }
    public float GetSplineLength(float stepSize = .01f) {
        float splineLength = 0f;

        Vector3 lastPosition = GetPositionAt(0f);

        for (float t = 0; t < 1f; t += stepSize) {
            splineLength += Vector3.Distance(lastPosition, GetPositionAt(t));

            lastPosition = GetPositionAt(t);
        }

        splineLength += Vector3.Distance(lastPosition, GetPositionAt(1f));

        return splineLength;
    }

    Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t) {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, amount);
    }

    Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t) {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);
        return Vector3.Lerp(ab_bc, bc_cd, amount);
    }

    public Vector3 GetPositionAt(float t) {
        return transform.position + CubicLerp(controlPoints[0].position, controlPoints[1].position, controlPoints[2].position, controlPoints[3].position, t);
    }

    public Vector3 GetForwardAt(float t) {
        Vector3 pos = GetPositionAt(t);
        Vector3 nextPos = GetPositionAt(t + 0.1f);
        Vector3 dir = (nextPos - pos).normalized;
        return dir;
    }
}
