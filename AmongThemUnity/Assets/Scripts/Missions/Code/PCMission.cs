using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PCMission : MonoBehaviour
{
    public static PCMission Instance() {return _singleton;}
    private static PCMission _singleton;
    
    public Canvas missionCanvas;
    public GameObject pcCanvas;
    public GameObject windowCanvas;

    public Button filesButton;
    public Button fileInfo;
    public Button exitButton;
    
    // Start is called before the first frame update
    void Start()
    {
        _singleton = this;
        filesButton.onClick.AddListener(delegate { OpenFiles(); });
        fileInfo.onClick.AddListener(delegate { GetInfo(); });
        exitButton.onClick.AddListener(delegate { CloseMissionPanel(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenFiles()
    {
        windowCanvas.SetActive(true);
    }

    public void GetInfo()
    {
        GameManager.Instance().GetNextTargetInformation();
        if (GameManager.Instance().IsTutorial)
        {
            CloseMissionPanel();
            TutorialManager.Instance().TPinHall();
            TutorialManager.Instance().NextStep();
        }
    }
    
    public void OpenMissionPanel()
    {
        missionCanvas.gameObject.SetActive(true);
        pcCanvas.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        GameManager.Instance().ChangePlayerCanMove(false);
        GameManager.Instance().ChangePlayerCanRotate(false);
    }
    
    public void CloseMissionPanel()
    {
        missionCanvas.gameObject.SetActive(false);
        pcCanvas.SetActive(false);
        windowCanvas.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance().ChangePlayerCanMove(true);
        GameManager.Instance().ChangePlayerCanRotate(true);
    }
}
