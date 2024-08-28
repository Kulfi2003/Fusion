using UnityEngine;
using UnityEngine.Events;

public class CubeToggle : MonoBehaviour
{
    public Transform targetTransform1; // First target transform
    public Transform targetTransform2; // Second target transform
    public float moveSpeed = 5f; // Speed of movement
    public UnityEvent OnToggleOn; // Event when cube is toggled to targetTransform1
    public UnityEvent OnToggleOff; // Event when cube is toggled to targetTransform2

    [SerializeField] GameObject onSwitch, offSwitch;

    [SerializeField] AudioSource AudioSource;

    private bool isToggledOn = true; // Toggle state
    [SerializeField] string playerPrefName;

    private void Start()
    {
        if (playerPrefName != null)
        {
            if (PlayerPrefs.GetInt(playerPrefName) != 1)
            {
                Toggle();
            }
        }
    }

    private void Update()
    {
        // Handle touch input
        if (Input.touchCount == 1 && !isMoving)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == transform)
                {
                    AudioSource.Play();
                    Toggle();
                }
            }
        }
    }

    private void Toggle()
    {
        isMoving = true;
        if (isToggledOn)
        {
            PlayerPrefs.SetInt(playerPrefName, 0);
            // Move to the first target transform
            StartCoroutine(MoveToPosition(targetTransform1.position));
            OnToggleOff?.Invoke();

            onSwitch.SetActive(false);
            offSwitch.SetActive(true);
        }
        else
        {
            PlayerPrefs.SetInt(playerPrefName, 1);
            // Move to the second target transform
            StartCoroutine(MoveToPosition(targetTransform2.position));
            OnToggleOn?.Invoke();

            onSwitch.SetActive(true);
            offSwitch.SetActive(false);
        }

        isToggledOn = !isToggledOn;
    }

    bool isMoving = false;

    public AnimationCurve movementCurve;
    public float duration = 0.25f;


    private System.Collections.IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            float curveValue = movementCurve.Evaluate(t);
            transform.position = Vector3.Lerp(startPosition, targetPosition, curveValue);
            yield return null;
        }

        // Ensure the final position is exactly the target position
        transform.position = targetPosition;


        isMoving = false;
    }
}
