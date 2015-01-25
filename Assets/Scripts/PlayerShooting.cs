using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	public float shotCooldown = 1.0f;
	float lastShotTime = 0.0f;
	Camera cam;

	void Start()
	{
		cam = gameObject.GetComponentInChildren<Camera>();
		if (cam == null)
		{
			Debug.LogError("No camera in children");
		}
	}

	void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			Shoot();
		}
	}

	public void Shoot()
	{
		if(Time.time < lastShotTime + shotCooldown)
		{
			return;
		}

		lastShotTime = Time.time;

		RaycastHit[] hits;
		hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward);
		float nearestHitDistance = 9999.0f;
		int nearestHitIndex = -1;
		for (int i = 0; i < hits.Length; ++i)
		{
			RaycastHit hit = hits[i];
			if (hit.collider.gameObject != gameObject
			 && hit.distance < nearestHitDistance)
			{
				nearestHitIndex = i;
				nearestHitDistance = hit.distance;
			}
		}
		if (nearestHitIndex >= 0)
		{
			RaycastHit hit = hits[nearestHitIndex];
			Transform transform = hit.collider.transform.parent ?? hit.collider.transform;
			Debug.Log("hit " + transform.name + " (distance: " + hit.distance + ")");

			Health health = transform.gameObject.GetComponent<Health>();
			PhotonView photonView = transform.gameObject.GetComponent<PhotonView>();
			if (health != null && photonView != null)
			{
				photonView.RPC("TakeDamage", PhotonTargets.All, 20.0f);
				//health.TakeDamage(20.0f);
			}
		}
		else
		{
			Debug.Log("hit nothing");
		}
	}
}
