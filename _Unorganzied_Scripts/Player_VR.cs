using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_VR : MonoBehaviour
{
    public Vector3 HMDPositionCal = new Vector3(0f, 0f, 0f);

    GameObject _LeftHand;
    GameObject _RowerHandle;

    GameObject _HandleTarget_L;
    GameObject _HandleTarget_R;
    GameObject _OarTarget_L;
    GameObject _OarTarget_R;


    GameObject _Player;
    GameObject _PlayerAnchor;

    // Start is called before the first frame update
    void Start()
    {
        _LeftHand = GameObject.Find("LeftHand");
        //Debug.Log(_LeftHand.ToString());

        _RowerHandle = GameObject.Find("RowerHandle");
        //Debug.Log(_RowerHandle.ToString());


        _OarTarget_L = GameObject.Find("HandleTarget.L");
        _OarTarget_R = GameObject.Find("HandleTarget.R");
        _HandleTarget_L = GameObject.Find("RowerHandle.L");
        _HandleTarget_R = GameObject.Find("RowerHandle.R");
        //Debug.Log(_OarTarget_L.ToString());
        //Debug.Log(_OarTarget_R.ToString());
        //Debug.Log(_HandleTarget_L.ToString());
        //Debug.Log(_HandleTarget_R.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        _RowerHandle.transform.SetPositionAndRotation(_LeftHand.transform.position, _LeftHand.transform.rotation);

        _OarTarget_L.transform.position = _HandleTarget_L.transform.position;
        _OarTarget_R.transform.position = _HandleTarget_R.transform.position;


    }
}
