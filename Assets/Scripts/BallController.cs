using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    private bool hasGenerated;
    public GameObject ball;
    private List<GameObject> _balls;
    private readonly Vector3 _spawnPos = new(0.0f, 0.2f, 0.0f);
    
    void Start()
    {
        _balls = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B) && !hasGenerated)
        {
            GenerateBall();
            hasGenerated = true;
        }
    }

    public void GenerateBall()
    {
        var newBall = Instantiate(ball, _spawnPos, Quaternion.identity);
        _balls.Add(newBall);
    }
}
