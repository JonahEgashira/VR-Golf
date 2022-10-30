using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    // Start is called before the first frame update
    private static ScoreController _instance;
    public static ScoreController Instance { get; private set; }

    public int count;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI endText;
    private List<string> distance = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            StoreResultInDevice();
            // StoreResultInPC();
            endText.text = "Finished!";
        }
        showScore();
    }

    private void showScore()
    {
        countText.text = count.ToString();
    }

    public void pushData(float dist, float x, float y)
    {
        var _dist = Math.Round((decimal)dist, 4).ToString();
        var _x = Math.Round((decimal)x, 4).ToString();
        var _y = Math.Round((decimal)y, 4).ToString();
        distance.Add(_dist + "," + _x + "," + _y);
    }
    
    private void StoreResultInPC()
    {
        var dateStr = DateTime.Now.ToString("MM_dd_HH_mm");
        var path = Application.dataPath + "/Distances/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var filePath = path + dateStr + ".csv";
        File.WriteAllLines(filePath, distance);
    }

    private void StoreResultInDevice()
    {
        var dateStr = DateTime.Now.ToString("MM_dd_HH_mm");
        var path = Application.persistentDataPath + "/Distances/";
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        var filePath = path + dateStr + ".csv";
        File.WriteAllLines(filePath, distance);
    }
}
