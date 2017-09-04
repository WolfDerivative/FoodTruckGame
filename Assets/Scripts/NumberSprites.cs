using UnityEngine;


public class NumberSprites : MonoBehaviour {

    [Tooltip("Element in the array represents the a number sprite.")]
    public Sprite[] Numbers;
    public string SortingLayerName;
    public int OrderInLayer;
    public Vector3 Scale = Vector3.one;

    private GameObject[] numberRepresentations;


    public void Start() {
        numberRepresentations = new GameObject[Numbers.Length];
        for(int i = 0; i < Numbers.Length; i++) {
            Sprite spriteRepr = Numbers[i];
            GameObject numberGO = new GameObject();
            numberGO.AddComponent<SpriteRenderer>();

            SpriteRenderer spriteRenderer   = numberGO.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite           = spriteRepr;
            spriteRenderer.sortingLayerName = SortingLayerName;
            spriteRenderer.sortingOrder     = OrderInLayer;

            numberGO.transform.parent = this.transform;
            numberGO.transform.localScale = Scale;
            numberGO.SetActive(false);
            numberGO.transform.localPosition = Vector3.zero;

            numberRepresentations[i] = numberGO;
        }//foreach
    }//Start


    public void Render(int number) {
        if (number >= Numbers.Length) {
            #if UNITY_EDITOR
                Debug.LogWarning("Passed number to render out of range: " + number + " passed. Array: " + Numbers.Length);
            #endif
            return;
        }//if length
        if(Numbers[number] == null) {
            #if UNITY_EDITOR
                if (Numbers[number] == null)
                    Debug.LogWarning("Number '" + number + "' has no sprite representation!");
            #endif  
            return;
        }//if null
        if(number < 10)
            HideAll();
        numberRepresentations[number].SetActive(true);
    }//Render


    public void HideAll() {
        foreach (GameObject numberRepr in numberRepresentations)
            numberRepr.SetActive(false);
    }

}//class
