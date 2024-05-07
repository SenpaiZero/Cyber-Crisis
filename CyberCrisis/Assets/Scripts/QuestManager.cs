using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public enum Level {
        LEVEL1, LEVEL2, LEVEL3
    }
    [SerializeField] GameObject[] nextLevel;
    [Header("Waypoint")]
    [SerializeField] GameObject[] target;
    [SerializeField] GameObject arrow;
    public Level level;


    [Header("Level 1")]
    [SerializeField] private GameObject[] npcs;
    [SerializeField] private GameObject[] trash;


    [Header("Level 2")]
    [SerializeField] private GameObject[] servers;
    [SerializeField] private GameObject[] fragments;
    [SerializeField] private GameObject[] npcLvl2;


    
    [Header("Level 3")]
    [SerializeField] private GameObject[] computers;
    [SerializeField] private GameObject[] grandpa;
    [SerializeField] private GameObject popup;

    TextMeshProUGUI taskTxt;

    String task;
    String origTask;
    int objCount;
    int fragmentCount;
    int serverCount;
    int trashCount;
    int computerCount;
    int targInd;

    public int getCompCount() {return computerCount;}

    String color1 = "<color=\"red\">",
            color2 = "<color=\"red\">",
            color3 = "<color=\"red\">";

    int lvl1StartClicks = 0;
    int lvl1AfterClicks = 0; 
    bool isLvl1After = false, firstTimeAfter = true;
    void Start()
    {
        taskTxt = GameObject.FindGameObjectWithTag("taskTxt").GetComponent<TextMeshProUGUI>();
        targInd = 0;
        objCount = 0;
        serverCount = 0;
        fragmentCount = 0;
        trashCount = 0;
        computerCount = 0;
        origTask = taskTxt.text;

        updateTaskText();
        updateWaypoint();

        StartCoroutine(waitStart());
    }
    IEnumerator waitStart() {
        yield return new WaitForSeconds(1f);
        if(level == Level.LEVEL1)
            GameObject.FindGameObjectWithTag("lvl1StartDialogue").GetComponent<NPC>().openDialouge(false);
    }
    IEnumerator waitLast() {
        firstTimeAfter = false;
        yield return new WaitForSeconds(0.5f);
        GameObject.FindGameObjectWithTag("lvl1AfterTask").GetComponent<NPC>().openDialouge(false);
    }
    void Update()
    {
        moveWaypoint(); 
        
        if(level == Level.LEVEL1)
        {
            if(Input.GetMouseButtonDown(0) || Input.touchCount > 1)
            {
                if(lvl1StartClicks < GameObject.FindGameObjectWithTag("lvl1StartDialogue").GetComponent<NPC>().dialogue.Length) {
                    GameObject.FindGameObjectWithTag("lvl1StartDialogue").GetComponent<NPC>().openDialouge(false);
                    lvl1StartClicks++;
                }
                
                if(isLvl1After) {
                    if(lvl1AfterClicks < GameObject.FindGameObjectWithTag("lvl1AfterTask").GetComponent<NPC>().dialogue.Length) {
                        GameObject.FindGameObjectWithTag("lvl1AfterTask").GetComponent<NPC>().openDialouge(false);
                        lvl1AfterClicks++;
                    }
                }
            }

            if(isLvl1After)
            {
                if(firstTimeAfter) {
                    StartCoroutine(waitLast());
                }
            }
        }
            
    }
    public void increaseObjCount() {
        objCount += 1;
        updateWaypoint();
        updateTaskText();
    }
    public void increaseFragmentCount() {
        
        fragmentCount += 1;

        if(fragmentCount == fragments.Length) {
            color2 = "<color=\"green\">";

            GameObject.FindGameObjectWithTag("server").GetComponent<NPC>().enabled = true;
        }
        updateWaypoint();
        updateTaskText();
    }

    public void increaseComputerCount() {
        computerCount += 1;
        if(computerCount == computers.Length) {
                color2 = "<color=\"green\">";
                GameObject.FindGameObjectWithTag("grandpa").GetComponent<NPC>().enabled = true;
        }
        updateWaypoint();
        updateTaskText();
    }
    public void increaseTrashCount() {
        trashCount += 1;

        if(trashCount == trash.Length) {
            color2 = "<color=\"green\">";

            for(int i = 0; i < npcs.Length; i++) {
                npcs[i].GetComponent<NPC>().enabled = true;
            }
        }
        updateWaypoint();
        updateTaskText();
    }
    public void increaseServerCount() {
        serverCount += 1;
        if(serverCount == npcLvl2.Length) {
            color3 = "<color=\"green\">";
            GameObject.FindGameObjectWithTag("lvl2NPC").GetComponent<NPC>().enabled = true;
        }
        updateWaypoint();
        updateTaskText();
    }

    void moveWaypoint() {
        Debug.Log(target.Length + "  :: index:" + targInd);
        Vector2 direction = (target[targInd].transform.position - arrow.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    void updateWaypoint() {
        if(level == Level.LEVEL1) {
            if(trashCount != trash.Length) {
                target = trash;
            }
            else {
                if(objCount != npcs.Length)
                    target = npcs;
                else {
                    target = nextLevel;
                    isLvl1After = true;
                }
            }
        }

        if(level == Level.LEVEL2) {
            if(fragmentCount < fragments.Length) {
                target = fragments;
            }
            else {
                if(serverCount != servers.Length)
                {
                    target = servers;
                }
                else
                {
                    if(objCount != npcLvl2.Length)
                        target = npcLvl2;
                    else
                        target = nextLevel;
                }
            }
        }

        if(level == Level.LEVEL3) {
            if(computerCount < computers.Length) {
                target = computers;
            }
            else {
                if(objCount != grandpa.Length) {
                    target = grandpa;
                }
                else {
                    popupScript pop = popup.GetComponent<popupScript>();
                    pop.setData("CONGRATULATIONS!", "YOU SUCCESSFULLY SAVED YOUR GRANDFATHER");
                    popup.SetActive(true);
                }
            }
        }


        // Change target
        for(int i = 0; i < target.Length; i++) {
            if(target[i].GetComponent<NPC>().enabled) {
                targInd = i;
                break;
            }
        }

        Debug.Log("Updating waypoint..    Index: " + targInd);
    }
    void updateTaskText() {
        
        if(isTaskFinished()) {
            color1 = "<color=\"green\">";
            nextLevel[0].GetComponent<NPC>().enabled = true;
            targInd = 0;
            Debug.Log("Task finished");
        }

        if(level == Level.LEVEL1) {
            task = origTask + "\n\n<size=200%><b>Goal</b></size>\n" + color2 + trashCount + " / " + trash.Length + " Trash Found\n" +
             color1 + objCount + " / " + npcs.Length + " NPC's Talked";
        }
        else if(level == Level.LEVEL2) {
            if(fragmentCount == fragments.Length) color2 = "<color=\"green\">";
            task = origTask + "\n\n<size=200%><b>Goals</b></size>\n" + color2 + fragmentCount + " / " + fragments.Length + " Fragments Found\n"
                            + color3 + serverCount + " / " + servers.Length + " Server Fixed\n"
                            + color1 + objCount + " / " + npcLvl2.Length + " NPC Talked";
        }
        else if(level == Level.LEVEL3) {
            if(computerCount == computers.Length) color2 = "<color=\"green\">";
            task = origTask + "\n\n<size=200%><b>Goals</b></size>\n" + color2 + computerCount + " / " + computers.Length + " Servers Fixed\n"
                            + color1 + objCount + " / " + grandpa.Length + " Save Grandpa";
        }
        taskTxt.text = task;
    }

    bool isTaskFinished() {
        GameObject[] arr;
        if(level == Level.LEVEL1) arr = npcs;
        else if(level == Level.LEVEL2) arr = servers; 
        else if(level == Level.LEVEL3) arr = grandpa;
        else arr = new GameObject[0];

        if(arr.Length == objCount) return true;

        return false;
    }
}
