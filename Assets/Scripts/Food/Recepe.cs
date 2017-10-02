using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recepe {

    public float CookTime   = 2;

    public Sprite Icon;
    public string NameOnTheMenu = "Some Food";

    public Brain Brains;
    public Seasoning Seasoning;
    public Drink Drinks;
    [Tooltip("Price to sell this recepe for.")]
    public float Price = 5.0f;

    private float currentCookingTime;


    public Recepe(Recepe r) {
        this.Brains = r.Brains;
        this.Seasoning = r.Seasoning;
        this.Drinks = r.Drinks;
        this.Price = r.Price;
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
        Seasoning.Add(amount);
    }//AddBrain


    public void AddDrink(int amount) {
        Drinks.Add(amount);
    }//AddBrain


    public void AddPrice(int amount) {
        Price += amount;
        if (Price < 0)
            Price = 0;
    }//AddPrice

}//class
