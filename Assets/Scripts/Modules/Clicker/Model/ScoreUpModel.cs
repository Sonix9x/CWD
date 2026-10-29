namespace CWD.Scripts.Modules.Clicker.Model
{
    public class ScoreUpModel
    {
        public int ScoreForClick { get; private set; }

        public ScoreUpModel(int score)
        {
            ScoreForClick = score;
        }
    }
}