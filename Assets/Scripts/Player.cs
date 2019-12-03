using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;



public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 5f;
    float lookX;
    float lookY;
    public Text xText;
    public Text yText;
    public Text zText;
    public bool compressing;
    SocketIOComponent socket;
    int frameCounter = 0;
    int frameFreq = 3;
    public InputField frameFreqText; 

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        //Loading the server manager
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();

        double xpos = transform.position.x;
        xpos = xpos * Math.Pow(2,11);
        print(xpos);

        ushort shortxpos = Convert.ToUInt16(xpos);
        print(shortxpos);

        string xbin = BitConverter.ToString(BitConverter.GetBytes(shortxpos), 0).Replace("-","");
        print(xbin);

        int NumberChars = xbin.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2){
            bytes[i / 2] = Convert.ToByte(xbin.Substring(i, 2), 16);
        }

        ushort newx = BitConverter.ToUInt16(bytes, 0);

        print (newx);

        double fnewx = newx/ (Math.Pow(2,11));

        print((float)fnewx);

    }

    // Update is called once per frame
    void Update()
    {   
        //WASD movement
        if (Input.GetKey(KeyCode.W)){
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A)){
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S)){
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D)){
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        }
        if (Input.GetKeyDown(KeyCode.Space)){
            if (Physics.Raycast(transform.position,Vector3.down, 0.6f)){
                GetComponent<Rigidbody>().AddForce(Vector3.up*4f,ForceMode.Impulse);
            }
        }
        // if (Input.GetKeyDown(KeyCode.Escape)){
        //     Cursor.lockState = CursorLockMode.None;
        // }
        // if (Input.GetKeyDown(KeyCode.Mouse0)){
        //     Cursor.lockState = CursorLockMode.Locked;
        // }
        

        //Mouse looking
        lookX += Input.GetAxis("Mouse X") * 2;
        lookY += Input.GetAxis("Mouse Y") * 2;
        lookY = Mathf.Clamp(lookY,-80,80);

        transform.eulerAngles = new Vector3(0f, lookX, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(-lookY,0,0);

        //Text updating
        xText.text = "Player X: " + transform.position.x;
        yText.text = "Player Y: " + transform.position.y;
        zText.text = "Player Z: " + transform.position.z;

        //Updating frame frequency
        if (frameFreqText.text != null && frameFreqText.text != ""){
            if (int.Parse(frameFreqText.text) > 0){
                //print("changing framefreq");
                frameFreq = int.Parse(frameFreqText.text);
            }
        }
        
        

        //Sending player's position to server
        JSONObject playerData = new JSONObject(JSONObject.Type.OBJECT);
        if (!compressing){
            string xbinary = BitConverter.ToString(BitConverter.GetBytes(transform.position.x), 0).Replace("-","");
            string ybinary = BitConverter.ToString(BitConverter.GetBytes(transform.position.y), 0).Replace("-","");
            string zbinary = BitConverter.ToString(BitConverter.GetBytes(transform.position.z), 0).Replace("-","");
            playerData.AddField("x", xbinary);
            playerData.AddField("y", ybinary);
            playerData.AddField("z", zbinary);
            //print(xbinary);
            socket.Emit("update", playerData);
        }
        else{
            frameCounter++;
            if (frameCounter >= frameFreq){
                frameCounter = 0;
                double xpos = transform.position.x;
                xpos = xpos * Math.Pow(2,11);
                double ypos = transform.position.y;
                ypos = ypos * Math.Pow(2,11);
                double zpos = transform.position.z;
                zpos = zpos * Math.Pow(2,11);

                string xbinary = BitConverter.ToString(BitConverter.GetBytes(Convert.ToUInt16(xpos)), 0).Replace("-","");
                string ybinary = BitConverter.ToString(BitConverter.GetBytes(Convert.ToUInt16(ypos)), 0).Replace("-","");
                string zbinary = BitConverter.ToString(BitConverter.GetBytes(Convert.ToUInt16(zpos)), 0).Replace("-","");
                playerData.AddField("x", xbinary);
                playerData.AddField("y", ybinary);
                playerData.AddField("z", zbinary);
                //print(playerData);
                socket.Emit("update", playerData);
            }

            
        }
        

        
    }
}
