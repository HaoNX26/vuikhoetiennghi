using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
    public class DataCollections : CollectionBase
    {
        String _DataTable = "";
        String _ORDERBY = "";
        String _Filter = "";
        public DataCollections(String m_ObjectName)
        {
            this._DataTable = m_ObjectName;
        }
    public DataCollections( )
    {
        
    }
    public string FILTER
        {
            get
            {
                return _Filter;
            }
            set
            {
                _Filter = value;
            }
        }
        public string ORDERBY
        {
            get
            {
                return _ORDERBY;
            }
            set
            {
                _ORDERBY = value;
            }
        }
        public string DataTable
        {
            get
            {
                return _DataTable;
            }
            set
            {
                _DataTable = value;
            }
        }
        public void Add(DataTypes _DataType, String _DataField, FieldTypes _FieldType, Object _DataValue, String _Expression)
        {
            DataItems fField = new DataItems();
            fField.DataType = _DataType;
            if (_DataType == DataTypes.NVarchar )
            {
                if (_DataValue == null) _DataValue = "";
                fField.DataValue = _DataValue.ToString().Replace("'","''");
            }
            else if (_DataType == DataTypes.DateTime) {
                if (_DataValue != null) {
                    DateTime d = DateTime.Parse(_DataValue.ToString());
                    fField.DataValue  = d.Year.ToString() + "-" + d.Month.ToString() + "-" + d.Day.ToString() + " " + d.Hour.ToString() + ":" + d.Minute.ToString() + ":" + d.Second.ToString();  

                }
            }
            else
            {
                fField.DataValue = _DataValue;
            }
            fField.FieldType = _FieldType;
           
            fField.DataField = _DataField;
            fField.Expression = _Expression;
            base.List.Add(fField);
        }
        public void Add(String  _DataType, String _DataField, FieldTypes _FieldType, Object _DataValue, String _Expression)
        {
            DataItems fField = new DataItems();
            if (_DataType.ToUpper() == "NUMBER")
            {
                fField.DataType = DataTypes.Number;
            }
            else if (_DataType.ToUpper() == "NVARCHAR") {
                fField.DataType = DataTypes.NVarchar ;
            }
            else if (_DataType.ToUpper() == "DATETIME")
            {
                fField.DataType = DataTypes.DateTime;
            }
            else {
                fField.DataType = DataTypes.Undefined;
            }
            fField.FieldType = _FieldType;
            fField.DataValue = _DataValue;
            fField.DataField = _DataField;
            fField.Expression = _Expression;
            base.List.Add(fField);
        }
        public void Remove(DataItems value)
        {
            base.List.Remove(value);
        }
        public bool Contains(DataItems value)
        {
            return base.List.Contains(value);
        }
        public void Remove(String p_field_name )
        {
            for (int i = 0; i < base.List.Count; ++i)
            {
                DataItems fField = (DataItems)base.List[i];
                if (fField.DataField.Equals(p_field_name))
                {
                    base.List.RemoveAt(i);
                    break;
                }
            }
        }        

        public void EditValue(String p_field_name, Object p_value)
        {
            for (int i = 0; i < base.List.Count; ++i) {
                DataItems fField = (DataItems)base.List[i];
                if (fField.DataField.Equals(p_field_name))
                {
                    fField.DataValue = p_value;
                    base.List.RemoveAt(i);
                    base.List.Add(fField);
                    break;
                }
            }
        }        
        public DataItems this[int id]
        {
            get
            {
                return (DataItems)base.List[id];
            }
            set
            {
                base.List[id] = value;
            }
        }
        public new int Count()
        {
            return base.List.Count;
        }
        
    }
 
