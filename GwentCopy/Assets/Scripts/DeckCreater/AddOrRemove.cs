using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddOrRemove : MonoBehaviour
{
    GameObject out_deck;
    GameObject in_deck;
    GameObject text_cards;
    GameObject text_hero_cards;
    GameObject text_special_cards;
    GameObject text_totalStrenght_cards;
    GameObject holder_out;
    GameObject holder_in;
    GameObject leader_holder;
    GameObject leader_panel;

    public Cards stats;
    public int nr = 0;

    int hero = 0;
    int special = 0;
    int totalStrenght = 0;
    int totalNr = 0;

    private void Awake()
    {
        leader_panel = GameObject.Find("LP1").transform.GetChild(0).gameObject;
    }

    void Start()
    {
        out_deck = GameObject.Find("out_deck");
        in_deck = GameObject.Find("in_deck");
        text_cards = GameObject.Find("CardsInDeck");
        text_hero_cards = GameObject.Find("HeroCards");
        text_special_cards = GameObject.Find("SpecialCards");
        text_totalStrenght_cards = GameObject.Find("TotalStrenghtCards");

        holder_in = GameObject.Find("RightDeck").transform.Find("Holder").gameObject;
        holder_out = GameObject.Find("LeftDeck").transform.Find("Holder").gameObject;

        leader_holder = GameObject.Find("LeaderCardHolder").gameObject;

    }

    public void Add_Remove()
    {
        if(transform.parent.parent.name == "out_deck")
        {

            Transform t = in_deck.transform.Find("Content").transform;


            if (nr > 0)
            {
                Debug.LogError("iegn tobb van ennel " + stats.name);

                Transform nem = t.Find(this.name);

                if (nem)
                {
                    Debug.LogError("igen a transform kapott ilyent");
                    nem.GetComponent<AddOrRemove>().nr++;
                    this.nr--;
                    Debug.LogError(this.nr +" "+ nem.GetComponent<AddOrRemove>().nr);
                    if (t.Find(this.name).GetComponent<AddOrRemove>().nr > 0)
                    {
                        t.Find(this.name).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = t.Find(this.name).GetComponent<AddOrRemove>().nr + 1 + "x ";
                        t.Find(this.name).transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
                else
                {
                    GameObject obj = Instantiate(this.gameObject);
                    obj.transform.SetParent(t);
                    obj.name = this.name;
                    obj.GetComponent<AddOrRemove>().nr=0;
                    obj.GetComponent<AddOrRemove>().stats = this.stats;
                    obj.GetComponent<Button>().onClick.AddListener(obj.GetComponent<AddOrRemove>().Add_Remove);
                    obj.transform.GetChild(2).gameObject.SetActive(false);
                    this.nr--;
                    Debug.LogError(this.nr + " " + obj.GetComponent<AddOrRemove>().nr);
                    holder_in.GetComponent<SortCards>().cardsActive.Add(obj);
                }

                if (nr==0)
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (nr + 1)+ "x ";
                }

                if (nr<0)
                {
                    holder_out.GetComponent<SortCards>().cardsActive.Remove(this.gameObject);
                    Destroy(this.gameObject);
                }

            }
            else
            {
                Transform nem = t.Find(this.name);

                if (nem)
                {
                    Debug.LogError("igen a transform kapott ilyent");
                    nem.GetComponent<AddOrRemove>().nr++;
                    this.nr--;
                    Debug.LogError(this.nr + " " + nem.GetComponent<AddOrRemove>().nr);


                    if (t.Find(this.name).GetComponent<AddOrRemove>().nr>0)
                    {
                        t.Find(this.name).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = t.Find(this.name).GetComponent<AddOrRemove>().nr+1 + "x ";
                        t.Find(this.name).transform.GetChild(2).gameObject.SetActive(true);
                    }
                    if (nr < 0)
                    {
                        holder_out.GetComponent<SortCards>().cardsActive.Remove(this.gameObject);
                        Destroy(this.gameObject);
                    }
                }
                else
                {

                    int db = 0;
                    for (int i = 0; i < t.childCount; i++)
                    {
                        if (t.GetChild(i).GetComponent<AddOrRemove>().stats.strength > stats.strength)
                        {
                            db++;
                        }
                    }
                    transform.SetParent(t);
                    transform.SetSiblingIndex(db);

                    holder_in.GetComponent<SortCards>().cardsActive.Add(this.gameObject);
                    holder_out.GetComponent<SortCards>().cardsActive.Add(this.gameObject);
                }

            }

            //megkeresni az all button, es meghivni a jo voidot
            holder_in.GetComponent<SortCards>().SelectCards(holder_in.transform.Find("AllCardsBt"));

        }
        else if (transform.parent.parent.name == "in_deck")
        {
            Transform t = out_deck.transform.Find("Content").transform;

            if (nr > 0)
            {
                Debug.LogError("iegn tobb van ennel " + stats.name);

                Transform nem = t.Find(this.name);

                if (nem)
                {
                    Debug.LogError("igen a transform kapott ilyent");
                    nem.GetComponent<AddOrRemove>().nr++;
                    this.nr--;
                    Debug.LogError(this.nr + " " + nem.GetComponent<AddOrRemove>().nr);
                    if (t.Find(this.name).GetComponent<AddOrRemove>().nr > 0)
                    {
                        t.Find(this.name).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = t.Find(this.name).GetComponent<AddOrRemove>().nr + 1 + "x ";
                        t.Find(this.name).transform.GetChild(2).gameObject.SetActive(true);
                    }
                }
                else
                {
                    GameObject obj = Instantiate(this.gameObject);
                    obj.transform.SetParent(t);
                    obj.name = this.name;
                    obj.GetComponent<AddOrRemove>().nr = 0;
                    obj.GetComponent<AddOrRemove>().stats = this.stats;
                    Debug.LogError(obj.GetComponent<AddOrRemove>().stats.row.ToString());
                    obj.GetComponent<Button>().onClick.AddListener(obj.GetComponent<AddOrRemove>().Add_Remove);
                    obj.transform.GetChild(2).gameObject.SetActive(false);
                    this.nr--;
                    Debug.LogError(this.nr + " " + obj.GetComponent<AddOrRemove>().nr);
                    holder_out.GetComponent<SortCards>().cardsActive.Add(obj);
                }

                if (nr == 0)
                {
                    transform.GetChild(2).gameObject.SetActive(false);
                }
                else
                {
                    transform.GetChild(2).gameObject.SetActive(true);
                    transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (nr + 1) + "x ";
                }

                if (nr < 0)
                {
                    holder_in.GetComponent<SortCards>().cardsActive.Remove(this.gameObject);
                    Destroy(this.gameObject);
                }

            }
            else
            {
                Transform nem = t.Find(this.name);

                if (nem)
                {
                    Debug.LogError("igen a transform kapott ilyent");
                    nem.GetComponent<AddOrRemove>().nr++;
                    this.nr--;
                    Debug.LogError(this.nr + " " + nem.GetComponent<AddOrRemove>().nr);


                    if (t.Find(this.name).GetComponent<AddOrRemove>().nr > 0)
                    {
                        t.Find(this.name).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = t.Find(this.name).GetComponent<AddOrRemove>().nr + 1 + "x ";
                        t.Find(this.name).transform.GetChild(2).gameObject.SetActive(true);
                    }
                    if (nr < 0)
                    {
                        holder_in.GetComponent<SortCards>().cardsActive.Remove(this.gameObject);
                        Destroy(this.gameObject);
                    }
                }
                else
                {

                    holder_in.GetComponent<SortCards>().cardsActive.Remove(this.gameObject);
                    holder_out.GetComponent<SortCards>().cardsActive.Add(this.gameObject);
                    int db = 0;
                    for (int i = 0; i < t.childCount; i++)
                    {
                        if (t.GetChild(i).GetComponent<AddOrRemove>().stats.strength > stats.strength)
                        {
                            db++;
                        }
                    }
                    transform.SetParent(t);
                    transform.SetSiblingIndex(db);
                }

            }
            holder_out.GetComponent<SortCards>().SelectCards(holder_out.transform.Find("AllCardsBt"));
        }

        UIChange();

    }

    public void LeaderChange()
    {
        Debug.Log("igen" + name);

        leader_holder.GetComponent<Image>().sprite = GetComponent<Image>().sprite;
        leader_panel.SetActive(false);
    }


    void UIChange()
    {
        Transform t = in_deck.transform.Find("Content").transform;


        hero = 0;
        special = 0;
        totalStrenght = 0;
        totalNr = 0;

        for (int i = 0; i < t.childCount; i++)
        {
            totalStrenght += t.GetChild(i).GetComponent<AddOrRemove>().stats.strength;
            totalNr = totalNr + t.GetChild(i).GetComponent<AddOrRemove>().nr + 1;
            if (t.GetChild(i).GetComponent<AddOrRemove>().stats.unique==true)
            {
                hero++;
            }
            if (t.GetChild(i).GetComponent<AddOrRemove>().stats.faction == "Special")
            {
                special = special + t.GetChild(i).GetComponent<AddOrRemove>().nr + 1;
            }
        }
        text_hero_cards.GetComponent<TextMeshProUGUI>().text = hero.ToString();
        text_special_cards.GetComponent<TextMeshProUGUI>().text = special + "/10";
        text_totalStrenght_cards.GetComponent<TextMeshProUGUI>().text = totalStrenght.ToString();
        text_cards.GetComponent<TextMeshProUGUI>().text = totalNr.ToString();


    }
    

   

}
