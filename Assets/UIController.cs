using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Text[] Known;
    public Text[] Unknown;

    public Text NameText;
    public InputField NameInput;

    public string name;
    public bool Ttoggle;

    public ParticleSystem MouseToucher;

    private NameGenerator _nameGen;


    private void Start()
    {
        _nameGen = GetComponent<NameGenerator>();
        Application.ExternalCall("getName");
    }

    private void Update()
    {
        if (Ttoggle)
        {
            Ttoggle = false;
            SetName(name);
        }
                foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                MouseToucher.transform.position = touch.position;
            }
        }
    }
    public void SetName(string name)
    {
        NameText.text = name;
        if (name.Equals(""))
            ToggleVisibility(false);
        else
            ToggleVisibility(true);
    }


    private void ToggleVisibility(bool isKnown)
    {
        if (isKnown)
        {
            foreach (var item in Known)
            {
                item.gameObject.SetActive(true);
            }
            foreach (var item in Unknown)
            {
                item.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var item in Known)
            {
                item.gameObject.SetActive(false);
            }
            foreach (var item in Unknown)
            {
                item.gameObject.SetActive(true);
            }
            NameInput.text = _nameGen.GenerateName();
        }
    }



    public void RegisterName()
    {
        if (NameInput.text.Length > 0)
            Application.ExternalCall("setName", NameInput.text);
    }
}
