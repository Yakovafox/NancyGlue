using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuspectMugshot : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public Sprite MugshotSprite { get; private set; }
    [field: SerializeField] public List<string> Notes { get; set; }
    private TextMeshProUGUI _tmpText;
    [SerializeField] private GameObject _mainPage;

    [Header("Details Page Data")]
    [SerializeField] private GameObject _detailPage;
    [SerializeField] private Transform _detailMugshot;
    [SerializeField] private List<TextMeshProUGUI> _detailNotes;
    [SerializeField] private TextMeshProUGUI _detailNote1;
    [SerializeField] private TextMeshProUGUI _detailNote2;
    [SerializeField] private TextMeshProUGUI _detailNote3;
    [SerializeField] private GameObject _interrogateButton; 
    [SerializeField] private InterrogateButtonScript _interrogateButtonScript; 

    [field: SerializeField] public NPCTracker _npcTracker { get; set; }

    public void Awake()
    {
        _tmpText = GetComponentInChildren<TextMeshProUGUI>();
        _mainPage = transform.parent.parent.gameObject;
        _detailMugshot = transform.parent.parent.parent.GetChild(0).GetChild(1);
        _detailPage = transform.parent.parent.parent.GetChild(0).gameObject;
        _interrogateButton = _detailPage.transform.GetChild(3).gameObject;
        _interrogateButtonScript = _interrogateButton.GetComponent<InterrogateButtonScript>();

        for (int i = 1; i < 4; i++) //assign the note objects
        {
            _detailNotes.Add(_detailPage.transform.GetChild(0).GetChild(i).GetComponent<TextMeshProUGUI>());
        }
    }
    public void SetData(string name, Sprite mugshot) //sets the character data for the mugshot
    {
        Name = name;
        MugshotSprite = mugshot;
        _tmpText.text = Name;
        transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = MugshotSprite;
    }
    public void Clicked()
    {
        //play sound effect
        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Paper/Paper_Shuffle_001");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        //disable the main page and enable the details page, set the details page mugshot to the suspects mugshot.
        _mainPage.SetActive(!_mainPage.activeSelf);
        _detailPage.SetActive(!_detailPage.activeSelf);
        _detailMugshot.GetChild(0).GetChild(0).GetComponent<Image>().sprite = MugshotSprite;
        _detailMugshot.GetChild(1).GetComponent<TextMeshProUGUI>().text = Name;

        // Enable/Disable Interrogation
        _npcTracker.EvidenceCheck();
        if (_npcTracker.canBeQuestioned)
        {
            _interrogateButton.SetActive(true);
            _interrogateButtonScript.SetNpc(_npcTracker);
        }
        else
        {
            _interrogateButton.SetActive(false);
        }

        if (_npcTracker.Notes.Count == 0) //set the notes to blank if the character has no notes
        {
            foreach (var details in _detailNotes)
                details.text = "";
        }
        else //otherwise store each note on the page.
        {
            for (int i = 0; i < _npcTracker.Notes.Count; i++)
            {
                _detailNotes[i].text = _npcTracker.Notes[i];
                if (_detailNotes.Count > i)
                {
                    for (int j = i; j < _detailNotes.Count - 1; j++)
                        _detailNotes[j + 1].text = "";
                }
            }
        }
    }
}
