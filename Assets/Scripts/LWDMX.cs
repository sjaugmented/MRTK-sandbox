using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWDMX : MonoBehaviour {

    public string x_pos_channel = "";
    public string y_pos_channel = "";
    public string z_pos_channel = "";

    public string x_rot_channel = "";
    public string y_rot_channel = "";
    public string z_rot_channel = "";

    SceneController _sceneController;

    void Start()
    {
        _sceneController = SceneController.GetInstance();	
	}

    Vector3 _lastPos = new Vector3(-1, -1, -1);
    Vector3 _lastRot = new Vector3(-1, -1, -1);

	void Update()
    {
	    if (this.transform.localPosition != _lastPos)
        {
            _lastPos = this.transform.position;

            Send(x_pos_channel, _lastPos.x);
            Send(y_pos_channel, _lastPos.y);
            Send(z_pos_channel, _lastPos.z);
        }

        if (this.transform.localEulerAngles != _lastRot)
        {
            _lastRot = this.transform.localEulerAngles;

            Send(x_rot_channel, _lastRot.x);
            Send(y_rot_channel, _lastRot.y);
            Send(z_rot_channel, _lastRot.z);
        }
    }

    void Send(string channel, float value)
    {
        if (string.IsNullOrEmpty(channel)) return;

        int address = 0;

        int.TryParse(channel, out address);

        _sceneController.SendDMX(address, Mathf.FloorToInt(value));
    }
}
