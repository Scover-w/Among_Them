using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance()
    {
        return _singleton;}

    public static TutorialManager _singleton;
    
    private string selectedLanguage;
    private int tutorialStep;
    
    [SerializeField]
    private Text textTutorial;
    
    [SerializeField]
    private GameObject player;
    
    [SerializeField]
    private Transform tpInRoomGO;
    
    [SerializeField]
    private Transform tpInHallGO;

    private List<int> nonActionStep = new [] {0,2}.ToList();
    
    
    // Start is called before the first frame update
    void Start()
    {
        _singleton = this;
        GameManager.Instance().StartTutorial();
        selectedLanguage = LanguageManager.Instance().GetSelectedLanguage();
        tutorialStep = 0;
        textTutorial.text = LanguageManager.Instance().GetTextWithReference($"text_tutorial_{tutorialStep.ToString()}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && nonActionStep.Contains(tutorialStep))
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        tutorialStep++;
        textTutorial.text = LanguageManager.Instance().GetTextWithReference($"text_tutorial_{tutorialStep.ToString()}");
    }
    
    public int GetStep()
    {
        return tutorialStep;
    }

    public void TPinRoom()
    {
        player.transform.position = tpInRoomGO.position;
    }
    
    public void TPinHall()
    {
        player.transform.position = tpInHallGO.position;
    }
}
