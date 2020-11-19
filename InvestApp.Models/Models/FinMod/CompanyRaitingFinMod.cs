namespace InvestApp.Domain.Models.FinMod
{
    public class CompanyRaitingFinMod
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public string Rating { get; set; }
        public int RatingScore { get; set; }
        public string RatingRecommendation { get; set; }
        public int RatingDetailsDCFScore { get; set; }
        public string RatingDetailsDCFRecommendation { get; set; }
        public int RatingDetailsROEScore { get; set; }
        public string RatingDetailsROERecommendation { get; set; }
        public int RatingDetailsROAScore { get; set; }
        public string RatingDetailsROARecommendation { get; set; }
        public int RatingDetailsDEScore { get; set; }
        public string RatingDetailsDERecommendation { get; set; }
        public int RatingDetailsPEScore { get; set; }
        public string RatingDetailsPERecommendation { get; set; }
        public int RatingDetailsPBScore { get; set; }
        public string RatingDetailsPBRecommendation { get; set; }
    }
}