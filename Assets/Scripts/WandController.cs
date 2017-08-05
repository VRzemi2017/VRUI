using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandController : MonoBehaviour {
    [SerializeField] GameObject m_WandModel;

    [SerializeField] line2 m_Line;

    private WandManager.WAND_STATE m_state;
    private bool m_is_get_gem = false;

    public WandManager.WAND_STATE STATE { get { return m_state; } }
    public bool IsGetGem { get { return m_is_get_gem; } }

	// Update is called once per frame
	void Update ( ) {
        //毎フレーム最初にステートを変える
        ChageState(　);

    }

    private void OnTriggerEnter( Collider collision ) {
        //ヒットした物の種類を取得するを取得する
        switch ( collision.gameObject.tag ) {
            case "Gem":
                m_is_get_gem = true;
                Destroy(collision.gameObject);
                break;
            default:
                break;
        }
    }

    private void ChageState ( ) {
        m_state = WandManager.WAND_STATE.IDEL;
        if ( m_Line.IsWarpInput ) {
            m_state = WandManager.WAND_STATE.WARP;
        }
    }

    public void ResetGemState ( ) {
        m_is_get_gem = false;
    }

}
