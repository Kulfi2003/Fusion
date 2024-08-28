using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DestroyObjectScript : MonoBehaviour
{
    public UnityEvent UnityEvent;
    public UnityEvent enableSuggestion;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject particleEffect;
    [SerializeField] GameManager gameManager;
    GameObject boundsObject;

    //private void Awake()
    //{
    //    isTrigger = gameObject.GetComponent<BoxCollider>().isTrigger;
    //}

    private void Start()
    {
        // Try to get the AudioSource component attached to the current GameObject
        audioSource = GetComponent<AudioSource>();
        boundsObject = GameObject.Find("Bounds");
        // If there is no AudioSource attached, find the "Bounds" object and reference its AudioSource
        if (audioSource == null)
        {
            
            if (boundsObject != null)
            {
                audioSource = boundsObject.GetComponent<AudioSource>();
            }
        }
    }

    private DestroyObjectScript destroyObjectScript;

    //trigger functionality is for destruction cube
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Cube"))
        {
            audioSource.Play();
            collision.gameObject.SetActive(false);
            UnityEvent.Invoke();

            Instantiate(particleEffect, collision.gameObject.transform.position, Quaternion.identity);

            //following section is to enable the hint suggestion light.
            if (collision.gameObject.CompareTag("Player"))
            {
                destroyObjectScript = boundsObject.GetComponent<DestroyObjectScript>();

                // Check if the script and event are not null before invoking the event
                if (destroyObjectScript != null && destroyObjectScript.enableSuggestion != null)
                {
                    // Invoke the enableSuggestion event
                    destroyObjectScript.enableSuggestion.Invoke();
                }
                else
                {
                    Debug.LogWarning("DestroyObjectScript or enableSuggestion event is not set.");
                }
            }
            


            //deactivating the destruction cube.
            transform.parent.gameObject.SetActive(false);

            
        }
    }

    //collision functionality is for bounds
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Cube"))
        {
            audioSource.Play();
            collision.gameObject.SetActive(false);
            UnityEvent.Invoke();

            Instantiate(particleEffect, collision.gameObject.transform.position, Quaternion.identity);

            if (collision.gameObject.CompareTag("Player"))
            {
                StartCoroutine(invokeIfGameGoing());
            }
        }
    }

    //functionality to turn on the reset particle system, only if the game is still going on
    private IEnumerator invokeIfGameGoing()
    {

        while (!gameManager.canPlay)
        {
            yield return null; // Wait until the next frame
        }

        yield return null; // wait one more frame just in case

        if (!gameManager.gameOver)
        {
            enableSuggestion.Invoke();
        }
    }
}