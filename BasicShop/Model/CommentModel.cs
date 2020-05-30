using System;
using System.ComponentModel;

namespace BasicShop.Model
{
    public class CommentModel : INotifyPropertyChanged
    {
        private int _commentId;
        private int _userId;
        private string _username;
        private string _comment;
        private double _rating;

        public int CommentId
        {
            get { return _commentId; }
            set
            {
                if (value == _commentId) return;

                _commentId = value;
                OnPropertyChanged("CommentId");
            }
        }
        public int UserId
        {
            get { return _userId; }
            set
            {
                if (value == _userId) return;

                _userId = value;
                OnPropertyChanged("UserId");
            }
        }
        public string Username
        {
            get { return _username; }
            set
            {
                if (value == _username) return;

                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment) return;

                _comment = value;
                OnPropertyChanged("Comment");
            }
        }
        public double Rating
        {
            get { return _rating; }
            set
            {
                if (value == _rating) return;

                _rating = value;
                OnPropertyChanged("Rating");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
