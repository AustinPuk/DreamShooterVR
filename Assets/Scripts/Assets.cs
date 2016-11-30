using UnityEngine;
using System.Collections;

public class Assets : MonoBehaviour {

    private const string prefabsPath = "Prefabs/";
   
    public static readonly GameObject player = Resources.Load<GameObject>(prefabsPath + "Player");
    //public static readonly GameObject enemy = Resources.Load<GameObject>(prefabsPath + "Enemy");
    //public static GameObject bear = Resources.Load<GameObject>(prefabsPath + "ZomBear");
    //public static GameObject bunny = Resources.Load<GameObject>(prefabsPath + "Zombunny");    
}
