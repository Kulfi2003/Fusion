using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwipeInput : MonoBehaviour
{
    private Vector2 startTouchPosition, endTouchPosition;
    private bool swipeDetected;
    private bool swipeIgnored;
    public float ignoreSwipeIfShorterThan;
    private float dpiScaleFactor;
    [SerializeField] GameManager GameManager;
    [SerializeField] Text text;

    private void Start()
    {
        dpiScaleFactor = Screen.dpi / 160f; // 160 is the baseline DPI for many devices
        ignoreSwipeIfShorterThan *= dpiScaleFactor;
    }

    void Update()
    {
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    swipeDetected = false;
                    swipeIgnored = IsTouchOnButton(startTouchPosition); // Check if the touch began on a button
                    break;

                case TouchPhase.Moved:
                    if (!swipeIgnored)
                    {
                        endTouchPosition = touch.position;
                    }
                    break;

                case TouchPhase.Ended:
                    if (!swipeDetected && !swipeIgnored)
                    {
                        endTouchPosition = touch.position;
                        DetectSwipeDirection();
                        swipeDetected = true;
                    }
                    break;
            }
        }
    }

    private bool IsTouchOnButton(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Button"))
            {
                return true;
            }
        }

        return false;
    }

    private void DetectSwipeDirection()
    {
        Vector2 swipeVector = endTouchPosition - startTouchPosition;
        float horizontalSwipeDistance = Mathf.Abs(swipeVector.x);
        float verticalSwipeDistance = Mathf.Abs(swipeVector.y);

        text.text = horizontalSwipeDistance.ToString() + ", " + verticalSwipeDistance.ToString();

        if (Vector3.Distance(startTouchPosition, endTouchPosition) > ignoreSwipeIfShorterThan)
        {
            if (horizontalSwipeDistance > verticalSwipeDistance)
            {
                // Horizontal swipe
                if (swipeVector.x > 0)
                {
                    OnSwipeRight();
                }
                else
                {
                    OnSwipeLeft();
                }
            }
            else
            {
                // Vertical swipe
                if (swipeVector.y > 0)
                {
                    OnSwipeUp();
                }
                else
                {
                    OnSwipeDown();
                }
            }
        }
    }

    private void OnSwipeUp()
    {
        if (GameManager.canPlay)
        {
            GameManager.GoUp();
        }
        else
        {
            StartCoroutine(InputBuffer("up"));
        }
    }

    private void OnSwipeDown()
    {
        if (GameManager.canPlay)
        {
            GameManager.GoDown();
        }
        else
        {
            StartCoroutine(InputBuffer("down"));
        }
    }

    private void OnSwipeLeft()
    {
        if (GameManager.canPlay)
        {
            GameManager.GoLeft();
        }
        else
        {
            StartCoroutine(InputBuffer("left"));
        }
    }

    private void OnSwipeRight()
    {
        if (GameManager.canPlay)
        {
            GameManager.GoRight();
        }
        else
        {
            StartCoroutine(InputBuffer("right"));
        }
    }

    private bool isRunning = false;

    IEnumerator InputBuffer(string input)
    {
        if (isRunning)
            yield break;

        isRunning = true;

        while (!GameManager.canPlay)
        {
            yield return null; // Wait until the next frame
        }

        switch (input)
        {
            case "up":
                GameManager.GoUp();
                break;
            case "down":
                GameManager.GoDown();
                break;
            case "left":
                GameManager.GoLeft();
                break;
            case "right":
                GameManager.GoRight();
                break;
        }

        isRunning = false;
    }
}
