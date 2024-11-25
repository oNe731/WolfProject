using System;

[Serializable]
public class CutData
{
    public enum TYPE { TYPE_NONE, TYPE_DIALOG, TYPE_FADE, TYPE_END }
    public enum FADE { FADE_NONE, FADE_IN, FADE_END }


    public TYPE m_dataType;

    public string dialogText;
    public string cutSpr;

    public FADE fade;
}
