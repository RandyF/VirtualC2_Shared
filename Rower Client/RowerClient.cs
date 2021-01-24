using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

using Concept2api;
using CSAFE_Fitness;



namespace Concept2Client
{



    public class RowerClient : MonoBehaviour
    {

        AJRUtil.ARJ_Utils _Util = new AJRUtil.ARJ_Utils();

        private CSAFE_Fitness.CSAFE_ConfigurationData _C2Data = new CSAFE_Fitness.CSAFE_ConfigurationData();

        // Remote endpoint for the socket.
        IPHostEntry ipHost;
        IPAddress ipAddr;
        IPEndPoint localEndPoint;

        public MachineState RowerState
        {
            get
            {
                if (_C2Data == null) return MachineState.Error;
                return _C2Data.SlaveState;
            }
        }

        public int WorkoutDistanceMeters
        {
            get
            {
                if (_C2Data == null) return -1;
                return _C2Data.WorkoutDistanceMeters;
            }
        }
        public int WorkoutDurationSeconds { get { return _C2Data.WorkoutTotalSeconds; } }
        public int CadenceSPM { get { return _C2Data.Cadence; } }
        public int CaloriesCal { get { return _C2Data.Calories; } }
        public int HeartRateBPM { get { return _C2Data.HeartRateBPM; } }
        public int PowerWatts { get { return _C2Data.PowerWatts; } }

        private int _PollCount = 0;
        public int PollCount { get { return _PollCount; } }
        private bool InWorkout = false;


        // Start is called before the first frame update
        void Start()
        {
            InitializeClient();
            C2_LoadDistanceWorkout();
            C2_StartWorkout();
        }

        ~RowerClient()
        {
            CancelInvoke("_PollMonitorData");
        }

        // Update is called once per frame
        void Update()
        {
            if(_C2Data.SlaveState == MachineState.Finished ||
                (InWorkout && _C2Data.SlaveState == MachineState.Ready) )
            {
                C2_EndWorkout();
            }
        }

        public bool C2_LoadDistanceWorkout()
        {
            return ExecuteServerCommand(C2ServerMessage.LOAD_DISTANCE_WORKOUT);
        }


        public bool C2_StartWorkout()
        {
            bool tRet = ExecuteServerCommand(C2ServerMessage.START_WORKOUT);
            InvokeRepeating("_PollMonitorData", 0f, 0.100f);
            InWorkout = true;
            return tRet;
        }


        public bool C2_EndWorkout()
        {
            CancelInvoke("_PollMonitorData");
            InWorkout = false;
            return true;
        }


        void _PollMonitorData()
        {
            ExecuteServerCommand(C2ServerMessage.GET_WORKOUT_DATA);
            _PollCount++;
        }


        void InitializeClient()
        {
            // Establish the remote endpoint for the socket. This example uses port 11111 on the local computer. 
            ipHost = Dns.GetHostEntry(Dns.GetHostName());
            ipAddr = ipHost.AddressList[0];
            localEndPoint = new IPEndPoint(ipAddr, 11111);
        }


        bool ExecuteServerCommand(C2ServerMessage Command = C2ServerMessage.GET_CONFIG_DATA)
        {
            try
            {
                // Creation TCP/IP Socket using Socket Class Costructor 
                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    // Connect Socket to the remote endpoint using method Connect() 
                    sender.Connect(localEndPoint);
                    //Debug.Log(string.Format("Socket connected to -> {0} ", sender.RemoteEndPoint.ToString() ));

                    // Creation of messagge that we will send to Server 
                    byte[] txBuffer = { (byte)Command };
                    // TODO APPEND COMMAND PARAMETERS

                    int byteSent = sender.Send(txBuffer);

                    // Data buffer 
                    byte[] rxBuffer = new byte[2048];

                    // We receive the messagge using the method Receive(). This method returns number of bytes received, that we'll use to convert them to string 
                    int byteRecv = sender.Receive(rxBuffer);
                    byte[] rxData = new byte[byteRecv];
                    Array.Copy(rxBuffer, rxData, byteRecv);
                    //Debug.Log(_Util.DumpHex(rxData) );

                    _C2Data = _Util.DeSerializedConfigData(rxData);

                    // Close Socket using the method Close() 
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                    return true;
                }

                // Manage of Socket's Exceptions 
                catch (System.ArgumentNullException ane)
                {

                    Debug.Log(string.Format("ArgumentNullException : {0}", ane.ToString()));
                }

                catch (SocketException se)
                {

                    Debug.Log(string.Format("SocketException : {0}", se.ToString()));
                }

                catch (System.Exception e)
                {
                    Debug.Log(string.Format("Unexpected exception : {0}", e.ToString()));
                }
            }

            catch (System.Exception e)
            {

                Debug.Log(e.ToString());
            }

            return false;
        }

    }

}


namespace AJRUtil
{


    public class ARJ_Utils
    {
        public string DumpHex(byte[] buff)
        {
            int iIdx = 0;
            string tStr = String.Format("[{0,03}] ---\n", buff.Length);
            foreach (byte b in buff)
            {
                tStr += String.Format("{0:x02} ", b);

                if (++iIdx % 16 == 0) tStr += "\n";
            }
            tStr += "\n      ---   \n";
            return tStr;
        }

        public byte[] SerializedConfigData(CSAFE_Fitness.CSAFE_ConfigurationData Data)
        {
            IFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, Data);
                return stream.ToArray();
            }
        }

        public CSAFE_Fitness.CSAFE_ConfigurationData DeSerializedConfigData(byte[] SerialData)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new MemoryStream(SerialData);

            var tObj = formatter.Deserialize(stream);

            return (CSAFE_Fitness.CSAFE_ConfigurationData)tObj;

        }
    }
}