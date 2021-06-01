using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialStep1 : MonoBehaviour
{
    private List<int> inputMade;
    
    // Start is called before the first frame update
    void Start()
    {
        inputMade = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            inputMade.Add(0);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TutorialManager.Instance().GetStep() == 1)
        {
            if (inputMade.TrueForAll(x => x == 1))
            {
                TutorialManager.Instance().NextStep();
            }

            if (Input.GetKeyDown(GameInputManager.GetKeyMapOn("Forward")))
            {
                inputMade[0] = 1;
            }
            
            if (Input.GetKeyDown(GameInputManager.GetKeyMapOn("Backward")))
            {
                inputMade[1] = 1;
            }
            
            if (Input.GetKeyDown(GameInputManager.GetKeyMapOn("Left")))
            {
                inputMade[2] = 1;
            }
            
            if (Input.GetKeyDown(GameInputManager.GetKeyMapOn("Right")))
            {
                inputMade[3] = 1;
            }
        }
        
    }
}
