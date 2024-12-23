using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Make sure to include this for UI components

public class Goal : MonoBehaviour
{
    
    private void Start()
    {
        // Make sure the congratulations text is not visible at the start
        // congratulationsText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the colliding object is the player
        {
            GameManager.Instance.showEnd(true);
        }
    }

    private void QuitGame()
    {
        // If running in the editor
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop playing the game in the editor
        #else
            Application.Quit(); // Quit the application
        #endif
    }
}
