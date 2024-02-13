using UnityEngine;

public class SmoothFollowCamera : MonoBehaviour
{
   public Transform target;
   public float smoothSpeed = 0.2f;
   public Vector3 offset;

   private void FixedUpdate()
   {
      Vector3 newPosition = target.position + offset;
      Vector3 smoothedPosition = Vector3.Lerp(transform.position, newPosition, smoothSpeed);
      transform.position = smoothedPosition;

      //transform.LookAt(target);
   }
}
