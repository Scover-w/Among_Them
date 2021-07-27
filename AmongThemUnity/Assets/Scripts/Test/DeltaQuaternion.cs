using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeltaQuaternion : MonoBehaviour
{
    [SerializeField] private Transform player;
    
    [SerializeField] private Transform crowdAgent;

    private Quaternion tempQuaternion;
    private Vector3 directionToSee;
    
    private Vector3 tempEuler;
    
    private void Update()
    {

        /*directionToSee = (crowdAgent.position - player.position).normalized;
        
    

        tempQuaternion = crowdAgent.rotation * new Quaternion(directionToSee.x, directionToSee.y);

        tempEuler = tempQuaternion.eulerAngles;
        
        Debug.Log(tempEuler.x + ", " + tempEuler.y + ", " + tempEuler.z);*/
        
        Vector3 relativeNormalizedPos = -(crowdAgent.position - player.position).normalized;
        Debug.DrawLine(Vector3.zero, relativeNormalizedPos, Color.red);
        
        Debug.DrawLine(Vector3.zero, crowdAgent.forward);

        float dot = Vector3.Dot(crowdAgent.forward, relativeNormalizedPos);
        float lengths = crowdAgent.forward.magnitude * relativeNormalizedPos.magnitude;
        
        Debug.Log("Degre : " + Math.Acos(dot / lengths) * (180/Math.PI));
        
        
        //float dot = Vector3.Dot(relativeNormalizedPos, crowdAgent.forward);
        
        //angle difference between looking direction and direction to item (radians)
        float angle = Mathf.Acos(dot);
        

        //Debug.Log("-------------------------------");
        //Debug.Log(reletiveNormalizedPos.x + ", " + reletiveNormalizedPos.y + ", " + reletiveNormalizedPos.z);
        //Debug.Log(angle * (180 / Math.PI) - 90f);
        //Debug.Log(dot);
        //Debug.Log("-------------------------------");
        
       
       /*directionToSee = (player.position - crowdAgent.position);
       Debug.DrawLine(crowdAgent.position, crowdAgent.position + directionToSee);
       tempQuaternion = Quaternion.LookRotation(directionToSee, Vector3.up);
       
       tempEuler = tempQuaternion.eulerAngles;

       Vector3 tempVector3 = Vector3.RotateTowards(crowdAgent.position, directionToSee, 6.28319f, 5);
       tempVector3 = crowdAgent.position - tempVector3;
       Debug.Log(tempVector3.x + ", " + tempVector3.y + ", " + tempVector3.z);
       Debug.DrawLine(crowdAgent.position, tempVector3, Color.green);
       
       Debug.DrawLine(crowdAgent.position, crowdAgent.position - crowdAgent.forward, Color.yellow);
       
       Debug.Log(Vector3.Angle(tempVector3, crowdAgent.forward));*/
       // Vector3 start, Vector3 end, Color color = Color.white, float duration = 0.0f, bool depthTest = true
       
       /*Vector3 forward = crowdAgent.transform.forward;
       // Zero out the y component of your forward vector to only get the direction in the X,Z plane
       forward.y = 0;
       float headingAngle = Quaternion.LookRotation(forward).eulerAngles.y;
       
       Debug.Log(headingAngle);*/
    }
}
