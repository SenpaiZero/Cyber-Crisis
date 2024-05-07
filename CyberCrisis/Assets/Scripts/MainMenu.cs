using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    sfxManager sfx;
    private void Start() {
        sfx = GameObject.FindGameObjectWithTag("sfx").GetComponent<sfxManager>();
    }
    public void PlayGame()
    {
        sfx.PlayMenuClick();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;

        GameObject.FindGameObjectWithTag("transition").GetComponent<transitionScript>().closing("Level1and2");
    }

    public void QuitGame()
    {
        sfx.PlayMenuClick();
        Debug.Log("Quit!");
        Application.Quit();
    }
}
