using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GuiScript : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI livesText;
    public int score, highScore, lives,enemyCount;
    private float speed, facing, enemyLost;
    private bool failed, lockhit,lockflip, endlock;
    public bool lockWin;
    public GameObject Level;
    public LevelParserStarter lSc;
    private SceneScript sSc;
    private GameObject manager;

    // Start is called before the first frame update
    void Start()
    {
        endlock = false;
        manager = GameObject.Find("SceneManager");
        sSc = manager.GetComponent<SceneScript>();
        lSc = Level.GetComponent<LevelParserStarter>();
        facing = 1;
        enemyCount = 0;
        speed = 0.05f;
        lockflip = false;
        lockhit = false;
        failed = false;
        lockWin = false;
        score = 0;
        highScore = sSc.highScore;
        lives = 3;
        livesText.text = lives.ToString("D1");
    }
    void reset()
    {
        facing = 1;
        enemyCount = 0;
        speed = 0.5f;
        lockflip = false;
        lockhit = false;
        failed = false;
        lockWin = false;
        score = 0;
        //highScore = 0;
        lives = 3;
        livesText.text = lives.ToString("D1");
        lSc.reset();
    }
    // Update is called once per frame
    void Update()
    {
        if (highScore < score)
        {
            highScore = score;
        }
        bool dirtyflip = false;
        scoreText.text = score.ToString("D4");
        highScoreText.text = highScore.ToString("D4");
        
        foreach (GameObject mobObj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            mobObj.transform.position += new Vector3((speed+enemyLost*.005f)*facing,0,0);//update to use lerp
            if (mobObj.transform.position.x > 31.5f || mobObj.transform.position.x < -1.5f)
            {
                dirtyflip = true;
            }
        }
        if (dirtyflip)
        {
            enemyCount = 0;
            foreach (GameObject mobObj in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                if (!mobObj.GetComponent<EnemyScript>().dead)
                    enemyCount += 1;
                mobObj.transform.position += new Vector3(0, -.5f, 0);//update to use lerp
            }
            updateFacing();
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                if (hit.collider.gameObject.name == "Shield(Clone)" || hit.collider.gameObject.name == "Enemy(Clone)")
                {
                    onAddCoin(hit);
                }
            }
        }
        if (lives <= 0 && !failed)
        {
            failed = true;
            //lockWin=true;
            Debug.Log("Player failed to beat level");
        }
        
    }
    public void updateFacing()
    {
        if (!lockflip)
        {
            lockflip = true;
            facing *= -1;
            lockflip = false;
        }
    }
    IEnumerator BlockHit(Transform Tran)
    {
        Tran.localScale *= 1.2f;
        yield return new WaitForSeconds(0.03f);
        Tran.localScale /= 1.2f;
    }
    IEnumerator END()
    {
        endlock = true;
        Debug.Log("CredsShouldRoll");
        yield return new WaitForSeconds(2.0f);
        sSc.Credits();
        Debug.Log("CREDSHOULDHAVEROLLED");
    }
    void onAddCoin(RaycastHit hit)
    {
        addScore(100);
        StartCoroutine(BlockHit(hit.transform));
    }
    public void onAddCoin(GameObject Question)
    {
        addScore(100);
        StartCoroutine(BlockHit(Question.transform));
    }
    public void killed(string name,GameObject otherObject)
    {
        if (!endlock)
        switch (name)
        {
            case "Tank(Clone)":
                if (!lockhit) {
                    lockhit = true;
                    lives -= 1;
                    livesText.text = lives.ToString("D1");
                    if (lives<=0)
                    {
                        Destroy(otherObject,1);
                        Animator anim = otherObject.GetComponent<Animator>();
                        anim.SetTrigger("Death");
                        gameOver("Player ran out of lives.");
                        StartCoroutine(END());
                    }
                    lockhit = false;
                }
                break;
            case "Enemy Lower(Clone)":
                    if (!otherObject.GetComponent<EnemyScript>().dead)
                    {
                        otherObject.GetComponent<EnemyScript>().dead = true;
                        enemyLost += 1;
                        Destroy(otherObject, 1);
                        Animator anim2 = otherObject.GetComponent<Animator>();
                        anim2.SetTrigger("Death");
                        enemyCount -= 1;
                        addScore(10);
                    }
                    break;
            case "Enemy Middle(Clone)":
                    if (!otherObject.GetComponent<EnemyScript>().dead){
                        otherObject.GetComponent<EnemyScript>().dead = true;
                        enemyLost += 1;
                        Animator anim3 = otherObject.GetComponent<Animator>();
                        anim3.SetTrigger("Death");
                        Destroy(otherObject, 1);
                        enemyCount -= 1;
                        addScore(20);
                    }
                    break;
            case "Enemy Top(Clone)":
                    if (!otherObject.GetComponent<EnemyScript>().dead)
                    {
                        otherObject.GetComponent<EnemyScript>().dead = true;
                        enemyLost += 1;
                        Destroy(otherObject, 1);
                        Animator anim4 = otherObject.GetComponent<Animator>();
                        anim4.SetTrigger("Death");
                        addScore(30);
                        enemyCount -= 1;
                    }
                    break;
            case "Enemy Boss(Clone)":
                Destroy(otherObject,1);
                Animator anim5 = otherObject.GetComponent<Animator>();
                anim5.SetTrigger("Death");
                addScore(Random.Range(30,100));
                break;
            case "Shield(Clone)":
                Destroy(otherObject);
                break;

        }
        if (enemyCount == 0)
        {
            gameOver("Player Won! Dun dun dun dun dun-dah-daaaa!");
            setWin();
        }

    }
    public void activate(string name, int col)
    {
        //later update so it also takes row
    }
    public void addScore(int val)
    {
        score += val;
    }
    public void gameOver(string reason)
    {
        Debug.Log("Game Over, "+reason);
        sSc.highScore = highScore;
        if (!endlock)
            StartCoroutine(END());
    }
    public void setWin()
    {
        if (!lockWin)
        {
            lockWin = true;
            //failed=true;
            gameOver("Player Won! Lives remaining: "+lives);
            addScore(100 * lives);
            
            //lSc.reset();
        }
    }
}
