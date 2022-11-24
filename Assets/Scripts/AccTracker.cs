using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class AccTracker : MonoBehaviour
{
    // Start is called before the first frame update
    private int queueLimit = 10;

    private Queue<Vector3> accQueue;
    private List<string> accList = new();
    
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
        if (OVRInput.GetDown(OVRInput.RawButton.A))
        {
            var acc = getAcc();
            var _x = Math.Round((decimal)acc.x, 4).ToString();
            var _y = Math.Round((decimal)acc.y, 4).ToString();
            var _z = Math.Round((decimal)acc.z, 4).ToString();
            accList.Add(_x + "," + _y + "," + _z);
        }

        if (OVRInput.GetDown(OVRInput.RawButton.B))
        {
            writeResults();
        }
    }

    private Vector3 getAcc()
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

    private void writeResults()
    {
        var dateStr = DateTime.Now.ToString("MM_dd_HH_mm");
        var path = Application.dataPath + "/RealAccData/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        var filePath = path + dateStr + ".csv";
        File.WriteAllLines(filePath, accList);
    }

}
