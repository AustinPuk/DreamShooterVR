using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public static class VKBoard{
    public static List<VKey> keyboard = new List<VKey>();

    public static KeyCode FetchKey()
    {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }

    public static KeyCode GetPressedKey()
    {
        foreach (var k in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey((KeyCode)k))
            {
                return (KeyCode)k;
            }
        }
        return KeyCode.None;
    }

    public static VKey GetKey(string name)
    {
        foreach (var v in keyboard)
        {
            if (v.name == name)
            {
                return v;
            }
        }
        return null;
    }

    public static void SetKey(string sname, KeyCode code)
    {
        if (HasKey(sname))
        {
            if (KeyName(code) != "" && KeyName(code) != null)
            {
                KeyCode tmp = GetKey(sname).code;
                Debug.Log(KeyName(code));
                GetKey(KeyName(code)).code = tmp;
                GetKey(sname).code = code;
                Debug.Log("Keys values switched!");
            }
            else if (KeyName(code) != null && KeyName(code) == "")
            {
                GetKey(KeyName(code)).name = sname;
            }

        }
        else
            {
                keyboard.Add(new VKey() { name = sname, code = code});
            }

        foreach (GameObject go in GameObject.FindObjectsOfType<GameObject>())
        {
            foreach (var c in go.GetComponents<Component>())
            {
                if (c.GetType() != null)
                {
                    if (c.GetType().GetMethod("UpdateKeys") != null)
                    {
                        c.GetType().GetMethod("UpdateKeys", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Invoke(c, null);
                    }
                }
            }
        }
    }

    public static bool HasKey(string name)
    {
        foreach (var v in keyboard)
        {
            if (v.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public static string KeyName(KeyCode k)
    {
        foreach (VKey vk in keyboard)
        {
            if (vk.code == k)
            {
                return vk.name;
            }
        }
        return null;
    }

    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.streamingAssetsPath + "/VirtualKeyBoard.vkb", FileMode.OpenOrCreate);
        bf.Serialize(fs, keyboard);
        fs.Close();
    }

    public static void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = File.Open(Application.streamingAssetsPath + "/VirtualKeyBoard.vkb", FileMode.Open);
        keyboard = (List<VKey>)bf.Deserialize(fs);
        fs.Close();
    }
}

[System.Serializable]
public class VKey
{
    public int id = 0;
    public string name = "";
    public KeyCode code = KeyCode.None;
    public KeyCode lastCode = KeyCode.None;
}
