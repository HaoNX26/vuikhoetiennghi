using System;
using System.Collections.Generic;
using System.Text;

public class DataItems
{

    public DataItems()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    DataTypes _DataType = DataTypes.NVarchar;
    Object _DataValue;
    String _DataField;
    FieldTypes _FieldType = FieldTypes.DefaultValue;
    String _Expression;
    public string Expression
    {
        get
        {
            return _Expression;
        }
        set
        {
            _Expression = value;
        }
    }
    public FieldTypes FieldType
    {
        get
        {
            return _FieldType;
        }
        set
        {
            _FieldType = value;
        }
    }
    public String DataField
    {
        get
        {
            return _DataField;
        }
        set
        {
            _DataField = value;
        }
    }
    public Object DataValue
    {
        get
        {
            return _DataValue;
        }
        set
        {
            _DataValue = value;
        }
    }
    public DataTypes DataType
    {
        get
        {
            return _DataType;
        }
        set
        {
            _DataType = value;
        }
    }
}