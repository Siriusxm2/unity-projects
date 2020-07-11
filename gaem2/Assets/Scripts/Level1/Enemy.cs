using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Enemy : MonoBehaviour {

	protected Animator anim;
	protected Rigidbody2D rb;

	protected virtual void Start () {
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
	}

	public void DeathAnimation()
    {
		anim.SetTrigger("Death");
		rb.velocity = Vector2.zero;
    }

	private void Annihilation()
    {
		Destroy(this.gameObject);
    }
}
