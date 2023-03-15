using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextScene : MonoBehaviour
{
    GameObject in_deck;
    GameObject specialCards;

    int isReady = 0;
    string path;

    // Start is called before the first frame update
    void Start()
    {
        in_deck = GameObject.Find("in_deck");
        specialCards = GameObject.Find("SpecialCards");
    }

    public void CreateDeckText()
    {
        string[] x = new string[1];
        x = specialCards.GetComponent<TextMeshProUGUI>().text.Split('/');
        
        //tobbi feltetel is
        if(int.Parse(x[0])>10)
        {
            Debug.LogError("NONONONONONo");
        }
        else
        {
            if (isReady==0)
            {
                path = Application.dataPath + "/Resources/deckToLoad.txt";
            }else path = Application.dataPath + "/Resources/deckToLoadEnemy.txt";

            if (!File.Exists(path))
            {
                File.WriteAllText(path, "");
            }

            List<string> deckNr = new List<string>();
            Transform t = in_deck.transform.Find("Content").transform;
            //vissza kell allitani az osszes kartyat az indecknel
            for (int i = 0; i < t.childCount; i++)
            {
                if (t.GetChild(i).gameObject.activeSelf==false)
                {
                    t.GetChild(i).gameObject.SetActive(true);
                }
                for (int j = 0; j <= t.GetChild(i).GetComponent<AddOrRemove>().nr; j++)
                {
                    deckNr.Add(t.GetChild(i).name);
                }
            }

            deckNr.Add(GameObject.Find("LeaderCardHolder").GetComponent<Image>().sprite.name);

            for (int i = 0; i < deckNr.Count; i++)
            {
                Debug.Log(deckNr[i]);
            }

            File.WriteAllLines(path, deckNr);
            isReady++;

            if (isReady==2)
            {
               SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else if (isReady==1)
            {
                foreach (Transform child in in_deck.transform.Find("Content").transform)
                {
                    Destroy(child.gameObject);
                }
                in_deck.transform.Find("Content").DetachChildren();

                Transform t1 = GameObject.Find("out_deck").transform;

                foreach (Transform child in t1.Find("Content").transform)
                {
                    Destroy(child.gameObject);
                }
                t1.Find("Content").DetachChildren();

                //listak reset
                t1.parent.Find("Holder").GetComponent<SortCards>().cardsActive.Clear();
                in_deck.transform.parent.Find("Holder").GetComponent<SortCards>().cardsActive.Clear();

                //listak ujratoltese
                //itt kell majd egy fix ha tobb deck van mert akkor ujra kell tolteni az egeszet
                //elvileg mukodik
                t1.GetComponentInChildren<LoadAllCards>().SortCards(t1.GetComponentInChildren<LoadAllCards>().list);
                t1.parent.Find("Holder").GetComponent<SortCards>().Start();

                in_deck.transform.Find("Content").localPosition = new Vector3(in_deck.transform.Find("Content").localPosition.x, 0, in_deck.transform.Find("Content").localPosition.z);
                t1.Find("Content").localPosition = new Vector3(t1.Find("Content").localPosition.x, 0, t1.Find("Content").localPosition.z);
                //!!ujra kell resetelni a kicsi UIt!!
            }
            UnityEditor.AssetDatabase.Refresh();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
