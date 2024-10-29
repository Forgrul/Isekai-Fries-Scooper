using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI components

public class Win : MonoBehaviour
{
    // public GameObject congratulationsText; // Assign your UI text GameObject in the Inspector

    private void Start()
    {
        // Make sure the congratulations text is not visible at the start
        // congratulationsText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the colliding object is the player
        {
            // Show the congratulations message
            // congratulationsText.SetActive(true);
            // Debug.Log("Player has won!"); // For debugging purposes

            QuitGame(); // Wait for 2 seconds before quitting (optional)
            // Call the method to quit the game
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
