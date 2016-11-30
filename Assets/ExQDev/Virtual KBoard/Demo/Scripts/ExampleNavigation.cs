using UnityEngine;
using System.Collections;

public class ExampleNavigation : MonoBehaviour {

    public KeyCode w;
    public KeyCode s;
    public KeyCode a;
    public KeyCode d;


    public void UpdateKeys()
    {
        if(VKBoard.HasKey("W"))w = VKBoard.GetKey("W").code;
        if(VKBoard.HasKey("A"))a = VKBoard.GetKey("A").code;
        if(VKBoard.HasKey("S"))s = VKBoard.GetKey("S").code;
        if(VKBoard.HasKey("D"))d = VKBoard.GetKey("D").code;
    }

	// Use this for initialization
	void Start () {
        UpdateKeys();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(w))
        {
            transform.Translate(transform.forward);
        }
        if (Input.GetKey(s))
        {
            transform.Translate(transform.forward * -1);
        }
        if (Input.GetKey(a))
        {
            transform.Translate(transform.right * -1);
        }
        if (Input.GetKey(d))
        {
            transform.Translate(transform.right);
        }
	}
}
