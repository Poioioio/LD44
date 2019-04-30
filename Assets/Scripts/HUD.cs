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
    AudioSource coinSound;

    public Text copperText;
    public Image copperImage;
    public Text silverText;
    public Image silverImage;
    public Text goldText;
    public Image goldImage;

    public Image showCopper;
    public Image showSilver;
    public Image showGold;

    private bool firstCopper = true;
    private bool firstSilver = true;
    private bool firstGold = true;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        coinSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            showCopper.gameObject.SetActive(false);
            showSilver.gameObject.SetActive(false);
            showGold.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void UpdateCopperCoin(int nb)
    {
        if (firstCopper)
        {
            firstCopper = false;
            showCopper.gameObject.SetActive(true);
            Time.timeScale = 0.01f;
        }

        bool gotSome = nb > 0;
        copperText.gameObject.SetActive(gotSome);
        copperImage.gameObject.SetActive(gotSome);
        copperText.text = "Tibet coins : " + nb.ToString();

        coinSound.enabled = true;
        if( gotSome )
            coinSound.Play();
    }

    public void UpdateSilverCoin(int nb)
    {
        if (firstSilver)
        {
            firstSilver = false;
            showSilver.gameObject.SetActive(true);
            Time.timeScale = 0.01f;
        }

        bool gotSome = nb > 0;
        silverText.gameObject.SetActive(gotSome);
        silverImage.gameObject.SetActive(gotSome);
        silverText.text = "Drachmes : " + nb.ToString();

        coinSound.enabled = true;
        coinSound.Play();
    }

    public void UpdateGoldCoin(int nb)
    {
        if (firstGold)
        {
            firstGold = false;
            showGold.gameObject.SetActive(true);
            Time.timeScale = 0.01f;
        }

        bool gotSome = nb > 0;
        goldText.gameObject.SetActive(gotSome);
        goldImage.gameObject.SetActive(gotSome);
        goldText.text = "Stateres : " + nb.ToString();

        coinSound.enabled = true;
        coinSound.Play();
    }
}
