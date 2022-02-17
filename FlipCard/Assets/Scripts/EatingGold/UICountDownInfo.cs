using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UICountDownInfo : MonoBehaviour
{

    public TextMeshProUGUI Text;
    public Image clockImage;
    public Image cloudImage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CountDown(int count)
    {
        Text.enabled = true;
        Text.text = count.ToString();
        clockImage.enabled = true;
        cloudImage.enabled = true;
        StartCoroutine(Count(count));
    }

    IEnumerator Count(int count)
    {
        while(count > 0)
        {
            yield return new WaitForSeconds(1);
            count -= 1;
            Text.text = "" + count;
        }
    }

    public void Dissmiss()
    {
        Text.enabled = false;
        clockImage.enabled = false;
        cloudImage.enabled = false;
    }
}
