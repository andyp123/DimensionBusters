using UnityEngine;
using System.Collections;

public class NetworkPlayer : Photon.MonoBehaviour
{
	Vector3 netPosition;
	Quaternion netRotation;

	void Update()
	{
		if(!photonView.isMine)
		{
			// this is bad network code, since it's adding lag, instead of predicting a position
			// based on velocity and reducing the effect of lag.
			transform.position = Vector3.Lerp(transform.position, netPosition, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, netRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting) // local player : send data
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else // remote player : receive data
		{
			netPosition = (Vector3)stream.ReceiveNext();
			netRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}
