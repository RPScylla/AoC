namespace RPSonline.AoC
{
    /// <summary> 
    /// AoC Day interface. 
    /// Used by the <see cref="AoCRunner"/>. 
    /// </summary>
    public interface IDay
    {
        /// <summary>
        /// Solves the AoC Day and returns the answer.
        /// </summary>
        /// <returns>Answers for the day <see cref="AnswerModel"/></returns>
        AnswerModel Solve();
    }
}
