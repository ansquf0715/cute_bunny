using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarControl : MonoBehaviour
{
    Player player;

    private Image content;
    public Sprite contentSprite;

    [SerializeField]
    private float lerpSpeed;

    private float maxFill;

    //private GameObject HPObj;
    private float currentFill;

    private bool isChanged;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        content = GetComponent<Image>();
        content.sprite = contentSprite;

        //HPObj = GameObject.FindGameObjectWithTag("HP_fill");
        //content = HPObj.GetComponent();

        setPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        if(isChanged == true)
        {
            SetHPUI();
            isChanged = false;
        }
    }

    public void isChange()
    {
        isChanged = true;
        Debug.Log("is CHange �ҷ���");
    }

    void setPlayer()
    {
        maxFill = player.getMaxHealth();
        currentFill = player.getHealth();
    }

    public void SetHPUI()
    {
        //currentFill = 
        //    GameObject.FindWithTag("Player").GetComponent<Player>().getHealth() 
        //    / maxFill;

        currentFill = Player.CurrentHealth / maxFill;
        //Debug.Log("SetHPUI �ҷ��� CurrentFill" + currentFill);

        //Debug.Log("Set HP UI ");

        content.fillAmount = currentFill;

        //content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill,
        //    lerpSpeed);
        //sprite.bounds.size.y = currentFill / maxFill;
    }

}
