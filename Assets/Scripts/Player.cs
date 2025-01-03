using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Player {

    private List<int> scores;
    private static int idcount;

    public int ID {
        get; private set;
    }

    public int BallsThrown {
        get { return scores.Where(s => s != -1).Count(); }
    }

    public Player() {
        scores = new List<int>();
        ID = ++idcount;
    }

    public int TotalScore {
        get { return scores.Sum(); }
    }

    public int[] Scores {
        get { return scores.ToArray(); }
    }

    public void AddScore(int currentScore) {
        scores.Add(currentScore);
    }

    public void Reset() {
        scores.Clear();
    }
}