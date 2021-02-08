using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OarControl_L : MonoBehaviour
{
    private GameObject _OarRotator;
    private GameObject _Target;

    // Start is called before the first frame update
    void Start()
    {
        _OarRotator = GameObject.Find("OarRot.L");
        _Target = GameObject.Find("HandleTarget.L");

        //Debug.Log(_OarRotator.ToString());
        //Debug.Log(_Target.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePos = _Target.transform.position - _OarRotator.transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        _OarRotator.transform.rotation = rotation;

    }
}
