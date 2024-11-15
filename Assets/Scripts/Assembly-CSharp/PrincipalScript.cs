﻿using System;
using UnityEngine;
using UnityEngine.AI;

 
public class PrincipalScript : MonoBehaviour
{
	 
	private void Start()
	{
		this.agent = base.GetComponent<NavMeshAgent>(); //Get the agent
		this.audioQueue = base.GetComponent<AudioQueueScript>();
		this.audioDevice = base.GetComponent<AudioSource>();
	}

	 
	private void Update()
	{
		if (this.seesRuleBreak)
		{
			this.timeSeenRuleBreak += 1f * Time.deltaTime;
			if ((double)this.timeSeenRuleBreak >= 0.5 & !this.angry) // If the principal sees the player break a rule for more then 1/2 of a second
			{
				this.angry = true;
				this.seesRuleBreak = false;
				this.timeSeenRuleBreak = 0f;
				this.TargetPlayer(); //Target the player
				this.CorrectPlayer();
			}
		}
		else
		{
			this.timeSeenRuleBreak = 0f;
		}
		if (this.coolDown > 0f)
		{
			this.coolDown -= 1f * Time.deltaTime;
		}
	}

	 
	private void FixedUpdate()
	{
		if (!this.angry) // If the principal isn't angry
		{
			this.aim = this.player.position - base.transform.position; // If he sees the player and the player has guilt
			if (Physics.Raycast(base.transform.position, this.aim, out this.hit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) & this.hit.transform.tag == "Player" & this.playerScript.guilt > 0f & !this.inOffice & !this.angry)
			{
				this.seesRuleBreak = true;
			}
			else
			{
				this.seesRuleBreak = false;
				if (this.agent.velocity.magnitude <= 1f & this.coolDown <= 0f)
				{
					this.Wander(); // Start wandering again
				}
			}
			this.aim = this.bully.position - base.transform.position;
			if (Physics.Raycast(base.transform.position, this.aim, out this.hit, float.PositiveInfinity, 769) & this.hit.transform.name == "Its a Bully" & this.bullyScript.guilt > 0f & !this.inOffice & !this.angry)
			{
				this.TargetBully();
			}
		}
		else
		{
			this.TargetPlayer(); // If the principal is angry, target the player
		}
	}

	 
	private void Wander()
	{
		this.playerScript.principalBugFixer = 1;
		this.wanderer.GetNewTarget();
		this.agent.SetDestination(this.wanderTarget.position);
		if (this.agent.isStopped)
		{
			this.agent.isStopped = false;
		}
		this.coolDown = 1f;
		if (UnityEngine.Random.Range(0f, 10f) <= 1f)
		{
			this.quietAudioDevice.PlayOneShot(this.aud_Whistle);
		}
	}

	 
	private void TargetPlayer()
	{
		this.agent.SetDestination(this.player.position);
		this.coolDown = 1f;
	}

	 
	private void TargetBully()
	{
		if (!this.bullySeen)
		{
			this.agent.SetDestination(this.bully.position);
			this.audioQueue.QueueAudio(this.audNoBullying);
			this.bullySeen = true;
		}
	}

	 
	private void CorrectPlayer()
	{
		this.audioQueue.ClearAudioQueue();
		if (this.playerScript.guiltType == "faculty")
		{
			this.audioQueue.QueueAudio(this.audNoFaculty);
		}
		else if (this.playerScript.guiltType == "running")
		{
			this.audioQueue.QueueAudio(this.audNoRunning);
		}
		else if (this.playerScript.guiltType == "drink")
		{
			this.audioQueue.QueueAudio(this.audNoDrinking);
		}
		else if (this.playerScript.guiltType == "escape")
		{
			this.audioQueue.QueueAudio(this.audNoEscaping);
		}
	}

	 
	private void OnTriggerStay(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			this.inOffice = true;
		}
		if (other.tag == "Player" & this.angry & !this.inOffice)
		{
			this.inOffice = true;
			this.playerScript.principalBugFixer = 0;
			this.agent.Warp(new Vector3(10f, 0f, 170f)); //Teleport the principal to the office
			this.agent.isStopped = true; //Stop the principal from moving
			this.cc.enabled = false;
			other.transform.position = new Vector3(10f, 4f, 160f); // Teleport the player to the office
			other.transform.LookAt(new Vector3(base.transform.position.x, other.transform.position.y, base.transform.position.z)); // Get the plaer to look at the principal
			this.cc.enabled = true;
			this.audioQueue.QueueAudio(this.aud_Delay);
			this.audioQueue.QueueAudio(this.audTimes[this.detentions]); //Play the detention time sound
			this.audioQueue.QueueAudio(this.audDetention);
			int num = Mathf.RoundToInt(UnityEngine.Random.Range(0f, 2f));
			this.audioQueue.QueueAudio(this.audScolds[num]); // Say one of the other lines
			this.officeDoor.LockDoor((float)this.lockTime[this.detentions]); // Lock the door
			if (this.baldiScript.isActiveAndEnabled) this.baldiScript.Hear(base.transform.position, 8f);
			this.coolDown = 5f;
			this.angry = false;
			this.detentions++;
			if (this.detentions > 4) // Prevent detention number from going above 4
			{
				this.detentions = 4;
			}
		}
	}

	 
	private void OnTriggerExit(Collider other)
	{
		if (other.name == "Office Trigger")
		{
			this.inOffice = false;
		}
		if (other.name == "Its a Bully")
		{
			this.bullySeen = false;
		}
	}

	 
	public bool seesRuleBreak;

	 
	public Transform player;

	 
	public Transform bully;

	 
	public bool bullySeen;

	 
	public PlayerScript playerScript;

	 
	public BullyScript bullyScript;

	 
	public BaldiScript baldiScript;

	 
	public Transform wanderTarget;

	 
	public AILocationSelectorScript wanderer;

	 
	public DoorScript officeDoor;

	 
	public float coolDown;

	 
	public float timeSeenRuleBreak;

	 
	public bool angry;

	 
	public bool inOffice;

	 
	private int detentions;

	 
	private int[] lockTime = new int[]
	{
		15,
		30,
		45,
		60,
		99
	};

	 
	public AudioClip[] audTimes = new AudioClip[5];

	 
	public AudioClip[] audScolds = new AudioClip[3];

	 
	public AudioClip audDetention;

	 
	public AudioClip audNoDrinking;

	 
	public AudioClip audNoBullying;

	 
	public AudioClip audNoFaculty;

	 
	public AudioClip audNoLockers;

	 
	public AudioClip audNoRunning;

	 
	public AudioClip audNoStabbing;

	 
	public AudioClip audNoEscaping;

	 
	public AudioClip aud_Whistle;

	 
	public AudioClip aud_Delay;

	 
	private NavMeshAgent agent;

	 
	private AudioQueueScript audioQueue;

	 
	private AudioSource audioDevice;

	 
	public AudioSource quietAudioDevice;

	 
	private RaycastHit hit;

	 
	private Vector3 aim;

	public CharacterController cc;
}
