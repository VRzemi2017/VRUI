using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class MessageDisplayer_TextMesh : MonoBehaviour {
    [SerializeField]
    private Messager msger;

    private void Start() {
        TextMesh mesh = GetComponent<TextMesh>();

        if (msger == null)
        {
            msger = GameObject.FindObjectOfType<Messager>();
        }

        if (msger)
        {
            msger.Message.Subscribe(s => 
            {
                mesh.text = s;
            }).AddTo(this);
        }
    }

}
