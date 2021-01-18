using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliCharacterstic : MonoBehaviour
{
    #region Variable
    [Header("Lift Properties")]
    public float maxLiftForce = 100f;
    public float liftPowerMultiplyier = 3f;
    public MainRotor mainRotor;
    float angleOfattack;


    [Header("Tail Rotor Properties")]
    public float tailForce = 2f;

    [Header("Cyclic Properties")]
    public float cyclicForce = 2f;
    public float CyclicforceMultiplyier = 1000f;
    public float maxAngle = 45;
    Vector3 flayFwrd;
    float frwdDot,rightDot;
    Vector3 flatRight;


    [Header("AutoLevel Properties")]
    public float AutoLevelFerce = 2f;
    #endregion

    #region BuildInregion
    void Start()
    {

    }


    void Update()
    {

    }
    #endregion

    #region CustomMethod
    public virtual void UpdateCharacterstic(Rigidbody rb,KeyBoardInput input)
    {
        HandleLift(rb,input);
        HandleCyclic(rb, input);
        HandlePadal(rb, input);
        AutoLevel(rb);
        CalculateAngle();
    }
    protected virtual void HandleLift(Rigidbody rb, KeyBoardInput input)
    {
        
        Vector3 liftForce = transform.up * (Physics.gravity.magnitude + maxLiftForce) * rb.mass;
        float normalizedRPM = mainRotor.CurrentRPM / 500f;
        rb.AddForce(liftForce* Mathf.Pow(normalizedRPM,2f) * Mathf.Pow(input.StickyCollectiveInput ,liftPowerMultiplyier));
       // Debug.Log(liftForce);
    }

    protected virtual void HandleCyclic(Rigidbody rb, KeyBoardInput input)
    {
        //angleOfattack = Vector3.Dot(rb.velocity.normalized, transform.up);
        //angleOfattack *= angleOfattack;
        //Debug.Log(angleOfattack);
        /*Vector3 HeliAngle =  transform.eulerAngles;
        HeliAngle.x = Mathf.Clamp(HeliAngle.x, maxAngle, -maxAngle);
        HeliAngle.z = Mathf.Clamp(HeliAngle.z, maxAngle, -maxAngle);*/

        float cyclicZForce = input.CyclicInput.x * cyclicForce;
        rb.AddRelativeTorque(cyclicZForce * Vector3.forward , ForceMode.Acceleration);

        float cyclicXForce = -input.CyclicInput.y * cyclicForce;
        rb.AddRelativeTorque(cyclicXForce * Vector3.right, ForceMode.Acceleration);


        Vector3 forwardVec = flayFwrd * frwdDot;
        Vector3 rightVec = flatRight * rightDot;
        Vector3 finalvec = forwardVec + rightVec;

    }
    protected virtual void HandlePadal(Rigidbody rb, KeyBoardInput input)
    {
        rb.AddTorque(Vector3.up * input.PadalInput * tailForce,ForceMode.Acceleration);
    }

    void AutoLevel(Rigidbody rb)
    {
        float rightForce = -frwdDot * AutoLevelFerce;
        float forwardForce = rightDot * AutoLevelFerce;

        rb.AddRelativeTorque(transform.right * rightForce, ForceMode.Acceleration);
        rb.AddRelativeTorque( transform.forward * forwardForce, ForceMode.Acceleration);
    }
    void CalculateAngle()
    {
        flayFwrd = transform.forward;
        flayFwrd.y = 0f;
        flayFwrd = flayFwrd.normalized;


        flatRight = transform.right;
        flatRight.y = 0f;
        flatRight = flatRight.normalized;

        frwdDot = Vector3.Dot(transform.up, flayFwrd);
        rightDot = Vector3.Dot(transform.up, flatRight);
    }
    #endregion
}
