using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalTime : MonoBehaviour
{
    public Text elapsedTimeText;
    public Text bestTimeText;
    public Text coinText;

    public RawImage redKey;
    public RawImage greenKey;
    public RawImage blueKey;
    public RawImage doubleJump;
    public RawImage walljump;
    public RawImage dash;

    // Update is called once per frame
    void Update()
    {
        int minutes1 = Mathf.FloorToInt(PlayerMovement.elapsedTime / 60);
        int seconds1 = Mathf.FloorToInt(PlayerMovement.elapsedTime % 60);

        if (seconds1 < 10)
        {
            elapsedTimeText.text = minutes1 + ":0" + seconds1;
        }
        else 
        {
            elapsedTimeText.text = minutes1 + ":" + seconds1;
        }

        int minutes2 = Mathf.FloorToInt(PlayerMovement.bestTime / 60);
        int seconds2 = Mathf.FloorToInt(PlayerMovement.bestTime % 60);

        if (seconds2 < 10)
        {
            bestTimeText.text = minutes2 + ":0" + seconds2;
        }
        else 
        {
            bestTimeText.text = minutes2 + ":" + seconds2;
        }

        coinText.text = "COINS: x" + PlayerMovement.coinCount;

        redKey.enabled = PlayerMovement.hasRedKey;
        greenKey.enabled = PlayerMovement.hasGreenKey; // Set to true to make it appear
        blueKey.enabled = PlayerMovement.hasBlueKey;

        doubleJump.enabled = PlayerMovement.canDoubleJump;
        walljump.enabled = PlayerMovement.canWallJumpAndSlide;
        dash.enabled = PlayerMovement.canDash;
    }
}
