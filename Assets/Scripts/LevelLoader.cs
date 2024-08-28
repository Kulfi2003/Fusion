using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.HDROutputUtils;
using UnityEngine.Events;


public class LevelLoader : MonoBehaviour
{
    public LoadingScreen WhiteScreen;
    public GameManager gameManager;
    public UnityEvent AllLevelsComplete;
    public int levelNumber;
    public UnityEvent enableHintLight;
    public int resetsBeforeHintLight;
    [SerializeField] InterstitialAdsBehaviour inter;
        
    int resetCount = 0;

    public void LoadScene()
    {
        StartCoroutine(LoadLevel());
    }

    public void NextScene()
    {
        resetCount = 0;
        if (levelNumber+1 < SceneManager.sceneCountInBuildSettings)
        {
            StartCoroutine(NextLevel());
            inter.incrementCount();
        }
        else
        {
            AllLevelsComplete.Invoke();
        }
    }

    public void ResetScene()
    {
        
        StartCoroutine(ResetLevel());
    }

    private IEnumerator LoadLevel()
    {
        SceneManager.LoadSceneAsync(levelNumber, LoadSceneMode.Additive).completed += (operation) =>
        {
            gameManager.InitializeLevel();
            WhiteScreen.LerpAlpha(0f);
        };

        yield return new WaitForSeconds(0.1f);

        
    }

    [SerializeField] TextChanger a, b;

    private IEnumerator NextLevel()
    {
        WhiteScreen.LerpAlpha(1f);

        yield return new WaitForSeconds(1);

        SceneManager.UnloadSceneAsync(levelNumber).completed += (operation) =>
        {
            //Set all level texts in the scene
            a.NextLevelText();
            b.NextLevelText();

            //this piece of code sets the currentlevel and leveltoload playerprefs.

            levelNumber++;
            PlayerPrefs.SetInt("levelToLoad", levelNumber);
            
            
            SceneManager.LoadSceneAsync(levelNumber, LoadSceneMode.Additive).completed += (operation) =>
            {
                gameManager.InitializeLevel();
                WhiteScreen.LerpAlpha(0f);
            };
        };
        yield return new WaitForSeconds(1);
        gameManager.InitializeLevel();
    }

    private IEnumerator ResetLevel()
    {
        resetCount++;

        WhiteScreen.LerpAlpha(1f);
        
        if (resetCount == resetsBeforeHintLight)
        {
            enableHintLight.Invoke();
        }

        yield return new WaitForSeconds(1);

        SceneManager.UnloadSceneAsync(levelNumber).completed += (operation) =>
        {
            SceneManager.LoadSceneAsync(levelNumber, LoadSceneMode.Additive).completed += (operation) =>
            {
                gameManager.InitializeLevel();
                WhiteScreen.LerpAlpha(0f);
            };
        };
    }

    public void GoToHome()
    {
        StartCoroutine(HomeLevel());
    }

    public MusicPlayer musicPlayer;
    private IEnumerator HomeLevel()
    {
        musicPlayer.StopMusicWithFadeOut();

        WhiteScreen.LerpAlpha(1f);

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(0);
    }

}
