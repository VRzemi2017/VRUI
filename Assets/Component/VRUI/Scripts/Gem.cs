using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    private bool IsEndAnimation = true;
    public bool Is_End_Animation { get { return IsEndAnimation; } }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetAnimationIsEnd( bool isEnd ) {
        IsEndAnimation = isEnd;
    }

}
