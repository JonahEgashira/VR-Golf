using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ParamsController : MonoBehaviour
{
    // Start is called before the first frame update
    public PhysicMaterial floorMaterial;
    public GameObject ball;
    private Rigidbody _ballRigidbody;
    private Ball _ball;
    public TextMeshProUGUI dynamicFriction;
    public TextMeshProUGUI ballMass;
    public TextMeshProUGUI ballDrag;
    public TextMeshProUGUI ballAngularDrag;
    public TextMeshProUGUI thrust;
    public TextMeshProUGUI arrowDynamicFriction;
    public TextMeshProUGUI arrowBallMass;
    public TextMeshProUGUI arrowBallDrag;
    public TextMeshProUGUI arrowBallAngularDrag;
    public TextMeshProUGUI arrowThrust;
    
    private const float _df = 0.6f;
    private const float _bm = 0.05f;
    private const float _bd = 0.5f;
    private const float _bad = 0.6f;
    private const float _th = 0.1f;

    private const float _deltaDF = 0.01f;
    private const float _deltaBM = 0.01f;
    private const float _deltaBD = 0.01f;
    private const float _deltaBAD = 0.01f;
    private const float _deltaThrust = 0.01f;
    private const int _paramNumber = 5;
    private int index;
    
    void Start()
    {
        _ball = ball.GetComponent<Ball>();
        _ballRigidbody = ball.GetComponent<Rigidbody>();
        initializeParams();
    }

    void initializeParams()
    {
        floorMaterial.dynamicFriction = _df;
        _ballRigidbody.mass = _bm;
        _ballRigidbody.drag = _bd;
        _ballRigidbody.angularDrag = _bad;
        _ball.thrust = _th;
    }

    // Update is called once per frame
    void Update()
    {
        getStickInput();
        displayValue();
        switchActiveParam();
    }

    void displayValue()
    {
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
        arrowDynamicFriction.text = "";
        arrowBallMass.text = "";
        arrowBallDrag.text = "";
        arrowBallAngularDrag.text = "";
        arrowThrust.text = "";

        switch (index)
        {
            case 0:
                arrowDynamicFriction.text = "<";
                break;
            case 1:
                arrowBallMass.text = "<";
                break;
            case 2:
                arrowBallDrag.text = "<";
                break;
            case 3:
                arrowBallAngularDrag.text = "<";
                break;
            case 4:
                arrowThrust.text = "<";
                break;
            default:
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
                floorMaterial.dynamicFriction += _deltaDF * sign;
                break;
            case 1:
                _ballRigidbody.mass += _deltaBM * sign;
                break;
            case 2:
                _ballRigidbody.drag += _deltaBD * sign;
                break;
            case 3:
                _ballRigidbody.angularDrag += _deltaBAD * sign;
                break;
            case 4:
                _ball.thrust += _deltaThrust * sign;
                break;
        }
    }
}
