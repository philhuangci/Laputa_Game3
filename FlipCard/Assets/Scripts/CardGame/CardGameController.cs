using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class CardGameController : MonoBehaviour
{
	public float Vertical = 0;
	public float Horizontal = 0;
	public float TurnSpeed = 3;
	public float Speed = 20.0f;

	public GameObject startCube;

	// Phil For CardGame setting
	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;

	public bool isAI;

	public Text _scoreText;


	private bool isFrozen = false;
	private Vector3 startLoc;
	private Quaternion startRotation;

	private int pickedCardType = -1;
	private int _score = 0;

	// Use this for initialization
	void Start()
	{
		isFrozen = true;
		startLoc = this.transform.position;
		startRotation = this.transform.rotation;
		_score = 0;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isFrozen)
		{
			Horizontal = Input.GetAxis("Horizontal");
			Vertical = Input.GetAxis("Vertical");
		}

		// pick up card
		if (Input.GetKeyDown(KeyCode.R))
		{
			// collider get the card
			Collider[] colloders = Physics.OverlapSphere(this.transform.position, 0.5f, 1 << LayerMask.NameToLayer("Cards"));
			Debug.Log("get the cards" + colloders.Length);
			// only pick up one card
			if (colloders.Length == 1)
			{
				Debug.Log("find card");
				Card pickedCard = colloders[0].GetComponent<Card>();
				if (pickedCard != null)
				{
					pickedCard.PickUpCard();
					pickedCardType = pickedCard.colorAndTextureId;
					Froze();
				}
				// can not move after picked up card.
			}
		}
	}

	public void CountScore(int val)
	{
		if (val == pickedCardType)
		{
			_score++;
			_scoreText.text = _score.ToString();
		}
		// finished count
		ResetPickedCardType();

	}

	public int GetPickedCardType()
	{
		return pickedCardType;
	}

	public void ResetPickedCardType()
	{
		pickedCardType = -1;
	}


	public void SetUserFrozen(bool isFrozen)
	{
		this.isFrozen = isFrozen;
	}

	public void SetUserVelocity(Vector3 vel)
	{
		this.GetComponent<Rigidbody>().velocity = vel;
	}

	public void Froze()
	{
		SetUserFrozen(true);
		SetUserVelocity(Vector3.zero);
	}

	public void UnFroze()
	{
		SetUserFrozen(false);
	}

	public void BackToStartPoint()
	{
		this.transform.position = startLoc;
		this.transform.rotation = startRotation;
	}

	public void Talk(string str)
	{
		StartCoroutine(TalkIE(str));
	}

	IEnumerator TalkIE(string str)
	{
		yield return null;
	}


	void Rotating(float hor, float ver)
	{
		Vector3 dir = new Vector3(hor, 0, ver);
		Quaternion quaDir = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * TurnSpeed);
	}


	public void Translate(float hor, float ver)
	{

		Vector3 dir = new Vector3(hor, 0, ver);

		//GetComponent<Rigidbody>().velocity = dir * 10;
		Vector3 transformValue = dir * Time.fixedDeltaTime * Speed;
		transform.Translate(transformValue, Space.World);
		// boundary limitation
		Vector3 lastPosition = new Vector3(Mathf.Clamp(this.transform.position.x, minX, maxX), 0,
			Mathf.Clamp(this.transform.position.z, minZ, maxZ));
		this.transform.position = lastPosition;

	}

	void FixedUpdate()
	{
		StartCoroutine(InternalUpdate());

	}

	IEnumerator InternalUpdate()
	{
		if (!isAI)
		{
			if ((Horizontal != 0 || Vertical != 0) &&
				!isFrozen)
			{
				Rotating(Horizontal, Vertical);
				Translate(Horizontal, Vertical);
			}
		}
		yield return null;
	}

	


}
