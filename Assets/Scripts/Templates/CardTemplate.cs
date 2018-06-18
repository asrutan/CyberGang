using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class CardTemplate {

    protected string m_description;
    protected string m_title;
    protected int m_refId;
    protected int m_sessionId;
    protected int m_apCost;
    protected int m_damage;
    protected int m_healing;
    protected int m_special;

    public CardTemplate(string title, string description, int refId, int sessionId, int apCost, int damage, int healing, int special)
    {
    }
}
