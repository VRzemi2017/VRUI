using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGetOut : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.parent = null;
        gameObject.SetActive( false );
    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
