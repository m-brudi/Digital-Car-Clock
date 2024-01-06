using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightingManager : MonoBehaviour
{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private Light DirectionalLightMoon;
    [SerializeField] private LightingPreset Preset;
    [SerializeField] private int timeDivider;
    [SerializeField, Range(0, 24)] private float TimeOfDay;

    private void Update() {
        if (Preset == null) return;
        if (Application.isPlaying) {
            //TimeOfDay = ((float)Controller.Instance.debugTime.Hour + ((float)Controller.Instance.debugTime.Minute * 0.01f));
            TimeOfDay = ((float)DateTime.Now.Hour + ((float)DateTime.Now.Minute * 0.01f));
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay/24f);
        } else {
            UpdateLighting(TimeOfDay / 24f);
        }
    }
    private void UpdateLighting(float timePercent) {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);
        if (DirectionalLight != null) {
            if (timePercent >= 0.1f) {
                DirectionalLight.GetComponent<Light>().enabled = true;
                DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
                DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 110f, 130f, 0));
            } else {
                DirectionalLight.GetComponent<Light>().enabled = false;
            }
        }
        if (DirectionalLightMoon != null) {
            DirectionalLightMoon.color = Preset.DirectionalColorMoon.Evaluate(timePercent);
            if (timePercent < 0.2f || timePercent > 0.80f) {
                DirectionalLightMoon.intensity = 0.2f;
                DirectionalLightMoon.GetComponent<Light>().enabled = true;
                DirectionalLightMoon.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 250f, (timePercent * 360f)-150f, 0));
                if (timePercent > 0.80f) {
                    DirectionalLightMoon.intensity = 0.1f;
                }
            } else {
                DirectionalLightMoon.intensity = 0;
                DirectionalLightMoon.GetComponent<Light>().enabled = false;
            }
        }
    }
}
