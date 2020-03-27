using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : MonoBehaviour {
    /*
    ArtNet.Engine _artNet;

    byte[] _dmxData;

    HoloToolkit.Unity.InputModule.GazeManager _gazeManager;

    SceneController _sceneController;

    
    public static MainController GetInstance()
    {
        var go = GameObject.Find("_mainController");
        return go.GetComponent<MainController>();
    }

    void Awake()
    {
        _artNet = new ArtNet.Engine();

        ResetDMX();

    }

    void Start () {
        hands = new Dictionary<uint, uint>();

        _sceneController = SceneController.GetInstance();

        _gazeManager = HoloToolkit.Unity.InputModule.GazeManager.Instance;
        //_gazeManager.FocusedObjectChanged += this.GazeChanged; 
    }


    int _nextLevel = -1;

    public void ShutDown(int nextLevel)
    {
        _nextLevel = nextLevel;
        _artNet.Shutdown();

        StartCoroutine(SwitchScene());
    }

    void Initialize()
    {
        
    }

    public void Quit()
    {
        if (_artNet != null)
        {
            _artNet.Shutdown();
        }

        Application.Quit();
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(0.1f);

        Application.LoadLevel(_nextLevel);

        yield return null;
    }


    void Update () {
        if (_nextLevel >= 0) return;

        _artNet.SendDMX(_dmxData);
    }

    public void Clear()
    {
        var list = GameObject.FindGameObjectsWithTag("Fixture");
        foreach (var entry in list)
        {
            entry.GetComponent<FixtureController>().Clear();
        }
    }

  
    public static byte[] GetEmpty512()
    {
        byte[] DMXData = new byte[512];
        for (int i = 0; i < DMXData.Length; i++)
        {
            DMXData[i] = 0;
        }

        return DMXData;
    }

    public void GlobalDimmerDown()
    {
        var list = GameObject.FindGameObjectsWithTag("Fixture");
        foreach (var entry in list)
        {
           // entry.GetComponentInChildren<HoloToolkit.Unity.GestureHandler>().DimmerDown();
        }
    }

    public void GlobalDimmerUp()
    {
        var list = GameObject.FindGameObjectsWithTag("Fixture");
        foreach (var entry in list)
        {
           // entry.GetComponentInChildren<HoloToolkit.Unity.GestureHandler>().DimmerUp();
        }
    }


    public void SetAddress(int address, int amount)
    {
        if (address <= 0) return;

        int x = amount;
        if (x < 0) x = 0;
        if (x > 255) x = 255;

        _dmxData[address - 1] = (byte)x;
    }

    public int ReadAddress(int address)
    {
        return (int)_dmxData[address - 1];
    }

    public void UpdateAddress(int address, int amount)
    {
        int x = (int)_dmxData[address - 1];

        x = x + amount;

        if (x < 0) x = 0;
        if (x > 255) x = 255;

        _dmxData[address - 1] = (byte)x;

        //Debug.Log(address + " set to " + _dmxData[address - 1]);
    }

    public byte[] CopyDMXBuffer()
    {
        var buf = new byte[_dmxData.Length];
        _dmxData.CopyTo(buf, 0);

        return buf;
    }

    int CheckInt(string s)
    {
        int x = 0;

        int.TryParse(s.Trim(), out x);
        return x;
    }

    public void DMXRotate(string addressList, int channels)
    {
        var list = addressList.Split(new char[] { ',' });
        if (list == null) return;
        if (list.Length <= 0) return;

        if (list.Length == 1) return; // Nothing to rotate

        var buf = CopyDMXBuffer();

        for (int i  = 0; i < list.Length - 1; i++)
        {
            CopyToDMX(CheckInt(list[i + 1]), CheckInt(list[i]), buf, channels);
        }

        CopyToDMX(CheckInt(list[0]), CheckInt(list[list.Length - 1]), buf, channels);
    }


    void CopyToDMX(int addressFrom, int addressTo, byte[] fromBuf, int channels)
    {
        for (int i = 0; i < channels; i++)
        {
            _dmxData[(addressTo - 1) + i] = fromBuf[(addressFrom - 1) + i];
        }
    }

    public void ResetDMX()
    {
        _dmxData = GetEmpty512();
        _artNet.SendDMX(_dmxData);
    }
    */
    /*
    void GazeChanged(GameObject previousObject, GameObject newObject)
    {
        // Gaze is handled with Focus Implementation

        /*
        if (previousObject != null)
        {
            previousObject.SendMessage("OnGazeLeave");
        }

        if (newObject != null)
        {
            newObject.SendMessage("OnGazeEnter");
        }
        */
    //}
/*
    public static string VectorToString(Vector3 v)
    {
        return "x:" + v.x.ToString("0.0000") + " y:" + v.y.ToString("0.0000") + " z:" + v.z.ToString("0.0000");
    }

    public static Vector3 ProcessVector(Vector3 v)
    {
        var processed = new Vector3(TruncateFloat(v.x), TruncateFloat(v.y), TruncateFloat(v.z));
        return processed;
    }

    public static float TruncateFloat(float f)
    {
        int x = (int)(f * 100.0f);
        return (float)x / 100.0f;
    }

    public static float RoundFloat(float f, int digits)
    {
        return (float)System.Math.Round((double)f, digits);
    }


    public static Vector3 RoundVector(Vector3 v)
    {
        return new Vector3(RoundFloat(v.x, 3), RoundFloat(v.y, 3), RoundFloat(v.z, 3));
    }


    bool[] handsUp = { false, false };

    Dictionary<uint, uint> hands;

    void SetHandsUp(uint which, bool value)
    {
        if (which < 0) return;
        if (which > 1) return;

        handsUp[which] = value;
        if (handsUp[0] == true && handsUp[1] == true)
        {
            _sceneController.TwoHandsUp();
        }
    }

    public void OnSourceDetected(HoloToolkit.Unity.InputModule.SourceStateEventData eventData)
    {
        Vector3 pos = Vector3.zero;
        eventData.InputSource.TryGetPointerPosition(eventData.SourceId, out pos);
        Debug.Log("OnSourceDetected: " + eventData.SourceId.ToString() + " " + pos);

        uint sourceId = eventData.SourceId;

        uint which = 0;
        if (pos.x > 0) which = 1;

        hands.Remove(sourceId);

        hands.Add(sourceId, which);

        SetHandsUp(which, true);

        if (which == 0)
        {
            _sceneController.LeftHandUp(eventData);
        }

        if (which == 1)
        {
            _sceneController.RightHandUp(eventData);
        }
    }

    public void OnSourceLost(HoloToolkit.Unity.InputModule.SourceStateEventData eventData)
    {
        Vector3 pos = Vector3.zero;
        eventData.InputSource.TryGetPointerPosition(eventData.SourceId, out pos);
        Debug.Log("OnSourceLost: " + eventData.SourceId.ToString() + " " + pos);

        uint which = 0;
        hands.TryGetValue(eventData.SourceId, out which);

        SetHandsUp(which, false);
    }
*/
}
