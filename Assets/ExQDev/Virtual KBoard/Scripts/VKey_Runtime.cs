using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[ExecuteInEditMode]
public class VKey_Runtime : MonoBehaviour, IPointerClickHandler {

    public VKey defaultKey = new VKey();
    public VKey currentKey = new VKey();
    public bool changingKey = false;
    public static GameObject pkt;
    // Use this for initialization

    string keyname;

	void Start () {
        keyname = GetComponentInChildren<Text>().text;
        if (VKBoard.HasKey(keyname))                        //If has key
        {
            defaultKey = VKBoard.GetKey(keyname);           //Set key
        }
        pkt = GameObject.Find("PressKeyText");                                          //Find the tip panel
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        changingKey = !changingKey;
    }

    public void UpdateKeys()
    {
        if (currentKey.code != KeyCode.None)                                            //If key changed, highlight the button of yellow.
        {
            Debug.LogError("Key yellow!");
            //GetComponentInChildren<Text>().text = currentKey.code.ToString();
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.yellow;
            colors.highlightedColor = Color.yellow;
            GetComponent<Button>().colors = colors;
        }
        else                                                                            //Else set key from keyboard and reset highlighting.
        {
            defaultKey = VKBoard.GetKey(keyname);
            var colors = GetComponent<Button>().colors;
            colors.normalColor = Color.white;
            colors.highlightedColor = Color.white;
            GetComponent<Button>().colors = colors;
        }
    }
        void FixedUpdate () {
       
        if (changingKey)                                                                //If state of button changing.
        {
            pkt.SetActive(true);                                                        //Show the tip
            KeyCode tmp = VKBoard.GetPressedKey();
            if (tmp != KeyCode.None)
            {
                currentKey.code = tmp;                                                  //Get presset key
                VKBoard.SetKey(keyname, tmp);                                           //And set key
                Debug.Log(keyname + "    " + tmp);
                VKBoard.Save();                                                         //And save new keys
                Debug.Log("Saved");
                changingKey = false;                                                    //Reset state
                pkt.SetActive(false);                                                   //Hide tip
            }
        }
	}
}
