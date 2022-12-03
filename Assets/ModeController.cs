using System.Linq;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOriginalMode = true;
    public TextMeshProUGUI modeText;
    public GameObject scoreController;
    private ScoreController _scoreController;
    private int numberOfModifyTypes = 4;
    private int numberOfConsecutiveHit = 5;
    private List<int> indexArray;
    private int arrayCounter;
    private bool canCountUp = true;
    private bool canCountUpArray = true;
    private bool hitFlag;
        
    void Start()
    {
        indexArray = new List<int>();
        for (var i = 0; i < numberOfModifyTypes; i++)
        {
            indexArray.Add(i);
        }
        indexArray = indexArray.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle list
        _scoreController = scoreController.GetComponent<ScoreController>();
        modeText.text = isOriginalMode ? "Original" : "Modified";
    }

    void updateModeState()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            isOriginalMode = !isOriginalMode;
        }
        modeText.text = isOriginalMode ? "Original" : "Modified";
    }

    public int chooseModifyType()
    {
        if (isOriginalMode)
        {
            return -1;
        }
        var mod = _scoreController.count % numberOfConsecutiveHit;
        // modが0になったら1周している
        if (mod == 0 && canCountUp && _scoreController.count != 0)
        {
            arrayCounter++;
            canCountUp = false;
        }
        if (mod != 0)
        {
            canCountUp = true;
        }

        var arrayMod = arrayCounter % numberOfModifyTypes;
        if (arrayMod == 0 && canCountUpArray && _scoreController.count >= numberOfConsecutiveHit * numberOfModifyTypes)
        {
            indexArray = indexArray.OrderBy(a => Guid.NewGuid()).ToList(); // Shuffle list
            canCountUpArray = false;
        }
        if (arrayMod != 0)
        {
            canCountUpArray = true;
        }

        return indexArray[arrayMod];
    }

    // Update is called once per frame
    void Update()
    {
        updateModeState();
    }
}
