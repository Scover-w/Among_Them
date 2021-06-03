using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public enum ElevatorGeoForm
{
    Rounded,
    Squared
}

public enum ElevatorOpenness
{
    Closed,
    Opened
}
public class ProceduralElevator : MonoBehaviour
{
    [SerializeField] private List<GameObject> RoundedElevators;
    
    [SerializeField] private List<GameObject> SquaredElevators;
    
    [SerializeField] private List<GameObject> RoundedBorder;
    
    [SerializeField] private List<GameObject> SquaredBorder;

    [Header("Gate")]
    [SerializeField] private GameObject RoundedGate;

    [SerializeField] private GameObject SquaredGate;

    [Header("Bottom")]
    [SerializeField] private GameObject RoundedBottom;
    
    [SerializeField] private GameObject SquaredBottom;
    
    [Header("Ceiling")]
    [SerializeField] private GameObject[] MiddleCeiling;
    
    [SerializeField] private GameObject[] ShortCeiling;
    
    [SerializeField] private GameObject[] LongCeiling;
    
    [Header("Ceiling")]
    [SerializeField] private GameObject[] Levels;

    [Header("Wealth Obstructed Location Value")]
    [Range(0.0f, 1.0f)] public float MiddleValueProb = 0.0f;

    [Range(0.0f, 1.0f)] public float ShortValueProb = 0.0f;

    [Range(0.0f, 1.0f)] public float LongValueProb = 0.0f;
    
    // Start is called before the first frame update
    public List<ObstructedLocation> LoadElevators(float wealthLevel)
    {
        var obstructedLocation = new List<ObstructedLocation>();
        var tempGeoForm = ChooseElevatorForm(wealthLevel);
        var tempOpenness = ChooseElevatorOpenness(wealthLevel);
        var tempBorderId = ChooseBorder(tempGeoForm, tempOpenness, wealthLevel);
        
        var profile = new ElevatorProfile()
        {
            Location = GetLocationElevator(wealthLevel),
            NbElevator = GetNbElevator(wealthLevel),
            Elevator = GetElevator(tempGeoForm, tempOpenness),
            Border = GetBorder(tempGeoForm, tempBorderId),
            Bottom = GetBottom(tempGeoForm),
            Gate = GetGate(tempGeoForm, tempOpenness)
        };

        obstructedLocation.Add(profile.Location);

        LoadCeilingAndLevel(profile);
        LoadElevatorProfile(profile);

        return obstructedLocation;
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
        var firstNumber = new KeyValuePair<int, float>(1, ProceduralManager.instance.minThresholdValue);
        var secondNumber = new KeyValuePair<int, float>(2, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstNumber, secondNumber, wealthLevel);
    }

    private ElevatorGeoForm ChooseElevatorForm(float wealthLevel)
    {
        var firstForm = new KeyValuePair<ElevatorGeoForm, float>(ElevatorGeoForm.Squared, ProceduralManager.instance.minThresholdValue);
        var secondForm = new KeyValuePair<ElevatorGeoForm, float>(ElevatorGeoForm.Rounded, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstForm, secondForm, wealthLevel);
    }

    private ElevatorOpenness ChooseElevatorOpenness(float wealthLevel)
    {
        var firstOpenness = new KeyValuePair<ElevatorOpenness, float>(ElevatorOpenness.Closed, ProceduralManager.instance.minThresholdValue);
        var secondOpenness = new KeyValuePair<ElevatorOpenness, float>(ElevatorOpenness.Opened, ProceduralManager.instance.maxTresholdValue);

        return ProceduralCalculations.GetRandomFrom2Value(firstOpenness, secondOpenness, wealthLevel);
    }

    private int ChooseBorder(ElevatorGeoForm form, ElevatorOpenness openness, float wealthLevel)
    {
        var border = new Dictionary<int, float>();

        int beginIndex = 0;
        
        if (openness == ElevatorOpenness.Opened)
            beginIndex = 1;
        
        if (form == ElevatorGeoForm.Squared)
        {
            for (int i = beginIndex; i < 4; i++)
            {
                border.Add(i, SquaredBorder[i].GetComponent<ProceduralEntity>().wealthValue);
            }
        }
        else
        {
            for (int i = beginIndex; i < 4; i++)
            {
                border.Add(i, RoundedBorder[i].GetComponent<ProceduralEntity>().wealthValue);
            }
        }

        return ProceduralCalculations.GetRandomTFromPool(border, wealthLevel);
    }

    private GameObject GetElevator(ElevatorGeoForm form, ElevatorOpenness openness)
    {
        if (form == ElevatorGeoForm.Squared)
        {
            if (openness == ElevatorOpenness.Closed)
                return SquaredElevators[0];
            else
                return SquaredElevators[1];
        }
        else
        {
            if (openness == ElevatorOpenness.Closed)
                return RoundedElevators[0];
            else
                return RoundedElevators[1];
        }
    }
    
    private GameObject GetBorder(ElevatorGeoForm form, int borderId)
    {
        if (form == ElevatorGeoForm.Squared)
            return SquaredBorder[borderId];
        else
            return RoundedBorder[borderId];
    }

