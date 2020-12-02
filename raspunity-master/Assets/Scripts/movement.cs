using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class movement : MonoBehaviour
{
    public NetworkConnection networkConnection;

    Rigidbody rigidbody;

    [SerializeField]
    GameObject arm;
    [SerializeField]
    GameObject leg;

    bool left = false;
    bool right = false;

    bool facingLeft = false;
    bool facingRight = true;
    [SerializeField]
    public float movementScaler = 1.0f;//speed scalar

    bool attack = false;
    float time;
    float timecap = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        arm.SetActive(false);
        leg.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))//left
        {
            left = true;
            facingLeft = true;
            facingRight = false;
        }
        else
            left = false;

        if (Input.GetKey(KeyCode.D))//right
        {
            right = true;
            facingLeft = false;
            facingRight = true;
        }
        else
            right = false;

        if (facingLeft)// rotates the player to face left
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f,0.0f));
        else if(facingRight)//rotates the player to face right
            transform.rotation = Quaternion.Euler(new Vector3(0.0f, 180.0f, 0.0f));

        if (Input.GetKey(KeyCode.RightShift) && !attack /*|| networkConnection.punch && !attack*/)//punch
        {
            arm.SetActive(true);
            attack = true;
        }

        if (Input.GetKey(KeyCode.RightControl) && !attack /*|| networkConnection.kick && !attack*/)//kick
        {
            leg.SetActive(true);
            attack = true;
        }

        if(attack)//timer for the attack to show
        {
            time += Time.deltaTime;
        }

        if(time >= timecap)//when the timer is done stops the attack
        {
            attack = false;
            arm.SetActive(false);
            leg.SetActive(false);
            time = 0.0f;
        }

    }

    private void FixedUpdate()
    {
        if (right)//where the movement happens
            rigidbody.AddForce(Vector3.left * movementScaler);

        if(left)
            rigidbody.AddForce(Vector3.right * movementScaler);
    }
}
