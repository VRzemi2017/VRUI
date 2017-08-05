 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandManager : MonoBehaviour {
    public enum WAND_STATE {
        IDEL,
        WARP,
        LAND,
        STATUS_MAX
    }

    //ジェムの数、種類が欲しい時にintではなくEnumに書き換える
    private int m_gem_num = 0;

    [SerializeField] private GameObject m_Wand;
    [SerializeField] private GemController m_GemController;

    public WAND_STATE STATE { get { return m_Wand.GetComponent<WandController>().STATE; } }
    public int GEM_NUM { get { return m_gem_num; } }
	
	// Update is called once per frame
	void Update ( ) {
        if ( m_Wand.GetComponent<WandController>( ).IsGetGem ) {
            GetGemAction( );
        }
    }

    //GemをGetしたときに呼ばれる関数
    private void GetGemAction ( ) {
        m_gem_num++;
        m_Wand.GetComponent<WandController>( ).ResetGemState( );
        m_GemController.SetGemNum( m_gem_num );
      
    }
}
