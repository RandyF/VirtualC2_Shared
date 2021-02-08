using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleMotionAssert : MonoBehaviour
{
    private GameObject _TargetR;
    private GameObject _TargetL;

    private Vector3 _OffsetR;
    private Vector3 _OffsetL;

    // Start is called before the first frame update
    void Start()
    {
        _TargetR = GameObject.Find("HandleTarget.R");
        _TargetL = GameObject.Find("HandleTarget.L");

        _OffsetR = _TargetR.transform.localPosition;
        _OffsetL = _TargetL.transform.localPosition;
        //Debug.Log(_OffsetR.ToString());
        //Debug.Log(_OffsetL.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        float _newX = 0f;
        float _newY = 0f;
        float _newZ = 0f;

        float _timeRads = Time.time % (2f * Mathf.PI);

        _newZ = 0.5f * Mathf.Sin(_timeRads + (0f * Mathf.PI + 0f)) + 0f;
        _newY = 0.25f * Mathf.Sin(2f * _timeRads + (0f * Mathf.PI + 0f)) + .125f;

        Vector3 _Trans = new Vector3(_newX, _newY, _newZ);

        _TargetR.transform.localPosition = _Trans + _OffsetR;
        _TargetL.transform.localPosition = _Trans + _OffsetL;
    }
}
