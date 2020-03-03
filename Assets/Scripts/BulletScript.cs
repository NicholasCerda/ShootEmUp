using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GuiScript guiSc;
    public GameObject MainCamera;
    void Start()
    {
        guiSc = MainCamera.GetComponent<GuiScript>();
        if (gameObject.name!="Bullet")
            Destroy(gameObject, 15.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Tank(Clone)")
        {
            guiSc.killed(collision.gameObject.name, collision.gameObject);
            Destroy(gameObject);
        }
        else if (collision.gameObject.name != "Tank(Clone)")
        {
            if (collision.gameObject.tag == "Enemy")
            {
                bool lockup = false;
                foreach (GameObject mobObj in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (!lockup)
                    {
                        GunScript mobSc = mobObj.GetComponent<GunScript>();
                        GunScript colSc = collision.gameObject.GetComponent<GunScript>();
                        switch (collision.gameObject.name)
                        {
                            case "Enemy Lower(Clone)":
                                if (mobObj.name == "Enemy Middle(Clone)" && mobSc.col == colSc.col)
                                {
                                    mobSc.bottomRow = true;
                                    lockup = true;
                                }
                                break;
                            case "Enemy Middle(Clone)":
                                if (mobObj.name == "Enemy Top(Clone)" && mobSc.col == colSc.col)
                                {
                                    mobSc.bottomRow = true;
                                    lockup = true;
                                }
                                break;
                        }
                    }
                }
            }
            guiSc.killed(collision.gameObject.name, collision.gameObject);
            Destroy(gameObject);
        }
    }
}
