using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace WLFU.Entities
{
    /// <summary>
    /// Describes the required settings for the rating
    /// </summary>
    interface IRating
    {
        int RatingId { get; set; }
        float Rating { get; }
        int NumberOfRatings { get; }

        IList<Grade> GradeHistory { get; }

        void AddRating(float grade);
    }
    /// <summary>
    /// Describes the required settings for the rating together with the owner of the rating
    /// </summary>
    /// <typeparam name="T">The owner of the rating</typeparam>
    interface IRating<T> : IRating
    {
        T RatingOwner { get; set; }
    }

    public class ProductRating : IRating<Product>
    {
        [Key, Column(Order = 0)]
        public int ProductId { get; set; }
        [Key, Column(Order = 1)]
        public int RatingId { get; set; }
        public float Rating { get; private set; }
        public int NumberOfRatings { get; private set; }
        public IList<Grade> GradeHistory { get; private set; }
        public ProductRating()
        {
            GradeHistory = new List<Grade>();
        }

        [ForeignKey("ProductId")]
        public Product RatingOwner { get; set; }

        public void AddRating(float grade)
        {
            Grade productGrade = new Grade() { GradeValue = grade };
            //to db
            GradeHistory.Add(productGrade);
            NumberOfRatings = GradeHistory.Count;
            //Rating = GradeHistory.Sum() / NumberOfRatings;
        }
    }

    public class CommentRating : IRating<Comment>
    {
        [Key, Column(Order = 0)]
        public int CommentId { get; set; }
        [Key, Column(Order = 1)]
        public int RatingId { get; set; }
        public float Rating { get; private set; }
        public int NumberOfRatings { get; private set; }
        public IList<Grade> GradeHistory { get; private set; }

        public CommentRating()
        {
            GradeHistory = new List<Grade>();
        }

        [ForeignKey("CommentId")]
        public Comment RatingOwner { get; set; }

        public void AddRating(float grade)
        {
            Grade commentGrade = new Grade() { GradeValue = grade };
            //todo
            GradeHistory.Add(commentGrade);
            NumberOfRatings = GradeHistory.Count;
            //Rating = GradeHistory.Sum() / NumberOfRatings;
        }
    }


    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        public float GradeValue { get; set; }
    }
}