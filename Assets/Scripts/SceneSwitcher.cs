using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void SwitchScene(int tarScene)
    {
        SceneManager.LoadScene(tarScene, LoadSceneMode.Single);
    }
}