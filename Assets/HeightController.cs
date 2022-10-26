using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject cameraRig;
    public GameObject putterBottom;
    public GameObject floorLevel;
    
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            FixHeight();
        }
    }

    void FixHeight()
    {
        float heightDiff = floorLevel.transform.position.y - putterBottom.transform.position.y;
        cameraRig.transform.position += Vector3.up * heightDiff;
    }
}