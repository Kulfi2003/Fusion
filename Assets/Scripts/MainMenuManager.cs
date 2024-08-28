using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject[] levelButtons;
    int currentLevel;
    [SerializeField] CameraPan cameraPan;
    public GameObject buttonParticles;
    [SerializeField] int totalLevels = 30;
    bool firstTime = false;

    private void Awake()
    {
        totalLevels = SceneManager.sceneCountInBuildSettings - 2;

        if (!PlayerPrefs.HasKey("currentLevel"))
        {
            // Initialize PlayerPrefs values
            PlayerPrefs.SetInt("currentLevel", 1);
            PlayerPrefs.SetInt("levelToLoad", 1);

            firstTime = true;

            // Save the changes
            PlayerPrefs.Save();
        }

        currentLevel = PlayerPrefs.GetInt("currentLevel");

        print("the current level playerpref is set to " + currentLevel);
    }

    [SerializeField] Transform firstTimePos;
    [SerializeField] GameObject firstTimeMenu;   
    private void Start()
    {
        //Application.targetFrameRate = 30; //setting target fps

        if (firstTime)
        {
            cameraPan.enabled = false;
            firstTime = false;
            cameraLerp.DirectTeleport(firstTimePos);
        } else
        {
            Destroy(firstTimeMenu);
        }

        //Setting camera bounds
        cameraPan.panLimitMin.y = levelButtons[currentLevel - 1].transform.position.z - 10;

        //Disabling all buttons first
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].SetActive(false);
        }

        //Enabling the buttons that have been enabled
        for (int i = 0; i < currentLevel; i++)
        {
            if (i < totalLevels)
            {
                levelButtons[i].SetActive(true);
            }
        }

        if (currentLevel < totalLevels+1)
        {
            buttonParticles.transform.position = levelButtons[currentLevel-1].transform.position;

            //lerps view to the position of the current level.
            if (currentLevel > 3)
            {
                StartCoroutine(goToCurrentLevel());
            }
        } else
        {
            //deactivating the particles if the game is complete.
            buttonParticles.SetActive(false);
        }
        
    }

    public LoadingScreen loadingScreen;

    public void LoadScene(int scene)
    {
        //keep offset of 1 because core is before level in build settings
        scene++;

        StartCoroutine(LoadLevel(scene));
    }

    public MusicPlayer musicPlayer;
    private IEnumerator LoadLevel(int scene)
    {
        musicPlayer.StopMusicWithFadeOut();

        PlayerPrefs.SetInt("levelToLoad", scene);
        loadingScreen.LerpAlpha(1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator goToCurrentLevel()
    {
        cameraPan.enabled = false;
        yield return new WaitForSeconds(1f);
        start.position = new(start.position.x, start.position.y, levelButtons[currentLevel - 2].transform.position.z);
        cameraLerp.MoveToPos(start);
        cameraPan.enabled = true;

    }

    [SerializeField] private Transform start;
    [SerializeField] CameraLerp cameraLerp;
}
