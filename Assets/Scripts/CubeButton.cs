using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CubeButton : MonoBehaviour
{
    Vector3 targetPosition; // The target position to lerp to
    public float lerpSpeed = 1.0f;   // Speed of the lerp
    public float pressAmount = 0.4f;
    private Vector3 originalPosition; // Store the original position of the object
    bool buttonPressed = false;
    [SerializeField] float delay = 1.5f;
    const float touchMovementThreshold = 10.0f; // Threshold to determine if touch has moved
    public bool dontTouchOnSwipe = false;

    public UnityEvent onCubeTouched;

    public AudioSource audioSource;

    private void Start()
    {
        originalPosition = transform.position;
        targetPosition = new Vector3(transform.position.x, transform.position.y - pressAmount, transform.position.z);
    }

    private Vector2 startTouchPosition;

    void Update()
    {
        if (!buttonPressed)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    if (!dontTouchOnSwipe)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform == transform)
                            {
                                float randomPitch = UnityEngine.Random.Range(0.5f, 1);
                                audioSource.pitch = randomPitch;
                                audioSource.Play();

                                StartCoroutine(LerpToTargetAndBack());
                                StartCoroutine(DisablePressForSeconds());
                            }
                        }
                    }
                    
                    // Record the start touch position
                    startTouchPosition = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended && dontTouchOnSwipe)
                {
                    Vector2 endTouchPosition = touch.position;

                    // Check if the touch moved significantly
                    if (Vector2.Distance(startTouchPosition, endTouchPosition) < touchMovementThreshold)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.transform == transform)
                            {
                                float randomPitch = UnityEngine.Random.Range(0.5f, 1);
                                audioSource.pitch = randomPitch;
                                audioSource.Play();

                                StartCoroutine(LerpToTargetAndBack());
                                StartCoroutine(DisablePressForSeconds());
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator LerpToTargetAndBack()
    {
        yield return StartCoroutine(LerpPosition(transform.position, targetPosition, lerpSpeed * 1.5f));

        onCubeTouched.Invoke();

        yield return StartCoroutine(LerpPosition(transform.position, originalPosition, lerpSpeed));
    }

    IEnumerator LerpPosition(Vector3 start, Vector3 end, float speed)
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, time);
            yield return null;
        }
        transform.position = end; // Ensure the final position is set
    }

    IEnumerator DisablePressForSeconds()
    {
        buttonPressed = true;

        yield return new WaitForSeconds(delay);

        buttonPressed = false;
    }
}
