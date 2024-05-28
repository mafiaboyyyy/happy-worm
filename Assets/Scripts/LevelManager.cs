using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioSource audioSourceSound;
    public AudioClip  bg, click;

    private int currentLevel;
    private List<int> levels = new List<int>();
    private GameObject btnReset;
    private GameObject btnBack;
    private GameObject btnBackToHome;
    public Toggle btnMusic;
    public Toggle btnSound;
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level") > 0 ? PlayerPrefs.GetInt("level") : 1;
        for (int i = 1; i <= currentLevel; i++)
        {
            levels.Add(i);
        }
        levels.Add(currentLevel);
        handleLevelComplete();
        loadUIButton();
        playMusic(bg, 1);
        
    }

    private void loadUIButton()
    {
        btnReset = GameObject.Find("btn_reset");
        btnReset.SetActive(false);
        btnBack = GameObject.Find("btn_back");
        btnBack.SetActive(false);
        btnBackToHome = GameObject.Find("btn_home");
        btnBackToHome.SetActive(true);
    }

    private void handleLevelComplete()
    {
        foreach(int level in levels)
        {
            GameObject levelComp = GameObject.Find(level.ToString());
            if (levelComp)
            {
                levelComp.GetComponent<Button>().interactable = true;
                levelComp.transform.Find("lock")?.gameObject.SetActive(false);
            }
        }
    }

    public void win()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        if (!levels.Contains(currentLevel))
        {
            levels.Add(currentLevel);
        }
    }

    public void onClickNext()
    {
        playSound(click);
        transform.GetChild(0).gameObject.SetActive(false);
        handleLevel(currentLevel);
    }

    public void onClickBack()
    {
        playSound(click);
        GameObject popup = transform.GetChild(0).gameObject;
        GameObject contentLevel = transform.GetChild(1).gameObject;
        popup.gameObject.SetActive(false);
        contentLevel.gameObject.SetActive(true);
        if (GameObject.Find("Level_" + currentLevel.ToString() + "(Clone)"))
        {
            Destroy(GameObject.Find("Level_" + currentLevel.ToString() + "(Clone)"));
        }
        btnBack.SetActive(false);
        btnReset.SetActive(false);
        btnBackToHome.SetActive(true);
        handleLevelComplete();
    }

    public void onClickBackToHome()
    {
        playSound(click);
        SceneManager.LoadScene("Home");
    }

    public void onClickLevel()
    {
        playSound(click);
        int selected = int.Parse(EventSystem.current.currentSelectedGameObject.name);
        currentLevel = selected;
        GameObject.Find("ContentLevel").SetActive(false);
        handleLevel(selected);
    }

    public void onClickResetLevel()
    {
        handleLevel(currentLevel);
    }

    public void playSound(AudioClip clip)
    {
        audioSourceSound.clip = clip;
        if (btnSound.isOn)
        {
            audioSourceSound.PlayOneShot(clip);
        }
    }

    public void playMusic(AudioClip clip, int volume)
    {
        audioSource.clip = clip;
        if (btnMusic.isOn)
        {
            audioSource.Play();
            audioSource.volume = volume;
        } else
        {
            audioSource.Stop();
        }
    }

    public void onClickSound()
    {
        if (GameObject.Find("Worm") && GameObject.Find("Worm").transform.GetChild(0))
        {
            GameObject.Find("Worm").transform.GetChild(0).GetComponent<Head>().isSound = btnSound.isOn;
        }
        playSound(click);
    }

    public void onClickMusic()
    {
        playMusic(bg, 1);
    }

    public void handleLevel(int levelSelected)
    {
        btnBack.SetActive(true);
        btnReset.SetActive(true);
        btnBackToHome.SetActive(false);
        if (GameObject.Find("Level_" + levelSelected.ToString() + "(Clone)"))
        {
            Destroy(GameObject.Find("Level_" + levelSelected.ToString() + "(Clone)"));
        }
        GameObject level = Instantiate(Resources.Load("Level_" + levelSelected.ToString())) as GameObject;
        if (level)
        {
            level.transform.parent = transform;
            level.transform.GetChild(0).transform.GetChild(0).GetComponent<Head>().isSound = btnSound.isOn;
        }
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
