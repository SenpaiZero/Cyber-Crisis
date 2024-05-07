using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject popup;
    QuestManager questManager;

    bool delay = false;
    
    private void Start() {
        questManager = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();
        StartCoroutine(addDelay());
    }

    private void Update()
    {
        if (player != null && delay)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize();
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        if(questManager.getCompCount() >= 4) {
            this.enabled = false;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            popupScript pop = popup.GetComponent<popupScript>();
            pop.setData("GAME OVER", "THE GLITCH HAS CORRUPTED YOU");
            popup.SetActive(true);
        }
    }

    IEnumerator addDelay() {
        yield return new WaitForSeconds(1.7f);
        delay = true;
    }
}
