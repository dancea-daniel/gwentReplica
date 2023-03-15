using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelList : MonoBehaviour
{
    SceneController controller;
    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Awake()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
        //AddCardsToContent();
    }

    public void AddCardsToContent()
    {
        foreach (GameObject card in controller.GetPlayer().handList)
        {
            CreateButton(card);
        }
    }


    public void ShowDiscard()
    {
        foreach (GameObject card in controller.GetPlayer().discardList)
        {
            CreateButton(card);
        }
    }

    public void Check()
    {
        //meg egy discard es megnezni hogy melyik discard hova tartozik

        foreach (GameObject card in controller.GetPlayer().discardList)
        {
            GameObject goButton = Instantiate(buttonPrefab) as GameObject;
            goButton.GetComponent<Button>().enabled = false;
            goButton.name = card.GetComponentInChildren<CardStats>()._id.ToString();
            goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
            goButton.transform.SetParent(this.transform);
            controller.loadPanel.SetActive(true);
        }
    }


    public void EmhyrAbility(List<GameObject> list)
    {
        if (this.gameObject.activeInHierarchy == false)
        {

            foreach (GameObject card in list)
            {
                GameObject goButton = Instantiate(buttonPrefab) as GameObject;
                goButton.GetComponent<Button>().enabled = false;
                goButton.name = card.GetComponentInChildren<CardStats>()._id.ToString();
                goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
                goButton.transform.SetParent(this.transform);
                controller.loadPanel.SetActive(true);
            }

        }
    }

    public void ViewDeck()
    {
        if(this.gameObject.activeInHierarchy==false)
        {

            foreach (GameObject card in controller.GetPlayer().deckList)
            {
                GameObject goButton = Instantiate(buttonPrefab) as GameObject;
                goButton.GetComponent<Button>().enabled = false;
                goButton.name = card.GetComponentInChildren<CardStats>()._id.ToString();
                goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
                goButton.transform.SetParent(this.transform);
                controller.loadPanel.SetActive(true);
            }

        }

    }

    void CreateButton(GameObject card)
    {
        if(controller.isMedicActive == true)
        {
            if(card.GetComponentInChildren<CardStats>().unique == true || card.GetComponentInChildren<CardStats>().faction.ToString()=="Special")
            {
                Debug.Log("nem teszem be ezt a kartyat " + card.name);
            }else
            {
                GameObject goButton = Instantiate(buttonPrefab) as GameObject;
                goButton.name = card.GetComponentInChildren<CardStats>()._id.ToString();
                goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
                goButton.transform.SetParent(this.transform);
                controller.loadPanel.SetActive(true);
            }
        }else
        {
            GameObject goButton = Instantiate(buttonPrefab) as GameObject;
            goButton.name = card.GetComponentInChildren<CardStats>()._id.ToString();
            goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
            goButton.transform.SetParent(this.transform);
            controller.loadPanel.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
}
