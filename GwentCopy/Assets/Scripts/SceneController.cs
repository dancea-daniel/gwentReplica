using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;


public class SceneController : MonoBehaviour
{
    public GameObject CardPrefab;


    public GameObject MainCanvas;

    public GameObject art;



    public bool isCardSelected = false;
    public bool isMedicActive = false;
    public bool swapActivated = false;
    public bool isPassed = false;
    public bool isWeatherCardActive = false;

    public bool emhyrMedic = false;

    public List<int> weatherList = new List<int>();
    public GameObject cardSelected;




    public enum Slot { close, range, close_range, siege, inventory, horn, scorch, spy };
    public Slot typeOfCard;

    public enum BattleState { PlayerTurn, EnemyTurn , Pre_Player, Pre_Enemy };
    public BattleState battleState;

    GameObject pausePanel;

    public GameObject changePanel;


    public GameObject player_close;
    public GameObject player_range;
    public GameObject player_siege;
    public GameObject[] player_horns = new GameObject[3];

    public GameObject enemy_close;
    public GameObject enemy_range;
    public GameObject enemy_siege;
    public GameObject[] enemy_horns = new GameObject[3];

    public GameObject Player_Field;
    public GameObject Enemy_Field;


    public GameObject loadPanel;
    public int loadPanelCount = 0;

    public Sprite closeNormal;
    public Sprite closeHighlight;
    public Sprite rangeNormal;
    public Sprite rangeHighlight;
    public Sprite siegeNormal;
    public Sprite siegeHighlight;
    public Sprite hornNormal;
    public Sprite hornHighlight;

    public GameObject weatherHolder;

    string testTest;
    TextAsset deckToLoadText;

    [SerializeField]
    public class MainInfo
    {
        public List<int> deckToLoadList = new List<int>();
        public List<GameObject> deckList = new List<GameObject>();
        public List<GameObject> discardList = new List<GameObject>();
        public List<GameObject> handList = new List<GameObject>();
        public List<GameObject> siegeList = new List<GameObject>();
        public List<GameObject> rangeList = new List<GameObject>();
        public List<GameObject> closeList = new List<GameObject>();
        public GameObject myHand;
        public int lifePoint = 2;
        public GameObject scoreUI;
        public GameObject leader;
        public GameObject deckHolder;
        public string card_faction;
    }
    public MainInfo PlayerInfo = new MainInfo();
    public MainInfo EnemyInfo = new MainInfo();
    public MainInfo myInfo = new MainInfo();


    void Awake()
    {
        battleState = BattleState.PlayerTurn;
        Debug.Log("turn " + battleState);


        PlayerInfo.myHand = GameObject.Find("Hand");
        EnemyInfo.myHand = GameObject.Find("Hand_enemy");

        PlayerInfo.scoreUI = GameObject.Find("Score_player");
        EnemyInfo.scoreUI = GameObject.Find("Score_enemy");

        PlayerInfo.leader = GameObject.Find("Leader_player");
        EnemyInfo.leader = GameObject.Find("Leader_enemy");

        PlayerInfo.deckHolder = GameObject.Find("DeckHolder_Player");
        EnemyInfo.deckHolder = GameObject.Find("DeckHolder_Enemy");

        //atadva utolag betoltesnel
        PlayerInfo.card_faction = "NR";
        EnemyInfo.card_faction = "NR";

        LoadDeck(Resources.Load<TextAsset>("deckToLoad"));
        battleState = BattleState.EnemyTurn;
        LoadDeck(Resources.Load<TextAsset>("deckToLoadEnemy"));
        battleState = BattleState.Pre_Player;

        pausePanel = GameObject.Find("PausePanel").gameObject;
        pausePanel.GetComponent<MenuButtons>().pauseMenu = pausePanel;
        pausePanel.GetComponent<MenuButtons>().menuButtons = pausePanel.transform.GetChild(0).gameObject;
        pausePanel.GetComponent<MenuButtons>().optionsButtons = pausePanel.transform.GetChild(1).gameObject;
        pausePanel.GetComponent<MenuButtons>().endingButtons = pausePanel.transform.GetChild(2).gameObject;
        pausePanel.SetActive(false);

        changePanel = GameObject.Find("ChangePanel").gameObject;
        changePanel.SetActive(false);

        Debug.Log("igen " + GetPlayer().handList.Count);

    }

    private void Start()
    {
        PlayerInfo.myHand.SetActive(false);
        EnemyInfo.myHand.SetActive(false);
        loadPanel.GetComponentInChildren<LoadPanelList>().AddCardsToContent();

        art = GameObject.Find("Big_art");
        art.SetActive(false);

        
        // GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            pausePanel.transform.GetChild(0).gameObject.SetActive(true);
            pausePanel.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (Input.GetKeyDown("f"))
        {
            Debug.Log(GetPlayer().closeList.Count);
            foreach (GameObject c in GetPlayer().closeList)
            {
                Debug.Log(c.name + "   " + c.GetComponentInChildren<CardStats>().actualStrength);
            }
            foreach (GameObject c in GetPlayer().handList)
            {
                Debug.Log(c.name + "   " + c.GetComponentInChildren<CardStats>().actualStrength);
            }
            Debug.Log(totalStrenght(PlayerInfo));
        }
        if (Input.GetKeyDown("d"))
        {
            InstantiateCard();
        }
        if (Input.GetKeyDown("g")) SpawnTenRandomCard();
        if (Input.GetKeyDown("b")) SortCards(GetPlayer().handList);
        if (Input.GetKeyDown("l")) Debug.Log(GetPlayer().deckList.Count);
        if (Input.GetKeyDown("p"))
        {
            foreach (Transform item in GetPlayer().myHand.transform)
            {
                Debug.Log(item.transform.position);
            }

            Debug.Log("a hand tagjai " + PlayerInfo.myHand.transform.childCount);
        }
        if (Input.GetKeyDown("s")) LoadDeck(Resources.Load<TextAsset>("deckToLoad"));
        if (Input.GetKeyDown("c"))RoundPanelChange("test");

    }

    public MainInfo GetPlayer()
    {
        if (battleState.ToString() == "PlayerTurn" || battleState.ToString() == "Pre_Player") return PlayerInfo;
        else if (battleState.ToString() == "EnemyTurn" || battleState.ToString() == "Pre_Enemy") return EnemyInfo;

        return null;
    } 

    //tr.parent.parent szerint visszateriti a sort
    //ide is kell az enemy info (mostmar asszem nem de meg kene nezni)
    public List<GameObject> GetCardListType(GameObject card)
    {
        //Debug.Log(card.transform.parent.parent.name);
        if (card.transform.parent.parent.parent.name.ToString() == "PlayerField")
        {
            Debug.Log("a card listaja playerfield");
            switch (card.transform.parent.parent.name)
            {
                case "Close": return PlayerInfo.closeList;
                case "Range": return PlayerInfo.rangeList;
                case "Siege": return PlayerInfo.siegeList;
                default: return null;
            }
        } else
        {
            Debug.Log("a card listaja enemyfield");
            switch (card.transform.parent.parent.name)
            {
                case "Close": return EnemyInfo.closeList;
                case "Range": return EnemyInfo.rangeList;
                case "Siege": return EnemyInfo.siegeList;
                default: return null;
            }
        }

    }

