using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SortCards : MonoBehaviour
{

    public List<GameObject> cardsActive = new List<GameObject>();
    public List<GameObject> cardsHidden = new List<GameObject>();

    GameObject content;
    GameObject holder;

    public void Start()
    {
        cardsActive.Clear();
        cardsHidden.Clear();

        if (transform.parent.name=="LeftDeck")
        {
            content = GameObject.Find("out_deck").transform.GetChild(0).gameObject;
            holder = GameObject.Find("LeftDeck").transform.Find("Holder").gameObject;
        }
        else
        {
            content = GameObject.Find("in_deck").transform.GetChild(0).gameObject;
            holder = GameObject.Find("RightDeck").transform.Find("Holder").gameObject;
        }

        Transform t = holder.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            int a = i;
            t.GetChild(i).GetComponent<Button>().onClick.AddListener(delegate { SelectCards(t.GetChild(a));});
        }

        for (int i = 0; i < content.transform.childCount; i++)
        {
            cardsActive.Add(content.transform.GetChild(i).gameObject);
        }

    }


    void UI_Change()
    {
        Transform t = holder.transform;

        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/idle/" + i);
        }


    }

    void Order(string s, string parentName)
    {
        List<GameObject> list = new List<GameObject>();

        foreach (GameObject c in cardsActive)
        {
            if (!c.GetComponent<AddOrRemove>().stats.row.Contains(s) && c.transform.parent.parent.parent.name == parentName)
            {
                c.SetActive(false);
                cardsHidden.Add(c);
            }
        }

        foreach (GameObject c in cardsHidden)
        {
            if (c.transform.parent.parent.parent.name == parentName)
            {
                if (c.GetComponent<AddOrRemove>().stats.row.Contains(s))
                {
                    cardsActive.Add(c);
                    c.SetActive(true);
                    list.Add(c);
                }
                else
                {
                    cardsActive.Remove(c);
                    c.SetActive(false);

                }
            }


        }

        foreach (GameObject c in list)
        {
            cardsHidden.Remove(c);
        }

        UI_Change();
    }

    public void SelectCards(Transform go)
    {

        if (go.name.Contains("All"))
        {
            foreach (GameObject c in cardsHidden)
            {

                if (c.activeSelf == false && c.transform.parent.parent.parent.name == this.transform.parent.name)
                {
                    c.SetActive(true);
                    cardsActive.Add(c);
                }
            }

            cardsHidden.Clear();

            UI_Change();

            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/all");

        }
        else if (go.name.Contains("Close"))
        {
            Order("close", this.transform.parent.name);

            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/close");

        }
        else if (go.name.Contains("Range"))
        {
            Order("range",this.transform.parent.name);

            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/range");
        }
        else if (go.name.Contains("Hero"))
        {
            List<GameObject> list = new List<GameObject>();

            foreach (GameObject c in cardsActive)
            {
                if (c.GetComponent<AddOrRemove>().stats.unique==false && c.transform.parent.parent.parent.name == this.transform.parent.name)
                {
                    c.SetActive(false);
                    cardsHidden.Add(c);
                }
            }

            foreach (GameObject c in cardsHidden)
            {
                if (c.transform.parent.parent.parent.name == this.transform.parent.name)
                {

                    if (c.GetComponent<AddOrRemove>().stats.unique == true)
                    {
                        cardsActive.Add(c);
                        c.SetActive(true);
                        list.Add(c);
                    }
                    else
                    {
                        cardsActive.Remove(c);
                        c.SetActive(false);

                    }
                }


            }

            foreach (GameObject c in list)
            {
                cardsHidden.Remove(c);
            }

            UI_Change();

            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/hero");
        }
        else if (go.name.Contains("Siege"))
        {
            Order("siege", this.transform.parent.name);
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/siege");
        }
        else if (go.name.Contains("Special"))
        {

            List<GameObject> list = new List<GameObject>();

            foreach (GameObject c in cardsActive)
            {
                if (!c.GetComponent<AddOrRemove>().stats.faction.Contains("Special") && c.transform.parent.parent.parent.name == this.transform.parent.name)
                {
                    c.SetActive(false);
                    cardsHidden.Add(c);
                }
            }

            foreach (GameObject c in cardsHidden)
            {
                if (c.transform.parent.parent.parent.name == this.transform.parent.name)
                {
                    if (c.GetComponent<AddOrRemove>().stats.faction.Contains("Special"))
                    {
                        cardsActive.Add(c);
                        c.SetActive(true);
                        list.Add(c);
                    }
                    else
                    {
                        cardsActive.Remove(c);
                        c.SetActive(false);

                    }
                }


            }

            foreach (GameObject c in list)
            {
                cardsHidden.Remove(c);
            }

            UI_Change();

            go.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/Menu/selected/special");
        }
    }
}
