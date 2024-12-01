using UnityEngine;

public class PopupController : MonoBehaviour
{
    public void ShowPopup(GameObject popupPanel)
    {
        popupPanel.SetActive(true);
    }

    // 方法：隱藏面板
    public void HidePopup(GameObject popupPanel)
    {
        popupPanel.SetActive(false);
    }
}
