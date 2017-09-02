using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour {
    [SerializeField] GameObject m_Gem;
    [SerializeField] List<Color> m_GemColor = new List<Color>( );
    [SerializeField] GameObject m_SmallGemPrefab;
    [SerializeField] Transform m_SmallGemParent;
    [SerializeField] Vector3 m_SmallGemLenght;
    [SerializeField] GameObject m_LineRenderer;
    [SerializeField] float m_getGemAnimationTime;
    [SerializeField] GameObject m_hitAnimation;
    private bool m_is_hit_gem = false;
    private bool m_is_game_start = false;
    private bool m_is_get_gem = false;
    private float m_hit_gem_time = 0;
    //ヒットしたGemを一時的に保存する
    private GameObject m_hit_gem;
    


    private List<GameObject> m_SmallGemList = new List<GameObject>();
    
    public bool IsGetGem { get { return m_is_get_gem; } }

    public void Start ( ) {
        m_Gem.GetComponent<Renderer>( ).material.SetColor( "_EmissionColor", m_GemColor[0] );
        m_LineRenderer.GetComponent<Renderer>().material.SetColor( "_EmissionColor", m_GemColor[0] );
    }

    public void Update ( ) {
        foreach ( GameObject gem in m_SmallGemList ) {
            gem.transform.RotateAround( m_SmallGemParent.position, m_SmallGemParent.forward, 1f );
        }
        if (m_is_hit_gem){
            m_hit_gem_time += Time.deltaTime;
            if (m_hit_gem_time >= m_getGemAnimationTime) {
                m_is_get_gem = true;
                m_is_hit_gem = false;
                m_hit_gem.SetActive(false);
                m_hitAnimation.SetActive(false);
                Animator anim = m_hitAnimation.GetComponent<Animator>();
                anim.SetBool("End", true);
                anim.SetBool("Start", false);
                m_hit_gem_time = 0.0f;
            }
        } else if (m_hit_gem_time > 0.0f) {
            m_hit_gem_time -= Time.deltaTime;
            if (m_hit_gem_time <= 0.0f) {
                m_hit_gem_time = 0.0f;
                m_hitAnimation.SetActive(false);
                Animator anim = m_hitAnimation.GetComponent<Animator>();
                anim.SetBool("End", true);
                anim.SetBool("Start", false);
            }
        } 
    }

    private void OnTriggerEnter(Collider collision) {
        if (!m_is_game_start) {
            return;
        }
        //ヒットした物の種類を取得するを取得する
        switch (collision.gameObject.tag){
            case "Gem":
                m_is_hit_gem = true;
                m_hit_gem = collision.gameObject;
                m_hitAnimation.SetActive(true);
                m_hitAnimation.transform.position = m_hit_gem.transform.position;
                SetHitAnimationSpeed(1);
                Animator anim = m_hitAnimation.GetComponent<Animator>();
                anim.SetBool("Start", true);
                anim.SetBool("End", false);
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (!m_is_game_start) {
            return;
        }
        //ヒットした物の種類を取得するを取得する
        switch (other.gameObject.tag)
        {
            case "Gem":
                m_is_hit_gem = false;
                SetHitAnimationSpeed(-1);
                break;
            default:
                break;
        }

    }

    private void SetHitAnimationSpeed(float spped) {
        Animator anim = m_hitAnimation.GetComponent<Animator>();
        anim.SetFloat("Speed", spped);
    }

    public void SetGemNum( int gem_num ) {
        GameObject gem = Instantiate( m_SmallGemPrefab );
        gem.transform.SetParent( m_SmallGemParent, false );
        m_SmallGemList.Add( gem );
        for ( int i = 0; i < m_SmallGemList.Count; i++ ) {
            Vector3 position = Quaternion.Euler(0, 0, ( 360 / m_SmallGemList.Count) * i ) * m_SmallGemLenght;
            m_SmallGemList[i].transform.localPosition = position;
        }

        if ( gem_num > m_GemColor.Count ) {
            return;
        }
        m_Gem.GetComponent<Renderer>( ).material.SetColor( "_EmissionColor", m_GemColor[gem_num] );
        m_LineRenderer.GetComponent<Renderer>( ).material.SetColor( "_EmissionColor", m_GemColor[gem_num] );
    }

    public void ResetHitState() {
        m_is_get_gem = false;
    }

    public void SetGameStart(bool start) {
        m_is_game_start = start;
    }

}
