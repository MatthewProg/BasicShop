﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicShop.Model
{
    public class CartModel : INotifyPropertyChanged
    {
        private int _productId;
        private string _productName;
        private int _quantity;

        public int ProductId
        {
            get { return _productId; }
            set
            {
                if (value == _productId) return;

                _productId = value;
                OnPropertyChanged("ProductId");
            }
        }
        public string ProductName
        {
            get { return _productName; }
            set
            {
                if (value == _productName) return;

                _productName = value;
                OnPropertyChanged("ProductName");
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                if (value == _quantity) return;

                _quantity = value;
                OnPropertyChanged("Quantity");
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