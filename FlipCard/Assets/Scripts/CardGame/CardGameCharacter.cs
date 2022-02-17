using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using TMPro;


[System.Serializable]
public class CardGameCharacter : CardSuperStateMachine
{
    private float Vertical = 0;
    private float Horizontal = 0;

    public float TurnSpeed = 3;
    public float Speed = 8.0f;


    public float minX = -246.0f;
    public float maxX = -233.0f;
    public float minZ = 25.0f;
    public float maxZ = 38.0f;

    // Audio
    public AudioClip m_WalkSound;
    public AudioClip m_LoseSound;
    public AudioClip m_WinSound;
    public AudioClip m_PutDownSound;
    public AudioClip m_ChooseCorrectCardSound;
    public AudioClip m_ChooseCard;
    public AudioClip m_FlipCard;

    // Audio Source

    private AudioSource m_AudioSource;

    public bool isAI;
    private bool startSelectCard = false;

    public TextMeshProUGUI _scoreText;

    public GameObject targetCardAi;

    public List<GameObject> targetDestListForAi;

    public enum PlayerState
    {
        Idle,
        Start,
        Selected,
        WaitingOthers,
        Jump,
        Win,
        Defeat,
        Rotate,
        FallDown,
        Run,
        Walk,
        PutUpIdle,
        OK,
        PutUp,
        FlipCard,
        PutDownCard
    }

    private Animator _animator = null;
    private CharacterController _characterController;

    private bool isFrozen = false;
    private bool isHoldingCard = false;
    private bool isWalkingBack = false;


    private Vector3 startLoc;
    private Quaternion startRotation;


    private NavMeshAgent _navMeshAgent;


    private int pickedCardType = -1;
    private int _score = 0;

