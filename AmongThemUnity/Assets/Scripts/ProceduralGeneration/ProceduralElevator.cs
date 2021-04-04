using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum ElevatorType
{
    Rounded,
    Squared
}
public class ProceduralElevator : MonoBehaviour
{
    [SerializeField] private List<GameObject> RoundedElevators;
    
    [SerializeField] private List<GameObject> SquaredElevators;
    
    [SerializeField] private List<GameObject> RoundedBorder;
    
    [SerializeField] private List<GameObject> SquaredBorder;

    [SerializeField] private GameObject RoundedGate;

    [SerializeField] private GameObject SquaredGate;

    [SerializeField] private GameObject RoundedBottom;
    
    [SerializeField] private GameObject SquaredBottom;

    
    [Header("Wealth Obstructed Location")]
    [Range(0.0f, 1.0f)] public float MiddleValueProb = 0.0f;

    [Range(0.0f, 1.0f)] public float ShortValueProb = 0.0f;

    [Range(0.0f, 1.0f)] public float LongValueProb = 0.0f;
    
    [Header("Wealth treshold")]
    [Tooltip("To choose the type of elevator (round/square)")]
    [Range(0.0f, 1.0f)] public float TresholdType = 0.0f;
    
    [Header("Wealth treshold ")]
    [Tooltip("To choose the number of elevator (1 or 2)")]
    [Range(0.0f, 1.0f)] public float TresholdNumber = 0.0f;


    

    // Start is called before the first frame update
    public void LoadElevators(float wealthLevel)
    {
        ObstructedLocation locationElevator = GetLocationElevator(wealthLevel);
        int nbElevator = GetNbElevator(wealthLevel);
        ElevatorType elevatorType = GetElevatorType(wealthLevel);

        Debug.Log("ObstructedLocation : " + locationElevator);
        Debug.Log("nbElevator : " + nbElevator);
        Debug.Log("ElevatorType : " + elevatorType);
    }

    private ObstructedLocation GetLocationElevator(float wealthLevel)
    {
        var locations = new Dictionary<ObstructedLocation, float>();
        locations.Add(ObstructedLocation.Middle, MiddleValueProb);
        locations.Add(ObstructedLocation.Short, ShortValueProb);
        locations.Add(ObstructedLocation.Long, LongValueProb);

        return ProceduralCalculations.GetRandomTFromPool(locations, wealthLevel);
    }

    private int GetNbElevator(float wealthLevel)
    {
        var numbers = new Dictionary<int, float>();
        
        numbers.Add(1, 1 - TresholdNumber);
        numbers.Add(2, TresholdNumber);

        return ProceduralCalculations.GetRandomTFromPool(numbers, wealthLevel);
    }

    private ElevatorType GetElevatorType(float wealthLevel)
    {
        var types = new Dictionary<ElevatorType, float>();
        
        types.Add(ElevatorType.Squared, 1 - TresholdType);
        types.Add(ElevatorType.Rounded, TresholdType);

        return ProceduralCalculations.GetRandomTFromPool(types, wealthLevel);
    }
}
