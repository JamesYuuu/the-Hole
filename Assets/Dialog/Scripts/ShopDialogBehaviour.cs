using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

namespace Dialog.Scripts
{
    /// <summary>
    /// Handles the NPC interaction with a player.
    /// Takes in a text file with a conversation to be displayed.
    /// Passes the file to the DialogManger to parse.
    /// </summary>
    public class ShopDialogBehaviour : DialogBehaviour
    {
        [Header("ShopDialogBehaviour Child Vars")]
        [SerializeField] [Tooltip("Has a list of greetings messages picked at random.")]
        private List<TextAsset> greetingTextFiles; 
        [SerializeField] [Tooltip("Has a list of farewell messages picked at random.")]
        private List<TextAsset> farewellTextFiles;
        
        [SerializeField] private GameObject uiPanelsTalking;
        // assign the text Mesh Pro object in the base class.
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator

        public UnityEvent OnConvoEnd; // triggers shopMgr::EnableGrab and enables item panel
        
        // used to pick greeting and farewell
        private Random _rand = new();
        private int[] _last3Greetings = new int[3];
        private int[] _last3Farewells = new int[3];

        protected override void Start()
        {
            _inputManager = InputManager.GetInstance();

            // testing, uncomment for production
            // ShowShopInterface();
        }
        
        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public override void StartDialog()
        {
            _isConvoOngoing = true;
            
            // decide which conversation to show
            int randIntGreet = _rand.Next(greetingTextFiles.Count);
            // don't show the player the past 3 conversations they heard before
            while (ConversationIsStale(randIntGreet, _last3Greetings))
            {
                randIntGreet = _rand.Next(greetingTextFiles.Count);
            }

            _convoQueue = dialogParser.ParseTextFileAsQueue(greetingTextFiles[randIntGreet]);
            
            uiPanelsTalking.SetActive(true); // open dialog panel
            _currSpeech = _convoQueue.Dequeue();
            StartCoroutine(TypeCurrSpeech(_currSpeech.speech, _currSpeech.speed));
            
        }

        private bool ConversationIsStale(int curr, int[] prevSelections)
        {
            return prevSelections.Contains(curr);
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
            OnConvoEnd.Invoke();
        }

        /// <summary>
        /// Trigger for calling by UnityEvent
        /// </summary>
        public void TriggerEndInteraction()
        {
            StartCoroutine(EndInteraction());
        }
        
        /// <summary>
        /// Says goodbye to the player. Triggered by Checkout Unity Event.
        ///
        /// Different from merely closing the dialog pane
        /// </summary>
        public IEnumerator EndInteraction()
        {
            int randIntFarewell = _rand.Next(greetingTextFiles.Count);
            while (ConversationIsStale(randIntFarewell, _last3Farewells))
            {
                randIntFarewell = _rand.Next(greetingTextFiles.Count);
            }

            _convoQueue = dialogParser.ParseTextFileAsQueue(farewellTextFiles[randIntFarewell]);
            _currSpeech = _convoQueue.Dequeue();
            yield return StartCoroutine(TypeCurrSpeech(_currSpeech.speech, _currSpeech.speed));
            EndDialog();
        }
    }
}
