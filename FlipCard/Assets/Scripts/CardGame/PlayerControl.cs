using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public enum PlayerDirection
    {
        North,
        East,
        South,
        West
    }

    // public SceneControl SceneControl;

    public enum Selection
    {
        Left,
        Forward,
        Right
    }

    public bool IsAI;

    public PlayerDirection PlayerDir;

    Selection PlayerSel;


    bool Selected = false;
    bool Selecting = false;



    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsAI && Selecting)
        {
            if(PlayerDir == PlayerDirection.North)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    PlayerSel = Selection.Forward;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    PlayerSel = Selection.Right;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    PlayerSel = Selection.Left;
                    Selected = true;
                }
            }
            else if(PlayerDir == PlayerDirection.East)
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    PlayerSel = Selection.Left;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    PlayerSel = Selection.Forward;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerSel = Selection.Right;
                    Selected = true;
                }
            }
            else if (PlayerDir == PlayerDirection.South)
            {
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    PlayerSel = Selection.Right;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    PlayerSel = Selection.Left;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerSel = Selection.Forward;
                    Selected = true;
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    PlayerSel = Selection.Right;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    PlayerSel = Selection.Forward;
                    Selected = true;
                }
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    PlayerSel = Selection.Left;
                    Selected = true;
                }
            }
        }
    }

    public void Reset()
    {
        Selected = false;
        Selecting = false;
    }

    public void StartSelection()
    {
        Selecting = true;
    }

    public void EndSelection()
    {
        if (!Selected)
        {
            PlayerSel = (Selection)Random.Range(0, 3);
            Selected = true;
        }
        Selecting = false;
    }

    public Selection GetSelection()
    {
        return PlayerSel;
    }

    public int GetSelectedCloud()
    {
        if (PlayerDir ==  PlayerDirection.North)
        {
            if (PlayerSel == Selection.Forward)
            {
                return 4;
            }
            else if (PlayerSel == Selection.Left)
            {
                return 2;
            }
            else
            {
                return 0;
            }
        }
        else if (PlayerDir == PlayerDirection.South)
        {
            if (PlayerSel == Selection.Forward)
            {
                return 4;
            }
            else if (PlayerSel == Selection.Left)
            {
                return 6;
            }
            else
            {
                return 8;
            }
        }
        else if (PlayerDir == PlayerDirection.West)
        {
            if (PlayerSel == Selection.Forward)
            {
                return 4;
            }
            else if (PlayerSel == Selection.Left)
            {
                return 0;
            }
            else
            {
                return 6;
            }
        }
        else
        {
            if (PlayerSel == Selection.Forward)
            {
                return 4;
            }
            else if (PlayerSel == Selection.Left)
            {
                return 8;
            }
            else
            {
                return 2;
            }
        }
    }

    public void Jump(int cloud, bool success)
    {
        if(success)
        {
            StartCoroutine(JumpToCloud(cloud));
        }
        else
        {
            StartCoroutine(JumpAndColl(cloud));
        }
    }

    IEnumerator JumpToCloud(int cloud)
    {
        // Vector3 dest = SceneControl.Clouds[cloud].transform.position + new Vector3(0,0,0);
        // Vector3 delta = (dest - gameObject.transform.position) / 10;

        for(int i = 0; i < 10; i ++)
        {
            // gameObject.transform.position += delta;
            yield return new WaitForSeconds(.05f);
        }
    }

    IEnumerator JumpAndColl(int cloud)
    {
        //jump
        // Vector3 dest = SceneControl.Clouds[cloud].transform.position + new Vector3(0, 0, 0);
        // Vector3 delta = (dest - gameObject.transform.position) / 15;

        for (int i = 0; i < 10; i++)
        {
            // gameObject.transform.position += delta;
            yield return new WaitForSeconds(.05f);
        }

        // collision
        // dest = GetDefaultPostion();
        // delta = (dest - gameObject.transform.position) / 3;

        for (int i = 0; i < 3; i++)
        {
            // gameObject.transform.position += delta;
            yield return new WaitForSeconds(.05f);
        }

        yield return 0;
    }

    public void Return()
    {
        StartCoroutine(JumpBack());
    }

    IEnumerator JumpBack()
    {
        Vector3 dest = GetDefaultPostion();
        
        Vector3 delta = (dest - gameObject.transform.position) / 10;
        for (int i = 0; i < 10; i++)
        {
            gameObject.transform.position += delta;
            yield return new WaitForSeconds(.05f);
        }
    }

    Vector3 GetDefaultPostion()
    {
        Vector3 dest;
        if (PlayerDir == PlayerDirection.North)
        {
            // dest = SceneControl.Clouds[1].transform.position + new Vector3(0, 0, 0);
        }
        else if (PlayerDir == PlayerDirection.East)
        {
            // dest = SceneControl.Clouds[5].transform.position + new Vector3(0, 0, 0);
        }
        else if (PlayerDir == PlayerDirection.South)
        {
            // dest = SceneControl.Clouds[7].transform.position + new Vector3(0, 0, 0);
        }
        else
        {
            // dest = SceneControl.Clouds[3].transform.position + new Vector3(0, 0, 0);
        }
        // return dest;
        return new Vector3();
    }
    
}
