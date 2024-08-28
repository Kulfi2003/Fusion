using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int level;
    [SerializeField] TextMeshPro textMeshPro;
    [SerializeField] float textRotationRange = 10f;

    private void Start()
    {
        ExtractLevelNumber();

        // Rotating the text on the y-axis randomly using Euler angles
        float randomYRotation = Random.Range(-textRotationRange, textRotationRange);
        textMeshPro.rectTransform.rotation = Quaternion.Euler(90f, randomYRotation, 0f);

        // Set the text to the level number
        textMeshPro.text = "LEVEL " + FormatNumber(level);
    }

    public string FormatNumber(int number)
    {
        return number.ToString("D2");
    }

    public void OpenLevel()
    {
        MainMenuManager mainMenuManager = FindObjectOfType<MainMenuManager>();
        mainMenuManager.LoadScene(level);
    }

    public void ExtractLevelNumber()
    {
        string objectName = gameObject.name; // Get the name of the gameObject
        string prefix = "Level (";          // Define the prefix to search for

        if (objectName.StartsWith(prefix) && objectName.EndsWith(")"))
        {
            // Extract the number between the parentheses
            string numberString = objectName.Substring(prefix.Length, objectName.Length - prefix.Length - 1);

            // Try to parse the number string to an integer
            if (int.TryParse(numberString, out int extractedLevel))
            {
                level = extractedLevel;
                Debug.Log("Extracted level: " + level);
            }
            else
            {
                Debug.LogError("Failed to parse level number from: " + objectName);
            }
        }
        else
        {
            Debug.LogError("GameObject name does not follow the expected format: " + objectName);
        }
    }
}
