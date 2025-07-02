using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public Spawner spawner;
    public Canvas canvasToHide;
    public AudioSource musicSource;
    public AudioSource BGM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClicked()
    {
        spawner.gameObject.SetActive(true);
        canvasToHide.gameObject.SetActive(false);
        musicSource.Play();
        BGM.Stop();
    }
}
