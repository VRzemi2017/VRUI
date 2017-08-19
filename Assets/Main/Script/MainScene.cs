using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainScene : MonoBehaviour {
    [SerializeField]
    private List<GameObject> startPosition;
    [SerializeField]
    private GameObject player;

	// Use this for initialization
	void Start () 
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Title")
        {
            MainManager.ChangeState(MainManager.GameState.GAME_TITLE);
        }

		InitPlayerPosition();
	}

    void InitPlayerPosition()
    {
        if (player && TCAServer.PlayerNo >= 0 && TCAServer.PlayerNo < startPosition.Count)
        {
            player.transform.position = startPosition[TCAServer.PlayerNo].transform.position;
            player.transform.rotation = startPosition[TCAServer.PlayerNo].transform.rotation;
        }
    }
}
