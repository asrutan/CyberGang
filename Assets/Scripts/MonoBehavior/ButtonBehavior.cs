using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{

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
    private int m_id;

    private TextMesh textTitle;
    private TextMesh textDescription;

    private GameObject titleTextMeshObject;
    private GameObject descriptionTextMeshObject;

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

    //
    //Getters
    //
    public string GetTitle()
    {
        return m_title;
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

    public void SetId(int id)
    {
        m_id = id;
    }

    //Set the GameObject text mesh component text to display the title and description of the Button.
    //NOTE: Issue with NullReferenceException when we do not do GetComponent line in the same line as the .text line.
    public void UpdateTextMeshes()
    {
        titleTextMeshObject = gameObject.transform.GetChild(0).gameObject;
        textTitle = titleTextMeshObject.GetComponent<TextMesh>();

        descriptionTextMeshObject = gameObject.transform.GetChild(1).gameObject;
        textDescription = descriptionTextMeshObject.GetComponent<TextMesh>();

        textTitle.text = m_title;
        textDescription.text = m_description;
    }

    public void Select(bool setSelect)
    {
        if (selected != setSelect)
        {
            selected = setSelect;
            if (selected)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
            }
            else if (!selected)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -4.0f);
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
}
