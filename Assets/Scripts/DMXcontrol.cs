using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMXcontrol : MonoBehaviour
{
    ArtNet.Engine artNet;

    [SerializeField] int testNo;

    //byte[] dmxData;
    public byte channel1 = 0;
    public byte channel2 = 0;
    public byte channel3 = 0;
    public byte channel4 = 0;

    private void Awake()
    {
        artNet = new ArtNet.Engine();
        ResetDMX();
    }

    void Start()
    {
        
    }

    void ResetDMX()
    {
        //dmxData = GetEmpty512();
        //artNet.SendDMX(dmxData);
    }

    void Update()
    {
        //artNet.SendDMX(dmxData);
    }

    void Send(string channel, float value)
    {
        if (string.IsNullOrEmpty(channel)) return;

        int address = 0;

        int.TryParse(channel, out address);

        SendDMX(address, Mathf.FloorToInt(value));
    }

    void SendDMX(int address, int value)
    {
        if (address <= 0) return;

        int x = value;
        if (x < 0) x = 0;
        if (x > 255) x = 255;

        //dmxData[address - 1] = (byte)x;
    }
}
