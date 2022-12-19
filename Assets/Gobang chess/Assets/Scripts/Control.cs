using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Enumeration of states
public enum State {
    Ready,
    Black,
    White,
    BlackWin,
    WhiteWin
}

public struct ScorePos {
    public int score;
    public int posi;
    public int posj;
}

//ScorePos class comparison
class ScorePosComparer : IComparer<ScorePos> {
    public int Compare(ScorePos x, ScorePos y) {
        if (x.score < y.score) return 1;    //Order from largest to smallest
        else if (x.score > y.score) return -1;
        else return 0;
    }
}

public class Control : MonoBehaviour {

    #region variable
    public Transform BlackChess, WhiteChess;    //chess
    public GameObject Frame;                   //The box
    public GameObject Float;                    //Game over popover
    public Text Message;                        //The message
    public GameObject BlackIcon, WhiteIcon;     //Chess piece icon
    public GameObject StartButton, StartButton1;//Start button
    public Text BlackTime, WhiteTime;           //Black and white left time
    public Toggle Mode;                         //Computer mode
    private float computerTime;                 //Computer Thinking Time
    private int totalTime;                      //Total time
    private State state;                        //State of play
    private float blackCount, whiteCount;       //Timing of both parties
    private float thinkCount;                   //Thinking Time timing
    private bool frameBig;                      //The box changes direction
    private bool ComputerMode;                  //Computer intelligent mode
    private int width = 5;  //The width of the downward expansion of each layer
    private int depth = 7;  //Total number of layers (must be odd)
    ScorePosComparer compare;   //Rule of comparison
    #endregion

