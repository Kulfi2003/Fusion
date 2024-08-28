using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;
    int currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        textMeshPro = GetComponent<TextMeshPro>();

        currentLevel = PlayerPrefs.GetInt("levelToLoad") - 1;

        textMeshPro.text = "LEVEL " + currentLevel;
    }

    public void NextLevelText()
    {
        currentLevel++;
        textMeshPro.text = "LEVEL " + currentLevel;
    }
}
