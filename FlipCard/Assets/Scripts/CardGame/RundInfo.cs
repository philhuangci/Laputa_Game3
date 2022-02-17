using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RundInfo : MonoBehaviour
{
    Animator animator;
    Text myText;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        myText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowText(string info)
    {
        StartCoroutine(QuickShow(info));
    }


    public void ShowGameNotice(string info)
    {
        StartCoroutine(Show(info));
    }


    IEnumerator Show(string info)
    {
        myText.enabled = true;
        myText.text = info;
        // myText.IsActive
        animator.SetBool("ShowNotice", true);
        yield return new WaitForSeconds(1.5f);
        myText.enabled = false;
        animator.SetBool("ShowNotice", false);
    }

    IEnumerator QuickShow(string info )
    {
        myText.enabled = true;
        myText.text = info;
        animator.SetBool("Show", true);
        yield return new WaitForSeconds(1.0f);
        myText.enabled = false;
        animator.SetBool("Show", false);
    }


}
