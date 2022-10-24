using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class putterController : MonoBehaviour
{

    public GameObject putter;
    public GameObject point1;
    public GameObject point2;
    private float thrust = 50f;
    private int queueLimit = 10;
    private Queue<Vector3> accQueue;
    
    // Start is called before the first frame update
    void Start()
    {
        accQueue = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        var putterAcc = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        if (accQueue.Count > queueLimit)
        {
            accQueue.Dequeue();
        }
        accQueue.Enqueue(putterAcc);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Debug.Log("Entered");
            moveBall(other);
        }
    }

    private Vector3 averageAcc()
    {
        if (accQueue.Count == 0)
        {
            return new Vector3(0f, 0f, 0f);
        }
        var count = accQueue.Count;
        var result = new Vector3();
        while (accQueue.Count != 0)
        {
            result += accQueue.Dequeue();
        }
        return result / count;
    }

    private void moveBall(Collider other)
    {
        var ball_RigidBody = other.gameObject.GetComponent<Rigidbody>();
        var averageAcc = this.averageAcc();
        averageAcc.y = 0;
        var putterAcc = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        putterAcc.y = 0.0f;
        ball_RigidBody.AddForce(averageAcc * thrust);
        Debug.Log(averageAcc);
    }
}
