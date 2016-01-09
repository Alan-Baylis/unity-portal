using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PortalEffect : MonoBehaviour {

    public Material material;
    public Portal portal1, portal2;
    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        material.SetTexture("_Portal1Tex", portal1.viewTexture);
        material.SetTexture("_Portal2Tex", portal1.viewTexture);
        Graphics.Blit(source, destination, material);
    }
}
