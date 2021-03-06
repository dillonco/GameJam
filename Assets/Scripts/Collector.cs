﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collector : MonoBehaviour
{
    public int points = 0;
    public GameObject memorySpawner;
    public Vector2 size;
    public GameObject door;
    public Color colour;

    private float width;
    private float height;

    public float scale = 0;
    public float shrinkSpeed = 2.0f;
    public float shrinkAmount = 0.4f;
    public bool endCondition = false;

    /* which xvalue of the collector's scale will trigger a game over */
    public int gameOverValue = 1;
    public int timesShrunk = 0;

    private int doorCount = 0;
    private int dialogueCount = 0;
    void Start() {
        memorySpawner = GameObject.Find("EmotionSpawner");
        size = gameObject.GetComponent<SpriteRenderer>().size;
        colour = gameObject.GetComponent<SpriteRenderer>().color;
        width = size.x;
        height = size.y;

        /* shrinks at shrinkspeed starting in 5 seconds*/
        InvokeRepeating("ShrinkCollector", 15.0f,  shrinkSpeed);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {   
        if(coll.gameObject.tag == "emotion") {
            if(memorySpawner.GetComponent<MemorySpawner>().activeMemories.Count > 0) {
                if(coll.gameObject.GetComponent<Memory>().type == memorySpawner.GetComponent<MemorySpawner>().activeMemories.Peek().GetComponent<Memory>().type) {
                    Destroy(memorySpawner.GetComponent<MemorySpawner>().activeMemories.Dequeue());
                    foreach(GameObject memory in memorySpawner.GetComponent<MemorySpawner>().activeMemories)
                    {
                        memory.transform.position = new Vector3(memory.transform.position.x -1, memory.transform.position.y, 0);
                    }
                    StartCoroutine(changeColor(Color.green));
                    points++;
                    gameObject.transform.localScale += new Vector3(width + 1, height + 1, 0);
                    Destroy(coll.gameObject);
                } else
                {
                    StartCoroutine(changeColor(Color.red));
                }
            }
            if(coll.gameObject) {
                Destroy(coll.gameObject);
            }
        }
    }

    /* shrinks the collector */
    void ShrinkCollector() {
        gameObject.transform.localScale += new Vector3(width - shrinkAmount, height - shrinkAmount, 0);
        timesShrunk++;
    }

    void Update()
    {
        scale = gameObject.transform.localScale.x;
        /* GAME OVER CONDITION */
        if (gameObject.transform.localScale.x < gameOverValue) {
            SceneManager.LoadScene("GameOver");
        }

        /* Win condition */
        if(gameObject.transform.localScale.x > 36.0f && doorCount < 1)
        {
            doorCount++;
            GameObject a = (GameObject)Instantiate(door, new Vector3(-16.999f, -3.26f), Quaternion.identity);
        }

        if (gameObject.transform.localScale.x > 9.0f && dialogueCount < 1)
        {
            dialogueCount++;
            GameObject.Find("DialogueTrigger2").GetComponent<DialogueTrigger2>().TriggerDialogue2();
        }

        if (gameObject.transform.localScale.x > 11.0f && dialogueCount < 2)
        {
            dialogueCount++;
            GameObject.Find("DialogueTrigger3").GetComponent<DialogueTrigger2>().TriggerDialogue2();
        }

        if (gameObject.transform.localScale.x > 17.0f && dialogueCount < 3)
        {
            dialogueCount++;
            GameObject.Find("DialogueTrigger4").GetComponent<DialogueTrigger2>().TriggerDialogue2();
        }

        if (gameObject.transform.localScale.x > 21.0f && dialogueCount < 4)
        {
            dialogueCount++;
            GameObject.Find("DialogueTrigger5").GetComponent<DialogueTrigger2>().TriggerDialogue2();
        }

    }

    IEnumerator changeColor(Color colour)
    {
        gameObject.GetComponent<SpriteRenderer>().color = colour;
        yield return new WaitForSeconds(.5f);
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }


}
