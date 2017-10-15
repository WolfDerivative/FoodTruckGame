using UnityEngine;

public class FeedbackThoughts : Thoughts {

    public enum Feedbacks { Happy, Neutral, Sad, Angry }

    public Sprite HappyIcon;
    public Sprite NeutralIcon;
    public Sprite SadIcon;
    public Sprite AngryIcon;


    public void SetIcon(Feedbacks feedbackType, bool state) {
        switch (feedbackType) {
            case Feedbacks.Happy:
                SetIcon(HappyIcon, state);
                break;
            case Feedbacks.Neutral:
                SetIcon(NeutralIcon, state);
                break;
            case Feedbacks.Sad:
                SetIcon(SadIcon, state);
                break;
            case Feedbacks.Angry:
                SetIcon(AngryIcon, state);
                break;
        }//switch
    }//SetIcon

}//class
