using UnityEngine;
using System.Collections;

public class ObserverSingleton : MonoBehaviour {

	private static ObserverSingleton s_Instance;
    
	public static ObserverSingleton Instance
    {
        get
        {
            if (s_Instance == null)
            {
				s_Instance = FindObjectOfType(typeof(ObserverSingleton)) as ObserverSingleton;
                if (s_Instance == null)
                {
                    GameObject go = new GameObject("game");
					s_Instance = go.AddComponent<ObserverSingleton>();
                }
            }
            return s_Instance;
        }
    }

    void Start()
    {
        if (Instance != this) Destroy(this);
    }

    public static void Notify() {

    }
}
