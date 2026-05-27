using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI_Manager : MonoBehaviour
{
    public CanvasGroup BoxMenu;
    public CanvasGroup Fade;
    public CanvasGroup StartIcon;
    public ButtonSpawn ButtonSpawn;
    public CanvasGroup ButtonUiMobile;
    public CanvasGroup ShowEnd;
    public CanvasGroup ShowDie;
    public CanvasGroup ShowMenu;
    public CanvasGroup Menu;
    public TextMeshProUGUI point;

    [Header("Audio Settings")]
    public AudioClip bgMusic;
    public AudioSource bgAudioSource;
    public Slider AudioSider;

    void Start()
    {
        ButtonUiMobile.gameObject.SetActive(false);
        BoxMenu.gameObject.SetActive(false);
        StartIcon.gameObject.SetActive(true);

        if (bgAudioSource == null)
        {
            bgAudioSource = gameObject.AddComponent<AudioSource>();
            bgAudioSource.loop = true;
            bgAudioSource.playOnAwake = false;
        }

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
        Fade.gameObject.SetActive(true);
        RandomInon.FadeIn(Fade);
        ShowUIonMoblie();

        if (bgMusic != null && bgAudioSource != null)
        {
            bgAudioSource.clip = bgMusic;
            if (!bgAudioSource.isPlaying)
            {
                bgAudioSource.Play();
            }
        }
    }
    public void MenuOn()
    {
        Menu.gameObject.SetActive(true);
        ShowDie.gameObject.SetActive(false);
        BoxMenu.gameObject.SetActive(false);
        StartIcon.gameObject.SetActive(true);
        ShowMenu.gameObject.SetActive(false);
        if (bgAudioSource != null && bgAudioSource.isPlaying)
        {
            bgAudioSource.Stop();
        }
    }    
    public void SettingOn()
    {
        ShowMenu.gameObject.SetActive(true);
        RandomInon.FadeOut(ShowMenu);
        bgAudioSource.volume = AudioSider.value;
        GameManager.instance.audioSource.volume = AudioSider.value;
        if (GameManager.instance != null) GameManager.instance.isSettingsOpen = true;
    }    
    public void settingOff ()
    {
        ShowMenu.gameObject.SetActive(true);
        RandomInon.FadeIn(ShowMenu);
        bgAudioSource.volume = AudioSider.value;
        GameManager.instance.audioSource.volume = AudioSider.value;
        if (GameManager.instance != null) GameManager.instance.isSettingsOpen = false;
    }    
   
    public void Restart()
    {
        RandomInon.ButtonSound(GameManager.instance.audioSource, GameManager.instance.buttonSound);
        GameManager.instance.LoadLV(GameManager.instance.LvNow);
    }
   
    public void GoToChooseLV()
    {
        RandomInon.ButtonSound(GameManager.instance.audioSource, GameManager.instance.buttonSound);
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
                if (e > GameManager.instance.LvNow)
                {
                    e1.Lock.gameObject.SetActive(true);
                }
                else
                {
                    e1.Lock.gameObject.SetActive(false);
                }
                e1.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText((e + 1).ToString());
                e1.LV = e;
            }
        }
    }
    public void OnShowDie()
    {
        if (bgAudioSource != null && bgAudioSource.isPlaying)
        {
            bgAudioSource.Stop();
        }
        StartCoroutine(OnShowDieCoroutine());
    }    

    private IEnumerator OnShowDieCoroutine()
    {
        if (GameManager.instance != null && GameManager.instance.audioSource != null && GameManager.instance.deathSound != null)
        {
            RandomInon.ButtonSound(GameManager.instance.audioSource, GameManager.instance.deathSound);
        }

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
            RectTransform rect = ButtonUiMobile.GetComponent<RectTransform>();
            if (rect != null)
            {
                Rect safeArea = Screen.safeArea;
                Vector2 anchorMin = safeArea.position;
                Vector2 anchorMax = safeArea.position + safeArea.size;

                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                rect.anchorMin = anchorMin;
                rect.anchorMax = anchorMax;
            }
        }
    }
      
    void Update()
    {
        
    }
}
