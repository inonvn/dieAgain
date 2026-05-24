using System.Collections;
using TMPro;
using UnityEngine;
public class UI_Manager : MonoBehaviour
{
    public CanvasGroup BoxMenu;
    public CanvasGroup Fade;
    public CanvasGroup StartIcon;
    public ButtonSpawn ButtonSpawn;
    public CanvasGroup ButtonUiMobile;
    public CanvasGroup ShowEnd;
    public CanvasGroup ShowDie;
    public CanvasGroup Menu;
    public TextMeshProUGUI point;

    void Start()
    {
        ButtonUiMobile.gameObject.SetActive(false);
        BoxMenu.gameObject.SetActive(false);
        StartIcon.gameObject.SetActive(true);

        GameManager.instance.OnLevelLoaded += HandleLevelLoaded;
        GameManager.instance.OnDieLoaded += OnShowDie;
    }

    private void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnLevelLoaded -= HandleLevelLoaded;
            GameManager.instance.OnDieLoaded -= OnShowDie;
        }
    }

    private void HandleLevelLoaded(int level)
    {
        Menu.gameObject.SetActive(false);
        ShowDie.gameObject.SetActive(false);
        BoxMenu.gameObject.SetActive(false); 
        ShowUIonMoblie();
    }
    public void MenuOn()
    {
        Menu.gameObject.SetActive(true);
        ShowDie.gameObject.SetActive(false);
        BoxMenu.gameObject.SetActive(false);
        StartIcon.gameObject.SetActive(true);
    }    

   
    public void Restart()
    {
        GameManager.instance.LoadLV(GameManager.instance.LvNow);
    }
   
    public void GoToChooseLV()
    {
        BoxMenu.gameObject.SetActive(true);
        RandomInon.FadeOut(BoxMenu);
        StartIcon.gameObject.SetActive(false);
        var f = BoxMenu.transform.GetChild(0);
       foreach (Transform e in f.transform)
        {
            Destroy(e.gameObject);
        }    
        for (int e = 0; e < GameManager.instance.SaveLV.Count; e++)
        {
            {
                var e1 = Instantiate(ButtonSpawn, f.transform);
                e1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText((e + 1).ToString());
                e1.LV = e;
            }
        }
    }
    public void OnShowDie()
    {
        StartCoroutine(OnShowDieCoroutine());
    }    

    private IEnumerator OnShowDieCoroutine()
    {
        float waitTime = 1f; 

    
        if (GameManager.instance.currentPlayerObj != null)
        {
            Animator playerAnim = GameManager.instance.currentPlayerObj.GetComponentInChildren<Animator>();
            if (playerAnim != null)
            {
                
                yield return null;
                yield return null; 
                
               
                if (playerAnim.IsInTransition(0))
                {
                    waitTime = playerAnim.GetNextAnimatorStateInfo(0).length;
                }
                else
                {
                    waitTime = playerAnim.GetCurrentAnimatorStateInfo(0).length;
                }
            }
        }

       
        yield return new WaitForSeconds(waitTime);

        RandomInon.FadeOut(ShowDie);
        Menu.gameObject.SetActive(false);
        point.SetText("IQ :-" + GameManager.instance.deathCount);
    }    
    public void ShowUIonMoblie ()
    {
        if (GameManager.instance.CheckType == CheckTypeDriver.moblie)
        {
            ButtonUiMobile.gameObject.SetActive(true);
        }
    }
      
    void Update()
    {
        
    }
}
