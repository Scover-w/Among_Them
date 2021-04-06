using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ProceduralCalculations
{
    
    // Dictionary<int, float> -> <id, wealthValue of the object>
    
    public static T GetRandomTFromPool<T>(Dictionary<T, float> pool, float wealthLevel)
    {
        float totalProb = 0f;
        
        foreach (var obj in pool)
        {
            totalProb += obj.Value;
        }
        
        if(totalProb == 0.0f)
            return pool.Take(Random.Range(1,pool.Count + 1)).Last().Key;
        
        float actualProb = 0f;
        float pickedProb = GetRandomValue();
        
        foreach (var obj in pool)
        {
            float objectProb = (1 - GetAbsoluteDistance(wealthLevel, obj.Value)) / totalProb;
            //Debug.Log("Key : " + obj.Key + ", Value : " + objectProb);
            //Debug.Log("Actual Prop : " + actualProb + objectProb + ", pickedProb : " + pickedProb);
            if (actualProb + objectProb >= pickedProb)
                return obj.Key;

            actualProb += objectProb;
        }

        // Will always return in the foreach
        Debug.LogError("Outside the bound");
        return pool.First().Key;
    }

    public static float GetAbsoluteDistance(float nb1, float nb2)
    {
        return (nb1 > nb2) ? nb1 - nb2 : nb2 - nb1;
    }

    public static float GetRandomValue()
    {
        return Random.Range(0f, 1f);
    }
}
