using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    public Vector3 inputVec;

    Rigidbody rigid;
    public float speed;

    public bool isJump = false;
    float time = 1;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        inputVec.x = Input.GetAxisRaw("Horizontal");
        //inputVec.y = Input.GetAxisRaw("Vertical");


        

        if (isJump)
        {
            inputVec.y = 3f;
            time -= Time.deltaTime;
        }
        else
        {
            if (Input.GetButtonDown("Jump"))
            {
                isJump = true; 
            }
            inputVec.y = 0f;
        }

        if (time < 0)
        {
            isJump = false;
            time = 1f;
        }
    }

    void FixedUpdate()
    {
        Vector3 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }
}
