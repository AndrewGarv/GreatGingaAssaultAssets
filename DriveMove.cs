using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveMove : MonoBehaviour
{
    public float speed = 10000;
    Vector2 move;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = move * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
