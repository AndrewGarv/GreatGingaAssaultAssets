using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : MonoBehaviour
{
    public GameObject ImpactEnd;
    public AudioSource audiodata;
    public AudioClip explodeSnd;
    private Manager mng;
    private Spawner Spawn;
    // Start is called before the first frame update
    void Start()
    {
        GameObject myObject = GameObject.Find("AudioPlayer2");
        Spawn = GameObject.Find("RespawnPoint").GetComponent<Spawner>();
        audiodata = myObject.GetComponent<AudioSource>();
        audiodata.enabled = true;
        audiodata.clip = explodeSnd;
        mng = GameObject.Find("StateManager").GetComponent<Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audiodata.Play();
        Destroy(Instantiate(ImpactEnd, transform.position, transform.rotation, transform.parent), 1);
        Destroy(gameObject);
        mng.enemyLife--;
    }
}
