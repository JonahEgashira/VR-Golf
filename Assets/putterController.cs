using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class putterController : MonoBehaviour
{

    public GameObject putter;
    public GameObject golfBall;
    private List<GameObject> balls;
    private float thrust = 0.1f;
    private int queueLimit = 10;
    private Queue<Vector3> accQueue;
    public Vector3 ballSpawnPosition = new Vector3(0.0f, 0.2f, 0.0f);
    
    // Start is called before the first frame update
    void Start()
    {
        balls = new List<GameObject>();
        GenerateBall();
        accQueue = new Queue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            GenerateBall();
        }

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
            moveBall(other);
        }
    }

    private Vector3 averageAcc()
    {
        var count = accQueue.Count;
        if (count == 0)
        {
            return new Vector3(0f, 0f, 0f);
        }
        var result = new Vector3();
        while (accQueue.Count != 0)
        {
            result += accQueue.Dequeue();
        }
        return new Vector3(result.x / count, result.y / count, result.z / count);
    }

    private void moveBall(Collider other)
    {
        var ball_RigidBody = other.gameObject.GetComponent<Rigidbody>();
        var averageAcc = this.averageAcc();
        averageAcc.y = 0.0f; 
        ball_RigidBody.AddForce(averageAcc * thrust, ForceMode.Impulse);
    }
    
    private void GenerateBall()
    {
        var newBall = Instantiate(golfBall, ballSpawnPosition, Quaternion.identity);
        balls.Add(newBall);
    }
    
}
