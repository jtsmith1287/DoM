using UnityEngine;
using System.Collections;

public class DaylightSystem : MonoBehaviour {

	public Light Sun;
	public float DaylightTimeModifier;
	public Color MorningLight;
	public Color Daylight;
	public Color EveningLight;
	public Vector3 SunRotation;

	private void Start() {

		SunRotation = transform.rotation.eulerAngles;
	}

	private void Update() {

		SunRotation.x += Time.deltaTime * DaylightTimeModifier;
		float rot = SunRotation.x % 360;
		// Sunrise to noon
		if (rot >= 0f && rot <= 90f) {
			Sun.color = Color.Lerp(MorningLight, Daylight, rot / 90f);
			RenderSettings.ambientIntensity = Mathf.Lerp(0f, 1f, rot *3 / 90f);
		// Noon to sunset
		} else if (rot >= 90f && rot <= 180f) {
			Sun.color = Color.Lerp(Daylight, EveningLight, rot -90 / 90f);
			RenderSettings.ambientIntensity = Mathf.Lerp(1f, 0f, (rot -90) * 3 / 90f);
		}
		transform.rotation = Quaternion.Euler(SunRotation);
	}
}
