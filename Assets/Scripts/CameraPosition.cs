using UnityEngine;
using System.Collections;

public class CameraLerp : MonoBehaviour
{
    public float moveDuration = 2f;  // Duration of the movement
    public AnimationCurve easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);  // Default ease-in, ease-out curve

    private bool isMoving = false;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float elapsedTime = 0f;

    // This method starts the movement process
    public void MoveToPos(Transform targetTransform)
    {
        startPos = transform.position;
        targetPos = targetTransform.position;
        elapsedTime = 0f;
        isMoving = true;
    }

    public void DirectTeleport(Transform targetTransform)
    {
        transform.position = targetTransform.position;
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;

            // Apply the easing curve
            float easeT = easeCurve.Evaluate(t);

            // Lerp to the target position based on the eased time
            transform.position = Vector3.Lerp(startPos, targetPos, easeT);

            // Stop the movement if the duration has been reached
            if (elapsedTime >= moveDuration)
            {
                transform.position = targetPos;  // Ensure it ends exactly at the target position
                isMoving = false;
            }
        }
    }

}
