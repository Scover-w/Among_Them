using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CinematicCamBehaviour
{
    Middle,
    Short,
    Long
}

public class CamCinematic : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField] 
    private Camera cinematicCamera;


    [SerializeField] 
    private DoorElevatorManager elevatorManager;

    private CinematicCamBehaviour camBehav;
    
    private float direction = 1f;

    private bool isXDirection;
    
    
    // Parameters
    private float distanceBegin = 5f;
    private float camSpeed = 1f;
    private float maxElevatorHeight = 45f;
    private float timerCinematic = 10f;
    private float distanceToTravelCam = 30f;

    public void PlayCinematic()
    {
        StartCoroutine(nameof(PlayCinematicCo), elevatorManager.GetElevator());
    }

    public float GetCinematicTimer()
    {
        return timerCinematic;
    }
    
    IEnumerator PlayCinematicCo(GameObject elevator)
    {
        SelectPosition(elevator.transform.position);

        cinematicCamera.enabled = true;
        mainCamera.enabled = false;
        float timer = 0f;
        float heightDelta = maxElevatorHeight / timerCinematic;
        float distanceDelta = distanceToTravelCam / timerCinematic;
        
        while (timer < timerCinematic)
        {
            elevator.transform.Translate(0f, heightDelta * Time.deltaTime, 0f);
            
            cinematicCamera.transform.Translate( isXDirection? distanceDelta * direction * Time.deltaTime : 0f,0f, isXDirection? 0f : distanceDelta * direction * Time.deltaTime, Space.World);
            cinematicCamera.transform.rotation = Quaternion.LookRotation(elevator.transform.position - cinematicCamera.transform.position);
            
            timer += Time.deltaTime;
            yield return null;
        }
        
        
        
        cinematicCamera.enabled = false;
        mainCamera.enabled = true;
        GameManager.Instance().GetPlayer().SetActive(true);
    }

    private void SelectPosition(Vector3 elevator)
    {
        if (Math.Abs(elevator.x) > 90f)
        {
            camBehav = CinematicCamBehaviour.Long;
            isXDirection = true;
            direction = (elevator.x > 0f) ? -1f : 1f;
            cinematicCamera.transform.position = new Vector3(elevator.x + distanceBegin * direction, elevator.y + 2f, elevator.z);
        }
        else if (Math.Abs(elevator.z) > 60f)
        {
            camBehav = CinematicCamBehaviour.Short;
            isXDirection = false;
            direction = (elevator.z > 0f) ? -1f : 1f;
            cinematicCamera.transform.position = new Vector3(elevator.x , elevator.y + 2f, elevator.z + distanceBegin * direction);
        }
        else
        {
            camBehav = CinematicCamBehaviour.Middle;
            isXDirection = false;
            direction = (elevator.z >= 0f) ? -1f : 1f;
            cinematicCamera.transform.position = new Vector3(elevator.x , elevator.y + 2f, elevator.z + distanceBegin * direction);
        }
    }
}
