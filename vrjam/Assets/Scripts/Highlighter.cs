using UnityEngine;
using System.Collections;

public class Highlighter : MonoBehaviour {

    public Material m_default_material;
    public Material m_highlighted_material;
    public Color m_default_color;
    public Color m_highlight_color;

    public void Highlight () {
        renderer.material.color = m_highlight_color;
        //renderer.material = m_highlighted_material;
    }

   public void UnHighlight () {
        renderer.material.color = m_default_color;
    }
}