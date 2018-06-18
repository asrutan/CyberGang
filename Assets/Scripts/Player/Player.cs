using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{

    protected string m_description;
    protected string m_name;
    protected int m_id;
    protected int m_side;
    protected bool m_playerControlled;

    private int hitpoints;
    private int amtCombatants;
    private int[] combatantIDs;

    public Player(string name, int side, int id, bool playerControlled)
    {
        m_name = name;
        m_side = side;
        m_id = id;
        m_playerControlled = playerControlled;

        //Spawn two combatants for testing purposes.
        amtCombatants = 2;
        combatantIDs = new int[amtCombatants];
    }

    //
    //Getters
    //
    public string GetName()
    {
        return m_name;
    }

    public int GetSide()
    {
        return m_side;
    }

    public int GetId()
    {
        return m_id;
    }

    public bool GetPlayerControlled()
    {
        return m_playerControlled;
    }

    public int GetCombatantAmount()
    {
        return amtCombatants;
    }

    public int GetCombatantId(int spot)
    {
        return combatantIDs[spot];
    }

    //
    //Setters
    //

    //Inc/Dec hp by value;
    public void SetHP(int value)
    {
        hitpoints += value;
    }

    public void SetCombatantId(int spot, int id)
    {
        combatantIDs[spot] = id;
    }
}
