using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {
	public float maxHealth = 100.0f;
	float health = 100.0f;

	void Start()
	{
		health = maxHealth;
	}

	[RPC]
	public void TakeDamage(float amount)
	{
		health -= amount;
		Debug.Log("Took " + amount + " damage. " + health + " health remaining");

		if (health <= 0)
		{
			Die();
		}
	}

	[RPC]
	void Die()
	{
		PhotonView photonView = gameObject.GetComponent<PhotonView>();
		if (photonView == null || photonView.instantiationId == 0)
		{
			Destroy(gameObject);
		}
		else
		{
			if (PhotonNetwork.isMasterClient)
			{
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}
}
