using System;
using System.Collections.Generic;

[Serializable]

public class PlayData
{
    public enum TYPE { TYPE_NONE, TYPE_DIALOG, TYPE_BUTTON, TYPE_RESCUE, TYPE_NEGLECT, TYPE_POTION, TYPE_END }
    public enum POINT { TYPE_NONE, TYPE_ZEN, TYPE_CHAOS, TYPE_END }

    public TYPE dataType;
    public string profileSpr;
    public string dialogText;

    public List<string> choiceText;
    public List<string> choiceDialog;
    public List<POINT>  choicePoint;
    public List<int>    choiceValue;
}
