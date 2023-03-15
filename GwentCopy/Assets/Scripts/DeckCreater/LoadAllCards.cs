using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.Runtime.CompilerServices;

public class LoadAllCards : MonoBehaviour
{
    public GameObject buttonPrefab;


    //public Cards Stats;
    
    public class Custom
    {
        public Cards Stats;
        public int db = 0;
    }

    int deckIndex = 0;


    public string[] deckNames = new string[4] {"NR", "NF", "M", "SC"};
    public List<Custom> list = new List<Custom>();
    public List<GameObject> leaderList = new List<GameObject>();

    GameObject panel;

    void Awake()
    {
        deckIndex = 0;
        panel = GameObject.Find("LeaderPanel");
        igen();
    }

    private void Start()
    {
    }

    public void LButton()
    {
        if (deckIndex > 0)
        {
            deckIndex--;
        }
        else deckIndex = 3;

        ClearLeftRight();

        foreach (Transform item in panel.transform.GetChild(0))
        {
            Destroy(item.gameObject);
        }
        panel.transform.GetChild(0).DetachChildren();

        leaderList.Clear();
        panel.SetActive(true);
        igen();
        transform.parent.parent.Find("Holder").GetComponent<SortCards>().Start();
    }

    public void Rbutton()
    {
        if (deckIndex < 3)
        {
            deckIndex++;
        }
        else deckIndex = 0;

        ClearLeftRight();


        foreach (Transform item in panel.transform.GetChild(0))
        {
            Destroy(item.gameObject);
        }
        panel.transform.GetChild(0).DetachChildren();

        leaderList.Clear();
        panel.SetActive(true);
        igen();
        transform.parent.parent.Find("Holder").GetComponent<SortCards>().Start();
    }

    void ClearLeftRight()
    {
        //bal es jobb oldai content cleareles es active/hidden kartyak resetelese
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        transform.DetachChildren();
        list.Clear();
        transform.parent.parent.Find("Holder").GetComponent<SortCards>().cardsActive.Clear();


        Transform t1 = GameObject.Find("in_deck").transform;

        foreach (Transform child in t1.Find("Content").transform)
        {
            Destroy(child.gameObject);
        }
        t1.Find("Content").DetachChildren();
        t1.parent.Find("Holder").GetComponent<SortCards>().cardsActive.Clear();

        //UI atiras
        GameObject.Find("DeckName").GetComponent<TextMeshProUGUI>().text = deckNames[deckIndex];

        //content fel
        transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
    }

    public void igen()
    {

        TextAsset asset1 = Resources.Load<TextAsset>("Cards");
        CardsList cardsListinJson = JsonUtility.FromJson<CardsList>(asset1.text);


        foreach (Cards c in cardsListinJson.cardsList)
        {
            Custom item = new Custom();

            //implementalas, ha az a deck
            if (c.faction == deckNames[deckIndex] || c.faction == "N" || c.faction == "Special")
            {
                if (c.ability != "leader")
                {
                    item.Stats = c;
                    item.Stats._id = c._id;
                    item.Stats.strength = c.strength;
                    item.Stats.faction = c.faction;
                    item.Stats.unique = c.unique;
                    if (list.Any(Custom => Custom.Stats._id == item.Stats._id))
                    {
                        list.Find(y => y.Stats._id == item.Stats._id).db++;
                    }else
                    list.Add(item);

                }else
                {
                    //leader
                    item.Stats = c;
                    GameObject goButton = Instantiate(buttonPrefab) as GameObject;
                    Destroy(goButton.GetComponentInChildren<Discard>());
                    goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + item.Stats._id);
                    goButton.transform.SetParent(GameObject.Find("LeaderCardHolder").transform);
                    goButton.GetComponent<RectTransform>().anchorMax = new Vector2(.5f, 0.5f);
                    goButton.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
                    goButton.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
                    goButton.transform.localScale = new Vector3(5,5,5);
                    goButton.transform.localPosition = new Vector3(0,0,0);
                    goButton.name = c._id.ToString();
                    goButton.SetActive(false);
                    goButton.AddComponent<AddOrRemove>();
                    goButton.GetComponent<Button>().onClick.AddListener(goButton.GetComponent<AddOrRemove>().LeaderChange);
                    goButton.transform.parent.GetComponent<Image>().sprite = goButton.GetComponent<Image>().sprite;
                    
                    leaderList.Add(goButton);
                }
            }
        }

