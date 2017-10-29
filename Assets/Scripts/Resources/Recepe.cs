using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recepe : Resources{

    public float CookTime   = 2;
    public Sprite Icon;

    public float currentCookingTime;


    public Recepe(Recepe r) {
        this.Brains = r.Brains;
        this.Seasonings = r.Seasonings;
        this.Drinks = r.Drinks;
        this.Cash = r.Cash;
        this.CookTime = r.CookTime;
    }//ctor


    /// <summary>
    ///  Cook food by adding the amount of time to the cooking timer.
    /// Return true when food is ready and reset cooking time.
    /// False - food is not ready yet.
    /// </summary>
    /// <param name="timeToAdd">Time to add to cooking time.</param>
    /// <returns></returns>
    public bool Cook(float timeToAdd) {
        currentCookingTime += timeToAdd;
        if(currentCookingTime >= CookTime) {
            currentCookingTime = 0;
            return true;
        }//if
        return false;
    }//Cook


    public void AddBrain(int amount) {
        Brains.Add(amount);
    }//AddBrain


    public void AddSeasoning(int amount) {
        Seasonings.Add(amount);
    }//AddBrain


    public void AddDrink(int amount) {
        Drinks.Add(amount);
    }//AddBrain


    public void AddPrice(float amount) {
        this.Cash.Add(amount);
    }//AddPrice

}//class
