using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;


public class AttachObject : MonoBehaviour {
    public GameObject Target;

	// Use this for initialization
	void Start () 
    {    
        this.LateUpdateAsObservable().Subscribe(_ => 
        {
            if (Target)
            {
                transform.position = Target.transform.position;
                transform.rotation = Target.transform.rotation;
                transform.localScale = Target.transform.localScale;
            }
        });
	}
}
