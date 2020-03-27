using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SceneController : MonoBehaviour {


    /*
    MainController _mc;
    OscOut _oscOut;
    string _scenesJson = "";

    LWSceneCollection _sceneCollection;


    public static SceneController GetInstance()
    {
        return MainController.GetInstance().GetComponent<SceneController>();
    }

    private void Awake()
    {
        try
        {
            _mc = MainController.GetInstance();
            _oscOut = _mc.GetComponent<OscOut>();

            var temp = Resources.Load("scenes") as TextAsset;
            _scenesJson = temp.text;

            _sceneCollection = Newtonsoft.Json.JsonConvert.DeserializeObject<LWSceneCollection>(_scenesJson);

            if (lwEvents == null)
            {
                lwEvents = new List<HoloToolkit.Unity.LWEvent>();
            }
        }
        catch (System.Exception ex)
        {
//            Debug.LogError(ex.Message);
        }

    }

	// Update is called once per frame
	void Update () {
		
	}

    List<HoloToolkit.Unity.LWEvent> lwEvents;

    public void RegisterLWEvent(HoloToolkit.Unity.LWEvent lwEvent)
    {
        if (lwEvents == null)
        {
            lwEvents = new List<HoloToolkit.Unity.LWEvent>();
        }

        lwEvents.Add(lwEvent);
    }

    public void EndAllGestures()
    {
        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;

            e.EndGesture();
        }
    }

    public void EndAllGesturesOnOtherObjects(GameObject go)
    {
        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            if (e.gameObject == go) return;

            e.EndGesture();
        }
    }


    public void SendGroup1(string group)
    {
        GroupBehavior(group, 1.0f);
    }

    public void SendGroup0(string group)
    {
        GroupBehavior(group, 0.0f);
    }


    public void GroupBehavior(string group, float dataAsFloat)
    {
        //Debug.Log("GroupBehavior: " + group);
        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            if (e.group != group) continue;

            e.ExecuteBehavior(0, dataAsFloat);
        }
    }

    public bool showOSCReceive = false;
    public bool showOSCSend = false;

    public void OSCReceive(OscMessage message)
    {
        if (showOSCReceive)
        {
            float f = 0;
            message.TryGet(0, out f);

            Debug.Log("Receiving OSC (" + message.address + "): " + f.ToString());
        }

        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            e.OSCReceive(message);
        }
    }

    public void DestroyLW(GameObject go)
    {
        if (go == null) return;

        var list = go.GetComponents<HoloToolkit.Unity.LWEvent>();

        if (list == null)
        {
            Destroy(go);
            return;
        }
        
        if (list.Length <= 0)
        {
            Destroy(go);
            return;
        }

        foreach (var entry in list)
        {
            entry.DestroyMe();
        }
    }

    public void LeftHandUp(HoloToolkit.Unity.InputModule.SourceStateEventData eventData)
    {
        Debug.Log("LeftHandUp ");

        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            e.LeftHandUp(eventData);
        }
    }

    public void RightHandUp(HoloToolkit.Unity.InputModule.SourceStateEventData eventData)
    {
        Debug.Log("RightHandUp");

        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            e.RightHandUp(eventData);
        }
    }

    public void TwoHandsUp()
    {
        Debug.Log("TwoHandsUp");

        if (lwEvents == null) return;

        foreach (var e in lwEvents)
        {
            if (e == null) continue;
            e.TwoHandsUp();
        }
    }

    public void Quit()
    {
        _mc.Quit();
    }

    public void RunScene(string s)
    {
        if (string.IsNullOrEmpty(s)) return;

        if (s == "NEXTSCENE")
        {
            int currentIndex = Application.loadedLevel;
            currentIndex++;
            if (currentIndex >= Application.levelCount) currentIndex = 0;

            _mc.ShutDown(currentIndex);
            return;
        }

        if (s == "PREVSCENE")
        {
            int currentIndex = Application.loadedLevel;
            currentIndex--;
            if (currentIndex < 0) currentIndex = Application.levelCount - 1;

            _mc.ShutDown(currentIndex);
            return;
        }


        var scene = FindScene(s);
        if (scene == null)
        {
            Debug.LogError("Could not find scene: " + s);
            return;
        }

        foreach (var entry in scene.list)
        {
            SetEntry(entry);
        }
    }

    void SetEntry(LWSceneEntry entry)
    {
        var device = FindDevice(entry.id);
        if (device == null)
        {
            Debug.LogError("Device not found: " + entry.id);
            return;
        }

        if (device.type == "dmx")
        {
            SendDMX(device, entry);
        }
        else if (device.type == "hue")
        {
            SendHUE(device, entry);
        }
        else if (device.type == "osc")
        {
            SendOSC(device, entry);
        }
    }
    

    public void SendDMX(int address, int value)
    {
        //Debug.Log("[SendDMX] " + address + " : " + value);
        _mc.SetAddress(address, value);
    }

    public void UpdateDMX(int address, int value)
    {
        //Debug.Log("[SendDMX] " + address + " : " + value);
        _mc.UpdateAddress(address, value);
    }

    void SendDMX(LWDevice device, LWSceneEntry entry)
    {
        int startAddress = 0;
        int.TryParse(device.address, out startAddress);

        var values = entry.value.Split(new char[] { ',' });

        for (int i = 0; i < values.Length; i++)
        {
            var v = values[i].Trim();
            int d = 0;
            int.TryParse(v, out d);

            _mc.SetAddress(startAddress + i, d);
        }
    }

    public void DMXRotate(string addressList, int channels)
    {
        _mc.DMXRotate(addressList, channels);
    }

    public int ReadDMX(int address)
    {
        var addressToUse = address;

        return _mc.ReadAddress(addressToUse);
    }

    public void ResetDMX()
    {
        _mc.ResetDMX();
    }

    public void ReadDMXRGB(int address, out int r, out int g, out int b)
    {
        r = 0;
        g = 0;
        b = 0;

        var addressToUse = address;

        r = _mc.ReadAddress(addressToUse);
        g = _mc.ReadAddress(addressToUse + 1);
        b = _mc.ReadAddress(addressToUse + 2);
    }

    void SendHUE(LWDevice device, LWSceneEntry entry)
    {
        string action = "state";
        if (device.url.Contains("groups")) action = "action";

        string URL = "http://" + device.address + "/api/" + device.username + device.url + "/" + action;
        Debug.Log("Sending HUE: " + URL + " - " + entry.value);
        string hueBody = HueUtil.GetHUEBody(entry.value);
        byte[] data = System.Text.Encoding.UTF8.GetBytes(hueBody);
        StartCoroutine(HTTPPut(URL, data));
    }

    void SendOSC(LWDevice device, LWSceneEntry entry)
    {
       if (showOSCSend) Debug.Log("Sending OSC (" + entry.type + "): " + entry.value);

        var oscType = entry.type;

        if (oscType == "bool")
        {
            bool b = false;
            bool.TryParse(entry.value, out b);
            _oscOut.Send(device.url, b);
        }
        else if (oscType == "byte")
        {
            byte b = 0;
            byte.TryParse(entry.value, out b);
            _oscOut.Send(device.url, b);
        }
        else if (oscType == "int")
        {
            int x = 0;
            int.TryParse(entry.value, out x);
            _oscOut.Send(device.url, x);
        }
        else if (oscType == "float")
        {
            float f = 0;
            float.TryParse(entry.value, out f);
            _oscOut.Send(device.url, f);
        }
        else if (oscType == "string")
        {
            _oscOut.Send(device.url, entry.value);
        }
    }

    public void SendOSC(string address, float f)
    {
        if (showOSCSend) Debug.Log("Sending OSC (" + address + "): " + f);

        _oscOut.Send(address, f);
    }

    LWDevice FindDevice(string s)
    {
        if (_sceneCollection == null) return null;

        foreach (var entry in _sceneCollection.devices)
        {
            if (entry.id == s) return entry;
        }

        return null;
    }

    LWScene FindScene(string s)
    {
        if (_sceneCollection == null)
        {
            Debug.LogError("_sceneCollection == null");
            return null;
        }

        foreach (var entry in _sceneCollection.scenes)
        {
            if (entry.id == s) return entry;
        }

        return null;
    }

    IEnumerator HTTPPut(string url, byte[] data)
    {
        var www = UnityEngine.Networking.UnityWebRequest.Put(url, data);
        yield return www.Send();
    }

    [Serializable]
    public class LWDevice
    {
        public string id { get; set; }
        public string type { get; set; }
        public string address { get; set; }
        public string username { get; set; }
        public string url { get; set; }
    }

    [Serializable]
    public class LWSceneEntry
    {
        public string id { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }

    [Serializable]
    public class LWScene
    {
        public string id { get; set; }
        public List<LWSceneEntry> list { get; set; }
    }

    [Serializable]
    public class LWSceneCollection
    {
        public List<LWDevice> devices;
        public List<LWScene> scenes;
    }
    */
}
