using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private GameObject floor;
    [SerializeField] private float movementForce;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        rb.AddForce(Vector2.right * movementForce);
        if(Input.GetKeyDown(KeyCode.D)){
            rb.AddForce(Vector2.right * movementForce);
        }
        if(Input.GetKeyDown(KeyCode.A)){
            rb.AddForce(Vector2.left * movementForce);
        }
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(Vector2.up * movementForce);
        }
    }

    public void Death(){
        Debug.Log("-1 health");
    }
}
