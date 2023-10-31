using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GlowingText : MonoBehaviour
{
    public float minGlowPower;
    public float maxGlowPower;
    [SerializeField] private float glowFrequency;

    private TextMeshProUGUI text;
	private Material material;

	private void Awake()
	{
		text = GetComponent<TextMeshProUGUI>();
		material = text.fontMaterial;
	}

	private void Update()
	{
		float power = Mathf.Sin(Time.time * glowFrequency);
		power *= power;
		power = Mathf.Lerp(minGlowPower, maxGlowPower, power);
		material.SetFloat(ShaderUtilities.ID_GlowPower, power);
		text.UpdateMeshPadding();
	}
}
