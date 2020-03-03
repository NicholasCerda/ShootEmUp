using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "Tank(Clone)")
        {
            float move = Input.GetAxis("Horizontal");
            move *= 5;
            if (move<0 && myTransform.position.x < -1.5f || move>0 && myTransform.position.x > 31.5f)
            {
                move *= 0;
            }
            Vector3 temp = new Vector3(myTransform.transform.position.x + move, 0.0f, 0.0f);
            myTransform.position = Vector3.Lerp(myTransform.transform.position, temp, Mathf.Abs(move) * Time.deltaTime);
        }
        myTransform.position = new Vector3(myTransform.transform.position.x, 0.0f, 0.0f);
    }
}
