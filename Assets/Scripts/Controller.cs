using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    [Header("Board Setup")]
    [SerializeField] private int columns = 4;
    [SerializeField] private float spaceX = .06f;
    [SerializeField] private float spaceY = .06f;

    [Header("Card Setup")]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private GameObject gameBoard;
    [SerializeField] private Sprite cardBack;

    [Header("CardFaces")]
    [SerializeField] private Sprite[] cardFaces;//one face image for each pair

    [SerializeField] private Timer gameTimer;
    [SerializeField] private SceneController sceneController;

    //Associates each card to its cardFace image - eg index 0,1 use cardFaces[0], index 2,3 used cardFaces[1]
    private int[] cardFaceIndexes;

    private GameObject[] theCards;//keeps references to the card game objects for animation

    private int firstSelection; //index of the first selected card in the turn
    private int matchesMade;
    
    void Start()
    {
        firstSelection = -1;//nothing yet selected
        matchesMade = 0;

        //Number of cards dealt onto the board - twice the number of cards as card back images
        int numCards = cardFaces.Length * 2;

        theCards = new GameObject[numCards];

        //Contains indexes into cardFaces so that each pair of cards (0,1), (1,2), (2,3), etc.
        //points to the same image index in cardBacks eg (0,1) both point to cardBacks[0], (1,2) both point to cardBacks[1]
        //so before shuffling the array looks like [0,0,1,1,2,2,3,3,4,4,5,5] etc... depending on how many cards there are
        cardFaceIndexes = new int[numCards];
        for (int i = 0; i < numCards; i++)
        {
            cardFaceIndexes[i] = i / 2;//succesive pairs get the same index
        }
        Shuffle(cardFaceIndexes);


        for (int i = 0; i < numCards; i++)
        {
            GameObject go = Instantiate(cardPrefab);
            go.transform.SetParent(gameBoard.transform, false);

            //calc cards position on the game board
            int col = i % columns;
            int row = i / columns;

            float x = (col - (columns - 1) / 2f) * spaceX;
            int rows = Mathf.CeilToInt(numCards / columns);
            float y = ((rows - 1) / 2f - row) * spaceY;

            go.transform.localPosition = new Vector3(x, y, -.027f);//a little above the board

            //Set the materials for the card
            Material[] mats = go.GetComponent<Renderer>().materials;
            
            //material indexes are taken from the materials list on the prefab
            Material cf = mats[3];//CardBack
            cf.mainTexture = cardBack.texture;

            Material cb = mats[1];//CardBack           
            cb.mainTexture = cardFaces[cardFaceIndexes[i]].texture;

            Card card = go.GetComponent<Card>();
            card.Initialize(i);//inject index in the gameboard
            card.onCardClicked += OnCardClicked;

            theCards[i] = go;//store card instances in order to animate them

            gameTimer.StartTimer();
        }
    }

    public void goScene()
    {
        sceneController.LoadNewScene("test");
    }

    //index - the card's index in the gameboard
    void OnCardClicked(int index)
    {
        LeanTween.rotate(theCards[index], new Vector3(0, 180, 0), .5f);

        if(firstSelection == -1)
        {
            //first card in the turn was picked
            firstSelection = index;
        }
        else
        {
            if (index == firstSelection) return;//clicked the same card again

            //second card in the turn was picked
            int firstIndex = cardFaceIndexes[firstSelection];
            int secondIndex = cardFaceIndexes[index];

            if(firstIndex == secondIndex)
            {      
                //Match - remove the two cards from play
                theCards[firstSelection].GetComponent<Card>().onCardClicked -= OnCardClicked;
                theCards[index].GetComponent<Card>().onCardClicked -= OnCardClicked;

                LeanTween.scale(theCards[firstSelection], Vector3.one * 1.25f, 0.15f).setDelay(.5f).setLoopPingPong(1);
                LeanTween.scale(theCards[index], Vector3.one * 1.25f, 0.15f).setDelay(.5f).setLoopPingPong(1);

                matchesMade++;

                if(matchesMade == cardFaces.Length)
                {
                    //game over
                    gameTimer.StopTimer();
                }
            }
            else
            {                
                //rotate cards back - set a delay to wait for the second card flipped to stop rotating
                LeanTween.rotate(theCards[firstSelection], new Vector3(0, 0, 0), .5f).setDelay(1f);
                LeanTween.rotate(theCards[index], new Vector3(0, 0, 0), .5f).setDelay(1f);
            }

            firstSelection = -1;
        }
    }


    //Simple Fisher-Yates shuffle
    public static void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}
