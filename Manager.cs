/* Manager Script
    This script generally manages the flow of the game. It controls the state of the player character as well as the conditions for failure.
    Several other classes make use of the Manager class
    */
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public float lives = 1;//Amount of lives the player has
    public float enemyLife = 0;//Amount of enemies that have been destroyed. Used to calculate score
    public float ammo;//Amount of ammo the player has. Running out is a GAME OVER
    public float playerST = 0;
    public GameObject Lazeron;//The "Lazerons" are the names of the enemies. These are 4 objects dedicated to them. Bit of a useless variable as it is better used in the BegginerLevels script
    public GameObject LazeronWave;
    public GameObject LazeronFighter;
    public GameObject LazeronCommon;
    private bool respawnChecker = true;//A boolean to determine when a respawn should happen
    private GameObject StartText;//The object for the "Press Tab" text
    private BegginerLevels StartLevelB;//An object for the BegginerLevels script
    private GameObject PlayerCharacter;//Object for the Ship
    private GameObject GameOverText;//An object for the Game Over text
    private Spawner spnl;//An object for the respawning script
    private bool GameOverActivater = false;//A boolean made to ensure game overs happen properly
    private bool TimerRunner = true;//A timer. This one is not used so far
    private TextWriter GameOverObject;
    private DriveMove MovementObject;
    private GameObject activateSpawn;
    public List<GameObject> EnemyArray = new List<GameObject>();
    private string targetTag = "CPUOpponent";
    Camera mainCamera;
    public enum States { IDLE, ACTIVE, DEFEATED}//The three states of the ship. When Idle nothing will be happening. When Active, gameplay will be going on and when Defeated a game over has occured.
    public States shifter;
    /* Manager: Start
    Called before the first frame. This initializes many of the objects used in the game including the camera.
    */
    void Start()
    {
        mainCamera = Camera.main;
        StartLevelB = GetComponent<BegginerLevels>();
        PlayerCharacter = GameObject.FindWithTag("MainCharacterT");
        StartText = GameObject.Find("Text");
        activateSpawn = GameObject.Find("RespawnPoint");
        spnl = GameObject.Find("RespawnPoint").GetComponent<Spawner>();
        GameOverObject = GameObject.Find("EndText").GetComponent<TextWriter>();
        GameOverText = GameObject.Find("EndText");
        activateSpawn.SetActive(true);
        StartLevelB.enabled = false;
        EnemyArray.Add(Lazeron);//The Lazeron enemies are added to the EnemyArray. During gameplay, a random array is chosen and spawns that enemy to fight
        EnemyArray.Add(LazeronWave);
        EnemyArray.Add(LazeronFighter);
        EnemyArray.Add(LazeronCommon);
        shifter = States.IDLE;//The state of the ship. It is set to idle since nothing is happening at this point
        GameOverObject.enabled = true;
        ammo = 50;
    }
    /* Rephase()
     * This is identical to the Start() method. It is called after a game over to restart the game with some minor differences.
    */
    void Rephase()
    {
        shifter = States.IDLE;//Ship returns to an idle state since nothing is going on until the player presses tab again
        ammo = 80;
        GameOverActivater = false;//Set to false as orirginally constructed
        StartCoroutine(TriggerMethodAfterDelay(3f));
        lives = 1;//Resets lives
        StartText.SetActive(true);//Re-enables the "Press Tab" text
        mainCamera = Camera.main;
        StartLevelB = GetComponent<BegginerLevels>();
        PlayerCharacter = GameObject.FindWithTag("MainCharacterT");
        StartText = GameObject.Find("Text");
        activateSpawn = GameObject.Find("RespawnPoint");
        spnl = GameObject.Find("RespawnPoint").GetComponent<Spawner>();
        GameOverObject = GameObject.Find("EndText").GetComponent<TextWriter>();
        activateSpawn.SetActive(true);
        StartLevelB.enabled = false;//Turn off Object
        EnemyArray.Add(Lazeron);
        EnemyArray.Add(LazeronWave);
        EnemyArray.Add(LazeronFighter);
        EnemyArray.Add(LazeronCommon);
        GameOverObject.enabled = true;
    }

    // Update is called once per frame
    /* Manager Update
     * This method generally dictates when the game starts and ends. It begins with a check to see if the player has pressed the tab button or not. Once that button has been pressed the game will start by enabling
     * StartLevelB. Throughout the game, the script is constantly checking to see if the game over conditions have been fulfilled. When they have then a Game Over will be triggered. There are two ways a Game Over
     * can be achieved.
     * 
     * 1. By running out of lives
     * 
     * 2. By running out of ammo
    */
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Tab) && shifter == States.IDLE)//Activates when tab is pressed while in an idle state
        {
            StartText.SetActive(false);
            shifter = States.ACTIVE;//Be sure to increase the scrolling speed as well and play music
            StartLevelB.enabled = true;
        }
        //Make an if statement that checks what the current Shifter state is. Depending on that state it will call a level

        if (PlayerCharacter == null && lives > -1)
        {
            StartCoroutine(TriggerMethodAfterDelay(3f));
            respawnChecker = false;
        }
        else if(lives <= -1)//Game Over activates when lives have run out
        {
            
            
            if(GameOverActivater == false)
            {
                GameOverText.SetActive(true);//Activate Game Over text object
                shifter = States.DEFEATED;//Ship is now in a defeated state
                GameOverObject.Redo();//Display the Game Over text
                GameOverActivater = true;
                StartCoroutine(StartAfterGameOver(7f));//Begins method in 7 frames
            }
        }

        if(ammo <= 0)//The second way a Game Over is achieved by running out of ammo
        {
            UnityEngine.Debug.Log("Ammo was depleted");
            GameOverText.SetActive(true);//Activate Game Over text object
            shifter = States.DEFEATED;//Ship is now in a defeated state
            GameOverObject.Redo();
            GameOverActivater = true;
            StartCoroutine(StartAfterGameOver(7f));
        }







    }
    /* FirstLevel
    * A mostly useless method. Should be removed
    * 
   */
    void FirstLevel()
    {
     //If Boolean value that will be triggered by there not being any enemies on screen shows up
        if(ammo == 0)
        {
            ammo = 5;
        }


 
       
        //From here on out, input enemies. It is simply a matter of activating them/creating them. Be sure to make an if statement to make sure they don't activate over and over again. Basically you only want to ACTIVATE the enemies. The update method 
        //in their designated scripts will be what keeps them moving
    }

 /********************************************//**
 *  GameOver
 * This method activates the Game Over text and resets the game with the Rephase method
 ***********************************************/
    void GameOver()
    {
        if (GameOverActivater == false)
        {
            GameOverObject.Redo();
            GameOverActivater = true;
            Rephase();
        }
        //Go to reset method after this
    }
