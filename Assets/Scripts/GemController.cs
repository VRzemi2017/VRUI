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



    private List<GameObject> m_SmallGemList = new List<GameObject>();

    public void Start ( ) {
        m_Gem.GetComponent<Renderer>( ).material.SetColor( "_EmissionColor", m_GemColor[0] );
        m_LineRenderer.GetComponent<Renderer>().material.SetColor("_EmissionColor", m_GemColor[0]);
    }

    public void Update ( ) {
        foreach ( GameObject gem in m_SmallGemList ) {
            gem.transform.RotateAround( m_SmallGemParent.position, m_SmallGemParent.forward, 1f );
        }
    }

    public void SetGemNum( int gem_num ) {
        GameObject gem = Instantiate( m_SmallGemPrefab );
        gem.transform.SetParent( m_SmallGemParent, false );
        m_SmallGemList.Add( gem );
        for ( int i = 0; i < m_SmallGemList.Count; i++ ) {
            Vector3 position = Quaternion.Euler(0, 0, (360 / m_SmallGemList.Count) * i) * m_SmallGemLenght;
            m_SmallGemList[i].transform.localPosition = position;
        }

        if ( gem_num > m_GemColor.Count ) {
            return;
        }
        m_Gem.GetComponent<Renderer>( ).material.SetColor("_EmissionColor", m_GemColor[gem_num]);
        m_LineRenderer.GetComponent<Renderer>( ).material.SetColor("_EmissionColor", m_GemColor[gem_num]);
    }

}