        SortCards(list);
    }

    public void SortCards(List<Custom> list)
    {
        list = list.OrderByDescending(x => x.Stats.strength).ThenBy(y => y.Stats.faction).ToList();
        for (int i = 0; i < list.Count; i++)
        {
            CreateButton(list[i]);
        }
        panel.SetActive(false);
    }


    public void LeaderShow()
    {
        foreach (GameObject item in leaderList)
        {
            item.transform.SetParent(panel.transform.GetChild(0));
            item.SetActive(true);
            item.transform.localScale = new Vector3(6, 6, 6);
        }

        panel.SetActive(true);

    }

    void CreateButton(Custom c)
    {
        GameObject goButton = Instantiate(buttonPrefab) as GameObject;
        Destroy(goButton.GetComponentInChildren<Discard>());
        goButton.AddComponent<AddOrRemove>();

        goButton.GetComponent<AddOrRemove>().stats = c.Stats;
        goButton.GetComponent<AddOrRemove>().nr = c.db;
        goButton.name = c.Stats._id.ToString();
        goButton.GetComponent<Button>().onClick.AddListener(goButton.GetComponent<AddOrRemove>().Add_Remove);
        goButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + c.Stats._id);
        goButton.transform.SetParent(this.transform);


        //also kep beallitasai
        GameObject kepobj = new GameObject();
        Image kep = kepobj.AddComponent<Image>();
        kepobj.transform.SetParent(goButton.transform);
        kep.rectTransform.localScale = new Vector2(1, 1);
        kep.rectTransform.localPosition = new Vector2(20.984f, -26.073f);
        kep.rectTransform.anchorMax = new Vector2(1, 0);
        kep.rectTransform.anchorMin = new Vector2(0, 0);
        kep.rectTransform.sizeDelta = new Vector2(0.246f, 17.254f);
        kep.rectTransform.anchoredPosition = new Vector2(-0.016f, 8.47f);

        if(c.Stats.faction == "N" || c.Stats.faction == "Special")
        {
            if(c.Stats.unique == true)
            {
                kep.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/CCard/unique_neutral");
            }else kep.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/CCard/normal_neutral");
        }
        else
        {
            if (c.Stats.unique == true)
            {
                kep.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/CCard/unique_faction");
            }
            else kep.GetComponent<Image>().sprite = Resources.Load<Sprite>("DeckParts/CCard/normal_faction");
        }

        //nev text
        GameObject textObj = new GameObject();
        TextMeshProUGUI t = textObj.AddComponent<TextMeshProUGUI>();
        textObj.transform.SetParent(goButton.transform);
        t.text = c.Stats.name;
        t.color = Color.black;
        t.fontSize = 20;
        t.alignment = TextAlignmentOptions.Center;
        textObj.transform.localPosition = new Vector2(25, -28);

        //ha kell, szam hogy duplicate
        if (c.db > 0)
        {

            GameObject nr_obj = new GameObject();
            nr_obj.name = "Nr_text";
            TextMeshProUGUI nr_text = nr_obj.AddComponent<TextMeshProUGUI>();
            nr_obj.transform.SetParent(goButton.transform);
            nr_obj.GetComponent<RectTransform>().localPosition = new Vector2(43f, -35.5f);
            nr_obj.GetComponent<RectTransform>().localScale = new Vector3(.2f, .2f, .2f);
            nr_obj.GetComponent<RectTransform>().anchorMin = new Vector2(1,0);
            nr_obj.GetComponent<RectTransform>().anchorMax = new Vector2(1,0);
            nr_obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            nr_obj.GetComponent<RectTransform>().sizeDelta = new Vector2(43, 22);
            nr_obj.GetComponent<RectTransform>().pivot = new Vector2(1, 0);
            nr_text.text = c.db+1 +"x ";
            nr_text.color = Color.black;
            nr_text.fontSize = 24;
            nr_text.alignment = TextAlignmentOptions.Center;

        }

    }


    // Update is called once per frame
    void Update()
    {
        if (panel.activeInHierarchy==true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                panel.SetActive(false);
            }
        }

        //ezzel fel lehet vinni a contentet a tetejere
        if (Input.GetKeyDown("h"))
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
        }
    }
}
