using UnityEngine;
 
/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
     private static T m_Instance = null;
     public static T Instance
     {
         get
         {
             if (m_Instance == null)
             {
                 m_Instance = (T)FindObjectOfType(typeof(T));
                 if (m_Instance == null)
                     m_Instance = (new GameObject(typeof(T).Name)).AddComponent<T>();
                 //DontDestroyOnLoad(m_Instance.gameObject);
             }
             return m_Instance;
         }
     }
}