/********************************************//**
*  TriggerEveryTwentySeconds
* A useless method. Should be removed
***********************************************/
    private IEnumerator TriggerEveryTwentySeconds()
    {
        while (TimerRunner)
        {
            if(!EnemiesRemainingCheck())//This makes no sense. This should be "If there are no enemies on screen, trigger score display". Right now it is triggering it IF there are enemies on screen
            {
                TimerRunner = false;
            }
            
            yield return new WaitForSeconds(10f);
        }
    }
 /********************************************//**
 *  TriggerMethodAfterDelay
 * This method activates the respawn method. It does this at set intervals to give the player some time to process their defeat. Checks to see if the Ship object is on screen and replaces it with a clone if not
 ***********************************************/
    private IEnumerator TriggerMethodAfterDelay(float delayInSeconds)//Respawn method
    {
        yield return new WaitForSeconds(delayInSeconds);
        switch (respawnChecker)
        {
            case false:
                spnl.Reset();
                PlayerCharacter = GameObject.FindWithTag("MainCharacterT");
                respawnChecker = true;
                break;
            case true:
                UnityEngine.Debug.Log("Method was called in error");
                break;
        }

    }
/********************************************//**
* StartAfterGameOver
* Activates the Game Over text and then starts the game over with the Rephase() method
***********************************************/
    private IEnumerator StartAfterGameOver(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        GameOverActivater = true;
        GameOverText.SetActive(false);
        Rephase();

    }
/********************************************//**
*  ScoreDisplay()
*  A useless method. Should be removed
***********************************************/
    void ScoreDisplay()
    {
        //When this is triggered ACTIVATE THE LEVEL SCRIPT The level script will have a start and a repeat method just like the spawn script
    }
 /********************************************//**
    *  EnemiesRemainingCheck()
    *  This is not a completely useless method. While it is not used as it is right now, it does serve as the template for the actual implementation found in the BegginerLevels.cs script.
    *  It is essentially just signalling if objects of a certain tag are on screen or not. As it is right now it is entirely for debugging purposes.
 ***********************************************/
    public bool EnemiesRemainingCheck()//Every 20 seconds we check to see if enemies are on the screen
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(targetTag);
        foreach (GameObject obj in objectsWithTag)
        {
            Vector3 viewportPosition = mainCamera.WorldToViewportPoint(obj.transform.position);
            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1)
            {
                UnityEngine.Debug.Log("There are objects on the screen");
                return true;
            }
            else
            {
                UnityEngine.Debug.Log("There are no enemies on the screen");
                return false;
            }
        }
        UnityEngine.Debug.Log("There are no enemies on the screen");
        return false;
            
    }
}
