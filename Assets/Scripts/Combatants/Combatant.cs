using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Combatant {

    private Deck deck;
    private string gameDataProjectFilePath = "/JSON/Combatants/";

    public int jRefId;
    public string jName;
    public string jDescription;
    public int jSide; //Maybe make this jEnemy or jAllied
    public int jMaxhp;
    public int jMaxap;
    public int jCurrenthp;
    public int jCurrentap;
    public int jDeckId;

    private int m_refId = -1;
    private int m_sessionId = -1;
    private string m_name = "Default";
    private string m_description = "Description";
    private int m_side = -1;
    private int m_maxhp = 0;
    private int m_maxap = 0;
    private int m_currenthp = 0;
    private int m_currentap = 0;
    private int m_deckId = -1;

    //Load From JSON
    public Combatant(int sessionId, int refId, int side)
    {
        LoadCombatantData(refId);

        m_sessionId = sessionId;
        m_side = side;
        m_currenthp = m_maxhp;
        m_currentap = m_maxap;
        deck = new Deck(m_deckId);
    }

    public Combatant(int sessionId, string name, string description, int side)
    {
        m_sessionId = sessionId;
        m_name = name;
        m_description = description;
        m_side = side;
        m_maxhp = 10;
        m_maxap = 4;
        m_currenthp = m_maxhp;
        m_currentap = m_maxap;

        deck = new Deck(20000);
    }

    public Combatant(int side)
    {
        m_name = "Default Combatant";
        m_description = "Combatant Description";
        m_side = side;

        deck = new Deck();
    }

    public Combatant()
    {
        m_name = "Default Combatant";
        m_description = "Combatant Description";
        m_side = -1;

        deck = new Deck();
    }

    //JSON
    public void LoadCombatantData(int id)
    {
        string filePath = Application.dataPath + gameDataProjectFilePath + id.ToString() + ".json";

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Combatant jsonCombatant = JsonUtility.FromJson<Combatant>(dataAsJson);

            m_refId = jsonCombatant.jRefId;
            m_name = jsonCombatant.jName;
            m_description = jsonCombatant.jDescription;
            m_maxhp = jsonCombatant.jMaxhp;
            m_maxap = jsonCombatant.jMaxap;
            //m_currenthp = jsonCombatant.jCurrenthp;
            //m_currentap = jsonCombatant.jCurrentap;
            m_deckId = jsonCombatant.jDeckId;
        }
        else
        {
            Combatant jsonCombatant = new Combatant();
            Debug.Log("json failed");
        }
    }

    //
    //Getters
    //
    public string GetName()
    {
        return m_name;
    }

    public string GetDescription()
    {
        return m_description;
    }

    public int GetSide()
    {
        return m_side;
    }

    public int GetMaxHP()
    {
        return m_maxhp;
    }

    public int GetCurrentHP()
    {
        return m_currenthp;
    }

    public int GetMaxAP()
    {
        return m_maxap;
    }

    public int GetCurrentAP()
    {
        return m_currentap;
    }

    public int GetSessionId()
    {
        return m_sessionId;
    }

    public int GetRefId()
    {
        return m_refId;
    }

    public int GetDeckId()
    {
        return m_deckId;
    }

    
    public Deck GetDeck()
    {
        return deck;
    }
    

    public int GetDeckLength()
    {
        return deck.GetLength();
    }
}
