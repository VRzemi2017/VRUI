using UnityEngine;
using UniRx;
using UniRx.Triggers;
using System.Collections;

public class FadControl : MonoBehaviour
{
    [SerializeField] AnimationClip fadinClip;
    [SerializeField] AnimationClip fadoutClip;

    Animation anim;

    private Subject<Unit> fadin = new Subject<Unit>();
    public IObservable<Unit> OnFadinEnd { get { return fadin; } }

    private Subject<Unit> fadout = new Subject<Unit>();
    public IObservable<Unit> OnFadoutEnd { get { return fadout; } }

    bool fading;

    void Start()
    {
        anim = GetComponent<Animation>();

        if (MainManager.CurrentState == MainManager.GameState.GAME_FADIN || MainManager.CurrentState == MainManager.GameState.GAME_TITLE)
        {
            Fadout();

            if (MainManager.CurrentState == MainManager.GameState.GAME_FADIN)
            {
                MainManager.ChangeState(MainManager.GameState.GAME_START);
            }
        }
    }

    void FadinEnd()
    {
        fadin.OnNext(Unit.Default);
        fading = false;
    }

    void FadoutEnd()
    {
        fadout.OnNext(Unit.Default);
        fading = false;
    }

    public void Fadin()
    {
        if (fading)
        {
            return;
        }

        anim.clip = fadinClip;
        anim.Play();
        fading = true;
        
    }

    public void Fadout()
    {
        if (fading)
        {
            return;
        }

        anim.clip = fadoutClip;
        anim.Play();
        fading = true;
    }
}
