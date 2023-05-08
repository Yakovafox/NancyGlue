using Cinemachine;
using Dialogue;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterrogateButtonScript : MonoBehaviour
{
    // NPC being questioned
    private NPCTracker NPC;
    [SerializeField] private Button button;

    // Dialogue
    private DialogueSystem dialogue;

    //Animator
    [Header("Animators")]
    [SerializeField] private Animator _transitionAnimator;
    [SerializeField] private Animator _uiAnimator;

    // Button state tracking
    private bool clicked = false;

    private void Awake()
    {
        _transitionAnimator = GameObject.Find("Transition").GetComponent<Animator>();
        _uiAnimator = GameObject.Find("ClipBoardBack").GetComponent<Animator>();
        dialogue = FindObjectOfType<DialogueSystem>(true);
        clicked = false;
    }

    public void SetNpc(NPCTracker npc)
    {
        NPC = npc;
    }

    public void BringForInterrogation()
    {
        if (clicked) return;
        clicked = true;
        ZoneManager _zoneManager = FindObjectOfType<ZoneManager>();
        StartCoroutine(ZoneTransition(_zoneManager.CurrentCamera, _zoneManager.OfficeCam));
        AudioSource audioSource = GameObject.Find("Player").GetComponents<AudioSource>()[0];
        audioSource.clip = Resources.Load<AudioClip>("Sfx/Music/Interrogation_Music");
        audioSource.Play();
    }

    IEnumerator ZoneTransition(CameraSwitch oldCam, CameraSwitch newCam)
    {
        _transitionAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);

        AudioSource audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        audioSourceSound.clip = Resources.Load<AudioClip>("Sfx/SoundEffects/Chair/Chair_Drag-003");
        audioSourceSound.pitch = 1.2f;
        audioSourceSound.Play();

        oldCam.SwitchActiveCam();
        newCam.SwitchActiveCam();
        yield return new WaitForSeconds(1);
        GameObject.Find(NPC.attachedNPC).transform.position = GameObject.Find("SeatLocation").transform.position;
        _uiAnimator.SetBool("hide", true);
        dialogue.SetContainer(NPC.GetCurrentInterContainer(), NPC);
        _transitionAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0);
        clicked = false;
        _uiAnimator.gameObject.GetComponent<OpenCloseUI>().CloseUI();
    }
}
