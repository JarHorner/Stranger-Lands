using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LocationCanvas : MonoBehaviour
{
    #region Variables
    private static bool exists;
    [SerializeField] private TMP_Text locationText;
    [SerializeField] private Animator locationTextAnim;
    #endregion

    #region Methods
    private void Awake() 
    {
        //Singleton Effect
        if (!exists)
        {
            Debug.Log("Dialog up");
            exists = true;
            //ensures same player object is not destoyed when loading new scences
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Debug.Log("Remove copy dialog");
            Destroy (gameObject);
        }
    }

    //Used in PlayerSpawnController and AreaTransition objects to display area name.
    //adds text, set it active and starts the animations. When animations are over, the text wil be deleted and set for the next time.
    public IEnumerator PlaceNameCo(string placeName)
    {
        yield return new WaitForSeconds(0.5f);
        LocationText.text = placeName;
        locationTextAnim.SetBool("Fading", true);
        yield return new WaitForSeconds(5f);
        locationTextAnim.SetBool("Fading", false);
        LocationText.text = "";
    }

    public TMP_Text LocationText
    {
        get { return locationText; }
        set { locationText = value; }
    }

    public Animator LocationTextAnim
    {
        get { return locationTextAnim; }
    }

    #endregion

}
