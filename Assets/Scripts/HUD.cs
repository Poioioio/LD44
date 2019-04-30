using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    #region singletonstuff
    private static HUD instance;
    private HUD()
    {
        instance = this;
    }

    public static HUD GetInstance()
    {
        if (instance)
            return instance;
        else
        {
            instance = new HUD();
            return instance;
        }
    }
    #endregion
    AudioSource audioSource;
    public AudioClip coinSound;
    public AudioClip evilLaugh;

    public Text copperText;
    public Image copperImage;
    public Text silverText;
    public Image silverImage;
    public Text goldText;
    public Image goldImage;

    public Image showCopper;
    public Image showSilver;
    public Image showGold;
    public RawImage intro;
    public RawImage gameOver;
    public Image victory;

    private bool firstCopper = true;
    private bool firstSilver = true;
    private bool firstGold = true;

    Perso_controler protag;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();

        protag = FindObjectOfType<Perso_controler>();
        protag.enabled = false;

        intro.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if ( Input.anyKeyDown )
        {
            Time.timeScale = 1f;
            if( gameOver.gameObject.activeInHierarchy )
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            showCopper.gameObject.SetActive(false);
            showSilver.gameObject.SetActive(false);
            showGold.gameObject.SetActive(false);
            intro.gameObject.SetActive(false);
            gameOver.gameObject.SetActive(false);
            victory.gameObject.SetActive(false);

            StartCoroutine(ReEnableProtag());
        }        
    }

    IEnumerator ReEnableProtag()
    {
        yield return new WaitForSeconds(0.2f);
        protag.enabled = true;
    }


    public void UpdateCopperCoin(int nb)
    {
        bool gotSome = nb > 0;
        if( gotSome )
            audioSource.PlayOneShot(coinSound);

        if (firstCopper)
        {
            firstCopper = false;
            showCopper.gameObject.SetActive(true);
            protag.enabled = false;
            Time.timeScale = 0.01f;
        }

        copperText.gameObject.SetActive(gotSome);
        copperImage.gameObject.SetActive(gotSome);
        copperText.text = "Tibet coins : " + nb.ToString();

    }

    public void UpdateSilverCoin(int nb)
    {
        bool gotSome = nb > 0;
        if (gotSome)
            audioSource.PlayOneShot(coinSound);

        if (firstSilver)
        {
            firstSilver = false;
            showSilver.gameObject.SetActive(true);
            protag.enabled = false;
            Time.timeScale = 0.01f;
        }

        silverText.gameObject.SetActive(gotSome);
        silverImage.gameObject.SetActive(gotSome);
        silverText.text = "Drachmes : " + nb.ToString();

    }

    public void UpdateGoldCoin(int nb)
    {
        bool gotSome = nb > 0;
        if (gotSome)
            audioSource.PlayOneShot(coinSound);
        if (firstGold)
        {
            firstGold = false;
            showGold.gameObject.SetActive(true);
            protag.enabled = false;
            Time.timeScale = 0.01f;
        }

        goldText.gameObject.SetActive(gotSome);
        goldImage.gameObject.SetActive(gotSome);
        goldText.text = "Stateres : " + nb.ToString();

    }

    public void ShowVictory()
    {
        Time.timeScale = 0f;
        victory.gameObject.SetActive(true);
    }

    public void ShowGameOver(bool laugh = false)
    {
        Time.timeScale = 0f;
        audioSource.PlayOneShot(evilLaugh);
        protag.enabled = false;
        gameOver.gameObject.SetActive(true);
    }
}
