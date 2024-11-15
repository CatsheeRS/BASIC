﻿using System;
//using Rewired;
using UnityEngine;

 
public class CameraScript_Simple : MonoBehaviour
{
	 
	private void Start()
	{
		//this.playerInput = ReInput.players.GetPlayer(0);
		this.offset = base.transform.position - this.player.transform.position;
	}

	 
	private void LateUpdate()
	{
		base.transform.position = this.player.transform.position + this.offset;
		base.transform.rotation = this.player.transform.rotation * Quaternion.Euler(0f, (float)this.lookBehind, 0f);
	}

	 
	public GameObject player;

	 
	private int lookBehind;

	 
	private Vector3 offset;

	 
	//private Player playerInput;
}
