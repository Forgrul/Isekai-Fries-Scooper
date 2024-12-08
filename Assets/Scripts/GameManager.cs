using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    bool isFloating = false; 
    public bool isHardcore = false;

    List<List<int>> LevelRecord = new List<List<int>>();

    [System.Serializable]
    public class Contraint
    {
        public int hp;
        public float time;
    }

    [Header("Level Settings")]
    public int numberOfLevel = 4;
    public List<Contraint> normalMode = new List<Contraint>();
    public List<Contraint> hardcoreMode = new List<Contraint>();

    [Header("UI Settings")]
    public GameObject buttonPrefab; 
    public GameObject starPrefab;
    Transform buttonContainer; 
    int nowLevelNum;
    public GameObject MainUIPrefab;
    GameObject MainUI;
    public GameObject LevelSelctionMenuPrefab;
    GameObject LevelSelctionMenu;
    public GameObject WinCanvasPrefab;
    GameObject WinCanvas;
    public GameObject LoseCanvasPrefab;
    GameObject LoseCanvas;

    private void Awake()
    {
        // Implement singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep the GameManager persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        for(int i = 0; i < numberOfLevel + 1; i++) LevelRecord.Add(new List<int> {0, 0});
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;
        if (scene.buildIndex == 0 && MainUIPrefab != null && LevelSelctionMenuPrefab != null)
        {
            MainUI = Instantiate(MainUIPrefab);
            LevelSelctionMenu = Instantiate(LevelSelctionMenuPrefab);
            buttonContainer = LevelSelctionMenu.transform.Find("Container");

            MainUI.transform.Find("TutorialButton").GetComponent<Button>().onClick.
                AddListener(() =>
                {
                    SwitchScene(1);
                });
            MainUI.transform.Find("LevelSelectButton").GetComponent<Button>().onClick.
                AddListener(() =>
                {
                    HidePopup(MainUI);
                    ShowPopup(LevelSelctionMenu);
                    CreateButtons();
                });
            MainUI.transform.Find("HardcoreButton").GetComponent<Button>().onClick.
                AddListener(() =>
                {
                    switchHardcoreMode();
                    MainUI.transform.Find("HardcoreButton").GetComponent<ToggleSwitch>().Toggle();
                });

            LevelSelctionMenu.transform.Find("LevelBossButton").GetComponent<Button>().onClick.
                AddListener(() =>
                {
                    SwitchScene(6);
                });
            LevelSelctionMenu.transform.Find("BackButton").GetComponent<Button>().onClick.
                AddListener(() =>
                {
                    HidePopup(LevelSelctionMenu);
                    ShowPopup(MainUI);
                });

            LevelSelctionMenu.SetActive(false);
            StartCoroutine(ShowAfterDelay(MainUI));
        }
        else if(scene.buildIndex != 0)
        {   
            nowLevelNum = scene.buildIndex - 2;
            if(WinCanvasPrefab != null) 
            {
                WinCanvas = Instantiate(WinCanvasPrefab);
                WinCanvas.SetActive(false);
            }
            if(LoseCanvasPrefab != null) 
            {
                LoseCanvas = Instantiate(LoseCanvasPrefab);
                LoseCanvas.SetActive(false);
            }
        }
    }

    public void showEnd(bool isWin)
    {
        Time.timeScale = 0;
        if(isWin)
        {
            TMP_Text Title = WinCanvas.transform.Find("Title").GetComponent<TMP_Text>(); 
            TMP_Text WinningMessage = WinCanvas.transform.Find("WinningMessage").GetComponent<TMP_Text>(); 
            TMP_Text Constraint1 = WinCanvas.transform.Find("Constraint1").GetComponent<TMP_Text>(); 
            TMP_Text Constraint2 = WinCanvas.transform.Find("Constraint2").GetComponent<TMP_Text>();   
            if(nowLevelNum == -1) WinningMessage.text = "Congratulations!!\nYou pass the tutorial stage!\nLet's start our journey!";
            else
            {
                LevelTimer leveltimer = GameObject.Find("StatusUI").transform.Find("LevelTimer").GetComponent<LevelTimer>();
                Player p = GameObject.Find("Player").transform.Find("player").GetComponent<Player>();
                float timeConsume = leveltimer.getElapsedTime();
                int curHearts = p.getCurrentHearts();
                int thisRecord = 1, numOfStar = 1;
                if(isHardcore)
                {
                    Constraint1.text = "HP > " + (hardcoreMode[nowLevelNum].hp - 1);
                    Constraint2.text = "Time < " + hardcoreMode[nowLevelNum].time;
                    if(curHearts >= hardcoreMode[nowLevelNum].hp) 
                    {
                        thisRecord += 2;
                        numOfStar++;
                    }
                    if(timeConsume <= hardcoreMode[nowLevelNum].time) 
                    {
                        thisRecord += 4;
                        numOfStar++;
                    }
                }
                else
                {
                    Constraint1.text = "HP > " + (normalMode[nowLevelNum].hp - 1);
                    Constraint2.text = "Time < " + normalMode[nowLevelNum].time;
                    if(curHearts >= normalMode[nowLevelNum].hp) 
                    {
                        thisRecord += 2;
                        numOfStar++;
                    }
                    if(timeConsume <= normalMode[nowLevelNum].time) 
                    {
                        thisRecord += 4;
                        numOfStar++;
                    }
                }

                int mask = 1;
                Vector2 startPosition = new Vector2(125, 110);
                for(int i = 0; i < 3; i++)
                {
                    GameObject newStar = Instantiate(starPrefab, WinCanvas.transform);
                    
                    if((thisRecord & mask) == 0) newStar.GetComponent<Image>().color = Color.black;
                    mask <<= 1;
                    
                    RectTransform starTransform = newStar.GetComponent<RectTransform>();
                    starTransform.anchoredPosition = startPosition + new Vector2(0, i * -100);
                    starTransform.localScale = new Vector3(1f, 1f, 1f);
                }

                if(isHardcore) 
                {
                    if(thisRecord == 7) LevelRecord[nowLevelNum][1] = thisRecord;
                    else if(thisRecord == 1 || thisRecord == 5) LevelRecord[nowLevelNum][1] = Mathf.Max(LevelRecord[nowLevelNum][1], thisRecord);
                    else 
                    {
                        if(LevelRecord[nowLevelNum][1] == 5) LevelRecord[nowLevelNum][1] = thisRecord;
                        else LevelRecord[nowLevelNum][1] = Mathf.Max(LevelRecord[nowLevelNum][1], thisRecord);
                    }
                    
                }
                else 
                {
                    if(thisRecord == 7) LevelRecord[nowLevelNum][0] = thisRecord;
                    else if(thisRecord == 1 || thisRecord == 5) LevelRecord[nowLevelNum][0] = Mathf.Max(LevelRecord[nowLevelNum][0], thisRecord);
                    else 
                    {
                        if(LevelRecord[nowLevelNum][0] == 5) LevelRecord[nowLevelNum][0] = thisRecord;
                        else LevelRecord[nowLevelNum][0] = Mathf.Max(LevelRecord[nowLevelNum][0], thisRecord);
                    }
                }

                Title.text = "Congratulations!!\nYou get " + numOfStar + " star" + ((numOfStar >= 2)? "s!": "!");
                WinningMessage.text = "";
            }
            WinCanvas.SetActive(true);
        }
        else LoseCanvas.SetActive(true);
    }

    public void ChangeFloatStatus(bool newStatus)
    {
        isFloating = newStatus;
    }

    public bool GetFloatStatus()
    {
        return isFloating;
    }

    public bool GetHardcoreMode()
    {
        return isHardcore;
    }

    public void switchHardcoreMode()
    {
        isHardcore = !isHardcore;
    }

    IEnumerator ShowAfterDelay(GameObject a)
    {
        a.SetActive(false);
        yield return new WaitForSeconds(2.4f);
        a.SetActive(true);
    }

    public void CreateButtons()
    {
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        int nowRecord, mask;
        Vector2 startPosition = new Vector2(-50, -100);
        for (int i = 0; i < numberOfLevel; i++)
        {
            GameObject newButton = Instantiate(buttonPrefab, buttonContainer);
            
            if(isHardcore) nowRecord = LevelRecord[i][1];
            else nowRecord = LevelRecord[i][0];

            mask = 1;
            for(int j = 0; j < 3; j++)
            {
                GameObject newStar = Instantiate(starPrefab, newButton.transform);
                if((nowRecord & mask) == 0) newStar.GetComponent<Image>().color = Color.black;
                mask <<= 1;
                
                RectTransform starTransform = newStar.GetComponent<RectTransform>();
                starTransform.anchoredPosition = startPosition + new Vector2(j * 50, 0);
                starTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
            }
            
            if(isHardcore) newButton.GetComponent<Image>().color = Color.red;
            else newButton.GetComponent<Image>().color = Color.green;
            newButton.name = $"Button {i + 1}";
            newButton.GetComponentInChildren<TextMeshProUGUI>().text = $"{i + 1}";

            int index = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => {SceneManager.LoadScene(index + 2, LoadSceneMode.Single);});
        }

        if(isHardcore) LevelSelctionMenu.transform.Find("LevelBossButton").GetComponent<Image>().color = Color.red;
        else LevelSelctionMenu.transform.Find("LevelBossButton").GetComponent<Image>().color = Color.green;

        if(isHardcore) nowRecord = LevelRecord[numberOfLevel][1];
        else nowRecord = LevelRecord[numberOfLevel][0];
        mask = 1;
        for(int j = 0; j < 3; j++)
        {
            GameObject newStar = Instantiate(starPrefab, LevelSelctionMenu.transform.Find("LevelBossButton").transform);
            if((nowRecord & mask) == 0) newStar.GetComponent<Image>().color = Color.black;
            mask <<= 1;
                
            RectTransform starTransform = newStar.GetComponent<RectTransform>();
            starTransform.anchoredPosition = startPosition + new Vector2(j * 50, 0);
            starTransform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
    }

    public void ShowPopup(GameObject popupPanel)
    {
        popupPanel.SetActive(true);
    }

    public void HidePopup(GameObject popupPanel)
    {
        popupPanel.SetActive(false);
    }

    public void SwitchScene(int tarScene)
    {
        SceneManager.LoadScene(tarScene, LoadSceneMode.Single);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
