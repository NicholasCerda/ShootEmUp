using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelParserStarter : MonoBehaviour
{
    public string filename;
    public GameObject Player;
    public GameObject ELower;
    public GameObject EMiddle;
    public GameObject ETop;
    public GameObject Shield;
    public EnemyScript eSc;
    public GuiScript guiSc;
    public GameObject MainCamera;
    private int lc,mc,tc;
    public Transform parentTransform;
    // Start is called before the first frame update
    void Start()
    {
        lc = mc = tc = 0;
        RefreshParse();
        guiSc = MainCamera.GetComponent<GuiScript>();
    }
    public void reset()
    {
        foreach (GameObject mobObj in GameObject.FindGameObjectsWithTag("Bullet"))
        {
            if (mobObj.name!="Bullet")
                Destroy(mobObj);
        }
        lc = mc = tc = 0;
        guiSc.lockWin = false;
        RefreshParse();
    }
    private void FileParser()
    {
        string fileToParse = string.Format("{0}{1}{2}.txt", Application.dataPath, "/Resources/", filename);

        using (StreamReader sr = new StreamReader(fileToParse))
        {
            string line = "";
            int row = 0;
            while ((line = sr.ReadLine()) != null)
            {

                int column = 0;
                char[] letters = line.ToCharArray();
                foreach (var letter in letters)
                {
                    //Call SpawnPrefab
                    SpawnPrefab(letter,new Vector3(column, -row,-.5f));
                    column += 1;
                }
                row += 1;
            }
            sr.Close();
        }
    }

    private void SpawnPrefab(char spot, Vector3 positionToSpawn)
    {
        GameObject ToSpawn;
        switch (spot)
        {
            case 'p': Debug.Log("Spawn Player");
                ToSpawn = GameObject.Instantiate(Player, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            case 'l':
                Debug.Log("Spawn Lower Enemy");
                ToSpawn = GameObject.Instantiate(ELower, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                ToSpawn.gameObject.tag = "Enemy";
                guiSc.enemyCount += 1;
                GunScript lSc= ToSpawn.GetComponent<GunScript>();
                lSc.col = lc;
                //need to add to each enemy their number so that when destroyed the gui can check of next enemy (same number) is alive, if so give it
                //bottomrow so that it can shoot, if not, check the next enemy type, untill no more enemies remain.
                lc++;
                break;
            case 'm':
                Debug.Log("Spawn Middle Enemy");
                ToSpawn = GameObject.Instantiate(EMiddle, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                GunScript mSc = ToSpawn.GetComponent<GunScript>();
                mSc.col = mc;
                mc++;
                ToSpawn.gameObject.tag = "Enemy";
                guiSc.enemyCount += 1;
                break;
            case 't':
                Debug.Log("Spawn Top Enemy");
                ToSpawn = GameObject.Instantiate(ETop, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                GunScript tSc = ToSpawn.GetComponent<GunScript>();
                tSc.col = tc;
                tc++;
                guiSc.enemyCount += 1;
                ToSpawn.gameObject.tag = "Enemy";
                break;
            case 's': Debug.Log("Spawn Shield");
                ToSpawn = GameObject.Instantiate(Shield, parentTransform);
                ToSpawn.transform.localPosition = positionToSpawn;
                break;
            //default: Debug.Log("Default Entered"); break;
            default: return;
        }
    }

    public void RefreshParse()
    {
        GameObject newParent = new GameObject();
        newParent.name = "Environment";
        newParent.transform.position = parentTransform.position;
        newParent.transform.parent = this.transform;
        if (parentTransform)
            Destroy(parentTransform.gameObject);
        parentTransform = newParent.transform;
        FileParser();
    }
}
