xusing System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderManager : MonoBehaviour
{
    SceneController controller;
    bool EmhyrActive = false;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.Find("SceneManager");
        controller = controllerObject.GetComponent<SceneController>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (controller.loadPanel.gameObject.activeInHierarchy==true && EmhyrActive)
            {
                controller.loadPanel.SetActive(false);
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
            }
        }

    }


    public void LeaderAction()
    {
        switch(controller.GetPlayer().leader.transform.Find("Art").GetComponent<Image>().sprite.name.ToString())
        {
            //---------------------Foltest---------------------------

            //kod(weather range)
            case "51":
            {
                if (!controller.weatherList.Contains(204))
                {
                    controller.weatherList.Add(204);
                    controller.SetStrenghtToWeather(controller.PlayerInfo.rangeList);
                    controller.SetStrenghtToWeather(controller.EnemyInfo.rangeList);
                }
                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
            }
            break;

            //szep ido
            case "52":
            {
                Debug.Log("WEATHER");

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

                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
            }
            break;

            //range scorch
            case "53":
            {
                List<GameObject> listToDestroy = new List<GameObject>();
                int power = 0, maxStrenght = 0;
                if(transform.name == "Leader_player")
                {
                    foreach (GameObject c in controller.EnemyInfo.rangeList)
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
                                controller.EnemyInfo.rangeList.Remove(c);

                                if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                                {
                                    controller.UpdateMorale(controller.EnemyInfo.rangeList, c, true);
                                }
                                controller.EnemyInfo.discardList.Add(c);


                                c.transform.SetParent(controller.gameObject.transform);
                                c.SetActive(false);
                            }
                        }
                    }
                }else
                {
                    foreach (GameObject c in controller.PlayerInfo.rangeList)
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
                                controller.PlayerInfo.rangeList.Remove(c);

                                if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                                {
                                    controller.UpdateMorale(controller.PlayerInfo.rangeList, c, true);
                                }
                                controller.PlayerInfo.discardList.Add(c);


                                c.transform.SetParent(controller.gameObject.transform);
                                c.SetActive(false);
                            }
                        }
                    }
                }
                listToDestroy.Clear();

                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
                }
            break;

            //duplazo
            case "54":
            {
                if(controller.player_horns[2].transform.childCount==0 && transform.name == "Leader_player")
                {
                    Debug.Log(transform.name);
                    GameObject instantiatedCard = Instantiate(controller.CardPrefab);
                    instantiatedCard.name = "Commander's Horn";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>()._id = 202;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().name = "Commander's Horn";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().strength = 0;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().actualStrength = 0;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().unique = false;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().ability = "double_strenght";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().isMoraleBoosted = false;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().faction = "Special";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().row = "special";

                    instantiatedCard.transform.SetParent(controller.player_horns[2].transform);
                    instantiatedCard.transform.localScale = new Vector3(0.2734375f, 0.2734375f, 0.2734375f);


                    controller.PlayerInfo.siegeList.Add(instantiatedCard);
                    controller.DoubleRow(controller.PlayerInfo.siegeList);
                    transform.parent.GetComponentInChildren<RowClick>().isHornActive = true;
                    controller.UpdateRowStrenghtImage(controller.PlayerInfo.siegeList);
                    controller.PlayerInfo.siegeList.Add(instantiatedCard);
                    

                }
                else if (controller.enemy_horns[2].transform.childCount == 0 && transform.name == "Leader_enemy")
                {
                    Debug.Log(transform.name);
                    GameObject instantiatedCard = Instantiate(controller.CardPrefab);
                    instantiatedCard.name = "Commander's Horn";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>()._id = 202;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().name = "Commander's Horn";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().strength = 0;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().actualStrength = 0;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().unique = false;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().ability = "double_strenght";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().isMoraleBoosted = false;
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().faction = "Special";
                    instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().row = "special";
                    instantiatedCard.transform.SetParent(controller.enemy_horns[2].transform);
                    instantiatedCard.transform.localScale = new Vector3(0.2734375f, 0.2734375f, 0.2734375f);

                    controller.PlayerInfo.siegeList.Add(instantiatedCard);
                    controller.DoubleRow(controller.EnemyInfo.siegeList);
                    transform.parent.GetComponentInChildren<RowClick>().isHornActive = true;
                    controller.UpdateRowStrenghtImage(controller.EnemyInfo.siegeList);
                    controller.EnemyInfo.siegeList.Add(instantiatedCard);
                }else
                    {
                        Debug.Log("VAN MAR OTT NEM CSINALOK SEMMIT");
                        return;
                    }

                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
            }
            break;

            //siege scorch
            case "55":
            {
                List<GameObject> listToDestroy = new List<GameObject>();
                int power = 0, maxStrenght = 0;
                if (transform.name == "Leader_player")
                {
                    foreach (GameObject c in controller.EnemyInfo.siegeList)
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
                                controller.EnemyInfo.siegeList.Remove(c);

                                if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                                {
                                    controller.UpdateMorale(controller.EnemyInfo.siegeList, c, true);
                                }
                                controller.EnemyInfo.discardList.Add(c);


                                c.transform.SetParent(controller.gameObject.transform);
                                c.SetActive(false);
                            }
                        }
                    }
                }
                else
                {
                    foreach (GameObject c in controller.PlayerInfo.siegeList)
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
                                controller.PlayerInfo.siegeList.Remove(c);

                                if (c.GetComponentInChildren<CardStats>().ability == "morale_boost")
                                {
                                    controller.UpdateMorale(controller.PlayerInfo.siegeList, c, true);
                                }
                                controller.PlayerInfo.discardList.Add(c);


                                c.transform.SetParent(controller.gameObject.transform);
                                c.SetActive(false);
                            }
                        }
                    }
                }
                listToDestroy.Clear();


                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
                }
            break;

            //------------------------Emhyr--------------------------------


            //look 3 random card from opponents hand

            case "33":
            {
                List<GameObject> list = new List<GameObject>();

                EmhyrActive = true;


                //enemy
                if (controller.GetPlayer() == controller.EnemyInfo)
                {
                    if (controller.PlayerInfo.handList.Count>3)
                    {
                        for (int i = 0; i < 3; i++)
                        {

                            int rnd = Random.Range(0, controller.PlayerInfo.handList.Count);
                            while (list.Contains(controller.PlayerInfo.handList[rnd]))
                            {
                                rnd = Random.Range(0, controller.PlayerInfo.handList.Count);
                            //    Debug.LogWarning("rnd emhyr " + rnd);
                            }
                            list.Add(controller.PlayerInfo.handList[rnd]);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < controller.PlayerInfo.handList.Count; i++)
                        {
                            list.Add(controller.PlayerInfo.handList[i]);
                        }
                    }

                    controller.loadPanel.GetComponentInChildren<LoadPanelList>().EmhyrAbility(list);


                    transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                    transform.Find("Trigger").GetComponent<Button>().enabled = false;

                       

                }else if(controller.GetPlayer() == controller.PlayerInfo)
                {
                    if (controller.EnemyInfo.handList.Count > 3)
                    {
                        for (int i = 0; i < 3; i++)
                        {

                            int rnd = Random.Range(0, controller.EnemyInfo.handList.Count);
                            while (list.Contains(controller.EnemyInfo.handList[rnd]))
                            {
                                rnd = Random.Range(0, controller.EnemyInfo.handList.Count);
                                Debug.LogWarning("rnd emhyr " + rnd);
                            }
                            list.Add(controller.EnemyInfo.handList[rnd]);
                        }

                    }
                    else
                    {
                        for (int i = 0; i < controller.EnemyInfo.handList.Count; i++)
                        {
                            list.Add(controller.EnemyInfo.handList[i]);
                        }
                    }

                    controller.loadPanel.GetComponentInChildren<LoadPanelList>().EmhyrAbility(list);


                    transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                    transform.Find("Trigger").GetComponent<Button>().enabled = false;



                }
            }
            break;


            //disable the enemy leader, at kell rakni hogy kor elejetol menjen nem gombra
            case "34":
            {
                if (transform.parent.name == "Leader_player")
                {
                    transform.parent.parent.Find("Leader_enemy").Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                    transform.parent.parent.Find("Leader_enemy").Find("Trigger").GetComponent<Button>().enabled = false;
                }else
                {
                    transform.parent.parent.Find("Player_enemy").Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                    transform.parent.parent.Find("Player_enemy").Find("Trigger").GetComponent<Button>().enabled = false;
                }

                
                /*transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();*/
            }
            break;

            //emhyr medic ability
            case "35":
            {
                controller.emhyrMedic = true;
            }break;

            //emhyr rain
            case "36":
            {
                if (!controller.weatherList.Contains(206))
                {
                    controller.weatherList.Add(206);
                    controller.SetStrenghtToWeather(controller.PlayerInfo.siegeList);
                    controller.SetStrenghtToWeather(controller.EnemyInfo.siegeList);
                    Debug.LogWarning("igen valtozott");
                }
                transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                transform.Find("Trigger").GetComponent<Button>().enabled = false;
                controller.ChangePlayer();
                controller.AllCardsRaycast();
                controller.ChangeUI();
                }   
            break;


            //draw random card from enemy discardlist
            case "37":
            {

                    List<GameObject> seged = new List<GameObject>();


                    if (controller.GetPlayer()==controller.EnemyInfo)
                    {
                        for (int i = 0; i < controller.PlayerInfo.discardList.Count; i++)
                        {
                            Debug.LogError(controller.PlayerInfo.discardList[i]);
                            if(controller.PlayerInfo.discardList[i].GetComponentInChildren<CardStats>().unique==false)
                            {
                                seged.Add(controller.PlayerInfo.discardList[i]);
                            }
                        }
                        //playerinfoval
                        int szam = seged.Count;
                        if (szam > 0)
                        {
                            int rnd = Random.Range(0, seged.Count - 1);
                            Debug.Log("enemynek adja a player discardbol "+ controller.PlayerInfo.discardList[rnd].name);
                            GameObject card = seged[rnd];
                            card.SetActive(true);
                            card.transform.SetParent(controller.GetPlayer().myHand.transform);
                            card.transform.localScale = new Vector3(2f, 2f, 2f);
                            controller.GetPlayer().handList.Add(card);
                            controller.PlayerInfo.discardList.Remove(card);
                            controller.GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
                            controller.SortCards(controller.GetPlayer().handList);
                        }
                        else Debug.Log("hulye vagy");
                    
                    }else
                    {
                        //enemyinfoval
                        for (int i = 0; i < controller.EnemyInfo.discardList.Count; i++)
                        {
                            Debug.LogError(controller.EnemyInfo.discardList[i]);
                            if (controller.EnemyInfo.discardList[i].GetComponentInChildren<CardStats>().unique == false)
                            {
                                seged.Add(controller.EnemyInfo.discardList[i]);
                            }
                        }

                        int szam = seged.Count;
                        if (szam > 0)
                        {
                            int rnd = Random.Range(0, seged.Count - 1);
                            Debug.Log("enemynek adja a player discardbol " + controller.EnemyInfo.discardList[rnd].name);
                            GameObject card = seged[rnd];
                            card.SetActive(true);
                            card.transform.SetParent(controller.GetPlayer().myHand.transform);
                            card.transform.localScale = new Vector3(2f, 2f, 2f);
                            controller.GetPlayer().handList.Add(card);
                            controller.EnemyInfo.discardList.Remove(card);
                            controller.GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
                            controller.SortCards(controller.GetPlayer().handList);
                        }
                        else Debug.Log("hulye vagy");

                    }

                    transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                    transform.Find("Trigger").GetComponent<Button>().enabled = false;
                    controller.ChangePlayer();
                    controller.AllCardsRaycast();
                    controller.ChangeUI();

                }
            break;
        }
    }
}
