using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class LightBeamController : MonoBehaviour
{
    public GameObject parentRoot;
    public Transform orb;
    public Transform bossTarget;
    public LayerMask obstacleLayer;
    public float beamDuration { get; } = 5f;
    private float beamActiveTime = 0f;
    private LineRenderer lineRenderer;
    private BeamState beamState;
    private BossLightOrb bossLightOrb;
    public GameObject bossObj;
    private BossBehavior boss;
    private float damagePerSecond = 2f;
    private float windUp = 0f;
    public float maxLengthScaling = 60f;
    public AudioSource audioSource;
    public AudioClip beamSound;
    private double currentSoundCount = 0d;
    private double newSoundCount = 0d;


    // Start is called before the first frame update
    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        if(audioSource == null) {
            Debug.Log("Cannot find audioSource " + audioSource);
        }

        if(beamSound == null) {
            Debug.Log("Cannot find beamSound " + beamSound);
        }

        if (parentRoot == null)
        {
            Debug.Log("Cannot find parentRoot of LightBeamController");
        }
        else
        {
            bossLightOrb = parentRoot.GetComponentInChildren<BossLightOrb>();
        }

        if (bossLightOrb == null)
        {
            Debug.Log("Cannot find boss light orb");
        }
        boss = bossObj.GetComponent<BossBehavior>();

        DisableBeam();
        beamState = BeamState.Off;
    }

    void Update()
    {
        if (bossLightOrb.collected && beamState == BeamState.Off)
        {
            beamState = BeamState.On;
        }

        switch (beamState)
        {
            case BeamState.On:
                DrawBeam();
                beamActiveTime += Time.deltaTime;
                if (beamActiveTime >= beamDuration)
                {
                    beamState = BeamState.Off;
                }

                break;
            case BeamState.Off:
                if (lineRenderer.enabled)
                    beamActiveTime = 0;
                DisableBeam();
                newSoundCount = 0d;
                currentSoundCount = 0d;
                // readyPlaySound = true;
                break;
        }
    }

    public enum BeamState
    {
        On,
        Off
    }

    private void DrawBeam()
    {
        newSoundCount = Math.Ceiling(beamActiveTime / beamSound.length);
        if (newSoundCount > currentSoundCount) {
            PlayBeamSound();
            currentSoundCount = newSoundCount;
        }

        lineRenderer.enabled = true;
        if (orb == null || bossTarget == null || lineRenderer == null)
        {
            Debug.Log("Failed to draw beam");
            return;
        }

        Vector3 startPos = orb.position;
        Vector3 direction = (bossTarget.position - orb.position).normalized;
        float maxDistance = Vector3.Distance(orb.position, bossTarget.position);

        RaycastHit hit;
        Vector3 endPos;
        if (windUp < maxLengthScaling)
        {
            windUp += 1f;
            float scale = windUp / maxLengthScaling;
            float distance = maxDistance * scale;
            Vector3 vectorGrowth = direction * distance;
            endPos = startPos + vectorGrowth;
        }
        else
        {
            endPos = bossTarget.position;
        }

        // Check if there is an obstacle blocking the laser
        if (Physics.Raycast(startPos, direction, out hit, maxDistance, obstacleLayer))
        {
            endPos = hit.point;
        }
        else if (windUp >= maxLengthScaling)
        {
            boss.bossHealth -= damagePerSecond * Time.deltaTime;
        }

        // Update LineRenderer positions
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        float flicker = Mathf.PingPong(Time.time * 5, 1);
        lineRenderer.startColor = new Color(1, 0, 0, flicker);
        lineRenderer.endColor = new Color(1, 0, 0, flicker);
    }

    void PlayBeamSound()
    {
        Debug.Log("Play beam sound");
        audioSource.PlayOneShot(beamSound);
    }

    private void DisableBeam()
    {
        // Debug.Log("Disable beam");
        lineRenderer.enabled = false;
    }
}
