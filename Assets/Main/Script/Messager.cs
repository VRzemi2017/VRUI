using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Messager : MonoBehaviour {
    [SerializeField] bool debug = true; 

    private StringReactiveProperty message = new StringReactiveProperty();
    public ReadOnlyReactiveProperty<string>  Message { get { return message.ToReadOnlyReactiveProperty(); } }

    public void SetMessage(string msg)
    {
        message.Value = msg;
        if (debug)
        {
            Debug.Log(message.Value);  
        }
    }
}
