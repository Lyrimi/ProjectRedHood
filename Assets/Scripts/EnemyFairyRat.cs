using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFairyRat : EnemyFairy
{
	public GameObject surrenderParticle;
	public float surrenderRotationMin;
	public float surrenderRotationMax;

	new public void Death() {
		GameObject surrender = Instantiate(surrenderParticle, transform.position, transform.rotation);
		surrender.GetComponent<SpriteRenderer>().flipX = sr.flipX;
		float rotation = Random.Range(surrenderRotationMin, surrenderRotationMax);
		if (Random.Range(0, 2) == 0) {
			rotation *= -1;
		}
		Rigidbody2D rb = surrender.GetComponent<Rigidbody2D>();
		rb.angularVelocity = rotation;
		rb.velocity = this.rb.velocity;
		base.Death();
	}
}
