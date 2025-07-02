using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlanCollisionHandler : MonoBehaviour
{
    private int remainingLives = 6;
    public TextMeshProUGUI LifeText;
    public Spawner spawner;
    public Canvas GameOverCanvas;
    public AudioSource musicSource;
    public AudioSource GameOverBGM;
    public AudioSource HurtSound;
    public AudioSource GameOverSound;


    // Start is called before the first frame update
    void Start()
    {
        // remainingLives = 6;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
    }
    
    public void UpdateLife()
    {
        LifeText.text = "Life: " + remainingLives/2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            Debug.Log("Loss");
            Destroy(other.gameObject);
            ReduceLife();
        }
    }

    public void ReduceLife()
    {
        remainingLives--;
        HurtSound.Play();
        if (remainingLives <= 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        spawner.gameObject.SetActive(false);
        GameOverCanvas.gameObject.SetActive(true);
        remainingLives = 6;
        spawner.ClearSpawnedCubes();
        musicSource.Stop();
        GameOverSound.Play();
        GameOverBGM.Play();
    }
    
}
