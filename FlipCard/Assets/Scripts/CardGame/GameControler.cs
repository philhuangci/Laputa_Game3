using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameControler : MonoBehaviour
{
    public GameObject noticeCard;

    public GameObject carsMessageController;
    
    public GameObject characterMessageController;

    private int corretColorID;

    public static GameControler Instance;

    public RundInfo roundInfo;
    public RundInfo gameStartAndEndInfo;

    public RundInfo gameNoticeBoard;

    public Judger judger;

    // AudioClip
    public AudioClip m_StartPickCardSound;
    public AudioClip m_StopPickCardSound;

    public AudioClip m_GameOverSound;
    public AudioClip m_GameStartSound;

    public AudioClip m_RememberCardSound;
    public AudioClip m_NextRoundSound;
    public AudioClip m_FlipCardSound;

    // AudioSource;
    public AudioSource BackGroundAS;

    private AudioSource m_AudioSource;

    
    public GameObject countDownTimer;

    public List<Material> materialList;

    private void Awake()
    {
        Instance = this;
        corretColorID = 0;
    }

    private void PlayMusic(AudioClip audio)
    {
        m_AudioSource.clip = audio;
        m_AudioSource.Play();
    }


    IEnumerator PlayGame()
    {
        gameStartAndEndInfo.ShowText("Start !");
        yield return new WaitForSeconds(0.5f);
        PlayMusic(m_GameStartSound);

        yield return new WaitForSeconds(2.0f);
        // wait for other oject start
        UICountDownInfo l_timer = countDownTimer.GetComponent<UICountDownInfo>();


        CardsMessageGroup cardGroup = carsMessageController.GetComponent<CardsMessageGroup>();

        int gameRound = 3;
        for (int i = 0; i < gameRound; i++)
        {
            // BackGroundAS.Play();

            roundInfo.ShowText("Round " + (i + 1));
            yield return new WaitForSeconds(1.0f);

            gameNoticeBoard.ShowGameNotice("Remember the cards!");
            yield return new WaitForSeconds(0.5f);
            PlayMusic(m_RememberCardSound);
            yield return new WaitForSeconds(1.5f);

            // judger put up card 
            noticeCard.SetActive(true);
            judger.PutUpCard();

            // shuffle card and set materials
            cardGroup.InsideOutAlgorithm();
            cardGroup.SetUpCardMaterials(i);
            cardGroup.ShuffleCard();
            yield return new WaitForSeconds(1.0f);

            characterMessageController.BroadcastMessage("PlayReadyAnimation");
            yield return new WaitForSeconds(1.0f);
            // lift up card
            cardGroup.BroadcastMessage("RiseCard");
            yield return new WaitForSeconds(0.5f);

            // stars to flip card
            for (int flipRound = 0; flipRound < 1; flipRound++)
            {

                cardGroup.DisplayCardfForRound(i + 1);
                PlayMusic(m_FlipCardSound);
                yield return new WaitForSeconds(1.5f);
                
                cardGroup.UnflipOpenedCards();
                PlayMusic(m_FlipCardSound);
                yield return new WaitForSeconds(1.5f);

            }
            
            cardGroup.BroadcastMessage("LowerCard");
            yield return new WaitForSeconds(0.5f);

            gameNoticeBoard.ShowGameNotice("Find the card!");

            yield return new WaitForSeconds(1.5f);
            judger.FlipCard();
            RotateNoticeCard();

            yield return new WaitForSeconds(0.5f);
            PlayMusic(m_StartPickCardSound);
            // setup timer
            l_timer.CountDown(5);

            // set up notice card color and record the correct color id
            Card card = noticeCard.GetComponent<Card>();
            int colorTypes = (int)Mathf.Pow(2.0f, (float)i + 1);
            int colorId = Random.Range(0, colorTypes);
            // card.SetColor(colorId);
            card.SetTexture(colorId);
            corretColorID = colorId;
            // ============================================================

            // suer can start to pick up cards
            characterMessageController.BroadcastMessage("UnFroze");
            // Ai can move now
            characterMessageController.BroadcastMessage("AiMovement");
            
            // user have 5s to pick cards
            yield return new WaitForSeconds(5.0f);

            // PlayMusic(m_StopPickCardSound);
            characterMessageController.BroadcastMessage("Froze");
            // how picked card
            characterMessageController.BroadcastMessage("FlipCard"); 
            carsMessageController.BroadcastMessage("FlipPickedUpCard", corretColorID);
            l_timer.Dissmiss();

            yield return new WaitForSeconds(3.0f);
            
            carsMessageController.BroadcastMessage("BackToOriginalPosition");
            characterMessageController.BroadcastMessage("PutDownCard");
            yield return new WaitForSeconds(1.0f);
            MarkScore(corretColorID);

            yield return new WaitForSeconds(3.0f);

            // reset characters and cards
            characterMessageController.BroadcastMessage("WalkBack");
            //PlayMusic(m_NextRoundSound);
            //BackGroundAS.Pause();
            yield return new WaitForSeconds(5.0f);

            carsMessageController.BroadcastMessage("UnShowPickedCard");
            // hide notice card

            judger.FlipCard();
            RotateNoticeCard();
            yield return new WaitForSeconds(1.0f);

            judger.PutDownCard();
            noticeCard.SetActive(false);
            yield return new WaitForSeconds(2.0f);

        }
        gameStartAndEndInfo.ShowText("Game Over!");
        BackGroundAS.Pause();
        PlayMusic(m_GameOverSound);
        yield return new WaitForSeconds(1.0f);
    }

    private void MarkScore(int val)
    {
        var players = characterMessageController.GetComponentsInChildren<CardGameCharacter>();

        foreach (CardGameCharacter player in players)
        {
            player.CountScore(val);
        }
    }


    private void RotateNoticeCard()
    {
        Card tmpCard = noticeCard.GetComponent<Card>();
        tmpCard.FlipCard();
    }


    private void InitValue()
    {
        // textures = new List<Texture>();
        materialList = new List<Material>();
        for (int i = 1; i < 9; i++)
        {
            // textures.Add(Resources.Load("CardGameImage/Faces_Robot/Texture"+i) as Texture);
            materialList.Add(Resources.Load("CardGameImage/Faces_Robot/CardTexture" + i) as Material);
        }

        m_AudioSource = this.GetComponent<AudioSource>();

    }



    // Start is called before the first frame update
    void Start()
    {
        // call Card flip function
        InitValue();


        StartCoroutine(PlayGame());


    }

    // Update is called once per frame
    void Update()
    {

    }


}
