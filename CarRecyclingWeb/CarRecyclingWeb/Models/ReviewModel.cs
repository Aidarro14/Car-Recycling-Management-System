using System;

namespace CarRecyclingWeb.Models
{
    public class ReviewModel
    {
        public string Name { get; set; }
        public int Rating { get; set; } 
        public DateTime Date { get; set; }
        public string Text { get; set; }
    }
}