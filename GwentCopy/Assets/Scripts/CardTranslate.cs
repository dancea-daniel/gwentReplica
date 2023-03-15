using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class CardTranslate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    SceneController controller;

    public SceneController.Slot typeOfCard;
    public Vector3 onePos = new Vector3();
    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {

            if (controller.swapActivated)
            {
                switch (controller.battleState.ToString())
                {
                    case "PlayerTurn":
                        {
                            if (!controller.GetPlayer().handList.Contains(this.gameObject))
                            {
                                if (this.transform.parent.parent.parent.name.ToString() == "PlayerField" && this.GetComponentInChildren<CardStats>()._id!=203)
                                {
                                    controller.GetPlayer().handList.Remove(controller.cardSelected);
                                    controller.GetCardListType(this.gameObject).Add(controller.cardSelected);
                                    controller.GetCardListType(this.gameObject).Remove(this.gameObject);
                                    controller.GetPlayer().handList.Add(this.gameObject);
                                    controller.cardSelected.transform.SetParent(this.transform.parent);
                                    this.transform.SetParent(controller.GetPlayer().myHand.transform);
                                    controller.SortCards(controller.GetCardListType(controller.cardSelected));
                                    this.GetComponentInChildren<CardStats>().actualStrength = this.GetComponentInChildren<CardStats>().strength;
                                    this.GetComponent<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().text = this.GetComponentInChildren<CardStats>().actualStrength.ToString();
                                    this.GetComponentInChildren<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);

                                }
                            }

                        }
                        break;

                    case "EnemyTurn":
                        {
                            if (!controller.GetPlayer().handList.Contains(this.gameObject))
                            {
                                if (this.transform.parent.parent.parent.name.ToString() == "EnemyField" && this.GetComponentInChildren<CardStats>()._id != 203)
                                {
                                    controller.GetPlayer().handList.Remove(controller.cardSelected);
                                    controller.GetCardListType(this.gameObject).Add(controller.cardSelected);
                                    controller.GetCardListType(this.gameObject).Remove(this.gameObject);
                                    controller.GetPlayer().handList.Add(this.gameObject);
                                    controller.cardSelected.transform.SetParent(this.transform.parent);
                                    this.transform.SetParent(controller.GetPlayer().myHand.transform);
                                    controller.SortCards(controller.GetCardListType(controller.cardSelected));
                                    this.GetComponentInChildren<CardStats>().actualStrength = this.GetComponentInChildren<CardStats>().strength;
                                    this.GetComponent<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().text = this.GetComponentInChildren<CardStats>().actualStrength.ToString();
                                    this.GetComponentInChildren<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);

                                }
                            }
                        }
                        break;
                }
                this.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
                controller.swapActivated = false;
                controller.cardSelected.GetComponent<CanvasGroup>().blocksRaycasts = false;
                controller.Decoy(controller.GetCardListType(controller.cardSelected));
                controller.SortCards(controller.GetPlayer().handList);
                controller.art.SetActive(false);
                if (controller.weatherList.Count > 0)
                {
                    foreach (int weatherCard in controller.weatherList)
                    {
                        //biting frost
                        if (weatherCard == 200)
                        {
                            controller.SetStrenghtToWeather(controller.PlayerInfo.closeList);
                            controller.SetStrenghtToWeather(controller.EnemyInfo.closeList);
                        }
                        else if (weatherCard == 204)
                        {
                            //range
                            controller.SetStrenghtToWeather(controller.PlayerInfo.rangeList);
                            controller.SetStrenghtToWeather(controller.EnemyInfo.rangeList);
                        }
                        else if (weatherCard == 206)
                        {
                            //siege
                            controller.SetStrenghtToWeather(controller.PlayerInfo.siegeList);
                            controller.SetStrenghtToWeather(controller.EnemyInfo.siegeList);
                        }
                    }
                    if (controller.cardSelected.transform.parent.GetComponent<RowClick>().isHornActive)
                    {
                        foreach (GameObject card1 in controller.cardSelected.transform.parent.GetComponent<RowClick>().GetListType())
                        {
                            if (card1.GetComponentInChildren<CardStats>().unique == false) card1.GetComponentInChildren<CardStats>().actualStrength *= 2;
                        }
                    }
                }

                controller.cardSelected = null;
                controller.SortCards(controller.GetPlayer().handList);


                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();

            }
            else
            {

                if(transform.position == onePos)
                {
                    this.transform.Translate(0, 10, 0);
                }

                if (!controller.cardSelected)
                {
                    controller.cardSelected = this.gameObject;

                    controller.art.SetActive(true);
                    controller.art.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + gameObject.GetComponentInChildren<CardStats>()._id);

                    
                    if (controller.cardSelected.GetComponentInChildren<CardStats>().ability == "swap_card")
                    {
                        controller.swapActivated = true;
                        Debug.Log("DECOYT kap " + controller.swapActivated);
                    }
                    else
                    {
                        controller.TurnOffHighlight(this.gameObject);
                        controller.HighlightRows(this.gameObject);
                    }

                }



            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onePos = this.transform.position;
        if(!controller.swapActivated)
        {
            Debug.Log("belep");
            if (controller.cardSelected == false)
            this.transform.Translate(0, 10, 0);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(controller.swapActivated==false)
        {
            if (controller.cardSelected == false)
                // this.transform.Translate(0, -10, 0);
                transform.position = onePos;
        }

    }

    void Update()
    {
        if (controller.cardSelected!=null && controller.isMedicActive == false)
        {
            if(Input.GetMouseButtonDown(1))
            {
                controller.art.SetActive(false);
                controller.cardSelected.transform.Translate(0, -10, 0);
                controller.TurnOffHighlight(controller.cardSelected);
                controller.cardSelected = null;
            }
        }
    }
}
