using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public bool bottomRow;
    public Rigidbody rigidbody;
    public bool dead;
    // Start is called before the first frame update
    void Start()
    {
        dead = false;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        bottomRow = false;
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity *= 0.0f;
    }
}
