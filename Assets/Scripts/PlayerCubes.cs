using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCubes : MonoBehaviour
{
    public UnityEvent OnWin;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject gameManagerObject = GameObject.Find("GameManager");
            GameManager gameManager = gameManagerObject.GetComponent<GameManager>();
            gameManager.gameOver = true;
            gameManager.gameOverFunction();
            OnWin.Invoke();
        }
    }
}
