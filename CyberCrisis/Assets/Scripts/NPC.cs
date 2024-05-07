using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI npcNameText;
    public string npcName;
    public string[] dialogue;
    private int index;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;
    public bool isNextLevelNpc;

    private QuestManager questManager;
    sfxManager sfx;

    private void Start() {
        questManager = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();
        sfx = GameObject.FindGameObjectWithTag("sfx").GetComponent<sfxManager>();

    }
    private void Update()
    {
        if(gameObject.CompareTag("Fragments") || gameObject.CompareTag("trash")) return;

        if((Input.GetMouseButtonDown(0) || Input.touchCount > 1) && playerIsClose)
        {
            sfx.PlayMenuClick();
            Debug.Log("open dialog");
            openDialouge(true);
        }


        //For Continue Button
        if(dialogueText != null && dialogueText.text == dialogue[index])
        {
            if(!contButton.activeInHierarchy)
                contButton.SetActive(true);
        }
    }

    public void openDialouge(bool isNpc) {
        if(gameObject.CompareTag("Fragments")) return;

        if (dialoguePanel.activeInHierarchy)
        {
            NextLine(isNpc);
        } 
        else
        {
            dialoguePanel.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(Typing());
        }
    }

    public void zeroText() // close
    {
        bool isObj = true;

        if(gameObject.CompareTag("Fragments")) return;
        
        if(questManager.level == QuestManager.Level.LEVEL3 && gameObject.CompareTag("lvl3server")) 
        {
            if(isTouch)
            {
                sfx.PlayPickup();
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.GetComponent<NPC>().enabled = false;
                questManager.increaseComputerCount();
                isTouch = false;
                isObj = false;
            }
        }
        
        this.enabled = false;
        if(gameObject.CompareTag("server"))
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            questManager.increaseServerCount();
        }
        else if(isObj)
        {
            questManager.increaseObjCount();
        }


        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);

        if(isNextLevelNpc) {
            if(questManager.level == QuestManager.Level.LEVEL1)
                GameObject.FindGameObjectWithTag("transition").GetComponent<transitionScript>().closing("Level3and4");
            else if(questManager.level == QuestManager.Level.LEVEL2)
                GameObject.FindGameObjectWithTag("transition").GetComponent<transitionScript>().closing("FinalBoss");
        }
            
        if(questManager != null) {
            questManager = null;
        }
    }
    void closeText() // close
    {
        if(gameObject.CompareTag("Fragments")) return;
        
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }
    IEnumerator Typing()
    {
        sfx.PlayTalk();
        npcNameText.text = dialogue[index].Contains("[PLAYER]") ? "YOU" : npcName;
        string s = dialogue[index].Replace("[PLAYER]", "");
        foreach(char letter in s.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }


    public void NextLine(bool isNpc)
    {
        contButton.SetActive(false);

        if(index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StopAllCoroutines();
            StartCoroutine(Typing());
        }else
        {
            if(isNpc)
                zeroText();
            else
                closeText();
        }
    }

    bool isTouch= true;
    //Close Range Interaction
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;

            if(questManager.level == QuestManager.Level.LEVEL2 && gameObject.CompareTag("Fragments"))
            {
                if(isTouch)
                {
                    sfx.PlayPickup();
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<NPC>().enabled = false;
                    questManager.increaseFragmentCount();
                    isTouch = false;
                }
            }
        }

        if(questManager.level == QuestManager.Level.LEVEL1 && gameObject.CompareTag("trash")) 
        {
            if(isTouch)
                {
                    sfx.PlayPickup();
                    gameObject.GetComponent<SpriteRenderer>().enabled = false;
                    gameObject.GetComponent<NPC>().enabled = false;
                    questManager.increaseTrashCount();
                    isTouch = false;
                }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            closeText();
        }
    }
}
