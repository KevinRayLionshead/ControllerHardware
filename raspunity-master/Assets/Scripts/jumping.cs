
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumping : MonoBehaviour
{

    Rigidbody rigidbody;
    public NetworkConnection networkConnection;

    [SerializeField]
    float jumpVelocity = 80.0f;
    [SerializeField]
    float fallMultiplier = 2.5f;
    [SerializeField]
    public float lowJumpMultiplier = 2.5f;

    bool jump = false;
    [SerializeField]
    bool onGround = false;

    // Start is called before the first frame update
    void Start()
    {
        //sets the rigidbody to find the rigidbody attached to the dino
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // sets a bool to true which is the request for the dino to jump
        if ((Input.GetKeyDown(KeyCode.Space)) && onGround /*|| networkConnection.jump && rigidbody.velocity.y == 0.0f*/)//makes sure it is on the ground
            jump = true;
    }

    private void FixedUpdate()//physics update
    {
        if (jump)//where the jump happens
        {
            rigidbody.velocity += Vector3.up * jumpVelocity;
            jump = false;
            onGround = false;
        }

        if (rigidbody.velocity.y < 0)//where the fast fall happens
        {
            rigidbody.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rigidbody.velocity.y > 0 && !Input.GetKey(KeyCode.Space))//where the short jump happens
        {
            rigidbody.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "ground")
        {
            onGround = true;
        }
    }
}