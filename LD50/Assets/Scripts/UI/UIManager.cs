using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class UIManager : MonoBehaviour
{
    public GameObject uiPause;
    public GameObject uiGameplay;
    public GameObject uiDialogo;

    private TaskCompletionSource<bool> selectedPositionSource;

    // Start is called before the first frame update
    void Awake()
    {
        GameManager.SetUIManager(this);
        uiPause.SetActive(false);
        uiGameplay.SetActive(false);
        uiDialogo.SetActive(true);
    }

    public void Paused()
    {
        Time.timeScale = 0;
        uiGameplay.SetActive(false);
        uiPause.SetActive(true);
        uiDialogo.SetActive(false);
    }

    public void QuitPause()
    {
        Time.timeScale = 1;
        uiPause.SetActive(false);
        uiGameplay.SetActive(true);
        uiDialogo.SetActive(false);
    }


    public async void ShowDialogue(string text)
    {
        uiDialogo.GetComponent<DialogManager>().SetText(text);
        DialaguePause();
        //selectedPositionSource = new TaskCompletionSource<bool>();
        //await selectedPositionSource.Task;
    }

    public void DialaguePause()
    {
        uiGameplay.SetActive(false);
        uiPause.SetActive(false);
        uiDialogo.SetActive(true);
        Time.timeScale = 0;
    }

    public void QuitDialaguePause()
    {
        uiGameplay.SetActive(true);
        uiPause.SetActive(false);
        uiDialogo.SetActive(false);
        Time.timeScale = 1;
        //selectedPositionSource.TrySetResult(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
