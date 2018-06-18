using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    private int m_side = 0;
    private bool m_enabled = true;
    private bool m_myTurn = false;

    private bool m_wait = false;

    private float m_waitTime = 1.0f;
    private float m_startTime = 0f;

    GameObject game;
    GameBehavior gameScript;

	// Use this for initialization
	void Start () {
        game = GameObject.Find("GameManager");
        gameScript = game.GetComponent<GameBehavior>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_myTurn)
        {
            if (Time.time - m_startTime >= m_waitTime)
            {
                Debug.Log(Time.time);
                TakeTurn();
            }
        }
    }

    //Getters
    public bool GetEnabled()
    {
        return m_enabled;
    }

    public int GetSide()
    {
        return m_side;
    }

    public Transform GetPlayArea()
    {
        GameObject playArea = GameObject.FindGameObjectWithTag("PlayArea");
        return playArea.transform;
    }

    //Setters

    //The only way to make the AI play its turn is to call this function with true.
    public void IsTurn(bool flag)
    {
        //m_myTurn = flag;

        if (m_myTurn)
        {
            //TakeTurn();
        }
    }

    //Actions
    private void TakeTurn()
    {
        //test
        m_startTime = Time.time;
        if (!m_wait)
        {
            gameScript.AISelect(0);
            m_wait = true;
        }
        else
        {
            gameScript.AIUse(2);
            m_myTurn = false;
        }
    }

    public void CardMoveTest()
    {
        m_startTime = Time.time;
        m_myTurn = true;
    }
}
