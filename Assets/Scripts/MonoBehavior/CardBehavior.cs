using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehavior : MonoBehaviour {

    //
    //Public
    //
    public bool selected;
    public bool highlighted;
    public Material baseMaterial;
    public Material highlightMaterial;
    public Material selectMaterial;

    //
    //Private
    //
    private string m_description;
    private string m_title;
    private int m_refId;
    private int m_sessionId;
    private int m_effect; //0 = damage, 1 = healing
    private int m_apCost;
    private int m_damage;
    private int m_healing;
    private int m_special;
    private int m_combatant;
    private int m_occupyingSlot = -1;
    private bool m_playArea = false;
    private bool m_lerping = false;
    private float m_speed = 9f;
    private float m_startLerpTime;
    private float m_endLerpTime = 1f;

    private Vector3 m_fromPos = new Vector3();

    private TextMesh textTitle;
    private TextMesh textDescription;

    private GameObject titleTextMeshObject;
    private GameObject descriptionTextMeshObject;

    private GameObject playAreaGameObject;

    //Reset vars
    private GameObject m_currentParent;
    private Vector3 m_currentLocalPosition;

    /*
    // Use this for initialization
    public void Start () {
        titleTextMeshObject = gameObject.transform.GetChild(0).gameObject;
        textTitle = titleTextMeshObject.GetComponent<TextMesh>();

        descriptionTextMeshObject = gameObject.transform.GetChild(1).gameObject;
        textDescription = descriptionTextMeshObject.GetComponent<TextMesh>();

        //textTitle.text = m_title;
        //textDescription.text = m_description;
    }
    */

    /*
	// Update is called once per frame
	void Update () {
		
	}
    */

    public void Awake()
    {
        m_currentParent = transform.parent.gameObject;
        m_currentLocalPosition = transform.localPosition;
    }

    void Update()
    {
        if (m_lerping)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, m_currentLocalPosition, 1f * Time.deltaTime * m_speed);
            if(Time.time - m_startLerpTime >= m_endLerpTime)
            {
                transform.localPosition = m_currentLocalPosition;
                m_lerping = false;
            }
        }
    }
   
    //
    //Getters
    //
    public string GetTitle()
    {
        return m_title;
    }

    public int GetRefId()
    {
        return m_refId;
    }

    public int GetSessionId()
    {
        return m_sessionId;
    }

    public int GetEffect()
    {
        return m_effect;
    }

    public int GetDamageVal()
    {
        return m_damage;
    }

    public int GetApCost()
    {
        return m_apCost;
    }

    public bool GetInPlayArea()
    {
        return m_playArea;
    }

    public GameObject GetPlayAreaGameObject()
    {
        return playAreaGameObject;
    }

    public int GetSlot()
    {
        return m_occupyingSlot;
    }

    public int GetCombatant()
    {
        return m_combatant;
    }

    //
    //Setters
    //
    public void SetTitle(string title)
    {
        m_title = title;
    }

    public void SetDescription(string description)
    {
        m_description = description;
    }

    public void SetRefId(int refId)
    {
        m_refId = refId;
    }

    public void SetSessionId(int sessionId)
    {
        m_sessionId = sessionId;
    }

    public void SetApCost(int apCost)
    {
        m_apCost = apCost;
    }

    public void SetDamage(int damage)
    {
        m_damage = damage;
    }

    public void SetHealing(int healing)
    {
        m_healing = healing;
    }

    public void SetSpecial(int special)
    {
        m_special = special;
    }

    public void SetAreaParent()
    {
        if (m_playArea)
        {
            transform.parent = playAreaGameObject.transform;
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void SetSlot(int slot)
    {
        m_occupyingSlot = slot;
    }

    public void SetCombatant(int combatant)
    {
        m_combatant = combatant;
    }

    //Set the GameObject text mesh component text to display the title and description of the card.
    //NOTE: Issue with NullReferenceException when we do not do GetComponent line in the same line as the .text line.
    public void UpdateTextMeshes()
    {
        titleTextMeshObject = gameObject.transform.GetChild(0).gameObject;
        textTitle = titleTextMeshObject.GetComponent<TextMesh>();

        descriptionTextMeshObject = gameObject.transform.GetChild(1).gameObject;
        textDescription = descriptionTextMeshObject.GetComponent<TextMesh>();

        textTitle.text = m_title;
        textDescription.text = m_description +"\nDMG: " + m_damage + "\nAP:" + m_apCost;
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
            else if(!selected)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayArea")
        {
            m_playArea = true;
            playAreaGameObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayArea")
        {
            m_playArea = false;
            playAreaGameObject = null;
        }
    }

    public void Reset()
    {
        m_startLerpTime = Time.time;
        transform.parent = m_currentParent.transform;
        m_lerping = true;
        Select(false);
        m_playArea = false;
        playAreaGameObject = null;
    }

    public void MoveSlot(int newSlot, Transform newTransform)
    {
        m_occupyingSlot = newSlot;
        m_startLerpTime = Time.time;
        transform.parent = newTransform;
        m_currentParent = transform.parent.gameObject;
        m_currentLocalPosition = new Vector3(0, 0, 0);
        m_lerping = true;
    }

    public void AIPlay(Transform newTransform)
    {
        m_startLerpTime = Time.time;
        transform.parent = newTransform;
        m_currentParent = transform.parent.gameObject;
        m_currentLocalPosition = new Vector3(0, 0, 0);
        m_lerping = true;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
