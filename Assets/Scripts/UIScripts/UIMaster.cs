using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIMaster : MonoBehaviour
{
    public List<Objective> descriptions;
    public ScrollableList scrolist;
    public GameObject nextLevelButton;

    private string finalBossDefeatedText;

    void Start()
    {
        descriptions = new List<Objective>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            descriptions.Add(new Objective("Kill Him", "He Needs To Die", "kill", enemy.GetComponent<Enemy>(), new Vector3(0, 0, 0)));
        }
        finalBossDefeatedText = "YOU WIN!";
    }

    public void loadNextLevel()
    {
        if (Application.loadedLevel != Application.levelCount - 1)
        {
            Application.LoadLevel(Application.loadedLevel + 1);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            scrolist.doTheGUI();
        }

        if (Input.GetKeyUp("tab"))
        {
            ArrayList children = new ArrayList();
            foreach (Transform child in transform)
            {
                if (!child.name.Contains("Slider") && !child.name.Contains("Level"))
                {
                    children.Add(child.gameObject);
                }
            }
            foreach (GameObject child in children)
            {
                Destroy(child);
            }
        }

        if (!nextLevelButton.activeSelf)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss)
            {
                if (boss.GetComponent<Enemy>().state == EnemyState.DEAD)
                {
                    if (Application.loadedLevel == Application.levelCount - 1)
                    {
                        nextLevelButton.transform.Find("Text").GetComponent<Text>().text = finalBossDefeatedText;
                    }
                    nextLevelButton.SetActive(true);
                }
            }
        }
    }

}