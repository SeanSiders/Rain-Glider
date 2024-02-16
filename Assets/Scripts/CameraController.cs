using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    private Vector3 focus;
    private float focusTargetDistanceBuffer = 15.0f;
    private float cameraTargetDistanceBuffer = 20.0f;

    private void Start()
    {
        focus = target.position;
    }

    private void FixedUpdate()
    {
        focusTargetDistanceBuffer = 5.0f;
        Debug.Log(focusTargetDistanceBuffer);

        if ((target.position - focus).magnitude > focusTargetDistanceBuffer)
        {
            focus = Vector3.Lerp(focus, target.position, 0.05f);
        }

        double distanceFromFocus = (transform.position - focus).magnitude;
        if (distanceFromFocus > cameraTargetDistanceBuffer)
        {
            Vector3 translation = (focus - transform.position) * 0.02f;
            translation.y = 0.0f;
            transform.Translate(translation, Space.World);
        }

        float yPos = Mathf.Clamp(0.4f * (transform.position - target.position).magnitude, 10.0f, 50.0f);
        transform.Translate(0.0f, yPos - transform.position.y, 0.0f);

        transform.LookAt(focus);
    }
}
