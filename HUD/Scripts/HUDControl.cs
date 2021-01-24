using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

using TMPro;

using Concept2Client;
using CSAFE_Fitness;

public class HUDControl : MonoBehaviour
{
    private TMPro.TextMeshPro _MachineStatus;
    private TMPro.TextMeshPro _WorkoutCadence;
    private TMPro.TextMeshPro _WorkoutDistance;
    private TMPro.TextMeshPro _WorkoutDuration;
    private TMPro.TextMeshPro _WorkoutCalories;
    private TMPro.TextMeshPro _WorkoutHeartRate;
    private TMPro.TextMeshPro _WorkoutPower;

    private RowerClient _Rower;

    // Start is called before the first frame update
    void Start()
    {
        ConnectToRower();


    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
    }

    private void ConnectToRower()
    {
        _Rower = GameObject.Find("RowerClient").GetComponent<RowerClient>();
        Debug.Log(_Rower.RowerState);

        _MachineStatus = GameObject.Find("HUD_MachineStatus").GetComponent<TextMeshPro>();
        _MachineStatus.SetText("Hello, World!");

        _MachineStatus = GameObject.Find("HUD_MachineStatus").GetComponent<TextMeshPro>();
        _WorkoutCadence = GameObject.Find("HUD_WorkoutCadence").GetComponent<TextMeshPro>();
        _WorkoutDistance = GameObject.Find("HUD_WorkoutDistance").GetComponent<TextMeshPro>();
        _WorkoutDuration = GameObject.Find("HUD_WorkoutDuration").GetComponent<TextMeshPro>();
        _WorkoutCalories = GameObject.Find("HUD_WorkoutCalories").GetComponent<TextMeshPro>();
        _WorkoutHeartRate = GameObject.Find("HUD_WorkoutHeartRate").GetComponent<TextMeshPro>();
        _WorkoutPower = GameObject.Find("HUD_WorkoutPower").GetComponent<TextMeshPro>();
    }

    private void UpdateHUD()
    {
        if (_Rower == null) return;

        _MachineStatus.SetText(_Rower.RowerState.ToString());

        switch (_Rower.RowerState)
        {
            case MachineState.InUse:
            case MachineState.Paused:
                UpdateHUDWorkoutValues();
                break;

            case MachineState.Ready:
            case MachineState.Idle:
            case MachineState.OffLine:
            default:
                _WorkoutCadence.SetText(string.Empty);
                _WorkoutDistance.SetText(string.Empty);
                _WorkoutDuration.SetText(string.Empty);
                _WorkoutCalories.SetText(string.Empty);
                _WorkoutHeartRate.SetText(string.Empty);
                _WorkoutPower.SetText(string.Empty);
                break;

        }

    }

    private void UpdateHUDWorkoutValues()
    {
        string tHUDStr;

        tHUDStr = string.Format("{0}spm", _Rower.CadenceSPM.ToString());
        _WorkoutCadence.SetText(tHUDStr);

        tHUDStr = string.Format("{0}m", _Rower.WorkoutDistanceMeters.ToString());
        _WorkoutDistance.SetText(tHUDStr);

        int tDurSTotal = _Rower.WorkoutDurationSeconds;
        int tDurHr = tDurSTotal / 3600;
        int tDurMin = (tDurSTotal - 3600 * tDurHr) / 60;
        int tDurSec = (tDurSTotal - 3600 * tDurHr) - (tDurMin * 60);

        if (tDurHr > 0) tHUDStr = string.Format("{0}:{1}:{2}", tDurHr.ToString("00"), tDurMin.ToString("00"), tDurSec.ToString("00"));
        else tHUDStr = string.Format("{0}:{1}", tDurMin.ToString("00"), tDurSec.ToString("00"));
        _WorkoutDuration.SetText(tHUDStr);

        tHUDStr = string.Format("{0}cal", _Rower.CaloriesCal.ToString());
        _WorkoutCalories.SetText(tHUDStr);

        tHUDStr = string.Format("{0}bpm", _Rower.HeartRateBPM.ToString());
        _WorkoutHeartRate.SetText(tHUDStr);

        tHUDStr = string.Format("{0}W", _Rower.PowerWatts.ToString());
        _WorkoutPower.SetText(tHUDStr);
    }

}
