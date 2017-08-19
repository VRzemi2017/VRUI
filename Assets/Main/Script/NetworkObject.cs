using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkObject : MonoBehaviour {
    [SerializeField]
    private string objName;
    [SerializeField]
    private int group;

	// Use this for initialization
	void Start () {
		if (MonobitEngine.MonobitNetwork.inRoom)
        {
            GameObject obj = MonobitEngine.MonobitNetwork.Instantiate(objName, Vector3.zero, Quaternion.identity, group);
            obj.AddComponent<AttachObject>().Target = gameObject;
        }
	}
}
