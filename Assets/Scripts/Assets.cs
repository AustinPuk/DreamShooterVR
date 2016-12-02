using UnityEngine;
using System.Collections;

public class Assets : MonoBehaviour {

    private const string prefabsPath = "Prefabs/";
   
    public static readonly GameObject player = Resources.Load<GameObject>(prefabsPath + "Player");
    //public static readonly GameObject enemy = Resources.Load<GameObject>(prefabsPath + "Enemy");
    public static readonly GameObject bear = Resources.Load<GameObject>(prefabsPath + "ZomBear");
    public static readonly GameObject bunny = Resources.Load<GameObject>(prefabsPath + "Zombunny");
    public static readonly GameObject elephant = Resources.Load<GameObject>(prefabsPath + "Hellephant");
}