    private void Start() {
        Data.ResetMap();
        computerTime = 0.19f;
        totalTime = 1800;
        ComputerMode = false;
        state = State.Ready;
        blackCount = whiteCount = 0;
        BlackTime.text = GetTime(blackCount);
        WhiteTime.text = GetTime(whiteCount);
        thinkCount = 0;
        frameBig = true;
        compare = new ScorePosComparer();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            OnClickReset();
        }
        if (Mode.isOn != ComputerMode) {
            ComputerMode = Mode.isOn;
        }
        if (state == State.Black) {
            //The player clicks on chess
            blackCount += Time.deltaTime;
            BlackTime.text = GetTime(blackCount);
            if (Input.GetMouseButtonDown(0))
            {  //Press
                if (PutChess(Input.mousePosition, BlackChess))
                {    //Can play chess
                    state = State.White;
                    if (CheckWin())
                    {   //Victory or not
                        state = State.BlackWin;
                    }
                }
            }
            UpdateFrame();
        }
        else if (state == State.White) {
            //Computer chess
            whiteCount += Time.deltaTime;
            thinkCount += Time.deltaTime;
            if (thinkCount >= computerTime) {   //Waiting time
                //Computer calculation
                thinkCount = 0;
                if (Calculate(WhiteChess))
                {    //Have a solution
                    if (CheckWin())
                    {   //Victory or not
                        state = State.WhiteWin;
                    }
                    else {
                        state = State.Black;
                    }
                }
                else {
                    state = State.BlackWin;
                }
                whiteCount += 2.5f;    //Algorithm running time compensation
            }
            WhiteTime.text = GetTime(whiteCount);
            UpdateFrame();
        }
        else if (state == State.BlackWin) {
            //Player wins
            Float.SetActive(true);
            Message.text = "WIN";
            Message.color = new Color(0, 1, 0, 1);
            WhiteIcon.SetActive(false);
        }
        else if (state == State.WhiteWin) {
            //Computer wins
            Float.SetActive(true);
            Message.text = "LOSE";
            Message.color = new Color(1, 0, 0, 1);
            BlackIcon.SetActive(false);
        }
    }

    //Make a chess
    private void CreateChess(int i, int j, Transform c) {
        if (c.name == "BlackChess") {
            Data.Map[i, j] = 1;
            BlackIcon.SetActive(false);
            WhiteIcon.SetActive(true);
        }
        if (c.name == "WhiteChess") {
            Data.Map[i, j] = 2;
            WhiteIcon.SetActive(false);
            BlackIcon.SetActive(true);
        }
        float a = 1.32f;
        Vector3 pos = new Vector3();
        pos.x = (i - 7) * a;
        pos.y = (j - 7) * a;
        pos.z = 1;
        Instantiate(c, pos, new Quaternion());
        Frame.SetActive(true);
        Frame.transform.position = pos;
    }

    //A player plays chess
    private bool PutChess(Vector2 pos, Transform c) {
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(pos);
        worldPos /= 1.32f;
        worldPos += new Vector2(7, 7);
        int i = Mathf.RoundToInt(worldPos.x);
        int j = Mathf.RoundToInt(worldPos.y);
        if (i < 0 || j < 0 || i > 14 || j > 14) return false;   //out
        if (Data.Map[i, j] != 0) return false;      //Existing chess pieces
        CreateChess(i, j, c);
        return true;
    }

    //Computer calculation
    private bool Calculate(Transform c) {
        int[,] blackScore = new int[15, 15];
        int[,] whiteScore = new int[15, 15];
        int maxScore = -1;
        int posi = 0, posj = 0;
        if (!ComputerMode)
        {    //Non-intelligent mode (calculate the weight of each point)
            for (int i = 0; i < 15; i++) {
                for (int j = 0; j < 15; j++) {
                    if (Data.Map[i, j] == 0) {
                        GetScore(i, j, ref blackScore[i, j], ref whiteScore[i, j], Data.Map);  //Reference parameter
                        if (maxScore < blackScore[i, j] + whiteScore[i, j]) {
                            maxScore = blackScore[i, j] + whiteScore[i, j];
                            posi = i;
                            posj = j;
                        }
                    }
                }
            }
            if (maxScore != -1)
            {   //Have a solution
                CreateChess(posi, posj, c);
                return true;
            }
        }
        else
        {  //Intelligent Mode (Game tree +αβ pruning optimization)
            GetAIScore();
            return true;
        }
        return false;
    }

    //Check for victory
    public bool CheckWin() {
        int i = Mathf.RoundToInt((Frame.transform.position.x / 1.32f) + 7); //The final position of the drop
        int j = Mathf.RoundToInt((Frame.transform.position.y / 1.32f) + 7);
        int chess = Data.Map[i, j];     //Final color (1: black, 2: white)
        //Cross sectional inspection
        int a = 0, b = 0;   //The number of consecutive pieces at each end
        for (int k = i - 1; k >= 0; k--) {
            if (Data.Map[k, j] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = i + 1; k < 15; k++) {
            if (Data.Map[k, j] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Longitudinal examination
        a = b = 0;
        for (int k = j - 1; k >= 0; k--) {
            if (Data.Map[i, k] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = j + 1; k < 15; k++) {
            if (Data.Map[i, k] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Oblique lower left
        a = b = 0;
        for (int m = i - 1, n = j - 1; m >= 0 && n >= 0; m--, n--) {
            if (Data.Map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j + 1; m < 15 && n < 15; m++, n++) {
            if (Data.Map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Oblique lower right
        a = b = 0;
        for (int m = i - 1, n = j + 1; m >= 0 && n < 15; m--, n++) {
            if (Data.Map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j - 1; m < 15 && n >= 0; m++, n--) {
            if (Data.Map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }
        return false;
    }
    public bool CheckWin(int i, int j, int[,] map) {
        int chess = map[i, j];     //Final color (1: black, 2: white)
        //Cross sectional inspection
        int a = 0, b = 0;   //The number of consecutive pieces at each end
        for (int k = i - 1; k >= 0; k--) {
            if (map[k, j] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = i + 1; k < 15; k++) {
            if (map[k, j] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Longitudinal examination
        a = b = 0;
        for (int k = j - 1; k >= 0; k--) {
            if (map[i, k] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = j + 1; k < 15; k++) {
            if (map[i, k] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Oblique lower left
        a = b = 0;
        for (int m = i - 1, n = j - 1; m >= 0 && n >= 0; m--, n--) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j + 1; m < 15 && n < 15; m++, n++) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }

        //Oblique lower right
        a = b = 0;
        for (int m = i - 1, n = j + 1; m >= 0 && n < 15; m--, n++) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j - 1; m < 15 && n >= 0; m++, n--) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        if (a + b + 1 >= 5) {
            return true;
        }
        return false;
    }

    //计算分数
    private void GetScore(int i, int j, ref int black, ref int white, int[,] map) {

        //If I put in black
        int chess = 1;
        int s1 = 0, s2 = 0, s3 = 0, s4 = 0; //The number of pieces in all four directions
        //Cross sectional inspection
        int a = 0, b = 0;   //The number of consecutive pieces at each end
        for (int k = i - 1; k >= 0; k--) {
            if (map[k, j] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = i + 1; k < 15; k++) {
            if (map[k, j] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s1 = a + b + 1;
        //Longitudinal examination
        a = b = 0;
        for (int k = j - 1; k >= 0; k--) {
            if (map[i, k] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = j + 1; k < 15; k++) {
            if (map[i, k] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s2 = a + b + 1;
        //Oblique lower left
        a = b = 0;
        for (int m = i - 1, n = j - 1; m >= 0 && n >= 0; m--, n--) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j + 1; m < 15 && n < 15; m++, n++) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s3 = a + b + 1;
        //Oblique down right
        a = b = 0;
        for (int m = i - 1, n = j + 1; m >= 0 && n < 15; m--, n++) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j - 1; m < 15 && n >= 0; m++, n--) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s4 = a + b + 1;
        black = GetScore(s1, -1) + GetScore(s2, -1) + GetScore(s3, -1) + GetScore(s4, -1);

        //If I put white
        chess = 2;
        s1 = s2 = s3 = s4 = 0;  //The number of pieces in all four directions
        //Cross sectional inspection
        a = b = 0;   //The number of consecutive pieces at each end
        for (int k = i - 1; k >= 0; k--) {
            if (map[k, j] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = i + 1; k < 15; k++) {
            if (map[k, j] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s1 = a + b + 1;
        //Longitudinal examination
        a = b = 0;
        for (int k = j - 1; k >= 0; k--) {
            if (map[i, k] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int k = j + 1; k < 15; k++) {
            if (map[i, k] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s2 = a + b + 1;
        //Oblique lower left
        a = b = 0;
        for (int m = i - 1, n = j - 1; m >= 0 && n >= 0; m--, n--) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j + 1; m < 15 && n < 15; m++, n++) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s3 = a + b + 1;
        //Oblique lower right
        a = b = 0;
        for (int m = i - 1, n = j + 1; m >= 0 && n < 15; m--, n++) {
            if (map[m, n] == chess) {
                a++;
            }
            else {
                break;
            }
        }
        for (int m = i + 1, n = j - 1; m < 15 && n >= 0; m++, n--) {
            if (map[m, n] == chess) {
                b++;
            }
            else {
                break;
            }
        }
        s4 = a + b + 1;
        white = GetScore(s1, 1) + GetScore(s2, 1) + GetScore(s3, 1) + GetScore(s4, 1);

    }

    //Intelligently calculating scores
    private void GetAIScore() {
        GameTree root = new GameTree();
        root.depth = 1;
        root.chess = 1; //Black just finished walking
        root.score = 0;
        GameTree(Data.Map, root);
        //I'm going to recursively figure out the fraction
        UpdateTreeScore(root);
        int ans = 0;
        int maxScore = 0;
        for (int i = 0; i < 0; i++) {
            if (root.child[i].score > maxScore) {
                maxScore = root.child[i].score;
                ans = i;
            }
        }
        int posi = root.child[ans].posi;
        int posj = root.child[ans].posj;
        CreateChess(posi, posj, WhiteChess);
    }

    //Game tree recursion
    private void GameTree(int[,] map, GameTree father) {
        ScorePos[] sp = new ScorePos[225];
        //Screening the first few good schemes
        for (int i = 0; i < 15; i++) {
            for (int j = 0; j < 15; j++) {
                if (map[i, j] == 0) {
                    int m = 0, n = 0;
                    GetScore(i, j, ref m, ref n, map);  //Reference parameter
                    sp[i * 15 + j].score = m + n;
                    sp[i * 15 + j].posi = i;
                    sp[i * 15 + j].posj = j;
                }
            }
        }
        Array.Sort(sp, compare);
        father.child = new GameTree[width];
        for (int i = 0; i < width; i++) {
            GameTree point = new GameTree();
            point.child = new GameTree[width];
            father.child[i] = point;        //Father and son relationship
            point.father = father;
            point.depth = father.depth + 1; //Depth of game tree
            point.posi = sp[i].posi;        //Position of drop
            point.posj = sp[i].posj;
            point.chess = father.chess % 2 + 1; //Alternate playing chess

            //Count a score
            if (father.depth % 2 == 0)
            {    //Even tier (us)
                point.score = sp[i].score;
            }
            else
            {  // Odd tier (opponent)
                point.score = -sp[i].score;
            }
            if (father.score < -6000 || father.score > 6000) {
                point.score = father.score;    //Once one side wins, there is no need to keep counting.
            }
            else {
                point.score += father.score;//Add the parent layer
            }

            //Updating the Solution
            if (point.depth == 2)
            { //The solution to which the node belongs
                point.solve = i;
            }
            else {
                point.solve = father.solve;
            }

            //Expand new maps
            int[,] map1 = new int[15, 15];
            for (int j = 0; j < 15; j++) {
                for (int k = 0; k < 15; k++) {
                    map1[j, k] = map[j, k];
                }
            }
            map1[point.posi, point.posj] = point.chess;
            if (point.depth < depth) {
                GameTree(map1, point);
            }

            //Maximum number of layers, start finishing
            else
            {
                int maxScore = -1;
                GameTree point1 = new GameTree();
                for (int i1 = 0; i1 < 15; i1++) {
                    for (int j1 = 0; j1 < 15; j1++) {
                        if (map1[i1, j1] == 0) {
                            int m = 0, n = 0;
                            GetScore(i1, j1, ref m, ref n, map1);  //Reference parameter
                            if (maxScore < m + n) {
                                maxScore = m + n;
                            }
                        }
                    }
                }

                //Update point score
                if (point.score > 6000 || point.score < -6000) {
                    point1.score = point.score;
                }
                else {
                    point1.score = point.score + maxScore;
                }
                point.score = point1.score;
                //print(point.ToString());
            }
        }
    }

    //Update the scores of all nodes in the tree
    private int UpdateTreeScore(GameTree p) {
        if (p.depth == depth)
        {    //At the end of
            return p.score;
        }
        if (p.depth % 2 == 0)
        {  //Even layers, find the smallest fraction
            int minScore = 100000;
            for (int i = 0; i < width; i++) {
                if (p.child[i].score < minScore) {
                    minScore = UpdateTreeScore(p.child[i]);
                }
            }
            return minScore;
        }
        else
        {  //Odd layer, find the maximum subfraction
            int maxScore = -100000;
            for (int i = 0; i < width; i++) {
                if (p.child[i].score > maxScore) {
                    maxScore = UpdateTreeScore(p.child[i]);
                }
            }
            return maxScore;
        }
    }

    //Score calculation formula
    public int GetScore(int x, int m) {
        int ans = 0;
        if (m == 1) {
            if (x == 1) ans += 1;
            else if (x == 2) ans += 10;
            else if (x == 3) ans += 100;
            else if (x == 4) ans += 1000;
            else ans += 10000;
        }
        else if (m == -1) {
            if (x == 1) ans += 1;
            else if (x == 2) ans += 8;
            else if (x == 3) ans += 64;
            else if (x == 4) ans += 512;
            else ans += 4096;
        }
        return ans;
    }

    //The box gets bigger and smaller
    private void UpdateFrame() {
        if (frameBig) {
            Frame.transform.localScale += new Vector3(0.003f, 0.003f, 0);
        }
        else {
            Frame.transform.localScale -= new Vector3(0.003f, 0.003f, 0);
        }
        if (Frame.transform.localScale.x > 0.7f) {
            frameBig = false;
        }
        if (Frame.transform.localScale.x < 0.5f) {
            frameBig = true;
        }
    }

    //reset
    public void OnClickReset() {
        SceneManager.LoadScene(0);
    }

    //First hand game
    public void OnClickStart() {
        state = State.Black;
        StartButton.SetActive(false);
        StartButton1.SetActive(false);
        BlackIcon.SetActive(true);
    }

    //Back hand game
    public void OnClickStart1() {
        CreateChess(7, 7, WhiteChess);
        OnClickStart();
    }

    //Get the time
    private string GetTime(float n) {
        float n1 = totalTime - n;
        if (n1 <= 0) {  //时间到
            if (state == State.Black) {
                state = State.WhiteWin;
            }
            if (state == State.White) {
                state = State.BlackWin;
            }
            return "00:00.00";
        }
        string min, sec, msec;
        if ((int)n1 / 60 < 10) {
            min = "0";
        }
        else {
            min = "";
        }
        min += (int)n1 / 60 + "";
        if ((int)n1 % 60 < 10) {
            sec = "0";
        }
        else {
            sec = "";
        }
        sec += (int)n1 % 60 + "";
        if ((int)(n1 * 100) % 100 < 10) {
            msec = "0";
        }
        else {
            msec = "";
        }
        msec += (int)(n1 * 100) % 100 + "";
        return min + ":" + sec + "." + msec;
    }

}