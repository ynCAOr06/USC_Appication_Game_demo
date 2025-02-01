using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public GameObject particle;

	private void OnTriggerEnter(Collider other)
	{
		if (particle)
		{
			Instantiate(particle, transform.position, Quaternion.identity);
		}

		Destroy(gameObject);

		BeHit behit = other.GetComponent<BeHit>();

		if (behit)
		{
			behit.GetHit(2);
		}
	}
}
