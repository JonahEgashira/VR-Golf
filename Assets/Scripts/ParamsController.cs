using System;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ParamsController : MonoBehaviour
{
    // Start is called before the first frame update
    public PhysicMaterial floorMaterial;
    public GameObject modeController;
    private ModeController _modeController;
    public GameObject ball;
    private Rigidbody _ballRigidbody;
    private Ball _ball;
    public TextMeshProUGUI staticFriction;
    public TextMeshProUGUI dynamicFriction;
    public TextMeshProUGUI ballMass;
    public TextMeshProUGUI ballDrag;
    public TextMeshProUGUI ballAngularDrag;
    public TextMeshProUGUI thrust;
    public TextMeshProUGUI arrowStaticFriction;
    public TextMeshProUGUI arrowDynamicFriction;
    public TextMeshProUGUI arrowBallMass;
    public TextMeshProUGUI arrowBallDrag;
    public TextMeshProUGUI arrowBallAngularDrag;
    public TextMeshProUGUI arrowThrust;

    private const float _sf = 0.1f;
    private const float _df = 0.08f;
    private const float _bm = 0.045f;
    private const float _bd = 0.5f;
    private const float _bad = 0.1f;
    private const float _th = 0.15f;

    public List<float> modifyBallMassList = new(); 

    private const float _deltaSF = 0.01f;
    private const float _deltaDF = 0.01f;
    private const float _deltaBM = 0.001f;
    private const float _deltaBD = 0.01f;
    private const float _deltaBAD = 0.01f;
    private const float _deltaThrust = 0.01f;
    private const int _paramNumber = 6;
    private int index;

    void Start()
    {
        _ball = ball.GetComponent<Ball>();
        _ballRigidbody = ball.GetComponent<Rigidbody>();
        // modifyBallMassList = new List<float>() { 0.056f, 0.064f, 0.080f, 0.088f };
        modifyBallMassList = new List<float>() { 0.02f, 0.04f, 0.060f, 0.08f };
        _modeController = modeController.GetComponent<ModeController>();
        initializeParams();
    }

    void initializeParams()
    {
        floorMaterial.staticFriction = _sf;
        floorMaterial.dynamicFriction = _df;
        _ballRigidbody.mass = _bm;
        _ballRigidbody.drag = _bd;
        _ballRigidbody.angularDrag = _bad;
        _ball.thrust = _th;
    }

    void updateModifiedParams()
    {
        var modifyIndex = _modeController.chooseModifyType();
        if (modifyIndex == -1)
        {
            return;
        }
        var modifiedMass = modifyBallMassList[modifyIndex];
        _ballRigidbody.mass = modifiedMass;
    }
    
    // Update is called once per frame
    void Update()
    {
        updateModifiedParams();
        getStickInput();
        displayValue();
        switchActiveParam();
    }

    void displayValue()
    {
        staticFriction.text = floorMaterial.staticFriction.ToString();
        dynamicFriction.text = floorMaterial.dynamicFriction.ToString();
        ballMass.text = _ballRigidbody.mass.ToString();
        ballDrag.text = _ballRigidbody.drag.ToString();
        ballAngularDrag.text = _ballRigidbody.angularDrag.ToString();
        thrust.text = _ball.thrust.ToString();
    }

    void updateIndex()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickDown))
        {
            index++;
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickUp))
        {
            index--;
        }
        index = (index + _paramNumber) % _paramNumber;
    }

    void switchActiveParam()
    {
        arrowStaticFriction.text = "";
        arrowDynamicFriction.text = "";
        arrowBallMass.text = "";
        arrowBallDrag.text = "";
        arrowBallAngularDrag.text = "";
        arrowThrust.text = "";

        switch (index)
        {
            case 0:
                arrowStaticFriction.text = "<";
                break;
            case 1:
                arrowDynamicFriction.text = "<";
                break;
            case 2:
                arrowBallMass.text = "<";
                break;
            case 3:
                arrowBallDrag.text = "<";
                break;
            case 4:
                arrowBallAngularDrag.text = "<";
                break;
            case 5:
                arrowThrust.text = "<";
                break;
        }
    }

    void getStickInput()
    {
        updateIndex();
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            updateParams(true);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            updateParams(false);
        }
    }

    void updateParams(bool isPositive)
    {
        var sign = isPositive ? 1.0f : -1.0f;
        switch (index)
        {
            case 0:
                floorMaterial.staticFriction += _deltaSF * sign;
                break;
            case 1:
                floorMaterial.dynamicFriction += _deltaDF * sign;
                break;
            case 2:
                _ballRigidbody.mass += _deltaBM * sign;
                break;
            case 3:
                _ballRigidbody.drag += _deltaBD * sign;
                break;
            case 4:
                _ballRigidbody.angularDrag += _deltaBAD * sign;
                break;
            case 5:
                _ball.thrust += _deltaThrust * sign;
                break;
        }
    }
}
