using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps_Visuals : MonoBehaviour
{
    [SerializeField] private LayerMask whatIsGround;


    [SerializeField] private TrailRenderer leftFoot;
    [SerializeField] private TrailRenderer rightFoot;

    [Range(0.001f, 0.3f)]
    [SerializeField] private float checkRaidus = 0.05f;
    [Range(-.15f, .15f)]
    [SerializeField] private float rayDistance = -0.05f;

    private void Update()
    {
       CheckFootStep(leftFoot);
       CheckFootStep(rightFoot);
    }


    private void CheckFootStep(TrailRenderer foot)
    {
        Vector3 checkPositon  = foot.transform.position + Vector3.down * rayDistance;

        bool touchingGround = Physics.CheckSphere(checkPositon, checkRaidus, whatIsGround);

        foot.emitting = touchingGround;

    }

    private void OnDrawGizmos()
    {
        DrawFootGizmo(leftFoot.transform);
        DrawFootGizmo(rightFoot.transform);
    }


    private void DrawFootGizmo(Transform foot)
    {
        if (foot == null) 
            return;
        Gizmos.color = Color.blue;
        Vector3 checkPositon = foot.transform.position + Vector3.down * rayDistance;

        Gizmos.DrawWireSphere(checkPositon, checkRaidus);
    }

}
