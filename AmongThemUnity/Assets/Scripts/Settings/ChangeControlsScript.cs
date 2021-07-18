using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeControlsScript : MonoBehaviour
{

    private KeyCode inputKey;

    [SerializeField]
    private TMP_Text textButton;

    public string actionKey = "";

    public GameObject panelBinding;

    public TMP_Text description;
    public TMP_Text keyBind;
    public Button saveKeyBind;

    private bool changingBinding;
    
    // Start is called before the first frame update
    void Start()
    {
        inputKey = GameInputManager.GetKeyMapOn(actionKey);
        textButton.text = GameInputManager.GetKeyMapOn(actionKey).ToString();
        changingBinding = false;
    }

    // Update is called once per frame
    void Update()
    {
        textButton.text = GameInputManager.GetKeyMapOn(actionKey).ToString();
       
    }

    private void OnGUI()
    {
        if (panelBinding.activeInHierarchy && changingBinding)
        {
            Event e = Event.current;
            if (e != null && e.isKey && e.keyCode != KeyCode.None)
            {
                Debug.Log(e.keyCode);
                inputKey = e.keyCode;
                keyBind.text = e.keyCode.ToString();
            }
        }
    }

    public void ChangeMappingKey(string keyMap)
    {
        //KeyCode newKeyCode = (KeyCode) System.Enum.Parse(typeof(KeyCode), keyCode) ; 
        changingBinding = false;
        GameInputManager.SetKeyMap(keyMap,inputKey );
        panelBinding.SetActive(false);
    }

    public void InputKey()
    {
        SoundManager.Instance.Click();
        changingBinding = true;
        description.text = "Choose a key to bind the action : " + actionKey;
        keyBind.text = GameInputManager.GetKey(actionKey).ToString();
        panelBinding.SetActive(true);
        saveKeyBind.onClick.AddListener(delegate { ChangeMappingKey(actionKey);panelBinding.SetActive(false); });
    }
}
