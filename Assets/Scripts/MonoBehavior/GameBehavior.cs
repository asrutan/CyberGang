using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehavior : MonoBehaviour
{

    private int MAX_COMBATANTS = 8;
    private int MAX_CARDS = 128;
    private int MAX_HANDSLOTS = 3;

    public bool AI_Enabled = false;

    //prefab GameObjects
    public GameObject[] prefabCombatants = new GameObject[1];
    public GameObject[] prefabDecks = new GameObject[1];
    public GameObject[] prefabCards = new GameObject[1];

    //active GameObjects
    private GameObject[] activeCombatants;
    private GameObject[] activeDecks = new GameObject[1];
    private GameObject[] activeCards;
    private GameObject combatantSpawnParent;
    private GameObject[] combatantSpawnSideParents; //Maybe this should just be transform? No, can't do transforms without GameObject.

    //AI Opponent
    private GameObject enemy;

    //Transforms
    private Transform[] handSlotTransforms;
    private Vector3 oldCardPosition;

    //GameObject Scripts
    private CombatantBehavior[] combatantScripts;
    private DeckBehavior[] deckScripts = new DeckBehavior[1];
    private CardBehavior[] cardScripts;
    private CardBehavior selectedCard;
    private ButtonBehavior selectedButton;
    private CardBehavior highlightedCard;
    private CardBehavior playAreaCard;
    private ButtonBehavior highlightedButton;
    private CombatantBehavior selectedCombatant;
    private CombatantBehavior highlightedCombatant;
    private Combatant combatantHolder = new Combatant();

    private EnemyBehavior enemyScript;

    //Flags
    private bool updateMousePos = false;

    //Players
    private Player[] players;

    //Camera
    private new Camera camera = new Camera();

    //Board
    private GameObject board;

    //UI TEXT
    private GameObject turnTextMeshObject0;
    private GameObject turnTextMeshObject1;
    private TextMesh turnText0;
    private TextMesh turnText1;

    //Deck Script (not monobehavior)
    //Deck m_deck = new Deck();
    Deck m_deck;

    //VARS
    [Range(0, 8)] //0 - MAX_COMBATANTS
    public int maxCombatantAmount = 8;
    [Range(0, 3)] //0 - MAX_HANDSLOTS
    public int handAmount = 3;
    public int sidesAmount = 2;

    //Team/Game/Player Vars
    private int amtPlayers = 2;
    private int amtCombatants = 0; //use this to assign combatant IDs.
    private int sideTurn = 0;
    private int turnCount = 0;

    //no use 
    //Make cards children of combatants and do active card identification with GetChild()
    private int cardCount = 0;
    private int nextAvailableIndex = 0;


    // Use this for initialization
    void Start()
    {
        //m_deck = new Deck(20000);
        //Get board
        board = GameObject.Find("Board");

        //Set TextMeshes
        turnTextMeshObject0 = board.transform.GetChild(1).gameObject;
        turnText0 = turnTextMeshObject0.GetComponent<TextMesh>();

        turnTextMeshObject1 = board.transform.GetChild(2).gameObject;
        turnText1 = turnTextMeshObject1.GetComponent<TextMesh>();

        //Get camera
        camera = Camera.main;

        //AI Opponent
        enemy = GameObject.Find("EnemyController");
        enemyScript = enemy.GetComponent<EnemyBehavior>();
        if (enemyScript.GetEnabled())
        {
            AI_Enabled = true;
        }

        //Instantiate Players
        players = new Player[amtPlayers];
        players[0] = new Player("Player 1", 0, 0, false); //Name, side, ID, PlayerControlled
        players[1] = new Player("Player 2", 1, 1, true);

        //Initialize Combatant Side Transform Array
        combatantSpawnSideParents = new GameObject[2];

        //Initialize arrays with max numbers
        activeCombatants = new GameObject[MAX_COMBATANTS];
        activeCards = new GameObject[MAX_CARDS];

        combatantScripts = new CombatantBehavior[MAX_COMBATANTS];
        cardScripts = new CardBehavior[MAX_CARDS];

        //Number of handslots per combatant
        handSlotTransforms = new Transform[MAX_COMBATANTS * MAX_HANDSLOTS];

        //Get Deck information from each combatant.
        //activeDecks[0] = Instantiate(prefabDecks[combatantHolder.getDeckID()]);
        //deckScripts[0] = activeDecks[0].GetComponent<DeckBehavior>();

        //
        //Spawn Combatants
        //
        combatantSpawnParent = GameObject.Find("CombatantSpawns"); //Combatant Spawn
        for (int j = 0; j < sidesAmount; j++)
        {
            combatantSpawnSideParents[j] = combatantSpawnParent.transform.GetChild(j).gameObject; //Side Spawn
        }

        //Spawn Combatants for each player
        for (int i = 0; i < amtPlayers; i++)
        {
            for (int k = 0; k < players[i].GetCombatantAmount(); k++)
            {
                SpawnCombatant(players[i].GetSide(), k);
            }
        }

        //Fill Combatant Hands
        //For each player, call FillHand() of the combatant in the lth spot in each player's combatant array.
        //For instance, the active combatants could look something like [0, 1, 4, 5].
        //for (int k = 0; k < amtCombatants; k++)
        for (int k = 0; k < amtCombatants; k++)
        {
            FillHand(activeCombatants[k]);
        }

        if (AI_Enabled)
        {
            if (!players[sideTurn].GetPlayerControlled())
            {
                enemyScript.IsTurn(true);
            }
            else enemyScript.IsTurn(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            enemyScript.CardMoveTest();
        }

        if (updateMousePos)
        {
            selectedCard.transform.position = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 140)); //Don't use constant for z but it's fucked up.
        }

        Hover(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && !updateMousePos)
        {
            Click(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (selectedButton != null)
            {
                selectedButton.Select(false);
                selectedButton = null;
                EndTurn();
            }
        }

        if (Input.GetMouseButtonUp(0) && updateMousePos)
        {
            updateMousePos = false;
            if (selectedButton != null)
            {
                selectedButton.Select(false);
                selectedButton = null;
                //PlayCard();
            }

            if (selectedCard != null && !selectedCard.GetInPlayArea())
            {
                //selectedCard.transform.localPosition = oldCardPosition;
                selectedCard.Reset();
                //selectedCard.Select(false);
                selectedCard = null;
                //PlayCard();
            }
            else if(selectedCard != null && selectedCard.GetInPlayArea())
            {
                if(playAreaCard != null)
                {
                    playAreaCard.Reset();
                }
                selectedCard.SetAreaParent();
                playAreaCard = selectedCard;
            }
        }
    }

    //Spawn Combatants for player. Player should have amount it wishes to spawn and which side the player is on.
    void SpawnCombatant(int side, int slot)
    {

        //TODO: Should check for not equal numbers of combatants or spawnpoints

        //BUG: SINCE WE ARE FINDING CARD SLOT BY CHILD INDEX, NEW CHILDREN CAUSE ISSUES.

        //using "CombatantSpawns" gameObject children transforms during instantiation
        //Get children of combatSpawns with i to assign each one to a transform
        //https://docs.unity3d.com/ScriptReference/Transform.GetChild.html

        //Spawn combatants based on side.
        //Spawn specific combatants.
        handSlotTransforms[slot] = combatantSpawnSideParents[side].transform.GetChild(slot);
        activeCombatants[amtCombatants] = Instantiate(prefabCombatants[0], handSlotTransforms[slot]);
        combatantScripts[amtCombatants] = activeCombatants[amtCombatants].GetComponent<CombatantBehavior>();
        //combatantScripts[amtCombatants].Init(amtCombatants, "" + amtCombatants, "Combatant Number: " + amtCombatants, side);
        //Spawn combatant using ID from JSON
        combatantScripts[amtCombatants].Init(amtCombatants, 10000, side);
        //Debug.Log(combatantScripts[amtCombatants].GetId());

        //Flip Card Transforms
        if (side == 1)
        {
            Transform cardSlotFlip;
            for (int i = 0; i < MAX_HANDSLOTS; i++)
            {
                cardSlotFlip = activeCombatants[amtCombatants].transform.GetChild(i);
                cardSlotFlip.transform.localPosition = new Vector3(cardSlotFlip.transform.localPosition.x * -1, cardSlotFlip.transform.localPosition.y, cardSlotFlip.transform.localPosition.z);
            }
        }
        amtCombatants++;
    }

    void FillHand(GameObject combatant)
    {
        //For each active combatant, Instantiate a card using their HandSlot children as parents
        for (int j = 0; j <= handAmount; j++)
        {
            DrawCard(combatant.GetComponent<CombatantBehavior>());
        }
    }

    void DrawCard(CombatantBehavior combatant)
    {
        //In cardbehavior
        int slot = combatant.GetNextOpenSlot();
        if (slot != -1)
        {
            combatant.SetOccupiedSlot(slot, true, SpawnCardRandomDeck(combatant.transform.GetChild(slot), slot, combatant.GetSessionId(), combatant.GetDeckLength()));
            //combatant.SetOccupiedSlot(slot, true, SpawnCardRandomDeck(combatant.transform.GetChild(slot), slot, combatant.GetId(), combatant.GetDeckLength()));
            //combatant.SetOccupiedSlot(slot, true, SpawnCardFullDeck(combatant.transform.GetChild(slot), slot, combatant.GetId()));
        }
    }
    
    //Spawn card method in each combatant?

    //Spawn a card at transform of a slot. FULL DECK VERSION. GOES THROUGH EACH CARD.
    int SpawnCardFullDeck(Transform slotTransform, int slot, int combatant)
    {
        activeCards[cardCount] = Instantiate(prefabCards[0], slotTransform);
        cardScripts[cardCount] = activeCards[cardCount].GetComponent<CardBehavior>();

        //set Card GameObject information
        cardScripts[cardCount].SetTitle(m_deck.GetCardTitle(cardCount));
        cardScripts[cardCount].SetDescription(m_deck.GetCardDescription(cardCount));
        cardScripts[cardCount].SetRefId(m_deck.GetCardRefId(cardCount));
        cardScripts[cardCount].SetSessionId(cardCount);
        cardScripts[cardCount].SetApCost(m_deck.GetCardApCost(cardCount));
        cardScripts[cardCount].SetDamage(m_deck.GetCardDamage(cardCount));
        cardScripts[cardCount].SetHealing(m_deck.GetCardHealing(cardCount));
        cardScripts[cardCount].SetSpecial(m_deck.GetCardSpecial(cardCount));
        cardScripts[cardCount].SetCombatant(combatant);
        cardScripts[cardCount].SetSlot(slot);
        //Display Title/Description
        cardScripts[cardCount].UpdateTextMeshes();
        cardCount++;

        return cardCount - 1;
    }

    //Spawn a card at transform of a slot. RANDOM IN RANGE VERSION.
    int SpawnCardRandomDeck(Transform slotTransform, int slot, int combatant, int highRange)
    {
        nextAvailableIndex = GetNextAvailableIndex();
        int randomSelection = (Mathf.RoundToInt(Random.Range(0, highRange)));
        activeCards[nextAvailableIndex] = Instantiate(prefabCards[0], slotTransform);
        cardScripts[nextAvailableIndex] = activeCards[nextAvailableIndex].GetComponent<CardBehavior>();

        //New -- Decks inside of combatant
        cardScripts[nextAvailableIndex].SetTitle(combatantScripts[combatant].GetCardTitle(randomSelection));
        cardScripts[nextAvailableIndex].SetDescription(combatantScripts[combatant].GetCardDescription(randomSelection));
        cardScripts[nextAvailableIndex].SetRefId(combatantScripts[combatant].GetCardRefId(randomSelection));
        cardScripts[nextAvailableIndex].SetSessionId(cardCount);
        cardScripts[nextAvailableIndex].SetApCost(combatantScripts[combatant].GetCardApCost(randomSelection));
        cardScripts[nextAvailableIndex].SetDamage(combatantScripts[combatant].GetCardDamage(randomSelection));
        cardScripts[nextAvailableIndex].SetHealing(combatantScripts[combatant].GetCardHealing(randomSelection));
        cardScripts[nextAvailableIndex].SetSpecial(combatantScripts[combatant].GetCardSpecial(randomSelection));

        cardScripts[nextAvailableIndex].SetCombatant(combatant);
        cardScripts[nextAvailableIndex].SetSlot(slot);
        //Display Title/Description
        cardScripts[nextAvailableIndex].UpdateTextMeshes();
        cardCount++;

        return cardCount - 1;
    }

    //Click on a card to return information about it.
    void SelectCard()
    {
    }

    /*
    //PlayCard With Button
    void PlayCard()
    {
        if (selectedCard != null && selectedCombatant != null)
        {
            //Attack Card
            if (selectedCard.GetEffect() == 0 && selectedCombatant.side != sideTurn) //make side a getter.
            {
                if (selectedCombatant.Damage(selectedCard.GetDamageVal()) <= 0) //Call HPAdjust, do damage, if it brings the combatant's HP zero, kill it.
                {
                    selectedCombatant.Die();
                    selectedCombatant = null;
                }
                selectedCard.Die();
                selectedCard = null;
                EndTurn();
            }
            //Healer Card
            else if (selectedCard.GetEffect() == 1)
            {
                Debug.Log("Healing Card. Not yet implemented.");
                EndTurn();
            }
            else
            {
                Debug.Log("Card Has No Effect");
            }
            //EndTurn();
        }
    }
    */

    //PlayCard With PlayArea
    void PlayCard()
    {
        if (playAreaCard != null && selectedCombatant != null)
        {
            //Attack Card
            if (playAreaCard.GetEffect() == 0 && selectedCombatant.side != sideTurn) //make side a getter.
            {
                CombatantBehavior owningCombatant = combatantScripts[playAreaCard.GetCombatant()];

                if (owningCombatant.EvalCardCost(playAreaCard.GetApCost())) //Evaluate if we have enough AP to use card then remove action points based on card AP cost.
                {

                    if (selectedCombatant.Damage(playAreaCard.GetDamageVal()) <= 0) //Call HPAdjust, do damage, if it brings the combatant's HP zero, kill it.
                    {
                        KillCombatant(selectedCombatant);
                    }

                    //owningCombatant.SubtractAp(playAreaCard.GetApCost()); //Remove action points based on card AP cost.
                    owningCombatant.SetOccupiedSlot(playAreaCard.GetSlot(), false, -1); //Set slot's occupied slot to empty.

                    ShuffleCards(owningCombatant, activeCombatants[playAreaCard.GetCombatant()].transform);

                    activeCards[playAreaCard.GetSessionId()] = null;
                    cardScripts[playAreaCard.GetSessionId()] = null;
                    playAreaCard.Die();
                    playAreaCard = null;
                    //EndTurn();
                }
                //Healer Card
                else if (playAreaCard.GetEffect() == 1)
                {
                    Debug.Log("Healing Card. Not yet implemented.");
                    EndTurn();
                }
                else
                {
                    Debug.Log("Card Has No Effect");
                }
                //EndTurn();
            }
        }
    }

    void ShuffleCards(CombatantBehavior combatant, Transform combatantTransform)
    {
        bool keepGoing = true;
        while (keepGoing)
        {
            if (combatant.GetNextOpenSlot() < handAmount  && combatant.GetNextOpenSlot() != -1)
            {
                int openSlot = combatant.GetNextOpenSlot();
                //Probably not good to try/catch this.
                try
                {
                    CardBehavior card = cardScripts[combatant.GetCardInSlot(openSlot + 1)];
                    card.MoveSlot(openSlot, combatantTransform.GetChild(openSlot));
                    combatant.SetOccupiedSlot(openSlot + 1, false, -1);
                    combatant.SetOccupiedSlot(card.GetSlot(), true, card.GetSessionId());
                }
                catch
                {
                    keepGoing = false;
                }
            }
            else keepGoing = false;
        }
    }

    void EndTurn()
    {
        sideTurn++;
        if (sideTurn > sidesAmount - 1)
        {
            sideTurn = 0;
        }

        turnCount++;

        for (int k = 0; k < amtCombatants; k++)
        {
            if (activeCombatants[k] != null)
            {
                FillHand(activeCombatants[k]);
                combatantScripts[k].ResetAp(sideTurn); //Fill combatant AP to max AP if it is the beginning of their turn.
            }
        }

        //Change Turn Text
        if (sideTurn == 0)
        {
            turnText0.text = "Turn";
            turnText1.text = "";
        }
        else
        {
            turnText0.text = "";
            turnText1.text = "Turn";
        }

        //Deselect Cards and Combatants
        if (selectedCard != null)
        {
            selectedCard.Reset();
            selectedCard = null;
        }

        if (selectedCombatant != null)
        {
            selectedCombatant.Select(false);
            selectedCombatant = null;
        }

        //Tell AI it's their turn.
        if (AI_Enabled)
        {
            if (sideTurn == enemyScript.GetSide())
            {
                enemyScript.IsTurn(true);
            }
        }
    }

    void KillCombatant(CombatantBehavior combatant)
    {
        int index = combatant.GetSessionId();
        combatantScripts[index].Die();
        activeCombatants[index] = null;
        combatantScripts[index] = null;
        combatant = null;
        selectedCombatant = null;
    }

    void Click(Vector3 mposition)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(mposition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            if (objectHit.tag == "Card")
            {
                updateMousePos = true;
                if (selectedCard != null)
                {
                    selectedCard.Select(false);
                }
                selectedCard = objectHit.gameObject.GetComponent<CardBehavior>();
                selectedCard.Select(true);
                oldCardPosition = selectedCard.transform.localPosition;
            }

            else if (objectHit.tag == "Button")
            {
                selectedButton = objectHit.gameObject.GetComponent<ButtonBehavior>();
                selectedButton.Select(true);
            }


            else if (objectHit.tag == "Combatant")
            {
                if (selectedCombatant != null)
                {
                    selectedCombatant.Select(false);
                }
                selectedCombatant = objectHit.gameObject.GetComponent<CombatantBehavior>();
                selectedCombatant.Select(true);

                if(playAreaCard != null)
                {
                    PlayCard();
                }
            }

            else if (objectHit.tag == "PlayArea")
            {
                if (playAreaCard != null)
                {
                    playAreaCard.Reset();
                    playAreaCard = null;
                }
            }

            else
            {
                if (selectedCard != null)
                {
                    selectedCard.Select(false);
                    selectedCard = null;
                }

                if (selectedCombatant != null)
                {
                    selectedCombatant.Select(false);
                    selectedCombatant = null;
                }
            }
        }
    }

    void Hover(Vector3 mposition)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(mposition);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            if (objectHit.tag == "Card")
            {
                if (highlightedCard != null)
                {
                    highlightedCard.Highlight(false);
                }
                highlightedCard = objectHit.gameObject.GetComponent<CardBehavior>();
                highlightedCard.Highlight(true);
            }

            else if (objectHit.tag == "Button")
            {
                highlightedButton = objectHit.gameObject.GetComponent<ButtonBehavior>();
                highlightedButton.Highlight(true);
            }

            else if (objectHit.tag == "Combatant")
            {
                highlightedCombatant = objectHit.gameObject.GetComponent<CombatantBehavior>();
                highlightedCombatant.Highlight(true);
            }

            else
            {
                if (highlightedCard != null)
                {
                    highlightedCard.Highlight(false);
                    highlightedCard = null;
                }
                if (highlightedButton != null)
                {
                    highlightedButton.Highlight(false);
                    highlightedButton = null;
                }
                if (highlightedCombatant != null)
                {
                    highlightedCombatant.Highlight(false);
                    highlightedCombatant = null;
                }
            }
        }
    }

    int GetNextAvailableIndex()
    {
        for (int i = 0; i < activeCards.Length; i++)
        {
            if (activeCards[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    //AI Controlled Opponent Methods

    public void AISelect(int card)
    {
        cardScripts[card].AIPlay(enemyScript.GetPlayArea());
        playAreaCard = cardScripts[card];
    }

    public void AIUse(int combatant)
    {
        selectedCombatant = combatantScripts[combatant];
        PlayCard();
    }
}