using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatantTemplate {

    protected string m_name;
    protected string m_description;
    protected int m_side;
    protected int m_maxhp;
    protected int m_currenthp;
    protected int m_maxap;
    protected int m_currentap;
    protected int m_id;

    protected int m_column;
    protected int m_deckId;

    public CombatantTemplate(int id, string title, string description, int side)
    {
    }
    public int getDeckID()
    {
        return m_deckId;
    }
}
