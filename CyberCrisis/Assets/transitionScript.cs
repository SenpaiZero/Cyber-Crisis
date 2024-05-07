using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class transitionScript : MonoBehaviour
{
    public Animation animOpen;
    public Animation animClose;
    [SerializeField] bool playStart = true;

    private void Start() {
        if(playStart)
            opening();
    }

    public void opening() {
        animOpen.Play();
        Debug.Log("transition opening");
    }

    public void closing() {
        animClose.Play();
        Debug.Log("transition closing");
    }

    public void closing(String sceneName) {
        StartCoroutine(close(sceneName));
    }

    IEnumerator close(String sceneName) {
        closing();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneName);
    }
}
