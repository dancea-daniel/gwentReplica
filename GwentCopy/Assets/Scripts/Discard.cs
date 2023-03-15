using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Discard : MonoBehaviour
{

    SceneController controller;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

    public void ChangeCard()
    {
        if(controller.isMedicActive==false)
        {
            Debug.Log(this.name);
            GameObject card = controller.GetCardWithId(int.Parse(name), controller.GetPlayer().handList);
            Debug.Log(controller.GetPlayer().handList.Count);
            controller.GetPlayer().handList.Remove(card);
            card.SetActive(false);
            card.transform.SetParent(controller.transform);
            Debug.Log(controller.GetPlayer().handList.Count);

            AddNewCard();


            controller.GetPlayer().deckList.Add(card);
            Debug.Log(controller.GetPlayer().handList.Count);
            controller.SortCards(controller.GetPlayer().handList);
            controller.loadPanelCount++;
            Debug.Log("szam " + controller.loadPanelCount);
            if (controller.loadPanelCount == 2)
            {
                Debug.Log(transform.parent.transform.childCount);
                for (int i = 0; i < transform.parent.transform.childCount; i++)
                {
                   Destroy(transform.parent.transform.GetChild(i).gameObject);
                }
                controller.TurnOffLoadPanel(); 
            }

        }else
        {
            GameObject card = controller.GetCardWithId(int.Parse(name), controller.GetPlayer().discardList);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            controller.HighlightRows(card);
            card.SetActive(true);
            controller.GetPlayer().discardList.Remove(card);
            controller.cardSelected = card;

            controller.art.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
            controller.art.SetActive(true);

            for (int i = 0; i < transform.parent.transform.childCount; i++)
            {
                Destroy(transform.parent.transform.GetChild(i).gameObject);
            }
            controller.TurnOffLoadPanel();

            Debug.Log("a discardnal a medic" + controller.isMedicActive);

        }

    }
    
    //itt lehet meg maskeppen kell majd, jelenleg atirja az aktualist
    //update nekem jo igy is
    void AddNewCard()
    {
        int rnd = Random.Range(0, controller.GetPlayer().deckList.Count);
        GameObject card = controller.GetPlayer().deckList[rnd];
        card.SetActive(true);
        card.transform.SetParent(controller.GetPlayer().myHand.transform);
        card.transform.localScale = new Vector3(2f, 2f, 2f);
        controller.GetPlayer().handList.Add(card);
        controller.GetPlayer().deckList.Remove(card);

        this.name = card.GetComponentInChildren<CardStats>()._id.ToString();
        this.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + int.Parse(name));
    }

    // Update is called once per frame
    void Update()
    {
        if(!controller.isMedicActive)
        {
            if (Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < transform.parent.transform.childCount; i++)
                {
                    Destroy(transform.parent.transform.GetChild(i).gameObject);
                }
                controller.loadPanelCount=2;
                controller.TurnOffLoadPanel();
            }
        }
    }
}
