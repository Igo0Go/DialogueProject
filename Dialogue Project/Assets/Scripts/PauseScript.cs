using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject pausePanel;
    public AsyncOperation loader;
    public List<ConditionPack> conditions;

    public bool Pause
    {
        get
        {
            return _pause;
        }
        set
        {
            _pause = value;
            if(_pause)
            {
                MyTime.TimeScale = 0;
            }
            else
            {
                MyTime.TimeScale = 1;
            }
            pausePanel.SetActive(_pause);
        }
    }

    private bool _pause;


    void Start()
    {
        Pause = false;
        loader = SceneManager.LoadSceneAsync("CityScene");
        loader.allowSceneActivation = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetPause();
        }
    }

    public void SetPause()
    {
        Pause = !Pause;
    }

    public void RestartScene()
    {
        ClearAllSettings();
        loader.allowSceneActivation = true;
    }

    public void ExitClick()
    {
        ClearAllSettings();
        Application.Quit();
    }

    private void ClearAllSettings()
    {
        conditions[0].SetBool(0, false);
        conditions[1].SetBool(0, false);
        conditions[1].SetBool(1, false);
        conditions[1].SetBool(2, false);
        conditions[1].SetInt(3, 0);
        conditions[1].SetBool(4, false);
        conditions[2].SetInt(0, 0);
    }
}
