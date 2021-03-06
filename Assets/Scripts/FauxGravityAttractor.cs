﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityAttractor : MonoBehaviour
{
    public float gravity = -10;

    public void Attract(Rigidbody body)
    {

        Vector3 gravityUp = (body.transform.position - transform.position).normalized;
        Vector3 bodyUp = body.transform.up;

        body.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(bodyUp, gravityUp);

        body.transform.rotation = targetRotation;// Quaternion.Slerp(body.transform.rotation, targetRotation, 50 * Time.deltaTime);

        Debug.Log(targetRotation);
        Debug.DrawRay(transform.position, gravityUp, Color.red);
        
    }
}
