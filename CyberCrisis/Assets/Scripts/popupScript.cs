using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class popupScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI desc;

    public void setData(String title_txt, String desc_txt) {
        title.text = title_txt;
        desc.text = desc_txt;
    }
    public void retry() {
        GameObject.FindGameObjectWithTag("transition").GetComponent<transitionScript>().closing(SceneManager.GetActiveScene().name);
    }

    public void quit() {
        GameObject.FindGameObjectWithTag("transition").GetComponent<transitionScript>().closing("Menu");
    }

}
