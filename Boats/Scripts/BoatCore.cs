using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Concept2Client;
using CSAFE_Fitness;


public class BoatCore : MonoBehaviour
{
    private RowerClient _Rower;

    private float _posX = 0.0f;
    private float _posY = 0.0f;
    private float _posZ = 0.0f;

    private float _TargetZ = 0.0f;
    private float _VelocityZ = 0.0f;

    private float _lastPollTime;
    private float _lastZ = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBoat();
    }


    // Update is called once per frame
    void Update()
    {
        CalculatePosition();
        PositionBoat();
    }

    private void InitializeBoat()
    {
        _Rower = GameObject.Find("RowerClient").GetComponent<RowerClient>();

        _posX = 0.0f;
        _posY = 0.0f;
        _posZ = 0.0f;
        _TargetZ = 0.0f;


        _lastPollTime = Time.time - 0.01f;
        _lastZ = 0;

    }

    private void PositionBoat()
    {
        string tDbg = string.Format("Z0:{0} Z1:{1} Vz:{2}", _posZ, _TargetZ, _VelocityZ);
        //Debug.Log(tDbg);
        transform.position = new Vector3(_posX, _posY, _posZ);
    }

    private void CalculatePosition()
    {

        if (_Rower == null) return;

        float tDeltaT = 10000;
        float tDeltaD = 0;
        float newZ = _posZ;

        float VelocityAdjustFactor = 1.015f;

        //bool updatedV = false;

        _TargetZ = (float)_Rower.WorkoutDistanceMeters;
        //Console.Write("Z0:{0} Z1:{1} ", _posZ, _TargetZ);
        //c2distances.Write("{0,10}, {1,10}", _TargetZ, _posZ);


        if (_lastZ < _TargetZ) // Update Velocity
        {
            tDeltaT = Time.time - _lastPollTime;
            tDeltaD = _TargetZ - _lastZ;

            _VelocityZ = tDeltaD / tDeltaT;

            _lastPollTime = Time.time;

            _lastZ = _TargetZ;
            //updatedV = true;

        }
        //else updatedV = false;

        //Console.Write("Vz:{0}, dT:{1} dD:{2} ", _VelocityZ, tDeltaT, tDeltaD);
        //c2distances.Write(", {0,10}, {1,10}, {2,10}, {3,10}, {4,10}", _lastZ, _VelocityZ, tDeltaT, tDeltaD, updatedV);

        newZ = _posZ + (_VelocityZ * Time.deltaTime);
        //Console.Write("Zn:{0} ", newZ);

        if (newZ < _TargetZ) // Catch Up
        {
            _VelocityZ = _VelocityZ * VelocityAdjustFactor;
        }
        if (newZ > _TargetZ) _VelocityZ = _VelocityZ / VelocityAdjustFactor;

        //if(Math.Abs(_TargetZ - newZ) < CloseLockDistance) newZ = _TargetZ;


        _posZ = newZ;

        //Console.WriteLine();
        //c2distances.WriteLine();

    }

}
