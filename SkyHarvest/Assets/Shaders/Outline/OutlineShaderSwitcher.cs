using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineShaderSwitcher : MonoBehaviour
{
    [SerializeField] private MeshRenderer r;
    private bool isOutlined = false;

    private void OnDisable()
    {
        CustomEvents.VFX.OnOutlineRemove -= SwitchToDefaultShader;
    }

    private void Start()
    {
        if(r == null)
        {
            try
            {
                r = GetComponent<MeshRenderer>();
            }
            catch
            {
                Debug.LogError($"No mesh renderer on {gameObject.name}");
            }
        }
    }

    public void SwitchToDefaultShader()
    {
        //r.materials[1] = null;
        r.materials[r.materials.Length - 1].color = new Color(r.materials[r.materials.Length - 1].color.r, r.materials[r.materials.Length - 1].color.g, r.materials[r.materials.Length - 1].color.b, 0f);
        CustomEvents.VFX.OnOutlineRemove -= SwitchToDefaultShader;
        isOutlined = false;
    }

    public void SwitchToOutlineShader()
    {
        if (isOutlined) return;
        //r.materials[1] = outlineMaterial;
        r.materials[r.materials.Length - 1].color = new Color(r.materials[r.materials.Length - 1].color.r, r.materials[r.materials.Length - 1].color.g, r.materials[r.materials.Length - 1].color.b, 1f);
        CustomEvents.VFX.OnOutlineRemove += SwitchToDefaultShader;
        isOutlined = true;
    }
}
