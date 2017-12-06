using System;
using System.Collections.Generic;
using System.Text;

namespace Botwave.XQP.API.Entity
{
    public class FiledInfo
    {
        private string fname;
        private string name;
        private string comment;
        private int itemDataType;
        private int itemType;
        private string dataSource;
        private string dataBinder;
        private string defaultValue;
        private int left;
        private int top;
        private int width;
        private int height;
        private bool rowExclusive;
        private bool require;
        private string validateType;
        private string maxVal;
        private string minVal;
        private string op;
        private string opTarget;
        private string errorMessage;
        private string showSet;
        private string readonlySet;

        public string FName
        {
            get { return fname; }
            set { fname = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public int ItemDataType
        {
            get { return itemDataType; }
            set { itemDataType = value; }
        }

        public int ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        public string DataSource
        {
            get { return dataSource; }
            set { dataSource = value; }
        }

        public string DataBinder
        {
            get { return dataBinder; }
            set { dataBinder = value; }
        }

        public string DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public bool RowExclusive
        {
            get { return rowExclusive; }
            set { rowExclusive = value; }
        }

        public bool Require
        {
            get { return require; }
            set { require = value; }
        }

        public string ValidateType
        {
            get { return validateType; }
            set { validateType = value; }
        }

        public string MaxVal
        {
            get { return maxVal; }
            set { maxVal = value; }
        }

        public string MinVal
        {
            get { return minVal; }
            set { minVal = value; }
        }

        public string Op
        {
            get { return op; }
            set { op = value; }
        }

        public string OpTarget
        {
            get { return opTarget; }
            set { opTarget = value; }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = value; }
        }

        public string ShowSet
        {
            get { return showSet; }
            set { showSet = value; }
        }

        public string ReadonlySet
        {
            get { return readonlySet; }
            set { readonlySet = value; }
        }

    }
}
