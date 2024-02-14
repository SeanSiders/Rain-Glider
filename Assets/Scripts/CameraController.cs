using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float focusTargetDistanceBuffer = 15.0f;
    public float cameraTargetDistanceBuffer = 20.0f;
    public float targetClosest = 15.0f;

    private Vector3 focus;

    private void Start()
    {
        focus = target.position;
    }

    private void FixedUpdate()
    {
        if ((target.position - focus).magnitude > focusTargetDistanceBuffer)
        {
            focus += 0.05f * (target.position - focus);
        }

        double distanceFromTarget = (transform.position - target.position).magnitude;
        if (distanceFromTarget > cameraTargetDistanceBuffer)
        {
            Vector3 translation = (target.position - transform.position) * 0.01f;
            translation.y = 0.0f;
            transform.Translate(translation, Space.World);
        }

        transform.LookAt(focus);
    }
}