    //egy listabol vissza teriti id szerint
    public GameObject GetCardWithId(int id, List<GameObject> list)
    {
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>()._id == id) return card;
        }
        return null;
    }

    void LoadDeck(TextAsset deckToLoadText)
    {
        string[] numberStrings = deckToLoadText.text.Split('\n');


        int[] numbers = new int[numberStrings.Length-1];

        for (int i = 0; i < numberStrings.Length-1; i++)
        {
            numbers[i] = int.Parse(numberStrings[i]);
        }
        GetPlayer().deckToLoadList.AddRange(numbers);



        //kartyak betoltese
        TextAsset asset1 = Resources.Load<TextAsset>("Cards");
        if (asset1 != null)
        {
            CardsList cardsListinJson = JsonUtility.FromJson<CardsList>(asset1.text);
            CardsList testToAdd = new CardsList();


            //beteszi az id-kat a decklistbe
            foreach (Cards card in cardsListinJson.cardsList)
            {
                //itt ez mukodhet majd ha decket kell kivalasztani
                if (GetPlayer().deckToLoadList.Contains(card._id))
                {
                    GetPlayer().deckToLoadList.Remove(card._id);
                    testToAdd.cardsList.Add(new Cards
                    {
                        name = card.name,
                        ability = card.ability,
                        faction = card.faction,
                        row = card.row,
                        strength = card.strength,
                        unique = card.unique,
                        _id = card._id
                    });
                    testToAdd.cardsList.ToArray();
                    testTest = JsonUtility.ToJson(testToAdd);
                }

            }
            testToAdd = JsonUtility.FromJson<CardsList>(testTest);
            foreach (Cards card in testToAdd.cardsList)
            {
                Debug.Log("deckbe megy ez a kartya " + card.name);
                if (card.ability == "leader")
                {  
                    GetPlayer().leader.transform.Find("Art").GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/135x87/" + card._id);
                    GetPlayer().card_faction = card.faction.ToString();

                    Debug.LogError(card._id+"");

                    if (card._id==34)
                    {
                        EnemyInfo.leader.transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                        EnemyInfo.leader.transform.Find("Trigger").GetComponent<Button>().enabled = false;
                     
                        PlayerInfo.leader.transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_used");
                        PlayerInfo.leader.transform.Find("Trigger").GetComponent<Button>().enabled = false;
                        
                    }else if (card._id==35)
                    {
                        emhyrMedic = true;
                    }

                }else 
                PutIntoDeckList(card);
            }

            //kezdo kartyak
            SpawnTenRandomCard();
            //sorrendbe teszi a kezet
            SortCards(GetPlayer().handList);

        }
        else Debug.Log("nem kapja");

        //kartyat tesz es a szamot arra allitja

        GameObject d = new GameObject();
        d.AddComponent<Image>();
        switch (GetPlayer().card_faction)
        {
            case "NR":
                {
                    d.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Back/NR");

                    GetPlayer().scoreUI.transform.Find("deck").GetComponent<TextMeshProUGUI>().text = "Northern Kingdoms";
                    GetPlayer().scoreUI.transform.Find("Avatar").transform.Find("Deck_image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/faction/NR");

                    break;
                }
            case "NF":
                {
                    d.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Back/NF");

                    GetPlayer().scoreUI.transform.Find("deck").GetComponent<TextMeshProUGUI>().text = "Nilfgaard";
                    GetPlayer().scoreUI.transform.Find("Avatar").transform.Find("Deck_image").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/faction/NF");

                    break;
                }
            case "SC":
                {
                    d.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Back/SC");
                    break;
                }
            case "M":
                {
                    d.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/Back/M");
                    break;
                }
        }

        for (int i = 0; i < GetPlayer().deckList.Count; i++)
        {
            GameObject dk = Instantiate(d);
            dk.transform.SetParent(GetPlayer().deckHolder.transform);
            dk.transform.GetComponentInChildren<RectTransform>().sizeDelta = new Vector3(100f, 150f, 1f);
            dk.transform.localPosition = new Vector3(4f * i, 0f, 0f);
            dk.transform.SetAsFirstSibling();

        }
        Destroy(d.gameObject);

    }

    public void TurnOffLoadPanel()
    {
        loadPanel.SetActive(false);
        if (battleState.ToString() == "Pre_Player")
        {
            battleState = BattleState.Pre_Enemy;
            loadPanel.GetComponentInChildren<LoadPanelList>().AddCardsToContent();
            loadPanelCount = 0;
        }
        else if (battleState.ToString() == "Pre_Enemy")
        {
            int rnd = Random.Range(0, 10);
            if (rnd % 2 == 0)
            {
                battleState = BattleState.PlayerTurn;
                Debug.Log("OH PANZER OF THE LAKE WHAT IS YOUR WISDOM? WHO STARTS? " + battleState);
                RoundPanelChange("The Player Starts");
                EnemyInfo.myHand.SetActive(false);
                PlayerInfo.myHand.SetActive(true);
            }
            else
            {
                battleState = BattleState.EnemyTurn;
                Debug.Log("OH PANZER OF THE LAKE WHAT IS YOUR WISDOM? WHO STARTS? " + battleState);
                RoundPanelChange("The Enemy Starts");
                EnemyInfo.myHand.SetActive(true);
                PlayerInfo.myHand.SetActive(false);
            }
            GetPlayer().leader.transform.Find("Trigger").gameObject.SetActive(true);
            GetPlayer().deckHolder.GetComponent<Button>().interactable = true;

            if ((battleState.ToString() == "PlayerTurn" && Player_Field.transform.localPosition.y > Enemy_Field.transform.localPosition.y) || (battleState.ToString() == "EnemyTurn" && Player_Field.transform.localPosition.y < Enemy_Field.transform.localPosition.y))
            {
                Vector2 onePos = new Vector2();
                onePos = Player_Field.transform.localPosition;
                Player_Field.transform.localPosition = Enemy_Field.transform.localPosition;
                Enemy_Field.transform.localPosition = onePos;

                //siege close csere
                onePos = Player_Field.transform.GetChild(0).transform.position;
                Player_Field.transform.GetChild(0).transform.position = Player_Field.transform.GetChild(2).transform.position;
                Player_Field.transform.GetChild(2).transform.position = onePos;

                onePos = Enemy_Field.transform.GetChild(0).transform.position;
                Enemy_Field.transform.GetChild(0).transform.position = Enemy_Field.transform.GetChild(2).transform.position;
                Enemy_Field.transform.GetChild(2).transform.position = onePos;

                onePos = PlayerInfo.scoreUI.transform.position;
                PlayerInfo.scoreUI.transform.position = EnemyInfo.scoreUI.transform.position;
                EnemyInfo.scoreUI.transform.position = onePos;

                //leader
                onePos = PlayerInfo.leader.transform.position;
                PlayerInfo.leader.transform.position = EnemyInfo.leader.transform.position;
                EnemyInfo.leader.transform.position = onePos;

                //deckUI
                onePos = PlayerInfo.deckHolder.transform.position;
                PlayerInfo.deckHolder.transform.position = EnemyInfo.deckHolder.transform.position;
                EnemyInfo.deckHolder.transform.position = onePos;
            }

            AllCardsRaycast();
            DeckChange();
        }
    }

    //nem feltetlen kell de be tesz minden kartyat ami a deckben van
    void InstantiateCard()
    {
        foreach (GameObject card in GetPlayer().deckList)
        {
            Debug.Log(card.name);
            card.SetActive(true);
            card.transform.parent = GetPlayer().myHand.transform;
            card.transform.localScale = new Vector3(2f, 2f, 2f);
            GetPlayer().handList.Add(card);
            GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
        }
    }

    //az elejen bespawnol eleg kartyat
    void SpawnTenRandomCard()
    {
        int rnd;
        for (int i = 0; i <= 9; i++)
        {
            rnd = Random.Range(0, GetPlayer().deckList.Count);
            //Debug.LogError(rnd);
            GameObject card = GetPlayer().deckList[rnd];
            card.SetActive(true);
            card.transform.SetParent(GetPlayer().myHand.transform);
            card.transform.localScale = new Vector3(2f, 2f, 2f);
            GetPlayer().handList.Add(card);
            GetPlayer().deckList.Remove(card);
        }
        GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();

    }

    //sort
    public int SortByStrength(GameObject o1, GameObject o2)
    {
        if (o1.GetComponentInChildren<CardStats>().strength == o2.GetComponentInChildren<CardStats>().strength)
        {
            return o1.GetComponentInChildren<CardStats>()._id.CompareTo(o2.GetComponentInChildren<CardStats>()._id);
        }
        return o1.GetComponentInChildren<CardStats>().strength.CompareTo(o2.GetComponentInChildren<CardStats>().strength);
    }

    public void SortCards(List<GameObject> list)
    {
        // list = list.OrderBy(go => go.GetComponentInChildren<CardStats>().strength).ToList();
        list.Sort(SortByStrength);
        int i = 0;
        foreach (GameObject card in list)
        {
            card.transform.SetSiblingIndex(i);
            i++;
        }


    }

    void PutIntoDeckList(Cards card)
    {
        GameObject instantiatedCard = Instantiate(CardPrefab);
        instantiatedCard.name = card.name;
        GetPlayer().deckList.Add(instantiatedCard);
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>()._id = card._id;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().name = card.name;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().strength = card.strength;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().actualStrength = card.strength;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().row = card.row;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().unique = card.unique;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().ability = card.ability;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().isMoraleBoosted = false;
        instantiatedCard.transform.Find("Stats").GetComponent<CardStats>().faction = card.faction;


        if (card.faction != "Special")
            instantiatedCard.GetComponent<CardTranslate>().typeOfCard = (Slot)System.Enum.Parse(typeof(Slot), card.row);
        else if (card.ability == "double_strenght") instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.horn;
        else if (card.ability == "kill_strongest") instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.scorch;
        else if (card.row == "weather")
        {
            if (card.ability.Contains("close")) instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.close;
            else if (card.ability.Contains("range")) instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.range;
            else if (card.ability.Contains("siege")) instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.siege;
            else if (card.ability.Contains("clear")) instantiatedCard.GetComponent<CardTranslate>().typeOfCard = Slot.scorch;
        }
        
        instantiatedCard.SetActive(false);
    }

    public void ShowDiscardList()
    {
        if(loadPanel.activeInHierarchy==false)
        loadPanel.GetComponentInChildren<LoadPanelList>().Check();
    }


    IEnumerator PanelChange()
    {
        changePanel.SetActive(true);

        yield return new WaitForSecondsRealtime(1.2f);

        changePanel.SetActive(false);
    }
    
    //UI- ki jon, ki nyert stb
    public void RoundPanelChange(string newText)
    {
        changePanel.GetComponentInChildren<TextMeshProUGUI>().text = newText;
        StartCoroutine(PanelChange());
    }

    //---------------------------------------------------------------Pass, EndRound, EndMatch----------------------------------------------

    public void ResetMatch()
    {
        
        ClearBoard();

        //emhyr 34 leader
        if (PlayerInfo.leader.transform.Find("Art").GetComponent<Image>().sprite.name!="34" && EnemyInfo.leader.transform.Find("Art").GetComponent<Image>().sprite.name!="34")
        {
            PlayerInfo.leader.transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_unused");
            PlayerInfo.leader.transform.Find("Trigger").GetComponent<Button>().enabled = true;

            EnemyInfo.leader.transform.Find("Trigger").GetComponent<Image>().sprite = Resources.Load<Sprite>("Field/leader_unused");
            EnemyInfo.leader.transform.Find("Trigger").GetComponent<Button>().enabled = true;   
        }

        //elet reset
        PlayerInfo.lifePoint = 2;
        EnemyInfo.lifePoint = 2;
        LifePointChange(PlayerInfo);
        LifePointChange(EnemyInfo);

        battleState = BattleState.PlayerTurn;
        foreach (GameObject card in PlayerInfo.discardList)
        {
            PlayerInfo.deckList.Add(card);
        }
        PlayerInfo.discardList.Clear();
        foreach (GameObject card in PlayerInfo.handList)
        {
            PlayerInfo.deckList.Add(card);
        }
        PlayerInfo.handList.Clear();

        while(PlayerInfo.myHand.transform.GetChild(0) != null)
        {
            Debug.Log(PlayerInfo.myHand.transform.GetChild(0).name);
            PlayerInfo.myHand.transform.GetChild(0).gameObject.SetActive(false);
            PlayerInfo.myHand.transform.GetChild(0).transform.SetParent(this.transform);
            if (PlayerInfo.myHand.transform.childCount == 0) break;
        }

        SpawnTenRandomCard();
        SortCards(GetPlayer().handList);

        battleState = BattleState.EnemyTurn;
        foreach (GameObject card in EnemyInfo.discardList)
        {
            EnemyInfo.deckList.Add(card);
        }
        EnemyInfo.discardList.Clear();
        foreach (GameObject card in EnemyInfo.handList)
        {
            EnemyInfo.deckList.Add(card);
        }
        EnemyInfo.handList.Clear();

        while (EnemyInfo.myHand.transform.GetChild(0) != null)
        {
            EnemyInfo.myHand.transform.GetChild(0).gameObject.SetActive(false);
            EnemyInfo.myHand.transform.GetChild(0).transform.SetParent(this.transform);
            if (EnemyInfo.myHand.transform.childCount == 0) break;
        }

        

        SpawnTenRandomCard();
        SortCards(GetPlayer().handList);

        loadPanelCount = 0;
        battleState = BattleState.Pre_Player;

        //score update
        Player_Field.transform.GetChild(0).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0";
        Player_Field.transform.GetChild(1).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0";
        Player_Field.transform.GetChild(2).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0"; 


        Enemy_Field.transform.GetChild(0).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0"; 
        Enemy_Field.transform.GetChild(1).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0"; 
        Enemy_Field.transform.GetChild(2).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = "0"; 

        PlayerInfo.scoreUI.transform.Find("Cards").GetComponentInChildren<TextMeshProUGUI>().text = PlayerInfo.handList.Count.ToString();
        EnemyInfo.scoreUI.transform.Find("Cards").GetComponentInChildren<TextMeshProUGUI>().text = EnemyInfo.handList.Count.ToString();

        PlayerInfo.scoreUI.transform.Find("Big_score").GetComponentInChildren<TextMeshProUGUI>().text = totalStrenght(PlayerInfo).ToString();
        EnemyInfo.scoreUI.transform.Find("Big_score").GetComponentInChildren<TextMeshProUGUI>().text = totalStrenght(EnemyInfo).ToString();

        //a scoiatelek!! update: jokes on you nem csinalom meg
        
        loadPanel.GetComponentInChildren<LoadPanelList>().AddCardsToContent();

    }

    public void EndMatch()
    {
        if(PlayerInfo.lifePoint==0 && EnemyInfo.lifePoint==0)
        {
            Debug.Log("dontetlen");
            ResetMatch();
        }else if (PlayerInfo.lifePoint == 0)
        {
            Debug.Log("ENEMY nyert");
            pausePanel.GetComponent<MenuButtons>().endingButtons.transform.Find("EndText").GetComponent<TextMeshProUGUI>().text = "The Enemy Won The Match";
            pausePanel.GetComponent<MenuButtons>().menuButtons.SetActive(false);
            pausePanel.GetComponent<MenuButtons>().endingButtons.SetActive(true);
            pausePanel.SetActive(true);
            //changePanel.GetComponentInChildren<TextMeshProUGUI>().text = "The Enemy Won The Match";
            //changePanel.SetActive(true);
        }
        else if (EnemyInfo.lifePoint == 0)
        {
            Debug.Log("Player nyert");
            
            pausePanel.GetComponent<MenuButtons>().endingButtons.transform.Find("EndText").GetComponent<TextMeshProUGUI>().text = "The Player Won The Match";
            pausePanel.GetComponent<MenuButtons>().menuButtons.SetActive(false);
            pausePanel.GetComponent<MenuButtons>().endingButtons.SetActive(true);
            pausePanel.SetActive(true);
            //changePanel.GetComponentInChildren<TextMeshProUGUI>().text = "The Player Won The Match";
            //changePanel.SetActive(true);
        }

    }

    public void ClearBoard()
    {
        foreach (GameObject card in PlayerInfo.closeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            PlayerInfo.discardList.Add(card);
            card.SetActive(false);
        }
        PlayerInfo.closeList.Clear();
        foreach (GameObject card in PlayerInfo.rangeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            PlayerInfo.discardList.Add(card);
            card.SetActive(false);
        }
        PlayerInfo.rangeList.Clear();
        foreach (GameObject card in PlayerInfo.siegeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            PlayerInfo.discardList.Add(card);
            card.SetActive(false);
        }
        PlayerInfo.siegeList.Clear();

        foreach (GameObject card in EnemyInfo.closeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            EnemyInfo.discardList.Add(card);
            card.SetActive(false);
        }
        EnemyInfo.closeList.Clear();
        foreach (GameObject card in EnemyInfo.rangeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            EnemyInfo.discardList.Add(card);
            card.SetActive(false);
        }
        EnemyInfo.rangeList.Clear();
        foreach (GameObject card in EnemyInfo.siegeList)
        {
            card.transform.SetParent(this.gameObject.transform);
            card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
            card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            EnemyInfo.discardList.Add(card);
            card.SetActive(false);
        }
        EnemyInfo.siegeList.Clear();

        SetStrenghtToNormal(PlayerInfo.closeList);
        SetStrenghtToNormal(PlayerInfo.rangeList);
        SetStrenghtToNormal(PlayerInfo.siegeList);
        SetStrenghtToNormal(EnemyInfo.closeList);
        SetStrenghtToNormal(EnemyInfo.rangeList);
        SetStrenghtToNormal(EnemyInfo.siegeList);

        weatherList.Clear();

        foreach (Transform card in weatherHolder.transform)
        {
            card.gameObject.SetActive(false);
        }

        player_close.GetComponent<RowClick>().isHornActive = false;
        player_range.GetComponent<RowClick>().isHornActive = false;
        player_siege.GetComponent<RowClick>().isHornActive = false;
        enemy_close.GetComponent<RowClick>().isHornActive = false;
        enemy_range.GetComponent<RowClick>().isHornActive = false;
        enemy_siege.GetComponent<RowClick>().isHornActive = false;

    }

    public void LifePointChange(MainInfo info)
    {
        if (info.lifePoint != 2)
        {
            if (info.lifePoint == 1)
            {
                info.scoreUI.transform.Find("RRuby").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/ruby_grey");
            }
            else
            {
                info.scoreUI.transform.Find("LRuby").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/ruby_grey");
            }

        }else
        {
            info.scoreUI.transform.Find("RRuby").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/ruby_red");
            info.scoreUI.transform.Find("LRuby").GetComponent<Image>().sprite = Resources.Load<Sprite>("Stats/ruby_red");
        }
    }

    public void EndRound()
    {
        if (totalStrenght(PlayerInfo) > totalStrenght(EnemyInfo))
        {
            Debug.Log("a gyoztes a Player");
            RoundPanelChange("The Player Won The Round");
            battleState = BattleState.PlayerTurn;
            //ha N akkor kap egy uj kartyat
            if (GetPlayer().card_faction == "NR")
            {
                int rnd = Random.Range(0, GetPlayer().deckList.Count);
                GameObject card = GetPlayer().deckList[rnd];
                card.SetActive(true);
                card.transform.SetParent(GetPlayer().myHand.transform);
                card.transform.localScale = new Vector3(2f, 2f, 2f);
                GetPlayer().handList.Add(card);
                GetPlayer().deckList.Remove(card);
                GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
                SortCards(GetPlayer().handList);
            }
            EnemyInfo.myHand.SetActive(false);
            PlayerInfo.myHand.SetActive(true);
            EnemyInfo.lifePoint--;
            LifePointChange(EnemyInfo);
        }
        else if (totalStrenght(PlayerInfo) == totalStrenght(EnemyInfo))
        {
            Debug.Log("DONTETLEN");
            //meg kell nezni hogy nilfggardi, majd ha scoiatel mert akkor o kezdi
            //a handeket is el kell rendezni

            PlayerInfo.lifePoint--;
            EnemyInfo.lifePoint--;

            if (PlayerInfo.card_faction == "NF")
            {
                PlayerInfo.lifePoint++;
                RoundPanelChange("The Player Won The Round");
            }
            if (EnemyInfo.card_faction == "NF")
            {
                EnemyInfo.lifePoint++;
                RoundPanelChange("The Enemy Won The Round");
            }
            if(PlayerInfo.card_faction != "NF"&&EnemyInfo.card_faction != "NF")
            RoundPanelChange("Draw");

            LifePointChange(PlayerInfo);
            LifePointChange(EnemyInfo);
        }
        else
        {
            Debug.Log("a gyoztes a ENEMY");
            RoundPanelChange("The Enemy Won The Round");
            battleState = BattleState.EnemyTurn;
            //ha N akkor kap egy uj kartyat
            if (GetPlayer().card_faction == "NR")
            {
                int rnd = Random.Range(0, GetPlayer().deckList.Count);
                GameObject card = GetPlayer().deckList[rnd];
                card.SetActive(true);
                card.transform.SetParent(GetPlayer().myHand.transform);
                card.transform.localScale = new Vector3(2f, 2f, 2f);
                GetPlayer().handList.Add(card);
                GetPlayer().deckList.Remove(card);
                GetPlayer().myHand.GetComponent<HandSize>().ResizeHand();
                SortCards(GetPlayer().handList);
            }
            EnemyInfo.myHand.SetActive(true);
            PlayerInfo.myHand.SetActive(false);
            PlayerInfo.lifePoint--;
            LifePointChange(PlayerInfo);
        }

        //jo helyre teszi a fieldeket, nincs lekezelve az ha dontetlen!!!
        if((battleState.ToString()=="PlayerTurn" && Player_Field.transform.localPosition.y > Enemy_Field.transform.localPosition.y) || (battleState.ToString() == "EnemyTurn" && Player_Field.transform.localPosition.y < Enemy_Field.transform.localPosition.y))
        {
            Vector2 onePos = new Vector2();
            onePos = Player_Field.transform.localPosition;
            Player_Field.transform.localPosition = Enemy_Field.transform.localPosition;
            Enemy_Field.transform.localPosition = onePos;

            //siege close csere
            onePos = Player_Field.transform.GetChild(0).transform.position;
            Player_Field.transform.GetChild(0).transform.position = Player_Field.transform.GetChild(2).transform.position;
            Player_Field.transform.GetChild(2).transform.position = onePos;

            onePos = Enemy_Field.transform.GetChild(0).transform.position;
            Enemy_Field.transform.GetChild(0).transform.position = Enemy_Field.transform.GetChild(2).transform.position;
            Enemy_Field.transform.GetChild(2).transform.position = onePos;

            onePos = PlayerInfo.scoreUI.transform.position;
            PlayerInfo.scoreUI.transform.position = EnemyInfo.scoreUI.transform.position;
            EnemyInfo.scoreUI.transform.position = onePos;

            //leader
            onePos = PlayerInfo.leader.transform.position;
            PlayerInfo.leader.transform.position = EnemyInfo.leader.transform.position;
            EnemyInfo.leader.transform.position = onePos;

            //deckUI
            onePos = PlayerInfo.deckHolder.transform.position;
            PlayerInfo.deckHolder.transform.position = EnemyInfo.deckHolder.transform.position;
            EnemyInfo.deckHolder.transform.position = onePos;

        }
        
        
        //kell ha monster maradjon egy kartya update: unom megcsinalni

        ClearBoard();
        ChangeUI();
        isPassed = false;
        EndMatch();

        if (GetPlayer() == EnemyInfo && PlayerInfo.leader.transform.Find("Trigger").gameObject.activeInHierarchy == true)
        {
            PlayerInfo.leader.transform.Find("Trigger").gameObject.SetActive(false);
            EnemyInfo.leader.transform.Find("Trigger").gameObject.SetActive(true);
        }
        else if(GetPlayer() == PlayerInfo && EnemyInfo.leader.transform.Find("Trigger").gameObject.activeInHierarchy == true)
        {
            PlayerInfo.leader.transform.Find("Trigger").gameObject.SetActive(true);
            EnemyInfo.leader.transform.Find("Trigger").gameObject.SetActive(false);
        }

    }

    void FlashPassed()
    {
        
        GameObject flashing_Label;
        if (battleState.ToString() == "PlayerTurn")
        {
            flashing_Label = EnemyInfo.scoreUI.transform.Find("Passed").gameObject;
        }
        else
        {
            flashing_Label = PlayerInfo.scoreUI.transform.Find("Passed").gameObject;
        }
        if (flashing_Label.activeSelf) flashing_Label.SetActive(false);
        else flashing_Label.SetActive(true);
        
    }

    public void Pass()
    {
        InvokeRepeating("FlashPassed", 0, .7f);
        ChangePlayer();
        ChangeUI();
        if(isPassed)
        {
            EndRound();
            CancelInvoke("FlashPassed");
            EnemyInfo.scoreUI.transform.Find("Passed").gameObject.SetActive(false);
            PlayerInfo.scoreUI.transform.Find("Passed").gameObject.SetActive(false);
        }
        else
        isPassed = true;
    }

    void DeckChange()
    {

        if(PlayerInfo.deckList.Count==0)
        {
           // PlayerInfo.deckHolder.GetComponent<Button>().enabled = false;
        }
        if (EnemyInfo.deckList.Count == 0)
        {
            //EnemyInfo.deckHolder.GetComponent<Button>().enabled = false;
        }

        int a = PlayerInfo.deckHolder.transform.childCount - PlayerInfo.deckList.Count - 1;
        PlayerInfo.deckHolder.GetComponentInChildren<TextMeshProUGUI>().text = PlayerInfo.deckList.Count.ToString();
        for (int i = 0; i < a; i++)
        {
            Destroy(PlayerInfo.deckHolder.transform.GetChild(i).gameObject);
        }

        int b = EnemyInfo.deckHolder.transform.childCount - EnemyInfo.deckList.Count - 1;
        EnemyInfo.deckHolder.GetComponentInChildren<TextMeshProUGUI>().text = EnemyInfo.deckList.Count.ToString();
        for (int i = 0; i < b; i++)
        {
            Destroy(EnemyInfo.deckHolder.transform.GetChild(i).gameObject);
        }

    }

    ///change
    public void ChangeUI()
    {
        if(!isPassed)
        {
            //jatekos palya csere
            Vector2 onePos = new Vector2();
            onePos = Player_Field.transform.localPosition;
            Player_Field.transform.localPosition = Enemy_Field.transform.localPosition;
            Enemy_Field.transform.localPosition = onePos;

            //siege close csere
            onePos = Player_Field.transform.GetChild(0).transform.position;
            Player_Field.transform.GetChild(0).transform.position = Player_Field.transform.GetChild(2).transform.position;
            Player_Field.transform.GetChild(2).transform.position = onePos;

            onePos = Enemy_Field.transform.GetChild(0).transform.position;
            Enemy_Field.transform.GetChild(0).transform.position = Enemy_Field.transform.GetChild(2).transform.position;
            Enemy_Field.transform.GetChild(2).transform.position = onePos;

            //scoreUI
            onePos = PlayerInfo.scoreUI.transform.position;
            PlayerInfo.scoreUI.transform.position = EnemyInfo.scoreUI.transform.position; 
            EnemyInfo.scoreUI.transform.position = onePos;

            //deckUI
            onePos = PlayerInfo.deckHolder.transform.position;
            PlayerInfo.deckHolder.transform.position = EnemyInfo.deckHolder.transform.position;
            EnemyInfo.deckHolder.transform.position = onePos;
            PlayerInfo.deckHolder.GetComponent<Button>().interactable = !PlayerInfo.deckHolder.GetComponent<Button>().interactable;
            EnemyInfo.deckHolder.GetComponent<Button>().interactable = !EnemyInfo.deckHolder.GetComponent<Button>().interactable;


            //leader
            onePos = PlayerInfo.leader.transform.position;
            PlayerInfo.leader.transform.position = EnemyInfo.leader.transform.position;
            EnemyInfo.leader.transform.position = onePos;
            if(PlayerInfo.leader.transform.Find("Trigger").gameObject.activeInHierarchy == true)
            {
               PlayerInfo.leader.transform.Find("Trigger").gameObject.SetActive(false);
               EnemyInfo.leader.transform.Find("Trigger").gameObject.SetActive(true);
            }else
            {
                PlayerInfo.leader.transform.Find("Trigger").gameObject.SetActive(true);
                EnemyInfo.leader.transform.Find("Trigger").gameObject.SetActive(false);
            }


        }


        //score update
        Player_Field.transform.GetChild(0).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(PlayerInfo.siegeList).ToString();
        Player_Field.transform.GetChild(1).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(PlayerInfo.rangeList).ToString();
        Player_Field.transform.GetChild(2).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(PlayerInfo.closeList).ToString();


        Enemy_Field.transform.GetChild(0).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(EnemyInfo.siegeList).ToString();
        Enemy_Field.transform.GetChild(1).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(EnemyInfo.rangeList).ToString();
        Enemy_Field.transform.GetChild(2).transform.Find("Score").GetComponentInChildren<TextMeshProUGUI>().text = rowStrength(EnemyInfo.closeList).ToString();

        PlayerInfo.scoreUI.transform.Find("Cards").GetComponentInChildren<TextMeshProUGUI>().text = PlayerInfo.handList.Count.ToString();
        EnemyInfo.scoreUI.transform.Find("Cards").GetComponentInChildren<TextMeshProUGUI>().text = EnemyInfo.handList.Count.ToString();

        PlayerInfo.scoreUI.transform.Find("Big_score").GetComponentInChildren<TextMeshProUGUI>().text = totalStrenght(PlayerInfo).ToString();
        EnemyInfo.scoreUI.transform.Find("Big_score").GetComponentInChildren<TextMeshProUGUI>().text = totalStrenght(EnemyInfo).ToString();

        DeckChange();

        
    }

    //turn
    public void AllCardsRaycast()
    {
        foreach (GameObject card in GetPlayer().handList)
        {
            card.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }


        if(GetPlayer().myHand.transform.childCount>0)
        {
            int db = Random.Range(0, GetPlayer().myHand.transform.childCount);
            int db1 = Random.Range(0, GetPlayer().myHand.transform.childCount);
            while (GetPlayer().myHand.transform.GetChild(db).transform.position.y != GetPlayer().myHand.transform.GetChild(db1).transform.position.y)
            {
                db = Random.Range(0, GetPlayer().myHand.transform.childCount);
                db1 = Random.Range(0, GetPlayer().myHand.transform.childCount);
            }

            foreach (Transform item in GetPlayer().myHand.transform)
            {
                if (item.transform.position.y != GetPlayer().myHand.transform.GetChild(db).transform.position.y) item.transform.position = new Vector2(item.transform.position.x, GetPlayer().myHand.transform.GetChild(db).transform.position.y);
            
            }

        }


    }

    public void ChangePlayer()
    {
        DeckChange();

        Debug.Log("most belep hogy jatekost valtson");
        if (!isPassed)
        {

            if (battleState.ToString() == "PlayerTurn")
            {
                PlayerInfo.myHand.SetActive(false);
                EnemyInfo.myHand.SetActive(true);
                battleState = BattleState.EnemyTurn;
                Debug.Log("most allit");
                RoundPanelChange("Enemy Turn");
            }
            else if (battleState.ToString() == "EnemyTurn")
            {
                EnemyInfo.myHand.SetActive(false);
                PlayerInfo.myHand.SetActive(true);
                battleState = BattleState.PlayerTurn;
                Debug.Log("most ENEMYROL PLAYERRE allit");
                RoundPanelChange("Player Turn");
            }

        }
        else Debug.Log("MARAD MERT PASSZOLT!!!");
    }

    //----------------------------------------------------------------- Score ------------------------------------------------------------

    //actual strenght allitas
    public void UpdateRowStrenghtImage(List<GameObject> list)
    {
        foreach (GameObject card in list)
        {
            
            if (card.GetComponentInChildren<CardStats>().unique == true || card.GetComponentInChildren<CardStats>().faction == "Special") Debug.Log("aight imma head out");
            else
            {
                Debug.Log(card.name);
                if (card.GetComponentInChildren<CardStats>().actualStrength > card.GetComponentInChildren<CardStats>().strength)
                {
                    card.GetComponent<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().color = new Color32(50, 173, 98, 200);
                } else if (card.GetComponentInChildren<CardStats>().actualStrength < card.GetComponentInChildren<CardStats>().strength)
                {
                    card.GetComponent<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().color = new Color32(238, 48, 92, 255);
                }
                else if (card.GetComponentInChildren<CardStats>().actualStrength == card.GetComponentInChildren<CardStats>().strength)
                {
                    card.GetComponentInChildren<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().color = new Color32(0, 0, 0, 255);
                }
                card.GetComponent<CardDisplayer>().strengthImage.GetComponent<TextMeshProUGUI>().text = card.GetComponentInChildren<CardStats>().actualStrength.ToString();
            }

        }
    }

    //vissza teriti a sor act.str. jet
    int rowStrength(List<GameObject> list)
    {
        int Strenght = 0;
        int db = 0;
        foreach (GameObject r in list)
        {
            db++;
            Strenght += r.GetComponentInChildren<CardStats>().actualStrength;
        }
        return Strenght;
    }

    //3 list erosseg
    int totalStrenght(MainInfo info)
    {
        int TotalStrenght = 0;
        TotalStrenght = rowStrength(info.closeList) + rowStrength(info.rangeList) + rowStrength(info.siegeList);
        return TotalStrenght;
    }

    //----------------------------------------------------------------- HighLight ------------------------------------------------------------


    //highlight rows
    public void HighlightRows(GameObject card)
    {
        isCardSelected = true;
        cardSelected = card;
        Debug.LogWarning("A kartya amit highlightol " + card.name);
        ///spriteok 
        ///

        if (battleState.ToString() == "PlayerTurn")
        {
            if (card.GetComponentInChildren<CardStats>().ability == "spy")
            {
                // enemy_close.GetComponent<Image>().sprite = closeHighlight;
                if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close_range")
                {
                    Debug.Log(player_close.name);
                    enemy_close.GetComponent<Image>().sprite = closeHighlight;
                    enemy_range.GetComponent<Image>().sprite = rangeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close")
                {
                    enemy_close.GetComponent<Image>().sprite = closeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "range")
                {
                    enemy_range.GetComponent<Image>().sprite = rangeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "siege")
                {
                    enemy_siege.GetComponent<Image>().sprite = siegeHighlight;

                }
            }
            else
        if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close_range")
            {
                //Debug.Log(player_close.name);
                player_close.GetComponent<Image>().sprite = closeHighlight;
                player_range.GetComponent<Image>().sprite = rangeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close")
            {
                player_close.GetComponent<Image>().sprite = closeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "range")
            {
                player_range.GetComponent<Image>().sprite = rangeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "siege")
            {
                player_siege.GetComponent<Image>().sprite = siegeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "horn")
            {
                foreach (GameObject horn in player_horns)
                {
                    if(horn.transform.childCount==0)
                    horn.GetComponent<Image>().sprite = hornHighlight;
                }
            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "scorch")
            {
                player_close.GetComponent<Image>().sprite = closeHighlight;
                player_range.GetComponent<Image>().sprite = rangeHighlight;
                player_siege.GetComponent<Image>().sprite = siegeHighlight;
            }
        }
        else if (battleState.ToString() == "EnemyTurn")
        {
            if (card.GetComponentInChildren<CardStats>().ability == "spy")
            {
                // enemy_close.GetComponent<Image>().sprite = closeHighlight;
                if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close_range")
                {
                    Debug.Log(player_close.name);
                    player_close.GetComponent<Image>().sprite = closeHighlight;
                    player_range.GetComponent<Image>().sprite = rangeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close")
                {
                    player_close.GetComponent<Image>().sprite = closeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "range")
                {
                    player_range.GetComponent<Image>().sprite = rangeHighlight;

                }
                else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "siege")
                {
                    player_siege.GetComponent<Image>().sprite = siegeHighlight;

                }
            }
            else
            {
            Debug.LogError("igen elemnt "+ card.GetComponent<CardTranslate>().typeOfCard);
            if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close_range")
            {
                //Debug.Log(player_close.name);
                enemy_close.GetComponent<Image>().sprite = closeHighlight;
                enemy_range.GetComponent<Image>().sprite = rangeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "close")
            {
                enemy_close.GetComponent<Image>().sprite = closeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "range")
            {
                enemy_range.GetComponent<Image>().sprite = rangeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "siege")
            {
                enemy_siege.GetComponent<Image>().sprite = siegeHighlight;

            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "horn")
            {
                foreach (GameObject horn in enemy_horns)
                {
                    if (horn.transform.childCount == 0)
                        horn.GetComponent<Image>().sprite = hornHighlight;
                }
            }
            else if (card.GetComponent<CardTranslate>().typeOfCard.ToString() == "scorch")
            {
                enemy_close.GetComponent<Image>().sprite = closeHighlight;
                enemy_range.GetComponent<Image>().sprite = rangeHighlight;
                enemy_siege.GetComponent<Image>().sprite = siegeHighlight;
            }
            }
        }

    }

    public void TurnOffHighlight(GameObject card)
    {
        player_close.GetComponent<Image>().sprite = closeNormal;
        player_range.GetComponent<Image>().sprite = rangeNormal;
        player_siege.GetComponent<Image>().sprite = siegeNormal;

        enemy_close.GetComponent<Image>().sprite = closeNormal;
        enemy_range.GetComponent<Image>().sprite = rangeNormal;
        enemy_siege.GetComponent<Image>().sprite = siegeNormal;
        foreach (GameObject horn in player_horns)
        {
            horn.GetComponent<Image>().sprite = hornNormal;
        }
        foreach (GameObject horn in enemy_horns)
        {
            horn.GetComponent<Image>().sprite = hornNormal;
        }

        isCardSelected = false;
        cardSelected = null;
    }


    //----------------------------------------------------------------- Ability ------------------------------------------------------------

    

    //horn, elvileg nincs bug?
    public void DoubleRow(List<GameObject> list)
    {

        if (list.Count > 1)
        {

            Debug.Log(list[0].name);


            //dandelion duplazas ha azt mondod hogy prepare for trouble and make it double (not smart)
            if (list[0].transform.parent.GetComponent<RowClick>().isHornActive)
            {
                foreach (GameObject card in list)
                {
                    Debug.Log("neve> " + card.name + " " + card.GetComponentInChildren<CardStats>().actualStrength);
                    if (card.GetComponentInChildren<CardStats>().ability == "commander_horn")
                    {
                        card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }

                }
            }else
            {
                Debug.LogWarning("igen belemegy");
                list[0].transform.parent.GetComponent<RowClick>().isHornActive = true;
                foreach (GameObject card in list)
                {
                    if (card.GetComponentInChildren<CardStats>().unique == false && card.GetComponentInChildren<CardStats>().faction != "Special")
                        card.GetComponentInChildren<CardStats>().actualStrength *= 2;
           
                }
            }

        }
    }

    //weather, elvileg nincs bug
    public void SetStrenghtToWeather(List<GameObject> list)
    {
        List<GameObject> listOfTight = new List<GameObject>();
        int db = 0;
        int db1 = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "tight_bond")
            {
                listOfTight.Add(card);
                db++;
                foreach (GameObject card1 in list)
                {
                    if (card.GetComponentInChildren<CardStats>()._id == card1.GetComponentInChildren<CardStats>()._id && card != card1)
                    {
                        listOfTight.Add(card1);
                        db++;
                    }
                }
                foreach (GameObject card1 in listOfTight)
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = db;
                }
                db = 0;
                listOfTight.Clear();
            } else if (card.GetComponentInChildren<CardStats>().unique == false)
                card.GetComponentInChildren<CardStats>().actualStrength = 1;
        }
        db1 = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
            {
                db1++;
            }
        }
        if (db1 > 0)
        {
            foreach (GameObject card1 in list)
            {
                if (card1.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1 - 1;
                }
                else if (card1.GetComponentInChildren<CardStats>().unique == false) card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1;
            }
        }

        if (list.Count > 0)
        {
            if (list[0].transform.parent.GetComponent<RowClick>().isHornActive)
            {
                foreach (GameObject card in list)
                {
                    Debug.Log("neve> " + card.name + " " + card.GetComponentInChildren<CardStats>().actualStrength);
                    if (card.GetComponentInChildren<CardStats>().unique == false && card.GetComponentInChildren<CardStats>().faction != "Special" && card.GetComponentInChildren<CardStats>().ability!="commander_horn")
                    {
                        card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }

                }
            }

        }

        UpdateRowStrenghtImage(list);
    }

    public void SetStrenghtToNormal(List<GameObject> list)
    {
        List<GameObject> listOfTight = new List<GameObject>();
        int db = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "tight_bond")
            {
                listOfTight.Add(card);
                db++;
                foreach (GameObject card1 in list)
                {
                    if (card.GetComponentInChildren<CardStats>()._id == card1.GetComponentInChildren<CardStats>()._id && card != card1)
                    {
                        listOfTight.Add(card1);
                        db++;
                    }
                }
                foreach (GameObject card1 in listOfTight)
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = db * card1.GetComponentInChildren<CardStats>().strength;
                }
                db = 0;
                listOfTight.Clear();
            }
            else
                card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
        }

        int db1 = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
            {
                db1++;
            }
        }
        if (db1 > 0)
        {
            foreach (GameObject card1 in list)
            {
                if (card1.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1 - 1;
                }
                else if (card1.GetComponentInChildren<CardStats>().unique == false && card1.GetComponentInChildren<CardStats>().faction != "Special") card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1;
            }
        }
        if (list.Count > 0)
        {
            if (list[0].transform.parent.GetComponent<RowClick>().isHornActive)
            {
                foreach (GameObject card in list)
                {
                        Debug.Log("neve> " + card.name + " " + card.GetComponentInChildren<CardStats>().actualStrength);
                    if (card.GetComponentInChildren<CardStats>().unique == false && card.GetComponentInChildren<CardStats>().faction != "Special" && card.GetComponentInChildren<CardStats>().ability != "commander_horn")
                    {
                        card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }

                }
            }

        }

        UpdateRowStrenghtImage(list);

    }

    ///decoy, elvileg nincs bug
    public void Decoy(List<GameObject> list)
    {
        List<GameObject> listOfTight = new List<GameObject>();
        int db = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "tight_bond")
            {
                listOfTight.Add(card);
                db++;
                foreach (GameObject card1 in list)
                {
                    if (card.GetComponentInChildren<CardStats>()._id == card1.GetComponentInChildren<CardStats>()._id && card != card1)
                    {
                        listOfTight.Add(card1);
                        db++;
                    }
                }
                foreach (GameObject card1 in listOfTight)
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = db * card1.GetComponentInChildren<CardStats>().strength;
                }
                db = 0;
                listOfTight.Clear();
            }
            else
                card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
        }

        int db1 = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
            {
                db1++;
            }
        }
        if (db1 > 0)
        {
            foreach (GameObject card1 in list)
            {
                if (card1.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1 - 1;
                }
                else if (card1.GetComponentInChildren<CardStats>().unique == false && card1.GetComponentInChildren<CardStats>().faction != "Special") card1.GetComponentInChildren<CardStats>().actualStrength = card1.GetComponentInChildren<CardStats>().actualStrength + db1;
            }
        }
        if (list.Count > 0)
        {
            if (list[0].gameObject.GetComponentInChildren<CardStats>()._id != 202)
            {
                if (list[0].transform.parent.GetComponent<RowClick>().isHornActive)
                {
                    foreach (GameObject card in list)
                    {
                        if (card.GetComponentInChildren<CardStats>().unique == false && card.GetComponentInChildren<CardStats>().faction != "Special") card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }
                }

            }
            else
            {
                //Debug.LogWarning("|| " + list.Count + "|| " + list[1].transform.parent.name);
                if (list.Count > 1 && list[1].transform.parent.parent.GetComponentInChildren<RowClick>().isHornActive )
                {
                    foreach (GameObject card in list)
                    {
                        if (card.GetComponentInChildren<CardStats>().unique == false && card.GetComponentInChildren<CardStats>().faction != "Special") card.GetComponentInChildren<CardStats>().actualStrength *= 2;
                    }
                }
            }

        }
        UpdateRowStrenghtImage(list);



    }

    //medic, elvileg nincs bug
    public void Medic()
    {
        Debug.Log(GetPlayer().discardList.Count);
        //double check, csak a biztonsag kedveert
        if (GetPlayer().discardList.Count != 0)
        {
            if (emhyrMedic == true)
            {
                List<GameObject> seged = new List<GameObject>();
                isMedicActive = true;
                foreach (GameObject c in GetPlayer().discardList)
                {
                    if (!c.GetComponentInChildren<CardStats>().unique)
                    {
                        seged.Add(c);
                    }
                }


                if (seged.Count>0)
                {
                    int rnd = Random.Range(0,seged.Count);
                    GameObject card = seged[rnd];
                    Debug.LogWarning(card + " " + seged.Count);
                    card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength;
                    card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;

                    TurnOffHighlight(card);
                    HighlightRows(card);
                    card.SetActive(true);
                    art.GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/List/591x380/" + card.GetComponentInChildren<CardStats>()._id);
                    art.SetActive(true);
                    GetPlayer().discardList.Remove(card);
                    cardSelected = card;
                }

            }
            else
            {
                isMedicActive = true;
                TurnOffHighlight(cardSelected);
                loadPanel.GetComponentInChildren<LoadPanelList>().ShowDiscard();
            }

        }
    }

    ///tight bond, elvileg nincs bug
    public void UpdateTightBond(List<GameObject> list, int a, int originalStrenght, bool isMorale)
    {
        float db = 0;
        foreach (GameObject card in list)
        {
            if (card.GetComponentInChildren<CardStats>()._id == a)
            {
                db++;
            }
        }
        if (db >= 1)
        {
            foreach (GameObject card in list)
            {
                if (card.GetComponentInChildren<CardStats>()._id == a)
                {
                    card.GetComponentInChildren<CardStats>().actualStrength = (int)db * originalStrenght;
                    card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
                }
            }
        }
        if (isMorale)
        {
            int db1 = 0;
            foreach (GameObject card in list)
            {
                if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    db1++;
                }
            }
            foreach (GameObject card in list)
            {
                if (card.GetComponentInChildren<CardStats>()._id == a)
                {
                    card.GetComponentInChildren<CardStats>().actualStrength = (int)db * originalStrenght + db1;
                    card.GetComponentInChildren<CardStats>().isMoraleBoosted = true;
                }
            }
        }
    }

    //morale, elvileg nincs bug
    public void UpdateMorale(List<GameObject> list, GameObject cardSelected, bool reduce)
    {
        float db = 0;
        if(cardSelected.GetComponentInChildren<CardStats>().ability == "morale_boost")
        {
            foreach (GameObject card in list)
            {
                if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    db++;
                }
                else
                    card.GetComponentInChildren<CardStats>().isMoraleBoosted = false;
            }

        }else foreach (GameObject card in list)
                {
                    if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
                    {
                        db++;
                    }
                }
        Debug.Log("a morales for " + db);

        if (reduce)
        {
            SetStrenghtToNormal(list);

        }
        else
        {
            foreach (GameObject card in list)
            {
                if (!card.GetComponentInChildren<CardStats>().isMoraleBoosted && !card.GetComponentInChildren<CardStats>().unique && card.GetComponentInChildren<CardStats>().faction != "Special")
                {
                    if (card.GetComponentInChildren<CardStats>().ability == "tight_bond" && cardSelected.GetComponentInChildren<CardStats>().ability == "morale_boost")
                    {
                        card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().actualStrength + (int)1;
                    }
                    else
                    {
                        card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength + (int)db;
                    }
                    card.GetComponentInChildren<CardStats>().isMoraleBoosted = true;
                }
                else if (card.GetComponentInChildren<CardStats>().ability == "morale_boost")
                {
                    card.GetComponentInChildren<CardStats>().actualStrength = card.GetComponentInChildren<CardStats>().strength + (int)db - 1;
                }
            }
        }
    }

    //scortch, elvileg nincs bug
    public void DestroyStrongest()
    {
        List<GameObject> listToDestroy = new List<GameObject>();
        int maxStrenght=0;
        foreach (GameObject card in PlayerInfo.closeList)
        {
            if(card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique==false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }
        }
        foreach (GameObject card in PlayerInfo.rangeList)
        {
            if (card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }

        }
        foreach (GameObject card in PlayerInfo.siegeList)
        {
            if (card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }
        }
        foreach (GameObject card in EnemyInfo.closeList)
        {
            if (card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }
        }
        foreach (GameObject card in EnemyInfo.rangeList)
        {
            if (card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }
        }
        foreach (GameObject card in EnemyInfo.siegeList)
        {
            if (card.GetComponentInChildren<CardStats>().actualStrength > maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Clear();
                listToDestroy.Add(card);
                maxStrenght = card.GetComponentInChildren<CardStats>().actualStrength;
            }
            else if (card.GetComponentInChildren<CardStats>().actualStrength == maxStrenght && card.GetComponentInChildren<CardStats>().unique == false)
            {
                listToDestroy.Add(card);
            }
        }

        Debug.Log(listToDestroy.Count);
        if (listToDestroy.Count != 0)
        {
            foreach (GameObject card in listToDestroy)
            {
            
                if(card.transform.parent.parent.parent.name=="PlayerField")
                {
                    switch (card.transform.parent.parent.name)
                    {

                        case "Close":
                        {
                            PlayerInfo.closeList.Remove(card);
                            break;
                        }
                        case "Range":
                            {
                                PlayerInfo.rangeList.Remove(card);
                                break;
                            }
                        case "Siege":
                            {
                                PlayerInfo.siegeList.Remove(card);
                                break;
                            }

                    }
                    if(card.GetComponentInChildren<CardStats>().ability == "morale_boost")
                    {
                        Debug.Log(GetCardListType(card).Count);
                        UpdateMorale(GetCardListType(card), card, true);
                    }
                    PlayerInfo.discardList.Add(card);
                }
                else
                if (card.transform.parent.parent.parent.name == "EnemyField")
                {

                    switch (card.transform.parent.parent.name)
                    {

                        case "Close":
                            {
                                EnemyInfo.closeList.Remove(card);
                                break;
                            }
                        case "Range":
                            {
                                EnemyInfo.rangeList.Remove(card);
                                break;
                            }
                        case "Siege":
                            {
                                EnemyInfo.siegeList.Remove(card);
                                break;
                            }

                    }
                    EnemyInfo.discardList.Add(card);
                }
                card.transform.SetParent(this.transform);
                card.SetActive(false);
                Debug.Log("player discard " + PlayerInfo.discardList.Count);
            }
        }

    }
}
