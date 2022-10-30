using System;
using System.Collections;
using System.Collections.Generic;
using OVR;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private bool _isHit;
    public float thrust = 0.1f;
    private int queueLimit = 10;
    private Queue<Vector3> accQueue;
    public GameObject ballController;
    private BallController _controller;
    private Rigidbody _rigidBody;
    public AudioSource audioSource;

    private void Start()
    {
        accQueue = new Queue<Vector3>();
        _rigidBody = gameObject.GetComponent<Rigidbody>();
        _controller = ballController.GetComponent<BallController>();
    }

    private void Update()
    {
        var putterAcc = OVRInput.GetLocalControllerAcceleration(OVRInput.Controller.RTouch);
        if (accQueue.Count > queueLimit)
        {
            accQueue.Dequeue();
        }
        accQueue.Enqueue(putterAcc);

        if (hasBallStopped())
        {
            var dist = calcScore(gameObject.transform.position);
            var x = gameObject.transform.position.x;
            var y = gameObject.transform.position.y;
            ScoreController.Instance.pushData(dist, x, y);
            ScoreController.Instance.count++;
            Destroy(gameObject);
            _controller.GenerateBall();
        }
    }

    private float calcScore(Vector3 ballPos)
    {
        const float targetX = -2.0f;
        const float targetY = 0.0f;
        var dx = Math.Abs(ballPos.x - targetX);
        var dy = Math.Abs(ballPos.y - targetY);
        var dist = (float)Math.Sqrt(dx * dx + dy * dy);
        return dist;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Putter") && !_isHit)
        {
            audioSource.Play();
            _isHit = true;
            moveBall();
            VibrationExtension.Instance.
                VibrateController(0.05f, 1, 0.5f, OVRInput.Controller.RTouch);
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

    private bool hasBallStopped()
    {
        if (!_isHit)
        {
            return false;
        }

        var x = gameObject.transform.position.x;
        var y = gameObject.transform.position.y;
        
        // out of area
        if (Math.Abs(x) > 3.0f || Math.Abs(y) > 0.5f)
        {
            return true;
        }
        
        // stopped
        if (_rigidBody.velocity.magnitude < 0.05f)
        {
            return true;
        }
        
        return false;
    }

    private void moveBall()
    {
        var averageAcc = this.averageAcc();
        averageAcc.y = 0.0f; 
        _rigidBody.AddForce(averageAcc * thrust, ForceMode.Impulse);
    }
}
