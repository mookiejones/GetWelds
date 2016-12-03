﻿using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetWelds
{
    public class Position : ViewModelBase
    {
        public Position()
        {
        }
        public Position(string filename, int style)
        {
        }

        /// <summary>
        /// The <see cref="Filename" /> property's name.
        /// </summary>
        public const string FilenamePropertyName = "Filename";

        private string _fileName = string.Empty;

        /// <summary>
        /// Sets and gets the Filename property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Filename
        {
            get
            {
                return _fileName;
            }

            set
            {
                if (_fileName == value)
                {
                    return;
                }

                RaisePropertyChanging(FilenamePropertyName);
                _fileName = value;
                RaisePropertyChanged(FilenamePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Style" /> property's name.
        /// </summary>
        public const string StylePropertyName = "Style";

        private int _style = -1;

        /// <summary>
        /// Sets and gets the Style property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public int Style
        {
            get
            {
                return _style;
            }

            set
            {
                if (_style == value)
                {
                    return;
                }

                RaisePropertyChanging(StylePropertyName);
                _style = value;
                RaisePropertyChanged(StylePropertyName);
            }
        }


        private bool _isContinuous;
        public bool IsContinuous { get { return _isContinuous; } set { _isContinuous = value; RaisePropertyChanged("IsContinuous"); } }

        private double _velocity;
        public double Velocity { get { return _velocity; } set { _velocity = value; RaisePropertyChanged("Velocity"); } }
        private string _name;
        public string Name { get { return _name; } set { _name = value; RaisePropertyChanged("PositionName"); } }
        private PositionType _motionType = PositionType.None;
        public PositionType MotionType { get { return _motionType; } set { _motionType = value; RaisePropertyChanged("MotionType"); } }

        private int _lineNumber;
        public int LineNumber { get { return _lineNumber; } set { _lineNumber = value; RaisePropertyChanged("LineNumber"); } }

        private string _x;
        public string X { get { return _x; } set { _x = value; RaisePropertyChanged("X"); } }
        private string _y;
        public string Y { get { return _y; } set { _y = value; RaisePropertyChanged("Y"); } }
        private string _z;
        public string Z { get { return _z; } set { _z = value; RaisePropertyChanged("Z"); } }
        private string _a;
        public string A { get { return _a; } set { _a = value; RaisePropertyChanged("A"); } }
        private string _b;
        public string B { get { return _b; } set { _b = value; RaisePropertyChanged("B"); } }
        private string _c;
        public string C { get { return _c; } set { _c = value; RaisePropertyChanged("C"); } }
        private string _s;
        public string S { get { return _s; } set { _s = value; RaisePropertyChanged("S"); } }
        private string _t;
        public string T { get { return _t; } set { _t = value; RaisePropertyChanged("T"); } }


        private string _e1;
        public string E1 { get { return _e1; } set { _e1 = value; RaisePropertyChanged("E1"); } }
        private string _e2;
        public string E2 { get { return _e2; } set { _e2 = value; RaisePropertyChanged("E2"); } }


    }
}
