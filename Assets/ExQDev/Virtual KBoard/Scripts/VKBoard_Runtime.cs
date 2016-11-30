using UnityEngine;
using System.Collections;

public class VKBoard_Runtime : MonoBehaviour {

    public KeyCode hotKey;
    public KeyCode esc;
    public GameObject kPanel;

    public void UpdateKeys()
    {
        if (VKBoard.HasKey("Esc"))
        {
            esc = VKBoard.GetKey("Esc").code;
        }
        
    }

    void Awake()
    {
        VKBoard.Load();                                         //Load VKBoard
    }

	// Use this for initialization
	void Start () {
        UpdateKeys();                                           //Invoke the Invoke keys method. Every set key invokes every UpdateKey() method of the scene
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            hotKey = VKBoard.GetPressedKey();                   //Set the hotkey
        }
        if (Input.GetKeyDown(hotKey))
        {
            Debug.Log("HotKey + " + hotKey + " pressed!");      //Notify
        }
        if (Input.GetKeyDown(esc))
        {
            kPanel.SetActive(!kPanel.activeSelf);               //Show/hide the virtual keyboard panel
        }
	}
}
