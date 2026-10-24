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
    [SerializeField] private Sprite cardFront;

    [Header("Card Images")]
    [SerializeField] private Sprite[] cards;
    
   
    
    void Start()
    {
        int numCards = cards.Length * 2;
        int curCardImage = 0;

        for (int i = 0; i < numCards; i++)
        {
            GameObject go = Instantiate(cardPrefab);
            go.transform.SetParent(gameBoard.transform, false);

            int col = i % columns;
            int row = i / columns;

            float x = (col - (columns - 1) / 2f) * spaceX;
            int rows = Mathf.CeilToInt(numCards / columns);
            float y = ((rows - 1) / 2f - row) * spaceY;

            go.transform.localPosition = new Vector3(x, y, 0);

            Material[] mats = go.GetComponent<Renderer>().materials;
            
            //material indexes are taken from the materials list on the prefab
            Material cf = mats[3];//CardFront
            cf.mainTexture = cardFront.texture;

            Material cb = mats[3];//CardBack
            cb.mainTexture = cards[i / 2].texture;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
