using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class VKBoard_Editor : EditorWindow {
    
    [MenuItem("Window/Virtual KeyBoard %k")]
    public static void Init()
    {
        VKBoard_Editor editor = GetWindow<VKBoard_Editor>();
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        if (File.Exists(Application.streamingAssetsPath + "/VirtualKeyBoard.vkb"))
        {
            VKBoard.Load();
        }
        else
        {
            VKBoard.keyboard = new System.Collections.Generic.List<VKey>();
            VKBoard.Save();
        }
        editor.title = "Virtual KBoard";
        editor.Show();
    }

    public static Vector2 kbscroll =  new Vector2();
    
    public IEnumerator WaitForKey(VKey vk)
    {
        KeyCode k = VKBoard.FetchKey();
        if (k != KeyCode.None)
        {
            vk.code = k;
            yield break;
        }
        yield return WaitForKey(vk);
    }

    void OnDestroy()
    {
        VKBoard.Save();
    }

    public static KeyCode EditorCatchKey()
    {
        Debug.Log(Event.current.keyCode);
        return Event.current.keyCode;
        //return KeyCode.None;
    }

    public static void PrepareKeyBoard()
    {
        GameObject kb = GameObject.Find("VKBoard Panel");
        foreach (Button b in kb.GetComponentsInChildren<Button>())
        {
            if (!b.GetComponent<VKey_Runtime>()) {
                b.gameObject.AddComponent<VKey_Runtime>();
            }
        }
    }

    public KeyCode tmp = KeyCode.None;
    public bool catching = false;
    public void OnSceneGUI()
    {
        if (catching)
        {
            Debug.Log("Press key please");
            tmp = Event.current.keyCode;
            if (tmp != KeyCode.None)
            {
                Debug.Log("Complete");
                catching = false;
            }
        }
    }
    public void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Prepare KeyBoard"))
        {
            PrepareKeyBoard();
        }
        GUILayout.EndHorizontal();
        kbscroll = GUILayout.BeginScrollView(kbscroll);
        foreach (VKey vk in VKBoard.keyboard)
        {
            GUILayout.BeginHorizontal();
            vk.name = GUILayout.TextField(vk.name);
            vk.code = (KeyCode)EditorGUILayout.EnumPopup(vk.code);
            if (GUILayout.Button("x"))
            {
                VKBoard.keyboard.Remove(vk);
                break;
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
        if (GUILayout.Button("Add"))
        {
            VKBoard.keyboard.Add(new VKey());
        }
    }
}

//[CustomPropertyDrawer(typeof(VKey))]
public class VKeyDrawer : PropertyDrawer
{
    const bool editting = false;
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
    {
        EditorGUI.LabelField( new Rect(pos.x, pos.y, pos.width / 2, pos.height), prop.FindPropertyRelative("name").stringValue);
        EditorGUI.TextField(new Rect(pos.x + pos.width / 2, pos.y, pos.width / 2, pos.height) , prop.FindPropertyRelative("code").stringValue);
    }
}