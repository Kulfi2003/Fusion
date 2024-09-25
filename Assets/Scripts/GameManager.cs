using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public int direction = 0;
    //0 = none
    //1 = up
    //2 = down
    //3 = left
    //4 = right

    public bool gameOver = false;
    public bool canPlay = true;
    //public bool hintShown = false;
    public float pauseAfterGameOver = 3f;
    public UnityEvent gameOverEvent;
    public UnityEvent gameOverEventWithRating;
    public UnityEvent initializeEvent;
    public DirectionalShake directionalShake;
    public ApplyForceToParticles[] applyForceToParticles;

    CubeMove[] rb;
    SnapToGrid[] SnapToGrid;
    //public BoxCollider[] boxCollidersWithRigidbodies;


    public AudioManager audioManager;

    [SerializeField] float moveForce = 10f;

    [SerializeField] Vector3 newVelocity = new(0, 0, 0);


    public LevelLoader LevelLoader;

    [SerializeField] int level;
    [SerializeField] bool testingLevels = false;

    private void Start()
    {
        //Application.targetFrameRate = 30; //setting target fps


        LevelLoader.levelNumber = PlayerPrefs.GetInt("levelToLoad");

        if (testingLevels)
        {
            LevelLoader.levelNumber = level + 1;
        }

        LevelLoader.LoadScene();

        
    }

    public void InitializeLevel()
    {
        initializeEvent.Invoke();

        canPlay = true;
        rb = null;
        SnapToGrid = null;

        rb = FindObjectsOfType<CubeMove>();
        SnapToGrid = FindObjectsOfType<SnapToGrid>();

        SnapToGridEvent();

        gameOver = false;
    }

    public void GoUp()
    {
        if (gameOver) { return; }

        SetIsMovingToTrue();
        canPlay = false;

        applyForceToParticles[0].ApplyForce(Vector3.forward);
        applyForceToParticles[1].ApplyForce(Vector3.forward);

        directionalShake.Shake(Vector3.forward);
        audioManager.cubeMovingSound.Play();
        SnapToGridEvent();
        newVelocity = Vector3.forward * moveForce;
 
        SetrbVelocity(newVelocity);
    }

    public void GoDown()
    {
        if (gameOver) { return; }

        SetIsMovingToTrue();
        canPlay = false;

        applyForceToParticles[0].ApplyForce(Vector3.back);
        applyForceToParticles[1].ApplyForce(Vector3.back);


        directionalShake.Shake(Vector3.back);

        audioManager.cubeMovingSound.Play();
        SnapToGridEvent();
        newVelocity = Vector3.back * moveForce;

        SetrbVelocity(newVelocity);
    }

    public void GoLeft()
    {
        if (gameOver) { return; }

        SetIsMovingToTrue();
        canPlay = false;

        applyForceToParticles[0].ApplyForce(Vector3.left);
        applyForceToParticles[1].ApplyForce(Vector3.left);

        directionalShake.Shake(Vector3.left);

        audioManager.cubeMovingSound.Play();
        SnapToGridEvent();
        newVelocity = Vector3.left * moveForce;

        SetrbVelocity(newVelocity);
    }

    public void GoRight()
    {
        if (gameOver) { return; }

        SetIsMovingToTrue();
        canPlay = false;

        applyForceToParticles[0].ApplyForce(Vector3.right);
        applyForceToParticles[1].ApplyForce(Vector3.right);

        directionalShake.Shake(Vector3.right);

        audioManager.cubeMovingSound.Play();
        SnapToGridEvent();
        newVelocity = Vector3.right * moveForce;

        SetrbVelocity(newVelocity);
    }

    public void SetrbVelocity(Vector3 newVelocity)
    {
        for (int i = 0; i < rb.Length; i++)
        {
            rb[i].velocity = newVelocity;
        }
    }

    public void SnapToGridEvent()
    {
        for (int i = 0; i < SnapToGrid.Length; i++)
        {
            if (SnapToGrid[i].isActiveAndEnabled)
            SnapToGrid[i].SnapToGridFunction();
        }
    }

    public void CheckIfAllStopped()
    {

        if (AllCubesStopped())
        {
            applyForceToParticles[0].StopForce();
            applyForceToParticles[1].StopForce();

            SnapToGridEvent();
            // Set the bool to true
            canPlay = true;
            //audioManager.cubeMovingSound.Stop();
        }
    }

    public void SetIsMovingToTrue()
    {
        foreach (CubeMove script in rb)
        {
            if (script.gameObject.activeInHierarchy)
            {
                script.isMoving = true;
            }
        }
    }

    bool AllCubesStopped()
    {
        foreach (CubeMove script in rb)
        {
            if (script.isMoving) // If any bool is true, return false
            {
                return false;
            }
        }
        return true; // All bools are false, return true

    }


    public void gameOverFunction(bool skipped)
    {
        audioManager.combineSFX.Play();

        Debug.Log("Win condition satisfied - your local neighbourhood game manager");

        gameOver = true;
        canPlay = false;

        //Increment level save playerpref
        if (PlayerPrefs.GetInt("currentLevel") < LevelLoader.levelNumber + 1)
        {
            PlayerPrefs.SetInt("currentLevel", LevelLoader.levelNumber);
            Debug.Log("the level that the player is currently on, is" + (LevelLoader.levelNumber - 1));
        }

        // Start coroutine to delay event invocation
        StartCoroutine(HandleGameOver(skipped));
    }

    int random;
    private IEnumerator HandleGameOver(bool skipped)
    {
        if (!skipped) yield return new WaitForSeconds(pauseAfterGameOver);

        //code that decides whether to send the player to the rate us page or the next level page
        if ((PlayerPrefs.GetInt("Rated")==0) && (LevelLoader.levelNumber > 3 && (LevelLoader.levelNumber+1)%3 == 0))
        {
            gameOverEventWithRating.Invoke();
        } else
        {
            // Invoke the game over event
            gameOverEvent.Invoke();
        }

        
    }
}