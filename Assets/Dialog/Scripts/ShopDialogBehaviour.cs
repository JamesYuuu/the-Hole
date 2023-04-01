using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = System.Random;

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
        public bool next = false; // next dialogue
        public Item item;
        
        [SerializeField] private float textSpeedNorm = 0.022f;
        [SerializeField] private float textSpeedSlow = 0.06f;
        [SerializeField] private float textSpeedFast = 0.01f;
        
        [SerializeField] private DialogParser dialogParser;
        [SerializeField] [Tooltip("Has a list of greetings messages picked at random, separated by @.")]
        private TextAsset greetingTextFile; 
        [SerializeField] [Tooltip("Has a list of farewell messages picked at random, separated by @.")]
        private TextAsset farewellTextFile;
        private List<(TextSpeed speed, string speech)> _greetings;
        private List<(TextSpeed speed, string speech)> _farewells;
        
        
        [FormerlySerializedAs("UiPanelsTalking")] [SerializeField] private GameObject uiPanelsTalking;
        [FormerlySerializedAs("UiPanelsShopInfo")] [SerializeField] private GameObject uiPanelsShopInfo;
        [SerializeField] private TextMeshProUGUI itemNameDisplay;
        [SerializeField] private TextMeshProUGUI priceDisplay;
        [SerializeField] private TextMeshProUGUI descDisplay;
        [SerializeField] private TextMeshProUGUI cashDisplay;
        [SerializeField] private TextMeshProUGUI cartTotalDisplay;
        // [SerializeField] private Animator nextPageIcon; // stretch goal: bouncing continue animator

        [FormerlySerializedAs("EnableGrab")] public UnityEvent enableGrab;
        
        private (TextSpeed speed, string speech) _currSpeech;
        private float _currTextSpeed;
        private bool _doneTalking; // done showing all the text in the description
        // assumption: desc is always longer than name or price
        private Random _rand = new(); // used to pick greeting and farewell

        private void Awake()
        {
            _doneTalking = true;
            _currTextSpeed = textSpeedNorm;
            _greetings = dialogParser.ParseTextFileAsList(greetingTextFile);
            _farewells = dialogParser.ParseTextFileAsList(farewellTextFile);
            
            // testing, remove for production
            // ShowShopInterface();
        }

        // only for debugging test
        private void Update()
        {
            if (next)
            {
                FinishSpeaking();
                next = !next;
            }

            if (item)
            {
                UpdateItemTextFields(item);
            }
        }

        /// <summary>
        /// Passes the dialog to display and the text panel to the
        /// DialogManager to parse and display.
        /// Triggered by interacting with NPCs (Unity Event).
        /// </summary>
        public void StartDialog()
        {
            uiPanelsTalking.SetActive(true); // open dialog panel
            int randInt = _rand.Next(_greetings.Count);
            StartCoroutine(TypeCurrSpeech(_greetings[randInt].speech, 
                descDisplay, _greetings[randInt].speed));
            enableGrab.Invoke(); // triggers shopMgr::EnableGrab
        }

        /// <summary>
        /// Enters 'buying mode', that shows the player how much
        /// each item costs when grabbed, their cash, and the total bill.
        /// </summary>
        public void ShowShopInterface()
        {
            uiPanelsShopInfo.SetActive(true);
            UpdateCartTotal(0);
        }

        /// <summary>
        /// deprecated.
        /// Displays the new sum of the cart's cost.
        /// Called by shopmanager when item enters cart
        /// </summary>
        /// <param name="cartTotalPrice">The total value of the items in the cart.</param>
        public void UpdateCartTotal(int cartTotalPrice)
        {
            StartCoroutine(TypeCurrSpeech(PlayerData.Money + "", cashDisplay));
            StartCoroutine(TypeCurrSpeech(cartTotalPrice + "", cartTotalDisplay));
        }
        
        /// <summary>
        /// deprecated.
        /// Displays the item's attributes in the shop ui.
        /// Called when an item is picked up by player.
        /// </summary>
        /// <param name="i">The item being displayed.</param>
        public void UpdateItemTextFields(Item i)
        {
            StartCoroutine(TypeCurrSpeech(i.GetName(), itemNameDisplay));
            StartCoroutine(TypeCurrSpeech(i.GetPrice() + "", priceDisplay));
            StartCoroutine(TypeCurrSpeech(i.GetDescription(), descDisplay));
        }
        
        /// <summary>
        /// Says goodbye to the player. Triggered by Checkout Unity Event.
        ///
        /// Different from merely closing the dialog pane
        /// </summary>
        public IEnumerator EndInteraction()
        {
            int randInt = _rand.Next(_farewells.Count);
            yield return StartCoroutine(TypeCurrSpeech(_farewells[randInt].speech, 
                descDisplay, _farewells[randInt].speed));
            EndDialog();
        }
        
        /// <summary>
        /// Closes the text panel and stops talking.
        ///
        /// Triggered by Cancel input action.
        /// </summary>
        public void EndDialog()
        {
            uiPanelsTalking.SetActive(false);
            StopCoroutine(nameof(TypeCurrSpeech));
        }
        
        /// <summary>
        /// Adds the speech letter by letter to the display box.
        /// </summary>
        private IEnumerator TypeCurrSpeech(string currSpeech, TextMeshProUGUI display, TextSpeed speed = TextSpeed.Normal)
        {
            display.text = "";
            _doneTalking = false; // flag, telling the show-remaining-speech line to show all if still talking
            // nextPageIcon.SetBool("doneTalking", false); // stop the Continue arrow from bouncing

            // set the talking speed
            _currTextSpeed = speed == TextSpeed.Normal
                ? textSpeedNorm
                : speed == TextSpeed.Slow
                ? textSpeedSlow
                : textSpeedFast;
            
            // add each letter to the display
            foreach (char letter in currSpeech)
            {
                display.text += letter;
                yield return new WaitForSeconds(_currTextSpeed);
            }

            _doneTalking = true;
            // nextPageIcon.SetBool("doneTalking", true); // make the Continue arrow bounce
        }
        
        /// <summary>
        /// If the NPC has not printed out the full speech, print it out.
        ///
        /// Triggered by Continue input action.
        /// </summary>
        public void FinishSpeaking()
        {
            if (!_doneTalking)
            {
                // show all remaining text in speech, stop typing
                descDisplay.text = _currSpeech.speech;
                _doneTalking = true;
            }
            StopCoroutine(nameof(TypeCurrSpeech));
            // make the continue arrow bounce
            // nextPageIcon.SetBool("doneTalking", true);
        }
        
    }
}
