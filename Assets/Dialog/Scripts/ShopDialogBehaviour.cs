using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialog.Scripts
{
    /// <summary>
    /// Handles the NPC interaction with a player.
    /// Takes in a text file with a conversation to be displayed.
    /// Passes the file to the DialogManger to parse.
    /// </summary>
    public class ShopDialogBehaviour : MonoBehaviour, ISpeakable
    {
        // triggers for debugging test only
        public bool stDia = false; // start dialogue
        public bool next = false; // next dialogue
        
        [SerializeField] private float textSpeedNorm = 0.022f;
        [SerializeField] private float textSpeedSlow = 0.06f;
        [SerializeField] private float textSpeedFast = 0.01f;
        
        [SerializeField] private DialogParser dialogParser;
        [SerializeField] [Tooltip("Has a list of greetings messages picked at random, separated by @.")]
        private TextAsset greetingTextFile; 
        [SerializeField] [Tooltip("Has a list of farewell messages picked at random, separated by @.")]
        private TextAsset farewellTextFile;
        [SerializeField] private GameObject UiPanelsTalking;
        [SerializeField] private GameObject UiPanelsShopInfo;
        [SerializeField] private TextMeshProUGUI itemNameDisplay;
        [SerializeField] private TextMeshProUGUI priceDisplay;
        [SerializeField] private TextMeshProUGUI descDisplay;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator
        
        private Queue<(TextSpeed speed, string speech)> speeches;
        private (TextSpeed speed, string speech) currSpeech;
        private float currTextSpeed;
        private bool doneTalking; // done showing all the text in the description
        // assumption: desc is always longer than name or price

        private void Awake()
        {
            doneTalking = true;
            currTextSpeed = textSpeedNorm;
        }

        // only for debugging test
        private void Update()
        {
            if (stDia)
            {
                StartDialog();
                stDia = !stDia;
            }
            
            if (next)
            {
                ShowNextSpeech();
                next = !next;
            }
        }

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public void StartDialog()
        {
            UiPanelsTalking.SetActive(true); // open dialog panel
            speeches = dialogParser.ParseTextFile(greetingTextFile);
            StartCoroutine(nameof(TypeCurrSpeech));
        }
        
        /// <summary>
        /// When player wants to see the next page,
        /// if it is the end of the dialog, exit it.
        /// if the NPC has not printed out the full speech, print it out.
        /// if the NPC has printed out the full speech, move on to the next one.
        ///
        /// Triggered by Continue input action.
        /// </summary>
        public void ShowNextSpeech()
        {
            if (speeches.Count == 0)
            {
                EndDialog();
                return;
            }

            if (!doneTalking)
            {
                // show all remaining text in speech, stop typing, prep next speech
                descDisplay.text = currSpeech.speech;
                StopCoroutine(nameof(TypeCurrSpeech));
                doneTalking = true;
                // currSpeech = speeches.Dequeue();
                
                // make the continue arrow bounce
                // nextPageIcon.SetBool("doneTalking", true);
            }
            else // start typing the next speech
            {
                StopCoroutine(nameof(TypeCurrSpeech));
                StartCoroutine(nameof(TypeCurrSpeech));
            }
        }
        
        /// <summary>
        /// Says goodbye to the player after they checkout.
        ///
        /// Different from merely closing the dialog pane
        /// </summary>
        public void EndInteraction()
        {
            speeches = dialogParser.ParseTextFile(farewellTextFile);
            StartCoroutine(nameof(TypeCurrSpeech));
            EndDialog();
        }
        
        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// </summary>
        public void EndDialog()
        {
            UiPanelsTalking.SetActive(false);
            StopCoroutine(nameof(TypeCurrSpeech));
        }
        
        /// <summary>
        /// Adds the speech letter by letter to the display box.
        /// TODO: modify to accept string and textmeshpro to reuse for 3 fields.
        /// </summary>
        private IEnumerator TypeCurrSpeech()
        {
            descDisplay.text = "";
            currSpeech = speeches.Dequeue();
            doneTalking = false; // flag, telling the show-remaining-speech line to show all if still talking
            // nextPageIcon.SetBool("doneTalking", false); // stop the Continue arrow from bouncing

            // set the talking speed
            currTextSpeed = currSpeech.speed == TextSpeed.Normal
                ? textSpeedNorm
                : currSpeech.speed == TextSpeed.Slow
                ? textSpeedSlow
                : textSpeedFast;
            
            // add each letter to the display
            foreach (char letter in currSpeech.speech)
            {
                descDisplay.text += letter;
                yield return new WaitForSeconds(currTextSpeed);
            }

            doneTalking = true;
            // nextPageIcon.SetBool("doneTalking", true); // make the Continue arrow bounce
        }
    }
}
