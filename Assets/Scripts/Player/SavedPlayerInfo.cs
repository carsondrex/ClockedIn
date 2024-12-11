using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavedPlayerInfo
{
    private static int[] savedCardCounts = { 0, 0, 0, 0 };
    private static string savedCurrentCard;

    public static int[] getSavedCardCounts()
    {
        return savedCardCounts;
    }

    public static void setSavedCardCounts(int[] set)
    {
        savedCardCounts = set;
    }

    public static string getSavedCurrentCard()
    {
        return savedCurrentCard;
    }

    public static void setSavedCurrentCard(string set)
    {

    }



}
