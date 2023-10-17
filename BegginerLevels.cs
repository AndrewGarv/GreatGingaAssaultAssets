/********************************************//**
*  BegginerLevels.cs
* This script exists to create enemies for the player to fight. It is essentially a "level" in the game. I generally use the term "wave" when referring to levels in this game.
* This is called BegginerLevels as it is the easiest type of waves that the player can fight. It looks at the EnemyArray list and chooses a random enemy from it to display
* on the field after each wave is completed. As of right now there are 4 enemy types. New enemies can be added fairly easily by simplying adding a new object to the list
***********************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Random = UnityEngine.Random;
public class BegginerLevels : MonoBehaviour
{
    private Manager manage;//The Manager script object.
    public Scoring scr;//The Scoring script object found in ScoreText
    private TextWriterMod twm;//The TextWriterMod script object found in LevelText
    private GameObject textObject;
    public bool LevelEnd;
    public GameObject Lazeron;
    public GameObject LazeronWave;
    public GameObject LazeronFighter;
    public GameObject LazeronCommon;
    public string targetTag = "CPUOpponent";//Tag that recognizes enemies
    public string ProjTag = "MiscObj";//Tag that recognizes projeciltes
    public int LevelCount = 1;
    private int FirstEnemy;
    private int SecondEnemy;
    private int ThirdEnemy;
    private GameObject[] objectsToDelete;//Array of objects that need to be deleted
    public List<GameObject> EnemyArray = new List<GameObject>();//The array which holds the enemies.
    private Vector3 spawnPosition1 = new Vector3(8f, -4.98f, 0f);//The three spawn positions are set in stone. I might rebalance this to add some random variance.
    private Vector3 spawnPosition2 = new Vector3(10.59f, 2.03f, 0f);
    private Vector3 spawnPosition3 = new Vector3(0.1014f, 2f, 0f);
    private Vector3 myScale = new Vector3(1.981554f, 1.825501f, 1f);
    // Start is called before the first frame update
/********************************************//**
*  BegginerLevels Start
*  Basic constructor that initializes variables. 4 enemies are added to the EnemyArray, it then calls the EnemySpawns method to spawn a random set of enemies
***********************************************/
    void Start()
    {
        textObject = GameObject.Find("LevelText");
        scr = GameObject.Find("StateManager").GetComponent<Scoring>();
        objectsToDelete = GameObject.FindGameObjectsWithTag(ProjTag);
        twm = GameObject.Find("LevelText").GetComponent<TextWriterMod>();
        EnemyArray.Add(Lazeron);
        EnemyArray.Add(LazeronWave);
        EnemyArray.Add(LazeronFighter);
        EnemyArray.Add(LazeronCommon);
        int listSize = EnemyArray.Count;
        UnityEngine.Debug.Log(listSize);
        LevelEnd = false;
        manage = GameObject.Find("StateManager").GetComponent<Manager>();
        EnemySpawns();
    }
    /********************************************//**
*  TitleScreen()
*  Simply calls the "Next Level" text
***********************************************/
    void TitleScreen()
    {
        twm.Redo();
    }
/********************************************//**
*  EnemySpawns()
*  The method that spawns the enemies every wave. Your ammo is set back to 5 and 3 enemies are placed at designated spots on the screen.
*  These enemies are selected from the EnemyArray randomly (3 random numbers are generated). As of right now this method only spawns TWO enemies
*  but I made that decision for balancing purposes. It can easily instantiate a third.
***********************************************/
    void EnemySpawns()
    {
        manage.ammo = 5;
        FirstEnemy = UnityEngine.Random.Range(0, EnemyArray.Count);
        SecondEnemy = UnityEngine.Random.Range(0, EnemyArray.Count);
        ThirdEnemy = UnityEngine.Random.Range(0, EnemyArray.Count);
        EnemyArray[FirstEnemy].transform.localScale = myScale;
        Instantiate(EnemyArray[FirstEnemy], spawnPosition1, Quaternion.identity);
        EnemyArray[SecondEnemy].transform.localScale = myScale;
        Instantiate(EnemyArray[SecondEnemy], spawnPosition2, Quaternion.identity);

    }
/********************************************//**
*  BegginerLevels Update
*  Every frame this script is checking the screen to make sure there enemies on screen by checking the result of the EnemyChecker() method.
*  When no enemies are on the screen, all projectiles are destroyed (this prevents lingering projectiles from hitting new enemies) and the 
*  EnemySpawns method is called again for the new wave. Scoring is also determined by calling CountScore from the Scoring script.
***********************************************/
    // Update is called once per frame
    void Update()
    {
        if (EnemyChecker() == false && manage.shifter == Manager.States.ACTIVE)//The ship needs to be in its active state in order for any of this to happen.
        {
            textObject.SetActive(true);
            foreach (GameObject obj in objectsToDelete)
            {
                Destroy(obj);
            }
            LevelCount++;//Keeps track of how many waves we are in
            TitleScreen();
            EnemySpawns();
            scr.CountScore();
            twm.enabled = true;//Activates the "Next Level" object
            twm.Redo();//May be unncessary
            StartCoroutine(TriggerMethodAfterDelay(8f));//Let's the "Next Level" text play out for a couple seconds
        }
        else if (manage.shifter == Manager.States.DEFEATED)//If you are in a defeated state then nothing will happen. This should probably be a Switch statement instead and is in fact probably unnecessary
        {
            UnityEngine.Debug.Log("Do nothing");//Debug message
        }



    }
/********************************************//**
* TriggerMethodAfterDelay(float delayInSeconds)
* This method just let's the "Next Level" text play off for a couple of seconds before turning it off
***********************************************/

    private IEnumerator TriggerMethodAfterDelay(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        textObject.SetActive(false);
    }
/********************************************//**
*  EnemyChecker()
*  Checks to see if any enemies are on screen. Returns false if there's none on the screen. It specifically checks to see if any objects with the "CPUOpponent" tag are on screen
*  so always make sure enemies have this tag on them.
***********************************************/
    public bool EnemyChecker()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(targetTag);

        foreach (GameObject obj in taggedObjects)
        {
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);
            if (screenPos.x >= 0 && screenPos.x <= Screen.width && screenPos.y >= 0 && screenPos.y <= Screen.height)
            {
                
                return true;
            }
        }
        return false;
    }
}