using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour
{
    public Canvas startMenu;
    public Canvas endMenu;
    public Scoring scoring;
    public AudioSource BGM;
    public AudioSource GameOverBGM;

    public void OnBackButtonClicked()
    {
        startMenu.gameObject.SetActive(true);
        endMenu.gameObject.SetActive(false);
        scoring.ResetScore();
        BGM.Play();
        GameOverBGM.Stop();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
