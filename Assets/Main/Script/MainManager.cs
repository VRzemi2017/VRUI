using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.SceneManagement;
using System.Linq;

using MonobitNetwork = MonobitEngine.MonobitNetwork;

public class MainManager : MonoBehaviour {
    [SerializeField]
    private TCAServer server;
    [SerializeField]
    private string titleName;
    [SerializeField]
    private string mainName;

    public enum GameState
    {
        GAME_INIT,
        GAME_NETWORK,
        GAME_TITLE,
        GAME_FADIN,
        GAME_START,
        GAME_PLAYING,
        GAME_TIMEUP,
        GAME_RESULT,
        GAME_FINISH,


        GAME_STATE_MAX,
    }
    private static GameState _state = GameState.GAME_INIT;
    public static GameState CurrentState { get { return _state; } }

    private static Subject<GameState> stateChanged = new Subject<GameState>();
    public static IObservable<GameState> OnStateChanged { get { return stateChanged; } }

    System.IDisposable disFidin;
    System.IDisposable disFidout;

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
        ChangeState(GameState.GAME_INIT);
    }

    void Start ()
    {
        ChangeState(GameState.GAME_NETWORK);
        
        server.OnEnterRoom.Subscribe(_ => 
        {
            if (CurrentState == GameState.GAME_NETWORK)
            {
                SceneManager.LoadScene(titleName);
            }
        }).AddTo(this);

        this.UpdateAsObservable().Subscribe(_ => 
        {
            SteamVR_ControllerManager m = GameObject.FindObjectOfType<SteamVR_ControllerManager>();
            if (m)
            {
                var leftObj = m.left.GetComponent<SteamVR_TrackedObject>();
                var rightObj = m.right.GetComponent<SteamVR_TrackedObject>();

                var left = (leftObj.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)leftObj.index) : null;
                var right = (rightObj.index != SteamVR_TrackedObject.EIndex.None) ? SteamVR_Controller.Input((int)rightObj.index) : null;

                if ((left != null && left.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)) || (right != null && right.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger)))
                {
                    if (_state == GameState.GAME_FADIN || _state == GameState.GAME_FINISH)
                    {
                        return;
                    }

                    int next = ((int)_state + 1) % (int)GameState.GAME_STATE_MAX;
                    if (next == (int)GameState.GAME_FADIN) {
                        FadControl fad = GameObject.FindObjectOfType<FadControl>();
                        if (fad)
                        {
                            fad.Fadin();
                            disFidin = fad.OnFadinEnd.Subscribe(i => 
                            {
                                MonobitNetwork.LoadLevel(mainName);
                                disFidin.Dispose();
                            });
                        }
                    }

                    if (next == (int)GameState.GAME_FINISH) {
                        FadControl fad = GameObject.FindObjectOfType<FadControl>();
                        if (fad)
                        {
                            fad.Fadin();
                            disFidin = fad.OnFadinEnd.Subscribe(i => 
                            {
                                MonobitNetwork.LoadLevel(titleName);
                                disFidin.Dispose();
                            });
                        }
                    }

                    ChangeState((GameState)next);
                }
                
            }
        });
	}

    public static void ChangeState(GameState state)
    {
        SetMessage(state.ToString() + " state ending...");
        _state = state;
        SetMessage( "Change to state " + state.ToString());
        stateChanged.OnNext(_state);
    }

    private static void SetMessage(string msg)
    {
        Messager message = GameObject.FindObjectOfType<Messager>();

        if (message)
        {
            message.SetMessage(msg); 
        }
    }
}
