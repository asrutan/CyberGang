using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Card
{
    private string gameDataProjectFilePath = "/JSON/Cards/";

    private string m_description = "NULL";
    private string m_title = "NULL";
    private int m_refId = -1;
    private int m_sessionId = -1;
    private int m_apCost = 0;
    private int m_damage = 0;
    private int m_healing = 0;
    private int m_special = -1;

    //For Json import
    public string jTitle;
    public string jDescription;
    public int jRefId;
    public int jApCost;
    public int jDamage;
    public int jHealing;
    public int jSpecial;

    public Card(string title, string description, int refId, int sessionId, int apCost,  int damage, int healing, int special)
    {
        m_title = title;
        m_description = description;
        m_refId = refId;
        m_sessionId = sessionId;
        m_apCost = apCost;
        m_damage = damage;
        m_healing = healing;
        m_special = special;
    }

    public Card(int id)
    {
        LoadCardData(id);
    }

    public void LoadCardData(int id)
    {
        string filePath = Application.dataPath + gameDataProjectFilePath + id.ToString() + ".json";

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Card jsonCard = JsonUtility.FromJson<Card>(dataAsJson);
            m_refId = jsonCard.jRefId;
            m_title = jsonCard.jTitle;
            m_description = jsonCard.jDescription;
            m_refId = jsonCard.jRefId;
            m_apCost = jsonCard.jApCost;
            m_damage = jsonCard.jDamage;
            m_healing = jsonCard.jHealing;
            m_special = jsonCard.jSpecial;
        }
        else
        {
            Deck jsondeck = new Deck();
            Debug.Log("json failed");
        }
    }
    

    public string GetTitle()
    {
        return (m_title);
    }

    public string GetDescription()
    {
        return (m_description);
    }

    public int GetRefId()
    {
        return (m_refId);
    }

    public int GetSessionId()
    {
        return (m_sessionId);
    }

    public int GetApCost()
    {
        return (m_apCost);
    }

    public int GetDamage()
    {
        return (m_damage);
    }

    public int GetHealing()
    {
        return (m_healing);
    }

    public int GetSpecial()
    {
        return (m_special);
    }
}