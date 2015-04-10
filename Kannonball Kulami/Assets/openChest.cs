using UnityEngine;
using System.Collections;

public class openChest : MonoBehaviour {
    public Animator openAnimator;
	public MeshCollider top;
	public MeshCollider keyHole;
	void OnMouseDown()
    {
        openAnimator.SetTrigger("isOpen");
		top.enabled = false;
		keyHole.enabled = false;
    }
}
