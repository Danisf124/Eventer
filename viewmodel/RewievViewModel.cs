using System;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Eventer
{
    internal class ReviewViewMode
    {
        public List<Review> Reviews {get; private set;}

        public string? ErrorMassage {get; private set;}
        public bool IsBusy {get; private set;}

        public ReviewViewMode()
        {
            Reviews = new List<Review>();
            IsBusy = false; 
            ErrorMassage = null;
        }

        public void AddReview(Review review)
        {
            Reviews.Add(review);
        }

        public bool CreateReview(Guid event_id, Guid user_id, int rating, string? comment)
        {
            IsBusy = true;
            ErrorMassage = null;

            try
            {
                Review review = new Review(event_id, user_id, rating, comment);

                AddReview(review);

                return true;

            }
            catch(Exception ex)
            {
                ErrorMassage = ex.Message;
                return false;
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}