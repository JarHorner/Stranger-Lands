using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthVisual : MonoBehaviour
{

    #region Variables
    public static HealthSystem healthSystemStatic;
    private float periodicTime = 0.05f;
    private float fullHealthAnimTime = 0.2f;
    private PlayerController player;
    private bool isHealing = false;
    [SerializeField] private Sprite heart0Sprite;
    [SerializeField] private Sprite heart1Sprite;
    [SerializeField] private Sprite heart2Sprite;
    [SerializeField] private Sprite heart3Sprite;
    [SerializeField] private Sprite heart4Sprite;
    [SerializeField] private AnimationClip heartFullClip;
    private List<HeartImage> heartImageList;
    private HeartImage currentHeart;
    private HealthSystem healthSystem;
    #endregion

    #region Methods

    void Awake() 
    {
        heartImageList = new List<HeartImage>();
    }

    void Start() 
    {
        if (healthSystemStatic == null && !SaveSystem.LoadedGame)
        {
            Debug.Log("Start health");
            player = FindObjectOfType<PlayerController>();
            healthSystem = new HealthSystem(player.TotalHearts);
            SetHealthSystem(healthSystem);
            currentHeart = heartImageList[heartImageList.Count-1];
        }
        else if (healthSystemStatic == null && SaveSystem.LoadedGame)
        {
            Debug.Log("Load health");
            player = FindObjectOfType<PlayerController>();
            healthSystem = new HealthSystem(SaveSystem.CurrentPlayerData.totalHearts);
            SetHealthSystem(healthSystem);
            int healthGone = (SaveSystem.CurrentPlayerData.totalHearts * 4) - SaveSystem.CurrentPlayerData.currentHealth;
            healthSystem.Damage(healthGone);
        }
    }

    void Update() 
    {
        periodicTime -= Time.deltaTime;
        if (periodicTime < 0f)
        {
            HealingAnimatedPeriodic();
            periodicTime = 0.05f;
        }
        fullHealthAnimTime -= Time.deltaTime;
        if (fullHealthAnimTime < 0f)
        {
            currentHeart.PlayHeartFullAnimation();
            fullHealthAnimTime = 0.2f;
        }
    }

    public void SetHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;
        healthSystemStatic = healthSystem;

        List<HealthSystem.Heart> heartList = healthSystem.HeartList;
        int row = 0;
        int col = 0;
        int colMax = 8;
        float rowColSize = 75f;
        for (int i=0; i< heartList.Count; i++)
        {
            HealthSystem.Heart heart = heartList[i];
            Vector2 heartAnchoredPos = new Vector2(col * rowColSize, -row * rowColSize);
            CreateHeartImage(heartAnchoredPos).SetHeartFragments(heart.Fragments);
            
            col++;
            if (col >= colMax)
            {
                row++;
                col = 0;  
            }
            
        }
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        //hearts health system was damaged
        RefreshAllHearts();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        //hearts health system was healed
        isHealing = true;
    }
    private void HealthSystem_OnDead(object sender, System.EventArgs e)
    {
        //hearts health system is dead
        Debug.Log("YOUR DEAD!");
        player.currentState = PlayerState.dead;
    }

    private void RefreshAllHearts()
    {
        List<HealthSystem.Heart> heartList = healthSystem.HeartList;
        for (int i=0; i< heartImageList.Count; i++)
        {
            HeartImage heartImage = heartImageList[i];
            HealthSystem.Heart heart = heartList[i];
            heartImage.SetHeartFragments(heart.Fragments);
            if (heartImage.Fragments != 0)
                currentHeart = heartImage;
        }
    }

    private void HealingAnimatedPeriodic()
    {
        if (isHealing)
        {
            bool fullyHealed = true;
            List<HealthSystem.Heart> heartList = healthSystem.HeartList;
            for (int i=0; i< heartList.Count; i++)
            {
                HeartImage heartImage = heartImageList[i];
                HealthSystem.Heart heart = heartList[i];
                if (heartImage.Fragments != heart.Fragments)
                {
                    //visual is different from logic
                    heartImage.AddHeartVisualFragment();
                    if (heartImage.Fragments == HealthSystem.MAX_FRAGMENT_AMOUNT)
                    {
                        //this heart was fully healed
                        heartImage.PlayHeartFullAnimation();
                    }
                    fullyHealed = false;
                    break;
                }
                if (heartImage.Fragments != 0)
                    currentHeart = heartImage;
            }
            if (fullyHealed)
            {
                isHealing = false;
            }
        }
    }
    private HeartImage CreateHeartImage(Vector2 anchoredPos)
    {
        //create game object
        GameObject heartGameObject = new GameObject("Heart", typeof(Image), typeof(Animation));
        //set as child of this transform
        heartGameObject.transform.SetParent(transform);
        heartGameObject.transform.localPosition = Vector3.zero;

        //locate and size heart
        heartGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        heartGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(65, 60);

        heartGameObject.GetComponent<Animation>().AddClip(heartFullClip, "Heart_Full");

        //set heart sprite
        Image heartImageUI = heartGameObject.GetComponent<Image>();
        heartImageUI.sprite = heart0Sprite;
        
        HeartImage heartImage = new HeartImage(this, heartImageUI, heartGameObject.GetComponent<Animation>());
        heartImageList.Add(heartImage);

        return heartImage;
    }

    //represents a single heart
    public class HeartImage
    {
        private int fragments;
        private Image heartImage;
        private HealthVisual healthVisual;
        private Animation animation;

        public HeartImage(HealthVisual healthVisual, Image heartImage, Animation animation)
        {
            this.healthVisual = healthVisual;
            this.heartImage = heartImage;
            this.animation = animation;
        }

        public void SetHeartFragments(int fragments)
        {
            this.fragments = fragments;
            switch (fragments)
            {
                case 0: heartImage.sprite = healthVisual.heart4Sprite; break;
                case 1: heartImage.sprite = healthVisual.heart3Sprite; break;
                case 2: heartImage.sprite = healthVisual.heart2Sprite; break;
                case 3: heartImage.sprite = healthVisual.heart1Sprite; break;
                case 4: heartImage.sprite = healthVisual.heart0Sprite; break;
            }
        }

        public void AddHeartVisualFragment()
        {
            SetHeartFragments(fragments + 1);
        }

        public void PlayHeartFullAnimation()
        {
            animation.Play("Heart_Full", PlayMode.StopAll);
        }

        public int Fragments
        {
            get { return fragments; }
        }
        
    }

    #endregion
}
