using UnityEngine;
using System.Collections;
using System.Security.Cryptography;

public class HintSystem : MonoBehaviour
{
    GameObject[] cubes; // Array of cubes to animate

    private void Awake()
    {
        transform.position = new Vector3(transform.position.x - 0.25f, transform.position.y, transform.position.z-0.25f); 
    }

    void OnEnable()
    {
        cubes = null;
        int childCount = transform.childCount;
        cubes = new GameObject[childCount];

        Vector3 temp;

        // Assign each child to the cubes array in the order they appear in the hierarchy, and set their initial transforms.
        for (int i = 0; i < childCount; i++)
        {
            cubes[i] = transform.GetChild(i).gameObject;
            cubes[i].transform.localScale = Vector3.zero;
            temp = cubes[i].transform.position;
            temp.y = 3f;
            cubes[i].transform.position = temp;
        }

        if (cubes.Length > 0)
        {
            StartCoroutine(StartWave());
        }
    }

    IEnumerator StartWave()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < cubes.Length; i++)
            {
                //StartCoroutine(MoveCubeUpAndDown(cubes[i]));
                StartCoroutine(ScaleCubeUpAndDown(cubes[i]));

                yield return new WaitForSeconds(1/waveSpeed);
            }

            yield return new WaitForSeconds(2f); // wait time between waves
            
        }

        gameObject.SetActive(false);


    }

    public AnimationCurve curve;
    [SerializeField] float waveSpeed = 10f;
    [SerializeField] float cubeDuration = 1f;

    //IEnumerator MoveCubeUpAndDown(GameObject cube)
    //{
    //    Vector3 startPosition = cube.transform.position;
    //    float elapsedTime = 0f;

    //    while (elapsedTime < cubeDuration)
    //    {
    //        elapsedTime += Time.deltaTime;
    //        float yOffset = curve.Evaluate(elapsedTime / cubeDuration);
    //        cube.transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
    //        yield return null;
    //    }

    //    // Ensure the final position is set precisely at the end
    //    cube.transform.position = startPosition;

    //}
    Vector3 originalScale = Vector3.one;

    IEnumerator ScaleCubeUpAndDown(GameObject cube)
    {
        
        float elapsedTime = 0f;

        while (elapsedTime < cubeDuration)
        {
            elapsedTime += Time.deltaTime;
            float scaleFactor = curve.Evaluate(elapsedTime / cubeDuration);
            cube.transform.localScale = originalScale * scaleFactor;
            yield return null;
        }

        // Ensure the final scale is set precisely at the end
        cube.transform.localScale = Vector3.zero;
    }
}
