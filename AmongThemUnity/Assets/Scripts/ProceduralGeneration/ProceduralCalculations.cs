using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ProceduralCalculations
{
    // Dictionary<int, float> -> <id, wealthValue of the object>
    public static T GetRandomTFromPool<T>(Dictionary<T, float> pool, float wealthLevel)
    {
        float totalProb = 0f;
        
        var normalizedPool = new Dictionary<T, float>();
        
        foreach (var obj in pool)
        {
            normalizedPool.Add(obj.Key, 1 - GetAbsoluteDistance(wealthLevel, pool[obj.Key]));
            totalProb += normalizedPool[obj.Key];
        }
        
        if(totalProb == 0.0f)
            return pool.Take(Random.Range(1,pool.Count + 1)).Last().Key;
        
        float actualProb = 0f;
        float pickedProb = GetRandomValue();
        
        foreach (var obj in normalizedPool)
        {
            float objectProb = obj.Value / totalProb;

            if (actualProb + objectProb >= pickedProb)
                return obj.Key;

            actualProb += objectProb;
        }
        
        Debug.LogError("Outside of the bound");
        return normalizedPool.First().Key;
    }

    public static T GetRandomFrom2Value<T>(KeyValuePair<T, float> firstProb, KeyValuePair<T, float> secondProb, float wealthLevel) // Order : False True
    {
        if (Math.Abs(firstProb.Value - secondProb.Value) < 0.001f)
            return GetRandomBool() ? firstProb.Key : secondProb.Key;

        var maxPair = (firstProb.Value > secondProb.Value) ? firstProb : secondProb;
        var minPair = (firstProb.Value < secondProb.Value) ? firstProb : secondProb;

        if (wealthLevel < minPair.Value)
            return minPair.Key;

        if (wealthLevel > maxPair.Value)
            return maxPair.Key;

        float firstNonNorm = wealthLevel - minPair.Value;
        float secondNonNorm = maxPair.Value - wealthLevel;

        float divider = firstNonNorm + secondNonNorm;

        // Switch : the closest has the best chance of being drawn
        var maxNormalizedPair = new KeyValuePair<T, float>(maxPair.Key, (firstNonNorm) / divider);
        var minNormalizedPair = new KeyValuePair<T, float>(minPair.Key, (secondNonNorm) / divider);

        float pickedProb = GetRandomValue();
        
        if (pickedProb < maxNormalizedPair.Value)
            return maxNormalizedPair.Key;

        return minNormalizedPair.Key;
    }

    public static float GetAbsoluteDistance(float nb1, float nb2)
    {
        return (nb1 > nb2) ? nb1 - nb2 : nb2 - nb1;
    }

    public static float GetRandomValue()
    {
        return Random.Range(0f, 1f);
    }

    public static bool GetRandomBool()
    {
        return Random.Range(0f, 1f) > 0.5f;
    }
}
