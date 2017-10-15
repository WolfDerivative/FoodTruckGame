﻿using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ThoughtProcessor : Thoughts {

    private ActionThoughts      _actionThoughts;
    private FoodThoughts        _foodThoughts;
    private FeedbackThoughts    _feedbackThoughts;
    private Thoughts[] _allThoughts {
        get { return new Thoughts[] { _actionThoughts, _foodThoughts, _feedbackThoughts }; }
    }//_allThoughts
    private int activeThoughts {
        get {
            int count = 0;
            foreach (Thoughts thought in _allThoughts)
                count += thought.IsActive ? 1 : 0;
            return count;
        }//get
    }//activeThoughts


    public override void Start () {
        base.Start();
        if (_foodThoughts == null)
            _foodThoughts = GetComponentInChildren<FoodThoughts>();
        if(_actionThoughts == null)
            _actionThoughts = GetComponentInChildren<ActionThoughts>();
        if(_feedbackThoughts == null)
            _feedbackThoughts = GetComponentInChildren<FeedbackThoughts>();
    }//Start


    public void ShowFoodChoice(Sprite icon, bool state) {
        _foodThoughts.SetIcon(icon, state);

        if (state && !_spriteRenderer.enabled)
            _spriteRenderer.enabled = true;
    }//ShowThoughts


    public void ShowAction(ActionThoughts.Actions actionType, bool state) {
        _actionThoughts.SetIcon(actionType, state);

        if (state && !_spriteRenderer.enabled)
            _spriteRenderer.enabled = true;
    }//ShowThoughts


    public void ShowFeedback(float rating, bool state) {
        _foodThoughts.Hide();
        _actionThoughts.Hide();

        _feedbackThoughts.SetIcon(FeedbackByRating(rating), state);
        if (state && !_spriteRenderer.enabled)
            _spriteRenderer.enabled = true;
    }//ShowFeedback

    
    public FeedbackThoughts.Feedbacks FeedbackByRating(float rating) {
        ConsumerRating.Grade grade = District.Instance.Ratings.GradeFromPoints(rating);

        switch (grade) {
            case (ConsumerRating.Grade.A):
                return FeedbackThoughts.Feedbacks.Happy;
            case (ConsumerRating.Grade.B):
                return FeedbackThoughts.Feedbacks.Neutral;
            case (ConsumerRating.Grade.C):
                return FeedbackThoughts.Feedbacks.Sad;
            default:
                return FeedbackThoughts.Feedbacks.Angry;
        }
    }//FeedbackByRating


    public void HideAll() {
        _spriteRenderer.enabled = false;
        foreach (Thoughts thought in _allThoughts)
            thought.Hide();
    }//HideAll

}//class