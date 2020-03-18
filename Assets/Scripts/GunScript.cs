using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    private float cooldown;
    public float timeLapse,col;
    private bool canFire;
    public bool bottomRow;
    public GameObject Bullet;
    public Rigidbody projectile;
    public float speed = 10.0f;
    public BulletScript bSc;
    public GuiScript guiSc;
    public GameObject MainCamera;
    public Rigidbody rigidbody;
    Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        guiSc = MainCamera.GetComponent<GuiScript>();
        col = -1;
        bottomRow = false;
        if (gameObject.name=="Enemy Lower(Clone)")
        {
            bottomRow = true;
        }
        canFire = false;
        cooldown = Random.Range(5.0f, 20.0f);
        if (gameObject.name == "Tank(Clone)")
        {
            cooldown = 0.1f;//edit this for playtesting. 1.0 for game, 0.1 for fun
        }
        timeLapse = 0;
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
            timeLapse += Time.deltaTime;
        if (timeLapse > cooldown && !canFire && (bottomRow || gameObject.name == "Tank(Clone)"))
        {
            canFire = true;
            timeLapse = 0;
            Debug.Log(gameObject.name + " Can Fire");
            //if (gameObject.name == "Tank(Clone)")//later remove this so all reset here
            //{
                m_Animator.ResetTrigger("Firing");
            //}
        }
        if (gameObject.name == "Tank(Clone)")
        {
            if (Input.GetButton("Jump") && canFire)
            {
                canFire = false;
                m_Animator.SetTrigger("Firing");
                Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y + 1.4f, -.5f);//p.transform.position.z
                p.velocity = new Vector3(0, 15, 0);
                Debug.Log("Player Fired!");
                
            }
        }else if (bottomRow)
        {
            if (canFire)
            {
                m_Animator.SetTrigger("Firing");
                canFire = false;
                Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
                p.transform.position = new Vector3(p.transform.position.x, p.transform.position.y - 1.4f, 0);//p.transform.position.z
                p.velocity = new Vector3(0, -15, 0);
                Debug.Log("Enemy(Clone) Fired!");
                float firerate = guiSc.enemyCount/2;
                cooldown = Random.Range(firerate/1.0f, firerate/20.0f);
            }
        }
    }
}
