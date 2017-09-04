using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This script lets you render text\font in a form of Sprite to the World space
/// based on the input string. It is particularly useful when you need to display
/// text in the world space that changes during the runtime.
/// 
/// Each letter must have a representation array of sprites. One array represents
/// an ASCII group (Alphabetic, Numeric, Special Character...), where first element
/// is a an ASCII code of the letter group. E.g. Upper case Alphabet in ASCII are
/// values between 65 to 90 both inclusive. Thus, element 0 is (65-65 = 0 = A);
/// element 1 in the AlphabetU array is 66-65 = 1 = B and so on.
/// 
/// Get a Font sprite with all the letters and numbers; Using Sprite Editor with a
/// "Multiple" Sprite Mode, slice each character. NOTE: make sure they all sliced with
/// the same Width and Height to get consistant spacing. Then, add each sliced sprite
/// to the sprite array groups (Numeric, AlphabetU, SpecialChars...). Refer to the
/// ASCII table for the Dec codes (http://www.asciitable.com/).
/// NOTE: int decCode = 'A';  // this will return 65.
/// </summary>
public class TextToWorld: MonoBehaviour {

    protected Vector2 AlphabetUCaseRange = new Vector2(65, 90);
    protected Vector2 SpecialCharRange   = new Vector2(32, 47);
    protected Vector2 NumericCharRange   = new Vector2(48, 57);

    public GameObject NumericPool;
    public GameObject AlphabetPool;
    public GameObject SpecialCharPool;
    [Tooltip("Space between letters.")]
    public float    LetterSpacing = 0.1f;
    [Tooltip("SpriteRenderer 'Sorting layer'.")]
    public string   SortingLayerName;
    [Tooltip("SpriteRenderer 'Order in layer'.")]
    public int      OrderInLayer;
    [Tooltip("'Pixel Per Unit' value used on the sprite itself.")]
    public int      PixelPerUnit = 100;
    [Tooltip("Text to be rendered in the world space using Sprite arrays.")]
    public string   TextToRender = "Hello World";
    [Tooltip("Scale for each letter.")]
    public Vector3  Scale = Vector3.one;
    public List<SpriteRenderer> renderedSprites;

    [Tooltip("Element in the array represents the a number sprite.")]
    public List<Sprite> Numeric;
    [Tooltip("Upper Case alphabet. Hex range from 65 to 90. where 65 is element 0 of the array.")]
    protected List<Sprite> AlphabetU;
    [Tooltip("Special Characters sprites. Hex range from 32 to 47. where 32 is element 0 of the array.")]
    protected List<Sprite> SpecialChars;


    public void Start() {
        renderedSprites = new List<SpriteRenderer>();
        Numeric = AlphabetU = SpecialChars = new List<Sprite>();
        ReadPool(ref NumericPool, ref Numeric);
        ReadPool(ref AlphabetPool, ref AlphabetU);
        ReadPool(ref SpecialCharPool, ref SpecialChars);

        RenderText(TextToRender);
    }//Start


    public void ReadPool(ref GameObject goPool, ref List<Sprite> spritePool) {
        if (goPool == null)
            return;
        foreach (SpriteRenderer sr in goPool.GetComponentsInChildren<SpriteRenderer>())
            spritePool.Add(sr.sprite);
    }//ReadPool


    /// <summary>
    ///  Rendre string into a world space representing each character with sprites.
    /// </summary>
    public void RenderText(string toRender) {
        for (int i = 0; i < toRender.Length; i++) {
            if (i < renderedSprites.Count && renderedSprites.Count != 0) {
                this.Replace(toRender[i], i);
            } else {
                Add(toRender[i]);
            }
        }//for
    }//RenderText


