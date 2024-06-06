using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreThing : MonoBehaviour
{
    [SerializeField] private Player player;
    private Text txt;

    private void Awake()
    {
        txt = this.GetComponent<Text>();
    }

    void Update()
    {
        int score = player.score;
        txt.text = score.ToString();
    }
}
