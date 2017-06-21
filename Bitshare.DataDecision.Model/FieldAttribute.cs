using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bitshare.DataDecision.Model
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FieldAttribute : Attribute
    {
        public FieldAttribute()
        {

        }
        public FieldAttribute(string columnName)
        {
            this.columnName = columnName;
        }
        private string columnName = null;
        private string description = null;
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }
        private string dataType;
        public string DataType
        {
            get
            {
                return dataType;
            }
            set
            {
                dataType = value;
            }
        }
        /// <summary>
        /// 数据展现形式
        /// </summary>
        private string dataFormat;
        public string DataFormat
        {
            get
            {
                return dataFormat;
            }
            set
            {
                dataFormat = value;
            }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        private int width;
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public bool IsFixed { set; get; }
        public bool IsFrozen { set; get; }
        /// <summary>
        /// 字段是否导出
        /// </summary>
        private bool isExport;
        public bool IsExport
        {
            get
            {
                return isExport;
            }
            set
            {
                isExport = value;
            }
        }
        /// <summary>
        /// 当字段类型为bool类型,true展现字段
        /// </summary>
        private string trueShow;
        public string TrueShow
        {
            get
            {
                return trueShow;
            }
            set
            {
                trueShow = value;
            }
        }
        /// <summary>
        /// 当字段类型为bool类型,false展现字段
        /// </summary>
        private string falseShow;
        public string FalseShow
        {
            get
            {
                return falseShow;
            }
            set
            {
                falseShow = value;
            }
        }

        private int order;
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }


        public bool IsPrimaryKey { set; get; }
    }
}
