using System;
using InvestApp.Domain.Models.Base;

namespace InvestApp.Domain.Models
{
    public class CompanyRaiting : BaseEntity
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public Rating Rating { get; set; }
        public int RatingScore { get; set; }
        public RatingRecommendation RatingRecommendation { get; set; }
        public int RatingDetailsDCFScore { get; set; }
        public RatingRecommendation RatingDetailsDCFRecommendation { get; set; }
        public int RatingDetailsROEScore { get; set; }
        public RatingRecommendation RatingDetailsROERecommendation { get; set; }
        public int RatingDetailsROAScore { get; set; }
        public RatingRecommendation RatingDetailsROARecommendation { get; set; }
        public int RatingDetailsDEScore { get; set; }
        public RatingRecommendation RatingDetailsDERecommendation { get; set; }
        public int RatingDetailsPEScore { get; set; }
        public RatingRecommendation RatingDetailsPERecommendation { get; set; }
        public int RatingDetailsPBScore { get; set; }
        public RatingRecommendation RatingDetailsPBRecommendation { get; set; }
    }

    public class Rating: BaseEntity
    {
        public string Name { get; set; }
        public int Power { get; set; } = 1;
    }

    public class RatingRecommendation : BaseEntity
    {
        public string Name { get; set; }
        public int Power { get; set; } = 1;
    }
}