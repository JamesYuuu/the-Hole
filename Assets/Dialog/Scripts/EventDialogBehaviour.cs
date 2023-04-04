using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Dialog.Scripts
{
    /// <summary>
    /// Enables NPC interaction that could trigger an event in every line in the dialog
    /// Takes in a text file with a conversation to be displayed.
    /// Passes the file to the DialogParser to parse.
    /// </summary>
    public class EventDialogBehaviour : DialogBehaviour
    {
        [Header("EventDialogBehaviour Child Vars")]
        [SerializeField] [Tooltip("A text file with the ::eventTagKey inside to trigger events")]
        private TextAsset eventTextFile; 
        [SerializeField] [Tooltip("Text keys that trigger events. Ensure that the index of the corresponding event is the same.")]
        private List<string> eventKeys;
        [SerializeField] [Tooltip("Events triggered by the text. Ensure that the index of the corresponding event is the same.")]
        private List<UnityEvent> eventTriggers;
        
        [SerializeField] private GameObject uiPanels;
        // assign the text Mesh Pro object in the base class.
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator
        
        private Queue<(TextSpeed, string, int)> convoQueueEvents;
        private (TextSpeed speed, string speech, int eventIdx) currSpeechEvents;
        // private IEnumerator currCoroutine; // use this instead of nameOf for TypeCurrSpeech
        
        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public override void StartDialog()
        {
            _isConvoOngoing = true;
            convoQueueEvents = dialogParser.ParseEventTextFileAsQueue(eventTextFile, eventKeys);
            uiPanels.SetActive(true);
            
            currSpeechEvents = convoQueueEvents.Dequeue();
            StartCoroutine(TypeCurrSpeech(currSpeechEvents.speech, 
                currSpeechEvents.speed, currSpeechEvents.eventIdx));
        }

        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// Triggered by the final convo in the queue
        /// </summary>
        public override void EndDialog()
        {
            StopCoroutine(nameof(TypeCurrSpeech));
            uiPanels.SetActive(false);
        }
        
        /// <summary>
        /// Adds the speech letter by letter to the display box.
        /// </summary>
        protected IEnumerator TypeCurrSpeech
            (string currSpeech, TextSpeed speed, int eventIdx)
        {
            base.textDisplay.text = "";
            _doneTalking = false;
            // nextPageIcon.SetBool("doneTalking", false); // stop the Continue arrow from bouncing

            // set the talking speed
            var currTextSpeed = speed == TextSpeed.Normal
                ? textSpeedNorm
                : speed == TextSpeed.Slow
                ? textSpeedSlow
                : textSpeedFast;
            
            // add each letter to the display
            foreach (char letter in currSpeech)
            {
                base.textDisplay.text += letter;
                yield return new WaitForSeconds(currTextSpeed);
            }

            // after this line completes typing
            _doneTalking = true;
            if (eventIdx != -1) eventTriggers[eventIdx].Invoke();
            // nextPageIcon.SetBool("doneTalking", true); // make the Continue arrow bounce
        }
        
        /// <summary>
        /// AKA: Next Sentence / Continue.
        /// When player wants to see the next page,
        /// if it is the end of the dialog, exit it.
        /// if the NPC has not printed out the full speech, print it out.
        /// if the NPC has printed out the full speech, move on to the next one.
        ///
        /// Triggered by Continue input action.
        /// </summary>
        public override void FinishCurrSentence()
        {
            if (!_letPlayerControlDialog) return;
            
            if (convoQueueEvents.Count == 0)
            {
                EndDialog();
                return;
            }

            if (!_doneTalking) // show all remaining text in speech, stop typing
            {
                StopCoroutine(nameof(TypeCurrSpeech));
                base.textDisplay.text = currSpeechEvents.speech;
                if (currSpeechEvents.eventIdx != -1) eventTriggers[currSpeechEvents.eventIdx].Invoke();
                _doneTalking = true;
                
                // make the continue arrow bounce
                // nextPageIcon.SetBool("doneTalking", true);
            }
            else // start typing the next speech
            {
                StopCoroutine(nameof(TypeCurrSpeech));
                currSpeechEvents = convoQueueEvents.Dequeue();
                StartCoroutine(TypeCurrSpeech(currSpeechEvents.speech,
                    currSpeechEvents.speed, currSpeechEvents.eventIdx));
            }
        }
    }
}
