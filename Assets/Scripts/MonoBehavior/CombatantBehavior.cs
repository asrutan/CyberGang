using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantBehavior : MonoBehaviour {

    public string name;
    public string description;
    public int sessionId;
    public int maxhp;
    public int currenthp;
    public int maxap;
    public int currentap;
    public int side;

    public int column;
    public int deckId;

    public bool selected;
    public bool highlighted;
    public Material baseMaterial;
    public Material highlightMaterial;
    public Material selectMaterial;

    public Combatant combatant;
    private Deck m_deck;

    private TextMesh healthText;
    private GameObject healthTextMeshObject;

    private TextMesh apText;
    private GameObject apTextMeshObject;

    private bool[] OccupiedSlots = new bool[3];
    private int[] cardId = new int[3];

    // Set Defaults when called
    void Awake() {
        for (int i = 0; i < 3; i++)
        {
            OccupiedSlots[i] = false;
        }

        //healthTextMeshObject = transform.FindChild("HealthValue").gameObject;
        healthTextMeshObject = transform.Find("HealthValue").gameObject;
        healthText = healthTextMeshObject.GetComponent<TextMesh>();

        apTextMeshObject = transform.Find("ApValue").gameObject;
        apText = apTextMeshObject.GetComponent<TextMesh>();

        //m_SetAllInfo();
    }

    public void Init(int cSessionId, int cRefId, int cSide)
    {
        combatant = new Combatant(cSessionId, cRefId, cSide);
        m_SetAllInfo();
    }


    public void Init(int cId, string cName, string cDescription, int cSide)
    {
        combatant = new Combatant(cId, cName, cDescription, cSide);
        m_SetAllInfo();
    }

    public void Init(int cSide)
    {
        combatant = new Combatant(cSide);
        m_SetAllInfo();
    }

 
    public void Init()
    {
        combatant = new Combatant();
        m_SetAllInfo();
    }
    
    //
    //Getters
    //

    public int GetNextOpenSlot()
    {
        for(int i = 0; i < OccupiedSlots.Length; i++)
        {
            if(OccupiedSlots[i] == false)
            {
                return i;
            }
        }
        return -1;
    }
    
    public int GetCardInSlot(int slot)
    {
        return cardId[slot];
    }

    public int GetSessionId()
    {
        return sessionId;
    }

    //Deck Getters
    public int GetDeckLength()
    {
        return m_deck.GetLength();
    }

    public string GetCardTitle(int index)
    {
        return m_deck.GetCardTitle(index);
    }

    public string GetCardDescription(int index)
    {
        return (m_deck.GetCardDescription(index));
    }

    public int GetCardRefId(int index)
    {
        return (m_deck.GetCardRefId(index));
    }

    public int GetCardApCost(int index)
    {
        return (m_deck.GetCardApCost(index));
    }

    public int GetCardDamage(int index)
    {
        return (m_deck.GetCardDamage(index));
    }

    public int GetCardHealing(int index)
    {
        return (m_deck.GetCardHealing(index));
    }

    public int GetCardSpecial(int index)
    {
        return (m_deck.GetCardSpecial(index));
    }

    //
    //Setters
    //

    private void m_SetAllInfo()
    {
        sessionId = combatant.GetSessionId();
        name = combatant.GetName();
        description = combatant.GetDescription();
        maxhp = combatant.GetMaxHP();
        currenthp = combatant.GetCurrentHP();
        maxap = combatant.GetMaxAP();
        currentap = combatant.GetCurrentAP();
        side = combatant.GetSide();
        deckId = combatant.GetDeckId();

        m_deck = combatant.GetDeck();

        UpdateHealthText();
        UpdateApText();
    }

    public int Damage(int damage)
    {
        currenthp -= damage;
        UpdateHealthText();
        return currenthp;

        //Update HP Text
    }

    public bool EvalCardCost(int apCost)
    {
        if (currentap - apCost >= 0)
        {
            currentap -= apCost;
            UpdateApText();
            return true;
        }
        else return false;
    }

    public void SetOccupiedSlot(int index, bool occupied, int id)
    {
        OccupiedSlots[index] = occupied;
        cardId[index] = id;
    }

    public void SubtractAp(int apVal)
    {
        currentap -= apVal;
    }

    public void ResetAp(int turn)
    {
        if(side == turn)
        {
            currentap = maxap;
            UpdateApText();
        }
    }

    public void UpdateHealthText()
    {
        healthText.text = "" + currenthp;
    }

    public void UpdateApText()
    {
        apText.text = "" + currentap;
    }

    //Set all variables based on ID parameter. 
    //For now, use combatant00 script, change later to get info from a file.
    public void setID(int id)
    {
    }

    public void Select(bool setSelect)
    {
        if (selected != setSelect)
        {
            selected = setSelect;
            if (selected)
            {
                GetComponent<Renderer>().material = selectMaterial;
            }
            else if (!selected)
            {
                GetComponent<Renderer>().material = baseMaterial;
            }
        }
    }

    public void Highlight(bool setHighlight)
    {
        if (highlighted != setHighlight && !selected)
        {
            highlighted = setHighlight;
            if (highlighted)
            {
                GetComponent<Renderer>().material = highlightMaterial;
            }
            else if (!highlighted)
            {
                GetComponent<Renderer>().material = baseMaterial;
            }
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
