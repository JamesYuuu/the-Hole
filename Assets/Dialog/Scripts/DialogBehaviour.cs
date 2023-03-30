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
    public class DialogBehaviour : MonoBehaviour, ISpeakable
    {
        [Header("DialogBehaviour Base Vars")]
        // triggers for debugging test only
        public bool stDia = false; // start dialogue
        public bool next = false; // next dialogue
        
        [SerializeField] protected float textSpeedNorm = 0.022f;
        [SerializeField] protected float textSpeedSlow = 0.06f;
        [SerializeField] protected float textSpeedFast = 0.01f;
        
        [SerializeField] protected DialogParser dialogParser;
        [SerializeField] private TextAsset convoTextFile;
        [SerializeField] private GameObject displayGroup;
        [SerializeField] private TextMeshProUGUI textDisplay;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator
        
        protected Queue<(TextSpeed speed, string speech)> _convoQueue;
        protected (TextSpeed speed, string speech) _currSpeech;
        protected bool _doneTalking; // done showing all the text in the current speech.
        protected bool _isConvoOngoing; // player is reading through the queue of speeches
        // ^ can also be speeches.Count == 0;

        protected virtual void Awake() // virtual means able to be overridden
        {
            _doneTalking = true;
        }

        // only for debugging test
        protected void Update()
        {
            if (stDia)
            {
                StartDialog();
                stDia = !stDia;
            }
            
            if (next)
            {
                FinishCurrSentence();
                next = !next;
            }
        }

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public virtual void StartDialog()
        {
            _isConvoOngoing = true;
            
            displayGroup.SetActive(true); // open dialog panel
            _convoQueue = dialogParser.ParseTextFileAsQueue(convoTextFile);
            _currSpeech = _convoQueue.Dequeue();
            StartCoroutine(TypeCurrSpeech(_currSpeech.speech, textDisplay, _currSpeech.speed));
        }
        
        /// <summary>
        /// When player wants to see the next page,
        /// if it is the end of the dialog, exit it.
        /// if the NPC has not printed out the full speech, print it out.
        /// if the NPC has printed out the full speech, move on to the next one.
        ///
        /// Triggered by Continue input action.
        /// </summary>
        public void FinishCurrSentence()
        {
            if (_convoQueue.Count == 0)
            {
                EndDialog();
                return;
            }

            if (!_doneTalking)
            {
                // show all remaining text in speech, stop typing
                textDisplay.text = _currSpeech.speech;
                StopCoroutine(nameof(TypeCurrSpeech));
                _doneTalking = true;
                
                // make the continue arrow bounce
                // nextPageIcon.SetBool("doneTalking", true);
            }
            else // start typing the next speech
            {
                StopCoroutine(nameof(TypeCurrSpeech));
                _currSpeech = _convoQueue.Dequeue();
                StartCoroutine(TypeCurrSpeech(_currSpeech.speech, textDisplay, _currSpeech.speed));
            }
        }
        
        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// </summary>
        public virtual void EndDialog()
        {
            displayGroup.SetActive(false);
            StopCoroutine(nameof(TypeCurrSpeech));
        }
        
        /// <summary>
        /// Adds the speech letter by letter to the display box.
        /// </summary>
        protected IEnumerator TypeCurrSpeech(string thisSentence, TextMeshProUGUI display, TextSpeed speed = TextSpeed.Normal)
        {
            display.text = "";
            _doneTalking = false; // flag, telling the show-remaining-speech line to show all if still talking
            // nextPageIcon.SetBool("doneTalking", false); // stop the Continue arrow from bouncing

            // set the talking speed
            var currTextSpeed = speed == TextSpeed.Normal
                ? textSpeedNorm
                : speed == TextSpeed.Slow
                ? textSpeedSlow
                : textSpeedFast;
            
            // add each letter to the display
            foreach (char letter in thisSentence)
            {
                display.text += letter;
                yield return new WaitForSeconds(currTextSpeed);
            }

            _doneTalking = true;
            // nextPageIcon.SetBool("doneTalking", true); // make the Continue arrow bounce
        }
    }
}
