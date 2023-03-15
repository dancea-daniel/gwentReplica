using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HornClick : MonoBehaviour, IPointerClickHandler
{
    SceneController controller;
    public SceneController.Slot typeOfCard;

    

    public Transform parentToReturnTo = null;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

   
    void addToRow(GameObject card)
    {
        switch (transform.parent.name)
        {
            case "Close":
                {
                    controller.GetPlayer().closeList.Add(card);
                }
                break;

            case "Range":
                {
                    controller.GetPlayer().rangeList.Add(card);

                }
                break;

            case "Siege":
                {
                    controller.GetPlayer().siegeList.Add(card);

                }
                break;

        }
    }

    void removeFromHand(GameObject card)
    {
        controller.GetPlayer().handList.Remove(card);
    }

    void PlaceCard()
    {
        GameObject card = controller.cardSelected;
        parentToReturnTo = this.transform;
        card.transform.SetParent(parentToReturnTo);
        card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        controller.TurnOffHighlight(controller.cardSelected);


        controller.art.SetActive(false);

        transform.parent.GetChild(0).GetComponent<RowClick>().isHornActive = true;

        if (controller.GetPlayer().handList.Count == 0)
        {
            controller.Pass();
        }
        else
        {
            controller.ChangePlayer();
            controller.AllCardsRaycast();
            controller.ChangeUI();
        }


        controller.GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
    }

    void ActivateHorn(GameObject card)
    {
        switch(transform.parent.name)
        {
            case "Close":
                {
                    controller.DoubleRow(controller.GetPlayer().closeList);
                    controller.UpdateRowStrenghtImage(controller.GetPlayer().closeList);
                }
            break;

            case "Range":
                {
                    controller.DoubleRow(controller.GetPlayer().rangeList);
                    controller.UpdateRowStrenghtImage(controller.GetPlayer().rangeList);
                }
                break;

            case "Siege":
                {
                    controller.DoubleRow(controller.GetPlayer().siegeList);
                    controller.UpdateRowStrenghtImage(controller.GetPlayer().siegeList);
                }
                break;

        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.isCardSelected == true)
        {
            CardTranslate d = controller.cardSelected.GetComponent<CardTranslate>();
            GameObject card = controller.cardSelected;

            switch(controller.battleState.ToString())
            {
                case "PlayerTurn":
                    {
                        if (d != null)
                        {
                            if (this.transform.parent.parent.name == "PlayerField" && transform.childCount==0)
                            {
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    ActivateHorn(card);
                                    PlaceCard();
                                }
                            }
                        }

                    }break;

                case "EnemyTurn":
                    {
                        if (d != null)
                        {
                            if (this.transform.parent.parent.name == "EnemyField" && transform.childCount == 0)
                            {
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    ActivateHorn(card);
                                    PlaceCard();
                                }
                            }
                        }
                    }break;
            }



        }
    }
}