    private GameObject GetBottom(ElevatorGeoForm form)
    {
        if (form == ElevatorGeoForm.Squared)
            return SquaredBottom;
        else
            return RoundedBottom;
    }

    private GameObject GetGate(ElevatorGeoForm form, ElevatorOpenness openness)
    {
        if (openness == ElevatorOpenness.Closed)
            return null;

        if (form == ElevatorGeoForm.Squared)
            return SquaredGate;
        else
            return RoundedGate;
    }

    private void LoadCeilingAndLevel(ElevatorProfile profile)
    {
        switch (profile.Location)
        {
            case ObstructedLocation.Middle:
                GameObject ceil = Instantiate(MiddleCeiling[ (profile.NbElevator == 1)? 0 : 1 ], ProceduralManager.ParentMap);
                ceil.transform.position = Vector3.zero;
                
                GameObject level = Instantiate(Levels[0], ProceduralManager.ParentMap);
                level.transform.position = Vector3.zero;
                
                break;
            
            case ObstructedLocation.Long:
                GameObject ceil2 = Instantiate(ShortCeiling[ (profile.NbElevator == 1)? 0 : 1 ], ProceduralManager.ParentMap);
                ceil2.transform.position = Vector3.zero;
                
                GameObject level2 = Instantiate(Levels[1], ProceduralManager.ParentMap);
                level2.transform.position = Vector3.zero;
                break;
            
            case ObstructedLocation.Short:
                GameObject ceil3 = Instantiate(LongCeiling[ (profile.NbElevator == 1)? 0 : 1 ], ProceduralManager.ParentMap);
                ceil3.transform.position = Vector3.zero;
                
                GameObject level3 = Instantiate(Levels[2], ProceduralManager.ParentMap);
                level3.transform.position = Vector3.zero;
                
                break;
        }
        
    }
    private void LoadElevatorProfile(ElevatorProfile profile)
    {
        float delta = 1.9f;

        switch (profile.Location)
        {
            case ObstructedLocation.Middle:
                if (profile.NbElevator == 1)
                {
                    LoadElevator(profile, Vector3.zero);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float xdirection = (i % 2 == 0) ? 1f : -1f;
                        float zdirection = (i < 2) ? 1f : -1f;
                        Vector3 position = new Vector3(xdirection * delta, 0, zdirection * delta);
                        LoadElevator(profile, position);
                    }
                }

                break;

            case ObstructedLocation.Long:
                var zCoord = 68.7f;
                if (profile.NbElevator == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float zdirection = (i % 2 == 0) ? 1f : -1f;
                        Vector3 position = new Vector3(0, 0, zdirection * zCoord);
                        LoadElevator(profile, position);
                    }

                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float xdirection = (i % 2 == 0) ? 1f : -1f;
                        float zdirection = (i < 2) ? 1f : -1f;
                        Vector3 position = new Vector3(xdirection * delta, 0, zdirection * zCoord);
                        LoadElevator(profile, position);
                    }
                }

                break;

            case ObstructedLocation.Short:
                var xCoord = 98.7f;
                if (profile.NbElevator == 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        float xdirection = (i % 2 == 0) ? 1f : -1f;
                        Vector3 position = new Vector3(xdirection * xCoord, 0, 0);
                        LoadElevator(profile, position);
                    }

                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        float xdirection = (i % 2 == 0) ? 1f : -1f;
                        float zdirection = (i < 2) ? 1f : -1f;
                        Vector3 position = new Vector3(xdirection * xCoord, 0, zdirection * delta);
                        LoadElevator(profile, position);
                    }
                }

                break;
        }
    }

    private int GetRotationElevator(Vector3 position, ObstructedLocation obstructedLocation)
    {
        int rotationToApply = 0;

        if (obstructedLocation == ObstructedLocation.Long)
            rotationToApply += 90;


        bool xAxis = position.x > 0;
        bool zAxis = position.z > 0;

        switch (obstructedLocation)
        {
            case ObstructedLocation.Middle:

                if (zAxis)
                    rotationToApply += 180;
                break;

            case ObstructedLocation.Short:
                if (!zAxis)
                    rotationToApply += 180;
                break;

            case ObstructedLocation.Long:
                if (!xAxis)
                    rotationToApply += 180;
                break;
        }

        return rotationToApply % 360;
    }

    private void LoadElevator(ElevatorProfile profile, Vector3 position)
    {
        GameObject elevator = Instantiate(profile.Elevator, ProceduralManager.ParentMap);
        int rotation = GetRotationElevator(position, profile.Location);
        elevator.transform.position = position;
        elevator.transform.Rotate(0, rotation, 0);
        
        GameObject border = Instantiate(profile.Border, ProceduralManager.ParentMap);
        border.transform.position = position;
        border.transform.Rotate(0, rotation, 0);
        
        GameObject bottom = Instantiate(profile.Bottom, ProceduralManager.ParentMap);
        bottom.transform.position = position;
        bottom.transform.Rotate(0, rotation, 0);

        if (profile.Gate != null)
        {
            GameObject gate = Instantiate(profile.Gate, ProceduralManager.ParentMap);
            gate.transform.position = position;
            gate.transform.Rotate(0, rotation, 0);
        }

        
    }
}