    /// <summary>
    ///  Create a GameObject with a Sprite representation of the passed symbol.
    /// </summary>
    /// <param name="symbol">A character symbol to be rendered in world space.</param>
    /// <param name="atPosition">Where to add this symbol in the currently rendrered sentance. default = -1 -> add to the end of list.</param>
    /// <returns>Return created in the world SpriteRenderer object.</returns>
    public SpriteRenderer Add(char symbol, int atPosition=-1) {
        Sprite letterSprite      = CharToSprite(symbol);

        SpriteRenderer letterObj = this.createWorldObject();
        letterObj.sprite = letterSprite;
        letterObj.sortingLayerName = SortingLayerName;
        letterObj.sortingOrder = OrderInLayer;

        Vector2 letterNewPos = Vector2.zero;
        if (this.renderedSprites.Count > 0) {
            Vector2 prevLetterSize = this.renderedSprites[this.renderedSprites.Count - 1].sprite.rect.size / PixelPerUnit;
            Vector2 prevLetterPos = this.renderedSprites[this.renderedSprites.Count - 1].transform.localPosition;

            if (this.renderedSprites.Count > 0) {
                var offset = prevLetterPos.x + prevLetterSize.x + LetterSpacing;
                letterNewPos = new Vector2(offset, 0);
            }//if
        }//if

        letterObj.transform.localPosition = letterNewPos;

        renderedSprites.Add(letterObj);

        if (letterObj == null) //safety net
            return null;       //no need to render NULl object.

        if (atPosition < 0 || renderedSprites.Count <= atPosition)
            renderedSprites.Add(letterObj);
        else
            renderedSprites.Insert(atPosition, letterObj);

        letterObj.gameObject.name = symbol.ToString().ToUpper();
        letterObj.gameObject.SetActive(true);
        return letterObj;
    }//Add


    /// <summary>
    ///  Remove rendrered character from the world.
    /// </summary>
    /// <param name="letterPosition">
    ///  Default(-1) will remove from the top of the list (end of sentance).
    /// Otherwise - pass an index of the letter to remove. Passing invalid index will do nothing.
    /// </param>
    /// <returns>True - letter was removed. False - nothing happened.</returns>
    public bool Remove(int letterPosition = -1) {
        if (letterPosition == -1)
            letterPosition = renderedSprites.Count - 1;

        if (letterPosition >= renderedSprites.Count)
            return false;

        if (renderedSprites.Count == 0)
            return false;

        SpriteRenderer letter = renderedSprites[letterPosition];
        renderedSprites.RemoveAt(letterPosition);
        if (letter != null)
            Destroy(letter.gameObject);
        return true;
    }//RemoveLetter


    /// <summary>
    ///  Replace a letter with the new symbol at requested position.
    /// </summary>
    /// <param name="symbol">New symbol to create.</param>
    /// <param name="atPosition">Position in the rendrered sentance to replace.</param>
    /// <returns>SpriteRenderer of the newly created world character.</returns>
    public SpriteRenderer Replace(char symbol, int atPosition) {
        if (atPosition >= renderedSprites.Count || atPosition < 0)
            return null;
        this.renderedSprites[atPosition].sprite = CharToSprite(symbol);
        this.renderedSprites[atPosition].name   = symbol.ToString();
        return this.renderedSprites[atPosition];
    }//Replace


    /// <summary>
    ///  Remove all of the created characters from the world space.
    /// </summary>
    public void RemoveAll() {
        TextToRender = "";
        for(int i = 0; i < renderedSprites.Count; i++) {
            Remove(i);
        }//for
    }//RemoveAll


