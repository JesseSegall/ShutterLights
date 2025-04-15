using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLightOrb : MonoBehaviour
{
    // private Material lightMaterial;
    // Start is called before the first frame update
    public GameObject mainPlayer;
    private MeshRenderer renderer;
    private LightDecayStatusBar statusBar;

    void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
        if(renderer == null) {
            Debug.Log("Renderer is missing");
        }
        statusBar = mainPlayer.GetComponentInChildren<LightDecayStatusBar>();

        // lightMaterial = renderer.material;
        // Debug.Log("Material is " + lightMaterial);

    }

    // Update is called once per frame
    void Update()
    {
        float currentColorRgb = 1 - (statusBar.timer / statusBar.decayDuration);
        Color newColor = new Color(currentColorRgb, currentColorRgb, currentColorRgb, 0.5f);
        renderer.material.SetColor("_BaseColor", newColor);
        renderer.material.SetColor("_EmissionColor", newColor);
    }
}
