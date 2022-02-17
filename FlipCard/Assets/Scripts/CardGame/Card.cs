using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card : MonoBehaviour
{
    public GameObject front;

    public int id;
    public int colorAndTextureId;
    public GameObject greenParticle;
    public GameObject redParticle;
    public GameObject cardCorrectParticle;
    public GameObject passCardParticle;


    //
    private AudioSource m_AudioSource;


    private Vector3 oriLocaiton;

    private bool isShowParticle = true;

    public bool isPicked = false;

    private int characterIn = 0;
    // Start is called before the first frame update
    void Start()
    {
        colorAndTextureId = -1;
        oriLocaiton = this.transform.position;
        m_AudioSource = this.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColor(int m)
    {
        var material = front.GetComponent<MeshRenderer>();
        material.material.color = Global.colorList[m];
        colorAndTextureId = m;
    }

    public void SetTexture(int textureId)
    {
        var material = front.GetComponent<MeshRenderer>();
        // material.material.SetTexture("_MainTex", GameControler.Instance.textures[textureId]);
        material.material = GameControler.Instance.materialList[textureId];
        colorAndTextureId = textureId;
    }


    public void FlipCard()
    {
        StartCoroutine(FlipCardIE());
    }

    public void RiseCard()
    {
        StartCoroutine(RiseCardIE());
    }

    public void SetRandomColorForGameRound(int i)
    {
        int colorTypes = (int)Mathf.Pow(2.0f, (float)i + 1);
        int colorId = Random.Range(0, colorTypes);

        SetColor(colorId);
    }
    // character pickupCard
    public void PickUpCard()
    {
        this.transform.position = this.transform.position + new Vector3(0, 5.0f, 0);
        this.transform.localEulerAngles = new Vector3(-231f, 0, 0);
    }


    public void ShowAllCard()
    {
        this.transform.Rotate(new Vector3(0, 0, 180));
    }

    public void UnShowAllCard()
    {
        this.transform.Rotate(new Vector3(0, 0, 180));
    }


    IEnumerator RiseCardIE()
    {
        for (int i = 0; i < 10; i++)
        {
            this.transform.position = this.transform.position + new Vector3(0, 0.2f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void BackToOriginalPosition()
    {
        if (isPicked)
        {
            this.transform.localEulerAngles = new Vector3(-180, 0, 180);
            this.transform.position = oriLocaiton + new Vector3(0, 0.5f, 0);
        }
        cardCorrectParticle.SetActive(false);

    }


    public void LowerCard()
    {
        StartCoroutine(LowerCardIE());
    }

    IEnumerator LowerCardIE()
    {
        for (int i = 0; i < 10; i++)
        {
            this.transform.position = this.transform.position + new Vector3(0, -0.2f, 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void MoveCard(Vector3 location)
    {
        Vector3 destDelta = (location - this.transform.position) / 10;
        StartCoroutine(MoveCardIE(destDelta));
    }

    IEnumerator MoveCardIE(Vector3 destDelta)
    {
        for (int i = 0; i < 10; i++)
        {
            this.transform.position = this.transform.position + destDelta;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void SetCardReadyToPick()
    {
        greenParticle.SetActive(true);
        redParticle.SetActive(false);
    }

    public void SetCardIsPicked()
    {
        isPicked = true;
    }

    public void FlipPickedUpCard(int id)
    {
        if (isPicked)
        {
            FlipCard();
            CorrectPickedCard(id);
        }

        isShowParticle = false;

    }

    public void UnShowPickedCard()
    {
        if (isPicked)
        {
            this.transform.Rotate(new Vector3(0, 0, 180));
            this.transform.position = oriLocaiton;
        }
        isPicked = false;

        isShowParticle = true;

    }

    public void CorrectPickedCard(int id)
    {
        if (colorAndTextureId == id)
            cardCorrectParticle.SetActive(true);
    }

    public void PickUpCardV2()
    {
        StartCoroutine(PickUpCardV2IE());
    }

    IEnumerator PickUpCardV2IE()
    {
        for (int i = 0; i < 8; i++)
        {
            this.transform.position = this.transform.position + new Vector3(0, 0.7f, 0);
            this.transform.localEulerAngles += new Vector3(5.5f, 0, 0);
            yield return new WaitForSeconds(0.05f);
        }

        for (int i = 0; i < 4; i++)
        {
            this.transform.position = this.transform.position + new Vector3(0, -0.1f, 0);
        }

    }

    public void DisableParticle()
    {
        // greenParticle.SetActive(false);
        // redParticle.SetActive(false);
        cardCorrectParticle.SetActive(false);
    }


    IEnumerator FlipCardIE()
    {
        Quaternion targetAngle = Quaternion.Euler(0, 0, 18);
        // int rotateStep = 180 / 10; 
        for (int i = 0; i < 10; i++)
        {
            this.transform.Rotate(new Vector3(0, 0, 18));
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        characterIn++;
        if (isShowParticle)
        {
            // Debug.Log("T character in" + characterIn);
            passCardParticle.SetActive(true);
            m_AudioSource.Play();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        characterIn--;
        if (isShowParticle)
        {
            // Debug.Log("character out" + characterIn);
            if (characterIn == 0)
            {
                passCardParticle.SetActive(false);
                m_AudioSource.Pause();
            }

        }
        else
        {
            passCardParticle.SetActive(false);
            m_AudioSource.Pause();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isShowParticle && passCardParticle.activeSelf)
            passCardParticle.SetActive(false);
    }



}
