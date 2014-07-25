using UnityEngine;
using System.Collections;


//Animationの設定を行うスクリプト
public class Synchronizer : Photon.MonoBehaviour {
	protected Animator anim;
	
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		// データを送る
		if (stream.isWriting) {
			//データの送信
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext(rigidbody.velocity);
			// アニメーションの連携
			anim = GetComponent< Animator >();
			stream.SendNext(anim.GetFloat("Speed"));
			stream.SendNext(anim.GetFloat("Direction"));
			stream.SendNext(anim.GetBool("Jump"));
			stream.SendNext(anim.GetBool("Rest"));
			stream.SendNext(anim.GetFloat("JumpHeight"));
			stream.SendNext(anim.GetFloat("GravityControl"));
		} else {
			//データの受信
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
			rigidbody.velocity = (Vector3)stream.ReceiveNext();
			anim = GetComponent< Animator >();
			anim.SetFloat("Speed",(float)stream.ReceiveNext());
			anim.SetFloat("Direction",(float)stream.ReceiveNext());
			anim.SetBool("Jump",(bool)stream.ReceiveNext());
			anim.SetBool("Rest",(bool)stream.ReceiveNext());
			anim.SetFloat("JumpHeight",(float)stream.ReceiveNext());
			anim.SetFloat("GravityControl",(float)stream.ReceiveNext());
		}
	}
}

