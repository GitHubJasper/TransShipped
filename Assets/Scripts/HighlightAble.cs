using UnityEngine;

public abstract class HighlightAble : MonoBehaviour {
    
    protected Outline outline;
    protected bool lastClicked;
    public bool highlight;
    public Material defaultMat;
    public Material highlightMat;

    public void InitHighlight(){
        outline = gameObject.AddComponent<Outline>();
        if(GetComponent<Renderer>() != null){
            if(highlight){
                GetComponent<Renderer>().material = highlightMat;
            } else{
                GetComponent<Renderer>().material = defaultMat;
            }
        }
        Game.instance.RegisterHighlight(this);
    }

    public void Highlight(bool highlight){
        this.highlight = highlight;
        if(GetComponent<Renderer>() != null && outline != null) {
            if(highlight) {
                outline.enabled = true;
                GetComponent<Renderer>().material = highlightMat;
            } else if(!lastClicked){
                lastClicked = false;
                outline.enabled = false;
                GetComponent<Renderer>().material = defaultMat;
            }
        }
    }

    public void ForceHighlight(bool highlight){
        this.highlight = highlight;
        if(GetComponent<Renderer>() != null && outline != null) {
            if(highlight) {
                outline.enabled = true;
                GetComponent<Renderer>().material = highlightMat;
            } else {
                lastClicked = false;
                outline.enabled = false;
                GetComponent<Renderer>().material = defaultMat;
            }
        }
    }
}