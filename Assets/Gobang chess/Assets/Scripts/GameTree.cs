//博弈树结点
public class GameTree {

    public GameTree[] child;
    public GameTree father;
    public int depth;
    public int posi;
    public int posj;
    public int score;
    public int chess;
    public int solve;

    public override string ToString() {
        string ans = "";
        ans += "depth：" + depth + "\n";
        ans += "Position：" + (posi - 7) + "," + (posj - 7) + "\n";
        ans += "Scores：" + score + "\n";
        if (chess == 1) ans += "Black chess\n";
        else if (chess == 2) ans += "White chess\n";
        ans += "Affiliated to the Programme：" + solve;
        return ans;
    }
}