    /// <summary>
    ///  Get a sprite representing the symbol based of the Sprite arrays.
    /// This function will use a ASCII(Dec) value of the symbol to search 
    /// for the representation.
    /// </summary>
    /// <param name="symbol">Symbol to find a Sprite representation for.</param>
    /// <returns>A Sprite representing the symbol. Null - no Sprite found.</returns>
    public Sprite CharToSprite(char symbol) {
        int decCode    = symbol;  //refer to ASCII table for the char codes.
        int arrayIndex  = -1;

        /* *** Parse Number character *** */
        if (this.IsNumber(symbol)) {
            arrayIndex = decCode - Mathf.FloorToInt(NumericCharRange.x);
            if (!validateArrayIndex(arrayIndex, ref Numeric))
                return null;
            return Numeric[arrayIndex];
        }//if number

        /* *** Parse Special character *** */
        if (IsSpecialChar(symbol)) {
            arrayIndex = decCode - Mathf.FloorToInt(SpecialCharRange.x);
            if (!validateArrayIndex(arrayIndex, ref SpecialChars))
                return null;
            return SpecialChars[arrayIndex];
        }//if special

        /* *** Parse Alphabetic character *** */
        char upperLetter = symbol.ToString().ToUpper()[0]; //make it upper, cause no lowercase support here.
        bool isLetter = IsUAlphabetic(upperLetter);
        if (isLetter) {
            arrayIndex = upperLetter - Mathf.FloorToInt(AlphabetUCaseRange.x);
            if (!validateArrayIndex(arrayIndex, ref AlphabetU))
                return null;
            return AlphabetU[arrayIndex];
        }//if alphabetic

        return null; //Unknown character was passed.
    }//BuildText


    /// <summary>
    ///  Create a GameObject instance with a SpriteRenderer component.
    /// Sorting Layer, Sorting Order, Parent, LocalScale, LocalPosition are
    /// set here.
    /// </summary>
    /// <returns>SpriteRenderer component of the created GameObject.</returns>
    protected SpriteRenderer createWorldObject() {
        GameObject numberGO = new GameObject();
        numberGO.AddComponent<SpriteRenderer>();

        SpriteRenderer spriteRenderer = numberGO.GetComponent<SpriteRenderer>();
        spriteRenderer.sortingLayerName = SortingLayerName;
        spriteRenderer.sortingOrder = OrderInLayer;

        numberGO.transform.parent = this.transform;
        numberGO.transform.localScale = Scale;
        numberGO.transform.localPosition = Vector3.zero;
        numberGO.SetActive(false);

        return spriteRenderer;
    }//CreateWorldFont


    /// <summary>
    ///  Check if symbol is between hex values of 32 and 47 (both inclusive).
    /// This is a special character range, based of: http://www.asciitable.com/
    /// </summary>
    public bool IsSpecialChar(char symbol) {
        int decCode = symbol;
        return decCode >= SpecialCharRange.x && decCode <= SpecialCharRange.y;
    }//IsSpecialChar


    /// <summary>
    ///  Check if between hex value of 65 and 90 (both inclusive) which
    ///  represent the Upper Case alphabetic characters: http://www.asciitable.com/
    /// </summary>
    public bool IsUAlphabetic(char symbol) {
        int decCode = symbol;
        return decCode >= AlphabetUCaseRange.x && decCode <= AlphabetUCaseRange.y;
    }//IsUpperLetter

    
    /// <summary>
    ///  Check if symbol is between 
    /// </summary>
    public bool IsNumber(char symbol) {
        int decCode = symbol;
        return decCode >= NumericCharRange.x && decCode <= NumericCharRange.y;
    }//IsNumber


    /// <summary>
    ///  Validates array for out of bounds and for null sprite
    /// at the index's position. I just want a slightly more
    /// readable message to be displayed with less panic when
    /// things go wrong...
    /// </summary>
    /// <param name="index">Index of the array to validate.</param>
    /// <param name="target">Array to validate on.</param>
    /// <returns>True if everything is good. 
    ///          False - something went wrong.
    /// </returns>
    protected bool validateArrayIndex(int index, ref List<Sprite> target) {
        if(target.Count == 0) {
            #if UNITY_EDITOR
                Debug.LogWarning("Array has no sprites set!");
            #endif
            return false;
        }
        if (index >= target.Count || index < 0) {
            #if UNITY_EDITOR
                Debug.LogWarning("Out Of range! Passed " + index + " for the array  " + target.Count);
            #endif
            return false;
        }//if length

        if (target[index] == null) {
            #if UNITY_EDITOR
                if (target[index] == null)
                    Debug.LogWarning("Array with index '" + index + "' has no sprite representation!");
            #endif
            return false;
        }//if null

        return true;
    }//validateArrayIndex

}//class
