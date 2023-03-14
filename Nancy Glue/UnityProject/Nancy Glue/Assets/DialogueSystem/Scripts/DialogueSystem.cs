using System.Collections;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    using ScriptableObjects;
    using Enumerators;
    using UnityEngine.Rendering;

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
        [SerializeField] private Image characterPortrait;
        [SerializeField] private TextMeshProUGUI characterNameUI;
        [SerializeField] private TextMeshProUGUI bodyTextUI;
        [SerializeField] private Grid optionGridUI;

        private DialogueSO currentDialogue;

        private void Awake()
        {
            currentDialogue = dialogue;

            ShowText();
        }

        private void ShowText()
        {
            characterNameUI.text = currentDialogue.characterName;
            bodyTextUI.text = currentDialogue.dialogueText;

            // Create buttons if dialogue is multiple choice
            if (currentDialogue.dialogueType == DialogueType.MultiChoice)
            {
                for (int i = 0; i < currentDialogue.dialogueChoices.Count; i++)
                {
                    bodyTextUI.text += $"\n{i+1}. {currentDialogue.dialogueChoices[i].text}";
                }
            }

            characterPortrait.sprite = Resources.Load<Sprite>(currentDialogue.dialogueSpriteAssetPath);

        }

        private void OnOptionChosen(int choiceIndex)
        {
            Debug.Log($"{currentDialogue.dialogueChoices}");

            DialogueSO nextDialogue = currentDialogue.dialogueChoices[choiceIndex].NextDialogue;

            if (nextDialogue == null)
            {
                //Destroy(this.gameObject);
                this.gameObject.SetActive(false);

                return;
            }

            currentDialogue = nextDialogue;

            ShowText();
        }

        private void Update()
        {
            // On click progress dialogue if the node is single choice
            if (currentDialogue.dialogueType == DialogueType.SingleChoice)
            {
                if (Input.anyKeyDown)
                {
                    OnOptionChosen(0);
                }
            }
            else
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
        }
        private void OnEnable()
        {
            currentDialogue = dialogue;

            ShowText();
        }
    }
}