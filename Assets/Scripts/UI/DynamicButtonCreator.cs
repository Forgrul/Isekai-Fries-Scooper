using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 

    public void CreateButtons(int numberOfButtons)
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < numberOfButtons; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            newButton.name = $"Button {i + 1}";
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{i + 1}";

            int index = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => {SceneManager.LoadScene(index + 2, LoadSceneMode.Single);});
        }
    }
}
