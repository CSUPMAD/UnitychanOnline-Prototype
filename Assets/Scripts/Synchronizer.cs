using UnityEngine;
using System.Collections;


//Animationの設定を行うスクリプト
public class Synchronizer : Photon.MonoBehaviour {
	protected Animator anim;
	
	//受信データ
	private Vector3 receivePosition = Vector3.zero;
	private Quaternion receiveRotation = Quaternion.identity;
	private Vector3 receiveVelocity = Vector3.zero;

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
			/*
			transform.position = (Vector3)stream.ReceiveNext();
			transform.rotation = (Quaternion)stream.ReceiveNext();
			rigidbody.velocity = (Vector3)stream.ReceiveNext();
			*/
			receivePosition = (Vector3)stream.ReceiveNext();
			receiveRotation = (Quaternion)stream.ReceiveNext();
			receiveVelocity = (Vector3)stream.ReceiveNext();

			anim = GetComponent< Animator >();
			anim.SetFloat("Speed",(float)stream.ReceiveNext());
			anim.SetFloat("Direction",(float)stream.ReceiveNext());
			anim.SetBool("Jump",(bool)stream.ReceiveNext());
			anim.SetBool("Rest",(bool)stream.ReceiveNext());
			anim.SetFloat("JumpHeight",(float)stream.ReceiveNext());
			anim.SetFloat("GravityControl",(float)stream.ReceiveNext());
		}
	}

	void Update() {
		//自分以外のプレイヤーの補正
		if(!photonView.isMine){
			transform.position = Vector3.Lerp(transform.position, receivePosition, Time.deltaTime * 10);
			transform.rotation = Quaternion.Lerp(transform.rotation, receiveRotation, Time.deltaTime * 10);
			rigidbody.velocity = Vector3.Lerp(rigidbody.velocity, receiveVelocity, Time.deltaTime * 10);
		}
	}

}

