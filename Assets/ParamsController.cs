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
    public TextMeshProUGUI dynamicFriction;
    public TextMeshProUGUI ballMass;
    public TextMeshProUGUI ballDrag;
    public TextMeshProUGUI ballAngularDrag;
    public TextMeshProUGUI arrowDynamicFriction;
    public TextMeshProUGUI arrowBallMass;
    public TextMeshProUGUI arrowBallDrag;
    public TextMeshProUGUI arrowBallAngularDrag;

    private const float _deltaDF = 0.01f;
    private const float _deltaBM = 0.01f;
    private const float _deltaBD = 0.01f;
    private const float _deltaBAD = 0.01f;
    private const int _paramNumber = 4;
    private int index;
    
    // 1. floor dynamic friction
    // 2. ball mass
    // 3. ball drag
    // 4. ball angular drag
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _ballRigidbody = ball.GetComponent<Rigidbody>();
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

        switch (index)
        {
            case 0:
                arrowDynamicFriction.text = "<--";
                break;
            case 1:
                arrowBallMass.text = "<--";
                break;
            case 2:
                arrowBallDrag.text = "<--";
                break;
            case 3:
                arrowBallAngularDrag.text = "<--";
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
        }
    }
}