    // Use this for initialization
    void Start()
    {
        isFrozen = true;
        startLoc = this.transform.position;
        startRotation = this.transform.rotation;
        _score = 0;
        _animator = this.GetComponent<Animator>();
        if (!_animator)
        {
            _animator = this.GetComponentInChildren<Animator>();
        }
        _characterController = this.GetComponent<CharacterController>();

        currentState = PlayerState.Idle;

        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.isStopped = true;

        m_AudioSource = this.GetComponent<AudioSource>();


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
        if (!isAI && startSelectCard)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SelectCard();
            }
        }

    }

    private void SelectCard()
    {
        // collider get the card
        Collider[] colloders = Physics.OverlapSphere(this.transform.position, 0.5f, 1 << LayerMask.NameToLayer("Cards"));
        // Debug.Log("get the cards" + colloders.Length);
        // only pick up one card
        if (colloders.Length == 1)
        {
            // Debug.Log("find card");
            Card pickedCard = colloders[0].GetComponentInParent<Card>();
            if (pickedCard != null && !pickedCard.isPicked)
            {
                Froze();
                pickedCard.PickUpCardV2();
                currentState = PlayerState.PutUp;
                pickedCard.SetCardIsPicked();
                HoldCard(pickedCard);
                pickedCardType = pickedCard.colorAndTextureId;

            }
            // can not move after picked up card.
        }
    }

    private void HoldCard(Card pickedCard)
    {
        if (!pickedCard)
            return;

        isHoldingCard = true;

        // currentState = PlayerState.PutUpIdle;

        Vector3 tmpLocation = new Vector3(pickedCard.transform.position.x, 0, pickedCard.transform.position.z - 1.5f);
        this.transform.position = tmpLocation;
        this.transform.localEulerAngles = new Vector3(0, -180, 0);
    }


    public void CountScore(int val)
    {
        if (!isHoldingCard)
            isHoldingCard = true;
        if (val == pickedCardType)
        {
            _score++;
            _scoreText.text = "X" + _score.ToString();
            currentState = PlayerState.Win;
        }
        else
        {
            currentState = PlayerState.Defeat;
        }

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
        SetUserVelocity(Vector3.zero);
        SetUserFrozen(true);


        if (isAI)
        {
            _navMeshAgent.isStopped = true;
        }

    }

    public void UnFroze()
    {
        SetUserFrozen(false);
        startSelectCard = true;
    }

    public void WalkBack()
    {
        startSelectCard = false;
        // finished count
        isHoldingCard = false;
        _navMeshAgent.SetDestination(startLoc);
        _navMeshAgent.isStopped = false;
        isWalkingBack = true;
        currentState = PlayerState.Walk;
    }

    public void BackToStartPoint()
    {
        // this.transform.position = startLoc;
        isWalkingBack = false;
        this.transform.rotation = startRotation;
        currentState = PlayerState.Idle;
        _navMeshAgent.ResetPath();
        _navMeshAgent.isStopped = true;
        // reset Ai target loc
        if (isAI)
        {
            // reset targetCardAi
            int tarGetIdx = Random.Range(0, 4);
            targetCardAi = targetDestListForAi[tarGetIdx];

        }
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


    public void FlipCard()
    {
        currentState = PlayerState.FlipCard;
    }

    public void PutDownCard()
    {
        currentState = PlayerState.PutDownCard;
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
        if (!isAI)
        {
            if ((Horizontal != 0 || Vertical != 0) &&
                !isFrozen)
            {
                Rotating(Horizontal, Vertical);
                Translate(Horizontal, Vertical);
                if (!currentState.Equals(PlayerState.Walk))
                    currentState = PlayerState.Walk;
            }
            else
            {
                if (!currentState.Equals(PlayerState.Idle) && !isHoldingCard && !isWalkingBack)
                    currentState = PlayerState.Idle;
            }

        }
        else
        {
            // for AI if is close to the dest, try to pick up the card
            if (Vector3.Distance(new Vector3(this.transform.position.x, 0, this.transform.position.z)
                , new Vector3(targetCardAi.transform.position.x, 0, targetCardAi.transform.position.z)) <= 0.05f)
            {
                _navMeshAgent.isStopped = true;
                currentState = PlayerState.Idle;
                if (!isFrozen && _navMeshAgent.isStopped && !isHoldingCard && startSelectCard)
                    SelectCard();

            }

        }
        // walk back to start point
        if (isWalkingBack)
        {
            if (Vector3.Distance(new Vector3(this.transform.position.x, 0, this.transform.position.z)
            , new Vector3(startLoc.x, 0, startLoc.z)) <= 0.05f)
            {
                BackToStartPoint();
            }
        }

    }



    // player status methods

    #region state methods

    public void PutUp_EnterState()
    {
        _animator.Play("PutUpCard");
        m_AudioSource.clip = m_ChooseCard;
        m_AudioSource.Play();
    }

    public void PutUp_SuperState()
    {

    }
    public void PutUp_ExitState()
    {
        // currentState = PlayerState.PutUpIdle;
    }


    public void OK_EnterState()
    {
        _animator.Play("ok");
    }

    public void OK_SuperState()
    {

    }

    public void OK_ExitState()
    {

    }

    public void PutDownCard_EnterState()
    {
        _animator.Play("PutDownCard");
        m_AudioSource.clip = m_PutDownSound;
        m_AudioSource.Play();
    }

    public void PutDownCard_SuperState()
    {

    }

    public void PutDownCard_ExitState()
    {
        // currentState = PlayerState.Idle;

    }

    public void Win_EnterState()
    {
        _animator.Play("win");
        m_AudioSource.clip = m_WinSound;
        m_AudioSource.Play();
    }

    public void Win_SuperState()
    {

    }

    public void Win_ExitState()
    {
        // currentState = PlayerState.Idle;
    }

    public void FlipCard_EnterState()
    {
        _animator.Play("FlipCard");
        m_AudioSource.clip = m_FlipCard;
        m_AudioSource.Play();
    }

    public void FlipCard_SuperState()
    {

    }

    public void FlipCard_ExitState()
    {
        // currentState = PlayerState.PutUpIdle;
    }


    public void Defeat_EnterState()
    {
        _animator.Play("defeat");
        m_AudioSource.clip = m_LoseSound;
        m_AudioSource.Play();
    }

    public void Defeat_SuperState()
    {

    }

    public void Defeat_ExitState()
    {
        // currentState = PlayerState.Idle;
    }

    public void Walk_EnterState()
    {
        _animator.Play("walk");
        if (!isAI)
        {
            m_AudioSource.clip = m_WalkSound;
            m_AudioSource.loop = true;
            m_AudioSource.Play();
        }

    }

    public void Walk_SuperState()
    {
    }

    public void Walk_ExitState()
    {
        if (!isAI)
        {

            m_AudioSource.loop = false;
            m_AudioSource.Pause();
        }
    }

    public void PutUpIdle_EnterState()
    {
        _animator.Play("PutUpIdle");
    }

    public void PutUpIdle_SuperState()
    {

    }

    public void PutUpIdle_ExitState()
    {

    }



    public void Pick_EnterState()
    {
    }

    public void Pick_SuperState()
    {

    }

    public void Pick_ExitState()
    {

    }

    public void Ready_EnterState()
    {
        _animator.Play("ready");
    }

    public void Ready_SuperState()
    {

    }

    public void Ready_ExitState()
    {

    }


    public void Run_EnterState()
    {
        _animator.Play("Run");
    }

    public void Run_SuperState()
    {

    }

    public void Run_ExitState()
    {

    }

    public void Rotate_EnterState()
    {

    }

    public void Rotate_SuperState()
    {

    }

    public void Rotate_ExitState()
    {

    }


    public void Idle_EnterState()
    {
        _animator.Play("idle");
    }

    public void Idle_SuperState()
    {

    }

    public void Idle_ExitState()
    {

    }



    public void Jump_EnterState()
    {

    }

    public void Jump_SuperState()
    {

    }

    public void Jump_ExitState()
    {

    }

    #endregion

    public void AiMovement()
    {
        StartCoroutine(AiMovementIE());
    }
    IEnumerator AiMovementIE()
    {
        if (isAI)
        {
            currentState = PlayerState.Walk;
            Vector3 targetPos = targetCardAi.transform.position;
            // StartCoroutine(AiMoveTo(targetPos));
            _navMeshAgent.SetDestination(targetPos);
            _navMeshAgent.isStopped = false;
        }
        yield return null;

    }

    public void PlayReadyAnimation()
    {
        _animator.Play("ready");
    }


    IEnumerator AiMoveTo(Vector3 dest)
    {
        currentState = PlayerState.Walk;
        while (this.transform.position != dest)
        {

            this.transform.position = Vector3.MoveTowards(this.transform.position,
                                                        dest, Speed * Time.deltaTime);

            yield return 0;
        }
        currentState = PlayerState.Idle;
        // move to the dest and try to pick card
        // SelectCard();
    }





}
