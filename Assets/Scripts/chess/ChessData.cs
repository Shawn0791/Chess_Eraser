[System.Serializable]
public class ChessData
{
    public ChessData(ChessType t)
    {
        chessType = t;
    }

    public enum ChessType
    {
        A,
        B,
        C,
        D,
        E
    }

    public ChessType chessType;
}
