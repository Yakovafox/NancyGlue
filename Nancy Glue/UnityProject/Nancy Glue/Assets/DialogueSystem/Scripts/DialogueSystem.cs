using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Dialogue
{
    using ScriptableObjects;
    using Enumerators;
    using UnityEngine.Rendering;
    using System;

    public class DialogueSystem : MonoBehaviour
    {
        // Scriptable Objects
        [SerializeField] private DialogueContainerSO dialogueContainer;
        public DialogueContainerSO DialogueContainer { set => dialogueContainer = value; }
        [SerializeField] private DialogueGroupSO dialogueGroup;
        [SerializeField] private DialogueSO dialogue;

        // Filters
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialogueOnly;

        // Indexes
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;
        [SerializeField] private DialogueSO startinagDialogue;

        // Interactables
        [SerializeField] private Image nancyPortrait;
        [SerializeField] private Image characterPortrait;
        [SerializeField] private TextMeshProUGUI characterNameUI;
        [SerializeField] private TextMeshProUGUI bodyTextUI;

        // Audio
        private AudioSource audioSourceMusic;
        private AudioSource audioSourceSound;

        private DialogueSO currentDialogue;
        private bool scrollingText = false;
        private string displayedText = "";
        private float scrollSpeed = 0.025f;

        private NPCTracker tracker;
        public bool inDialogue = false;

        [SerializeField] private Button[] buttons;

        private Color fadeColor = new Color(110f / 255f, 110f / 255f, 110f / 255f);
        private Color focusColor = new Color(255f / 255f, 255f / 255f, 255f / 255f);

        private void Awake()
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                // Set up buttons
                // Would have used OnOptionChosen(i) but since i isn't a static value it caused issues
                if (i == 0)
                {
                    buttons[i].onClick.AddListener(delegate { OnOptionChosen(0); });
                }
                else if (i == 1)
                {
                    buttons[i].onClick.AddListener(delegate { OnOptionChosen(1); });
                }
                else if (i == 2)
                {
                    buttons[i].onClick.AddListener(delegate { OnOptionChosen(2); });
                }
            }

            // Find appropriate audio sources
            audioSourceMusic = GameObject.Find("Player").GetComponents<AudioSource>()[0];
            audioSourceSound = GameObject.Find("Player").GetComponents<AudioSource>()[1];
        }

        private void ShowText()
        {
            // If we're on a note or location node
            if (currentDialogue.dialogueType != DialogueType.SingleChoice && currentDialogue.dialogueType != DialogueType.MultiChoice) return;

            // Start playing the set audio file
            audioSourceSound.clip = Resources.Load<AudioClip>(currentDialogue.dialogueAudioAssetPath);
            audioSourceSound.Play();
            audioSourceSound.loop = true;

            // Update character name and sprite
            characterNameUI.text = currentDialogue.characterName;
            characterPortrait.sprite = Resources.Load<Sprite>(currentDialogue.dialogueSpriteAssetPath);

            // Based on who's talking grey out whoever isn't talking
            // Also change Nancy's sprite when she's talking
            if (currentDialogue.characterName == "Nancy Glue")
            {
                characterPortrait.color = fadeColor;
                nancyPortrait.color = focusColor;
                nancyPortrait.sprite = Resources.Load<Sprite>("CharacterSprites/Nancy_sprite_1_colour 1");
            }
            else
            {
                nancyPortrait.color = fadeColor;
                characterPortrait.color = focusColor;
                nancyPortrait.sprite = Resources.Load<Sprite>("CharacterSprites/Nancy_sprite_2_colour 1");
            }

            // Enable typewrite text to allow text to slowly appear rather than all at once
            if (isActiveAndEnabled)
            {
                scrollingText = true;
                StartCoroutine(TypewriterText(currentDialogue.dialogueText));
            }
            else
            {
                bodyTextUI.text = currentDialogue.dialogueText;
            }

            // Create buttons if dialogue is multiple choice
            if (currentDialogue.dialogueType == DialogueType.MultiChoice)
            {
                for (int i = 0; i < currentDialogue.dialogueChoices.Count; i++)
                {
                    try
                    {
                        buttons[i].enabled = true;
                        buttons[i].image.enabled = true;
                        buttons[i].GetComponentInChildren<TMP_Text>().enabled = true;
                        buttons[i].GetComponentInChildren<TMP_Text>().text = $"{i + 1}. " + currentDialogue.dialogueChoices[i].text;
                    }
                    catch
                    {
                        bodyTextUI.text += $"\n{i + 1}. {currentDialogue.dialogueChoices[i].text}";
                    }
                }
            }

        }

        // Coroutine to typewrite text
        IEnumerator TypewriterText(string text)
        {
            // Loop trhough each character in the text
            foreach (char letter in text.ToCharArray())
            {
                // Skip to the end if scrolling is skipped
                if (!scrollingText) break;

                // Adjust aduio pitch of the audioSourceSound effect to add rudimentary 'speech'
                float pitchAdjust = UnityEngine.Random.Range(-5, 20) / 10;
                audioSourceSound.pitch = 1.5f + pitchAdjust;

                displayedText += letter;
                bodyTextUI.text = displayedText;

                yield return new WaitForSeconds(scrollSpeed);
            }

            bodyTextUI.text = text;
            displayedText = "";
            scrollingText = false;
            audioSourceSound.loop = false;
            StopCoroutine(TypewriterText(text));
        }

        private void OnOptionChosen(int choiceIndex)
        {
            // If DisallowMultipleComponent choice set buttons to false before continuing
            if (currentDialogue.dialogueType == DialogueType.MultiChoice)
            {
                foreach (Button button in buttons)
                {
                    button.enabled = false;
                    button.image.enabled = false;
                    button.GetComponentInChildren<TMP_Text>().enabled = false;
                }
            }

            DialogueSO nextDialogue = currentDialogue.dialogueChoices[choiceIndex].NextDialogue;

            // If there's no continuing dialogue then exit dialogue
            if (nextDialogue == null)
            {
                if (tracker != null)
                {
                    GameObject.Find(tracker.attachedNPC).transform.position = tracker.originalPosition;
                }

                gameObject.SetActive(false);
                inDialogue = false;

                // Reset the soundtrack
                AudioClip normalTrack = Resources.Load<AudioClip>("Sfx/Music/Atmosphere_001");
                if (audioSourceMusic.clip != normalTrack)
                {
                    audioSourceMusic.clip = normalTrack;
                    audioSourceMusic.Play();
                }
                audioSourceSound.loop = false;

                return;
            }

            currentDialogue = nextDialogue;

            ShowText();
        }

        private void Update()
        {
            // Allow scroll skipping
            if (scrollingText && Input.anyKeyDown)
            {
                scrollingText = false;
            }
            else
            {
                // On any key press progress dialogue if the node is single choice
                if (currentDialogue.dialogueType == DialogueType.SingleChoice)
                {
                    if (Input.anyKeyDown)
                    {
                        OnOptionChosen(0);
                    }
                }

                // Register choice through numeric input
                else if (currentDialogue.dialogueType == DialogueType.MultiChoice)
                {
                    if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        OnOptionChosen(0);
                    }
                    else if ((Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2)) && currentDialogue.dialogueChoices.Count >= 2)
                    {
                        OnOptionChosen(1);
                    }
                    else if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3)) && currentDialogue.dialogueChoices.Count >= 3)
                    {
                        OnOptionChosen(2);
                    }
                    else if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha4)) && currentDialogue.dialogueChoices.Count >= 4)
                    {
                        OnOptionChosen(3);
                    }
                    else if ((Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha1)) && currentDialogue.dialogueChoices.Count >= 5)
                    {
                        OnOptionChosen(4);
                    }
                }

                // Allow progression through any key press and add dialogue to register
                else if (currentDialogue.dialogueType == DialogueType.Evidence)
                {
                    if (tracker != null)
                    {
                        tracker.AddNote(currentDialogue.dialogueText);
                    }

                    OnOptionChosen(0);
                }

                else if (currentDialogue.dialogueType == DialogueType.Location)
                {
                    Debug.Log(currentDialogue.dialogueText);
                    OnOptionChosen(0);
                }
            }
        }

        // Find the starting node in the dialogue container
        private void findStartingNode()
        {
            // Check each ungrouped node to see if it is the starting node
            foreach (DialogueSO node in dialogueContainer.ungroupedDialogues)
            {
                if (node.isStartingDialogue)
                {
                    currentDialogue = node;
                    return;
                }
            }

            // Check each grouped node to see if it is the starting node
            foreach (List<DialogueSO> group in dialogueContainer.dialogueGroups.Values)
            {
                foreach (DialogueSO node in group)
                {
                    if (node.isStartingDialogue)
                    {
                        currentDialogue = node;
                        return;
                    }
                }
            }
        }

        // Setter for the dialogue continer
        // Used when dialogue is called to choose a graph
        public void SetContainer(string containerName, NPCTracker attachedNPC = null)
        {
            dialogueContainer = Resources.Load<DialogueContainerSO>($"Dialogues/{containerName}/{containerName}");
            tracker = attachedNPC;
            inDialogue = true;

            findStartingNode();
            EnableGameObj();
            ShowText();
        }

        // Overloaded setter
        public void SetContainer(DialogueContainerSO dialogue, NPCTracker attachedNPC = null)
        {
            dialogueContainer = dialogue;
            tracker = attachedNPC;
            inDialogue = true;

            findStartingNode();
            EnableGameObj();
            ShowText();
        }

        private void EnableGameObj()
        {
            foreach (Button button in buttons)
            {
                button.enabled = false;
                button.image.enabled = false;
                button.GetComponentInChildren<TMP_Text>().enabled = false;
            }

            transform.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            var shutter = FindObjectOfType<ShutterScript>();
            shutter.TriggerShutter();
        }

        private void OnDisable()
        {
            dialogueContainer = null;
            currentDialogue = null;
            var shutter = FindObjectOfType<ShutterScript>();
            shutter.TriggerShutter();
        }
    }
}