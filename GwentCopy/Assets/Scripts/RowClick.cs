using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class RowClick : MonoBehaviour, IPointerClickHandler
{
    SceneController controller;
    public SceneController.Slot typeOfCard;


    public Transform parentToReturnTo = null;

    public bool isMorale = false;
    public bool isHornActive = false;

    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if (controller.isCardSelected == true && controller.cardSelected!=false)
        {
            CardTranslate d = controller.cardSelected.GetComponent<CardTranslate>();
            GameObject card = controller.cardSelected;

            switch(controller.battleState.ToString())
            {
                case "PlayerTurn":
                    {
                        Debug.Log("PLAYERTURN");
                        if (d != null)
                        {
                            if (this.transform.parent.parent.name == "PlayerField" && card.GetComponentInChildren<CardStats>().ability != "spy")
                            {
                                if (d.typeOfCard.ToString() == "close_range")
                                {
                                    if (this.typeOfCard.ToString() == "close" || this.typeOfCard.ToString() == "range")
                                    {
                                        removeFromHand(card);
                                        addToRow(card);
                                        PlaceCard();
                                    }
                                }
                                else
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                                else if (d.typeOfCard.ToString() == "scorch")
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                            }
                            else if (this.transform.parent.parent.name == "EnemyField" && card.GetComponentInChildren<CardStats>().ability == "spy")
                            {
                                //itt jobban le kell kezelni a spyt enemy turnt, valoszinuleg battlestatel
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                            }

                        }

                    }
                    break;

                case "EnemyTurn":
                    {
                        Debug.Log("ENEMYTURN");

                        if (d != null)
                        {
                            if (this.transform.parent.parent.name == "EnemyField" && card.GetComponentInChildren<CardStats>().ability != "spy")
                            {
                                if (d.typeOfCard.ToString() == "close_range")
                                {
                                    if (this.typeOfCard.ToString() == "close" || this.typeOfCard.ToString() == "range")
                                    {
                                        removeFromHand(card);
                                        addToRow(card);
                                        PlaceCard();
                                    }
                                }
                                else
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                                else if (d.typeOfCard.ToString() == "scorch")
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                            }
                            else if (this.transform.parent.parent.name == "PlayerField" && card.GetComponentInChildren<CardStats>().ability == "spy")
                            {
                                //itt jobban le kell kezelni a spyt enemy turnt, valoszinuleg battlestatel
                                if (d.typeOfCard == this.typeOfCard)
                                {
                                    removeFromHand(card);
                                    addToRow(card);
                                    PlaceCard();
                                }
                            }

                        }

                    }
                    break;
            }

            //Debug.Log(this.transform.parent.name);
            
        }
    }

    void PlaceCard()
    {
        GameObject card = controller.cardSelected;

        controller.art.SetActive(false);

        Debug.Log("a kartya ami a kezedben van az a " + card.name);

        //weather cardok
        if(card.GetComponentInChildren<CardStats>().row.ToString()=="weather")
        {
            //clear weather
            if (card.GetComponentInChildren<CardStats>()._id==201)
            {
                controller.SetStrenghtToNormal(controller.PlayerInfo.closeList);
                controller.SetStrenghtToNormal(controller.PlayerInfo.rangeList);
                controller.SetStrenghtToNormal(controller.PlayerInfo.siegeList);
                controller.SetStrenghtToNormal(controller.EnemyInfo.closeList);
                controller.SetStrenghtToNormal(controller.EnemyInfo.rangeList);
                controller.SetStrenghtToNormal(controller.EnemyInfo.siegeList);

                controller.weatherList.Clear();
                foreach (Transform c in controller.weatherHolder.transform)
                {
                    Destroy(c.gameObject);
                }
            }
            else
            {
                if(!controller.weatherList.Contains(card.GetComponentInChildren<CardStats>()._id))
                controller.weatherList.Add(card.GetComponentInChildren<CardStats>()._id);
            }

        }
        else
        {
            parentToReturnTo = this.transform;
            card.transform.SetParent(parentToReturnTo);
        }

        //scorch
        if (card.GetComponentInChildren<CardStats>().ability == "kill_strongest")
        {
            Destroy(card);
            GetListType().Remove(card);
            controller.DestroyStrongest();
        }

        //medic
        if (card.GetComponentInChildren<CardStats>().ability == "medic")
        {
            int k = 0;

            foreach (GameObject c in controller.GetPlayer().discardList)
            {
                if (c.GetComponentInChildren<CardStats>().unique)
                {
                    k++;
                }
            }

            Debug.Log("K: " + k);

            if (controller.GetPlayer().discardList.Count != 0 && controller.GetPlayer().discardList.Count != k)
            {
                controller.Medic();
            }
            else controller.isMedicActive = false;

        }
        else controller.isMedicActive = false;


        //spy
        if (card.GetComponentInChildren<CardStats>().ability == "spy")
        {
            Debug.Log("drow 2 new cards from the players deck");

            if(controller.GetPlayer().deckList.Count==0)
            {
                Debug.Log("NICS TOBB KARTYA!!");
            }
            else
            if(controller.GetPlayer().deckList.Count<2)
            {
                GameObject card1 = controller.GetPlayer().deckList[0];
                card1.SetActive(true);
                card1.transform.SetParent(controller.GetPlayer().myHand.transform);
                card1.transform.localScale = new Vector3(2f, 2f, 2f);
                controller.GetPlayer().handList.Add(card1);
                controller.GetPlayer().deckList.Remove(card1);
            }
            else
            {
                int rnd;
                for (int i = 0; i <= 1; i++)
                {
                    rnd = Random.Range(0, controller.GetPlayer().deckList.Count);
                    GameObject card1 = controller.GetPlayer().deckList[rnd];
                    card1.SetActive(true);
                    card1.transform.SetParent(controller.GetPlayer().myHand.transform);
                    card1.transform.localScale = new Vector3(2f, 2f, 2f);
                    controller.GetPlayer().handList.Add(card1);
                    controller.GetPlayer().deckList.Remove(card1);
                }
            }
            controller.GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();   
        }
      
        //tight bond
        if (card.GetComponentInChildren<CardStats>().ability == "tight_bond")
        {
            TightBond(card);
        }
        else
        if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
        {
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = true;
            isMorale = true;
            MoraleBoost(card, false);
            //IsHorn(card);
        }
        else if (isMoraleCardActive())
        {

            Debug.Log("itt most a moraleboostos if ");
            MoraleBoost(card, false);
            //IsHorn(card);

        }
        else if (isMorale ==true && !isMoraleCardActive())
        {
            Debug.Log("minuszos if");
            MoraleBoost(card, true);
        }
        //scortch close
        if (card.GetComponentInChildren<CardStats>().ability == "scorch_close")
        {
            List<GameObject> listToDestroy = new List<GameObject>();
            int power=0, maxStrenght=0;
            if (this.transform.parent.parent.name == "PlayerField")
            {
                foreach (GameObject c in controller.EnemyInfo.closeList)
                {
                    power += c.GetComponentInChildren<CardStats>().actualStrength;
                    if (c.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && c.GetComponentInChildren<CardStats>().unique == false)
                    {
                        listToDestroy.Clear();
                        listToDestroy.Add(c);
                        maxStrenght = c.GetComponentInChildren<CardStats>().actualStrength;
                    }
                    else if (c.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && c.GetComponentInChildren<CardStats>().unique == false)
                    {
                        listToDestroy.Add(c);
                    }
                }

                if(power>=10)
                {
                    if(listToDestroy.Count>0)
                    {
                        foreach (GameObject c in listToDestroy)
                        {
                            controller.EnemyInfo.closeList.Remove(c);
                        
                            if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                            {
                                controller.UpdateMorale(controller.EnemyInfo.closeList, c, true);
                            }
                            controller.EnemyInfo.discardList.Add(c);

                            
                            c.transform.SetParent(controller.gameObject.transform);
                            c.SetActive(false);
                        }
                    }
                }
            }else
            {
                foreach (GameObject c in controller.PlayerInfo.closeList)
                {
                    power += c.GetComponentInChildren<CardStats>().actualStrength;
                    if (c.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && c.GetComponentInChildren<CardStats>().unique == false)
                    {
                        listToDestroy.Clear();
                        listToDestroy.Add(c);
                        maxStrenght = c.GetComponentInChildren<CardStats>().actualStrength;
                    }
                    else if (c.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && c.GetComponentInChildren<CardStats>().unique == false)
                    {
                        listToDestroy.Add(c);
                    }
                }

                if (power >= 10)
                {
                    if (listToDestroy.Count > 0)
                    {
                        foreach (GameObject c in listToDestroy)
                        {
                            controller.PlayerInfo.closeList.Remove(c);

                            if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                            {
                                controller.UpdateMorale(controller.PlayerInfo.closeList, c, true);
                            }
                            controller.PlayerInfo.discardList.Add(c);


                            c.transform.SetParent(controller.gameObject.transform);
                            c.SetActive(false);
                        }
                    }
                }
            }
            listToDestroy.Clear();
        }

        //horn
        if (card.GetComponentInChildren<CardStats>().ability == "commander_horn")
        {
            if(!isHornActive)
            {
                isHornActive = true;
                foreach (GameObject c in GetListType())
                {
                    Debug.Log("neve> " + c.name + " " + c.GetComponentInChildren<CardStats>().actualStrength);
                    if (c.GetComponentInChildren<CardStats>().unique == false && c.GetComponentInChildren<CardStats>().faction != "Special" && c!=card)
                    {
                        c.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }

                }
            }else
            {
                card.GetComponentInChildren<CardStats>().actualStrength *= 2;
            }

        }

        //weather
        if(controller.weatherList.Count>0)
        {
            foreach (int weatherCard in controller.weatherList)
            {
                //biting frost
                if(weatherCard==200)
                {
                    controller.SetStrenghtToWeather(controller.PlayerInfo.closeList);
                    controller.SetStrenghtToWeather(controller.EnemyInfo.closeList);
                }else if (weatherCard == 204)
                {
                    //range
                    controller.SetStrenghtToWeather(controller.PlayerInfo.rangeList);
                    controller.SetStrenghtToWeather(controller.EnemyInfo.rangeList);
                }
                else if(weatherCard == 206)
                {
                    //siege
                    controller.SetStrenghtToWeather(controller.PlayerInfo.siegeList);
                    controller.SetStrenghtToWeather(controller.EnemyInfo.siegeList);
                }
            }
            
        }
        else IsHorn(card);

        card.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if(!controller.isMedicActive)
        {
            controller.TurnOffHighlight(controller.cardSelected);
        }


        controller.SortCards(GetListType());
        controller.SortCards(controller.GetPlayer().handList);

        controller.GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
        ResizeHand();
        controller.UpdateRowStrenghtImage(GetListType());

        Debug.Log("a medic active ez " + controller.isMedicActive);
        //medic
        if(!controller.isMedicActive)
        {
            if(controller.GetPlayer().handList.Count==0)
            {
                controller.Pass();
            }else
            {
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
            }

        }

    }

    bool isMoraleCardActive()
    {
        foreach (GameObject card in GetListType())
        {
            if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
            {
                isMorale = true;
                return true;
            }
        }
        isMorale = false;
        return false;
    }

    public void IsHorn(GameObject card)
    {
        if (isHornActive)
        {
            //card.GetComponentInChildren<CardStats>().actualStrength *= 2;
            switch (card.GetComponentInChildren<CardStats>().ability)
            {
                case "tight_bond":
                    {
                        foreach (GameObject c in GetListType())
                        {
                            if (c.GetComponentInChildren<CardStats>().ability == "tight_bond")
                            {
                                c.GetComponentInChildren<CardStats>().actualStrength *= 2;
                            }
                        }
                    }
                    break;
                case "morale_boost":
                    {
                        foreach (GameObject c in GetListType())
                        {
                            if (c.GetComponentInChildren<CardStats>().ability == "tight_bond")
                            {
                                c.GetComponentInChildren<CardStats>().actualStrength += 1;
                            }
                            else if(c.GetComponentInChildren<CardStats>().unique==false)
                            {
                                c.GetComponentInChildren<CardStats>().actualStrength *= 2;
                            }
                        }
                    }
                    break;
                    default:
                        {
                            foreach (GameObject c in GetListType())
                            {
                                if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                                {
                                    c.GetComponentInChildren<CardStats>().actualStrength *= 2;
                                }
                        }
                            if(card.GetComponentInChildren<CardStats>().unique==false&& card.GetComponentInChildren<CardStats>().ability!="commander_horn")
                                card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                            }
                        break;
            }
            controller.UpdateRowStrenghtImage(GetListType());
        }

    }

    public List<GameObject> GetListType()
    {
        if(this.transform.parent.parent.name=="PlayerField")
        switch(typeOfCard.ToString())
        {
            case "close" : return controller.PlayerInfo.closeList;
            case "range": return controller.PlayerInfo.rangeList;
            case "siege": return controller.PlayerInfo.siegeList;
            default: return null;
        }
        else if(this.transform.parent.parent.name == "EnemyField")
        {
            switch (typeOfCard.ToString())
            {
                case "close": return controller.EnemyInfo.closeList;
                case "range": return controller.EnemyInfo.rangeList;
                case "siege": return controller.EnemyInfo.siegeList;
                default: return null;
            }
        }
        return null;
    }

   

    public void MoraleBoost(GameObject card, bool reduce)
    {
        controller.UpdateMorale(GetListType(), card, reduce);
    }

    //false=noveli, true csokkenti, decoynal lehet hasznalni
    public void TightBond(GameObject card)
    {     
        controller.UpdateTightBond(GetListType(), card.GetComponentInChildren<CardStats>()._id, card.GetComponentInChildren<CardStats>().strength, isMorale);
    }

    void addToRow(GameObject card)
    {
        if(card.GetComponentInChildren<CardStats>().row.ToString() == "weather"&& !controller.weatherList.Contains(card.GetComponentInChildren<CardStats>()._id))
        {
            card.transform.SetParent(controller.weatherHolder.transform);
            return;
        }else
        GetListType().Add(card);
        Debug.Log("itt adja hozza" + GetListType().Count);
    }

    void removeFromHand(GameObject card)
    {
        controller.GetPlayer().handList.Remove(card);
    }

    public void ResizeHand()
    {
        gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(-72, 0);

        float maxWidth = gameObject.GetComponent<RectTransform>().rect.width;
        float spacingX = gameObject.GetComponent<GridLayoutGroup>().spacing.x;
        int numberOfCards = 0;
        float cardWidth = 0;

        foreach (Transform child in transform)
        {
            numberOfCards += 1;
            RectTransform rt = (RectTransform)child.transform;
            cardWidth = rt.rect.width;
        }

        float handWidth = numberOfCards * cardWidth + (numberOfCards ) * spacingX;
        Debug.Log("resize " + handWidth);
        float offset = 15;
        if (handWidth > maxWidth - offset)
        {
            spacingX = (maxWidth - offset - numberOfCards * cardWidth) / numberOfCards - 1 ;
            gameObject.GetComponent<GridLayoutGroup>().spacing = new Vector2(spacingX, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
