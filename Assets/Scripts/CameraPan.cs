using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour
{
    public float panSpeed = 0.005f; // Speed for smoother panning
    public float inertiaDuration = 0.5f; // Duration of the inertia effect
    public float decelerationRate = 0.95f; // Rate of deceleration during inertia
    public Vector2 panLimitMin; // Minimum limits for camera panning (x and z)
    public Vector2 panLimitMax; // Maximum limits for camera panning (x and z)
    public float dpiNormalization = 1.0f; // Adjust based on DPI if necessary
    public float softBoundDistance = 1.0f; // Distance from bounds where the camera will be pushed back
    public float softBoundStrength = 1.0f; // Strength of the pushback effect near bounds

    private Vector3 lastPanPosition;
    private Vector3 panVelocity;
    private bool isPanning;
    private float inertiaTime;
    public bool waitBeforeStarting = false;

    void Start()
    {
        dpiNormalization = Screen.dpi / 160f; // Adjust for DPI
    }

    public void TwoSecondsWait()
    {
        waitBeforeStarting = true;
    }


    private void OnEnable()
    {
            StartCoroutine(WaitAndSetFalse());
    }

    void Update()
    {
        if (!waitBeforeStarting)
        {
            HandleTouchInput();
            ApplyInertia();
            if (inertiaTime <= 0f)
            {
                ApplySoftBounds();
            }
        }
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = new Vector3(touch.position.x, 0, touch.position.y);
                isPanning = true;
                inertiaTime = 0f;
            }
            else if (touch.phase == TouchPhase.Moved && isPanning)
            {
                Vector3 currentPanPosition = new Vector3(touch.position.x, 0, touch.position.y);
                Vector3 touchDelta = (currentPanPosition - lastPanPosition) / dpiNormalization;
                Vector3 panMovement = new Vector3(-touchDelta.x * panSpeed, 0, -touchDelta.z * panSpeed);

                transform.Translate(panMovement, Space.World);

                // Clamp the camera position to the defined limits
                Vector3 clampedPosition = transform.position;
                clampedPosition.x = Mathf.Clamp(clampedPosition.x, panLimitMin.x, panLimitMax.x);
                clampedPosition.z = Mathf.Clamp(clampedPosition.z, panLimitMin.y, panLimitMax.y);
                transform.position = clampedPosition;

                panVelocity = panMovement / Time.deltaTime; // Calculate velocity
                lastPanPosition = currentPanPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isPanning = false;
                inertiaTime = inertiaDuration;
            }
        }
    }

    void ApplyInertia()
    {
        if (!isPanning && inertiaTime > 0f)
        {
            transform.Translate(panVelocity * Time.deltaTime, Space.World);

            // Clamp the camera position to the defined limits
            Vector3 clampedPosition = transform.position;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, panLimitMin.x, panLimitMax.x);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, panLimitMin.y, panLimitMax.y);
            transform.position = clampedPosition;

            panVelocity *= decelerationRate;
            inertiaTime -= Time.deltaTime;

            if (inertiaTime <= 0f)
            {
                panVelocity = Vector3.zero; // Stop the movement when inertia time ends
            }
        }
    }

    void ApplySoftBounds()
    {
        Vector3 position = transform.position;

        if (position.x < panLimitMin.x + softBoundDistance)
        {
            float offset = (panLimitMin.x + softBoundDistance - position.x) * softBoundStrength;
            position.x += offset * Time.deltaTime;
        }
        else if (position.x > panLimitMax.x - softBoundDistance)
        {
            float offset = (position.x - (panLimitMax.x - softBoundDistance)) * softBoundStrength;
            position.x -= offset * Time.deltaTime;
        }

        if (position.z < panLimitMin.y + softBoundDistance)
        {
            float offset = (panLimitMin.y + softBoundDistance - position.z) * softBoundStrength;
            position.z += offset * Time.deltaTime;
        }
        else if (position.z > panLimitMax.y - softBoundDistance)
        {
            float offset = (position.z - (panLimitMax.y - softBoundDistance)) * softBoundStrength;
            position.z -= offset * Time.deltaTime;
        }

        transform.position = position;
    }

    private IEnumerator WaitAndSetFalse()
    {
        // Wait for 1 second
        yield return new WaitForSeconds(2f);

        // Set the bool to false
        waitBeforeStarting = false;
    }

}
