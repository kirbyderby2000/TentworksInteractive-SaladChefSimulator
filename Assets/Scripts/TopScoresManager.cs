using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TopScoresManager : MonoBehaviour
{

    [SerializeField]  private List<TMPro.TextMeshProUGUI> topScoreTexts;

    private List<int> topScores;

    public void DeletePlayerPrefs()
    {
        TopScoresPrefsManager.ClearTopScorePlayerPrefs();
    }

    public void UpdateAndDisplayBestScore(int bestScoreThisRound)
    {
        topScores = TopScoresPrefsManager.GetTopPlayerScores(bestScoreThisRound);
        UpdateScoresDisplayedWithList(topScores);
    }

    public void DisplayBestScore()
    {
        topScores = TopScoresPrefsManager.GetTopPlayerScores();
        UpdateScoresDisplayedWithList(topScores);
    }

    private void UpdateScoresDisplayedWithList(List<int> topScores)
    {
        for (int i = 0; i < topScoreTexts.Count; i++)
        {
            if(i < topScores.Count)
            {
                string newText = (i + 1).ToString() + ".) " + topScores[i].ToString();
                topScoreTexts[i].text = newText;
                topScoreTexts[i].gameObject.SetActive(true);
            }
            else
            {
                topScoreTexts[i].gameObject.SetActive(false);
            }
        }
    }


    private static class TopScoresPrefsManager
    {
        private static string TOP_SCORES_PLAYER_PREFS_KEY = "TOP_PLAYER_SCORES";

        /// <summary>
        /// Returns the list of top player scores
        /// </summary>
        /// <returns></returns>
        public static List<int> GetTopPlayerScores()
        {
            // Get the top player scores json string
            string jsonString = PlayerPrefs.GetString(TOP_SCORES_PLAYER_PREFS_KEY, DefaultJsonStringValue());

            Debug.Log(jsonString);

            // Parse it from an int array using the JsonUtility.FromJson
            TopScores topScoresInstance = JsonUtility.FromJson<TopScores>(jsonString);

            List<int> fromJsonArrayList = topScoresInstance == null ? new List<int>() : topScoresInstance.scores;

            if (fromJsonArrayList == null)
            {
                fromJsonArrayList = new List<int>();
            }

            if(fromJsonArrayList.Count > 1)
            {
                fromJsonArrayList = fromJsonArrayList.OrderByDescending(x => { return x; }).ToList();
            }

            // Finally, return the list
            return fromJsonArrayList;
        }

        public static List<int> GetTopPlayerScores(int scoreToSubmit)
        {
            // Get the list of top player scores
            List<int> topScores = GetTopPlayerScores();
            // If the top score count is less than 10, then simply append the score to the list of top scores
            if(topScores.Count < 10)
            {
                // Add the score
                topScores.Add(scoreToSubmit);
                // Order the list in descending order
                topScores = topScores.OrderByDescending(x => { return x; }).ToList();
                // Update the top scores
                UpdateTopScores(topScores);
            }
            // If the top score count is equal to 10, then check the last score in the list
            else if(topScores.Count == 10)
            {
                // Get the last score in the list
                int lastScore = topScores[topScores.Count - 1];
                // If the last score is less than the new score that needs to be submitted, then 
                // replace the last score in the list with the new score to submit
                if(lastScore < scoreToSubmit)
                {
                    topScores[topScores.Count - 1] = scoreToSubmit;

                    topScores = topScores.OrderByDescending(x => { return x; }).ToList();
                    // Update the top scores
                    UpdateTopScores(topScores);
                }
            }
            // return the top scores
            return topScores;
        }

        private static void UpdateTopScores(List<int> newTopScoresList)
        {
            TopScores newTopScores = new TopScores();
            newTopScores.scores = newTopScoresList;
            string jsonString = JsonUtility.ToJson(newTopScores);
            PlayerPrefs.SetString(TOP_SCORES_PLAYER_PREFS_KEY, jsonString);
        }


        private static string DefaultJsonStringValue()
        {
            TopScores myTopScores = new TopScores();
            myTopScores.scores = new List<int>();
            return JsonUtility.ToJson(myTopScores);
        }

        public static void ClearTopScorePlayerPrefs()
        {
            PlayerPrefs.DeleteKey(TOP_SCORES_PLAYER_PREFS_KEY);
        }

        [System.Serializable]
        public class TopScores
        {
            public List<int> scores = new List<int>();
        }
    }
}
