using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class Deck
{ 
    private string gameDataProjectFilePath = "/JSON/Decks/";

    public Card[] deck;

    public int jDeckId;
    public string jName;
    public int[] jCards;

    private int m_deckId;
    private string m_name;
    private int[] m_cards;
    private int m_length;

    public Deck()
    {
        /*
        deck = new Card[128];
        m_deckId = -1;
        m_length = deck.Length;

        for (int i = 0; i < m_length; i++)
        {
            if(i % 2 == 1)
            {
                deck[i] = new Card("Test Card", "This is a test card! Merely a test!", 0, -1, 2, 1, 0, -1);
            }
            else
            {
                deck[i] = new Card("Best Card", "NOT A DRILL", 1, -1, 4, 2, 0, -1);
            }
        }
        */
    }

    public Deck(int id)
    {
        //Debug.Log(id);
        LoadDeckData(id);
        deck = new Card[m_cards.Length];
        m_length = deck.Length;

        for (int i = 0; i < m_length; i++)
        {
            deck[i] = new Card(m_cards[i]);
        }
    }

    public void LoadDeckData(int id)
    {
        string filePath = Application.dataPath + gameDataProjectFilePath + id.ToString() + ".json";

        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            Deck jsonDeck = JsonUtility.FromJson<Deck>(dataAsJson);

            m_deckId = jsonDeck.jDeckId;
            m_name = jsonDeck.jName;

            m_cards = new int[jsonDeck.jCards.Length];
            for(int i = 0; i < jsonDeck.jCards.Length; i++)
            {
                m_cards[i] = jsonDeck.jCards[i];
            }
        }
        else
        {
            Deck jsonDeck = new Deck();
            Debug.Log("json failed");
        }
    }

    public string GetCardTitle(int index)
    {
        return (deck[index].GetTitle());
    }

    public string GetCardDescription(int index)
    {
        return (deck[index].GetDescription());
    }

    public int GetCardRefId(int index)
    {
        return (deck[index].GetRefId());
    }

    public int GetCardApCost(int index)
    {
        return (deck[index].GetApCost());
    }

    public int GetCardDamage(int index)
    {
        return (deck[index].GetDamage());
    }

    public int GetCardHealing(int index)
    {
        return (deck[index].GetHealing());
    }

    public int GetCardSpecial(int index)
    {
        return (deck[index].GetSpecial());
    }

    public int GetLength()
    {
        return m_length;
    }
}
