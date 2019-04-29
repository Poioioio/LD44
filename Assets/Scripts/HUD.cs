using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Text copperText;
    public Image copperImage;
    public Text silverText;
    public Image silverImage;
    public Text goldText;
    public Image goldImage;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCopperCoin(int nb)
    {
        bool gotSome = nb > 0;
        copperText.gameObject.SetActive(gotSome);
        copperImage.gameObject.SetActive(gotSome);
        copperText.text = "Tibet coins : " + nb.ToString();
    }

    public void UpdateSilverCoin(int nb)
    {
        bool gotSome = nb > 0;
        silverText.gameObject.SetActive(gotSome);
        silverImage.gameObject.SetActive(gotSome);
        silverText.text = "Drachmes : " + nb.ToString();
    }

    public void UpdateGoldCoin(int nb)
    {
        bool gotSome = nb > 0;
        goldText.gameObject.SetActive(gotSome);
        goldImage.gameObject.SetActive(gotSome);
        goldText.text = "Stateres : " + nb.ToString();
    }
}
