using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class Opponent : MonoBehaviour
{
    public Text xText;
    public Text yText;
    public Text zText;
    public Text playerFoundText;
    public InputField interpSpeedText;
    float interpSpeed = 1;
    SocketIOComponent socket;
    public bool compressing;

    bool playerFound;

    Vector3 mostRecentPosition;
    // Start is called before the first frame update
    void Start()
    {

        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("receive", getData);  

        mostRecentPosition = transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        //Text updating
        xText.text = "Opponent X: " + transform.position.x;
        yText.text = "Opponent Y: " + transform.position.y;
        zText.text = "Opponent Z: " + transform.position.z;

        //use linear interpolation to reduce jitter
        if (compressing && playerFound){
            transform.position = Vector3.Lerp(transform.position, mostRecentPosition, interpSpeed);
        }      

        if (interpSpeedText.text != null && interpSpeedText.text != ""){
            if (float.Parse(interpSpeedText.text) >= 0 && float.Parse(interpSpeedText.text) <= 1){
                interpSpeed = float.Parse(interpSpeedText.text);
            }
        }
    }

    //getting data from the server
    public void getData(SocketIOEvent e){
        playerFound = true;
        playerFoundText.text = "Opponent has been found";
        if (!compressing){
            float newx = BitConverter.ToSingle(StringtoByteArray(e.data.list[0].str), 0);
            float newy = BitConverter.ToSingle(StringtoByteArray(e.data.list[1].str), 0);
            float newz = BitConverter.ToSingle(StringtoByteArray(e.data.list[2].str), 0);

            transform.position = new Vector3(newx,newy,newz);
        }
        else{
            //decode short, then turn into float so it can be used as vector
            ushort newx = BitConverter.ToUInt16(StringtoByteArray(e.data.list[0].str), 0);
            ushort newy = BitConverter.ToUInt16(StringtoByteArray(e.data.list[1].str), 0);
            ushort newz = BitConverter.ToUInt16(StringtoByteArray(e.data.list[2].str), 0);

            double fnewx = newx/ (Math.Pow(2,11));
            double fnewy = newy/ (Math.Pow(2,11));
            double fnewz = newz/ (Math.Pow(2,11));

            //print((float)fnewx);
            mostRecentPosition = new Vector3((float)fnewx,(float)fnewy,(float)fnewz);
        }
    }

    public static byte[] StringtoByteArray(string hex){
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2){
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    

}


