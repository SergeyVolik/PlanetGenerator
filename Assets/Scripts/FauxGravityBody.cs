using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FauxGravityBody : MonoBehaviour
{

    public FauxGravityAttractor m_attractor;

    private Rigidbody m_rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        m_rigidbody.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_attractor.Attract(m_rigidbody);
    }
}
