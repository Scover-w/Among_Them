using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorElevatorManager : MonoBehaviour
{
    [SerializeField] 
    private Transform positionPlayer;
    
    private List<ElevatorEntity> elevators = new List<ElevatorEntity>();

    private ElevatorEntity activeElevator;
    
    private WaitForSeconds wait = new WaitForSeconds(0.2f);

    public void Add(ElevatorEntity elevatorEntity)
    {
        elevators.Add(elevatorEntity);
    }

    public void Reset()
    {
        elevators = new List<ElevatorEntity>();
    }

    public void OpenElevator()
    {
        activeElevator.ElevatorAnim.SetTrigger("OpenDoor");
        
        if(activeElevator.BorderAnim != null)
            activeElevator.BorderAnim.SetTrigger("OpenDoor");
        
        if(activeElevator.GateAnim != null)
            activeElevator.GateAnim.SetTrigger("OpenDoor");

        StartCoroutine(nameof(WaitCloseElevator));
    }

    public void TeleportPlayer()
    {
        activeElevator = elevators[Random.Range(0, elevators.Count)];
        positionPlayer.position = activeElevator.Elevator.transform.position;
        positionPlayer.rotation = activeElevator.Elevator.transform.rotation;
        positionPlayer.Rotate(Vector3.up, 180f);
    }

    IEnumerator ExitLevelCo()
    {
        yield return null;
    }

    IEnumerator WaitCloseElevator()
    {
        while (!IsDistant())
        {
            yield return wait;
        }
        
        activeElevator.ElevatorAnim.SetTrigger("CloseDoor");
        
        if(activeElevator.BorderAnim != null)
            activeElevator.BorderAnim.SetTrigger("CloseDoor");
        
        if(activeElevator.GateAnim != null)
            activeElevator.GateAnim.SetTrigger("CloseDoor");
    }
    
    private bool IsDistant()
    {
        Vector2 distance = new Vector2(positionPlayer.position.x - activeElevator.Elevator.transform.position.x , positionPlayer.position.z - activeElevator.Elevator.transform.position.z);

        if (distance.magnitude > 4f)
        {
            return true;
        }
        
        return false;
    }
}
