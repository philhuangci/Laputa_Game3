    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScoreInfo : MonoBehaviour
{
    public Text ScoreNorth;
    public Text ScoreEast;
    public Text ScoreSouth;
    public Text ScoreWest;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int scoreNorth, int scoreEast, int scoreSouth, int scoreWest)
    {
        ScoreNorth.text = "" + scoreNorth;
        ScoreEast.text = "" + scoreEast;
        ScoreSouth.text = "" + scoreSouth;
        ScoreWest.text = "" + scoreWest;
    }

}
