using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaVisualBasic.Runtime.Interpreter;

public abstract class VB6Visitor<T> : VB6BaseVisitor<T>
{
    public T AsType<T>(Vb6Value value)
    {
        if (!TryUnpack(value, out T val))
            throw new VBCompileErrorException("Type mismatch");
        return val;
    }

    public List<T> AsType<T>(IReadOnlyList<Vb6Value> indexes)
    {
        return indexes.Select(AsType<T>).ToList();
    }

    public bool TryUnpack<T>(Vb6Value val, out T tout)
    {
        tout = default!;
        if (typeof(T) == typeof(int))
        {
            if (val.Type == Vb6Value.ValueType.Integer)
            {
                tout = (T)(object)(int)val.Value!;
                return true;
            }
            if (val.Type == Vb6Value.ValueType.String)
            {
                if (int.TryParse((string?)val.Value ?? "", out var asInt))
                {
                    tout = (T)(object)asInt;
                    return true;
                }
            }
            return false;
        }
        if (typeof(T) == typeof(bool))
        {
            if (val.Type == Vb6Value.ValueType.Boolean)
            {
                tout = (T)(object)(bool)val.Value!;
                return true;
            }
            return false;
        }
        if (typeof(T) == typeof(float))
        {
            if (val.Type == Vb6Value.ValueType.Integer)
            {
                tout = (T)(object)(float)(int)val.Value!;
                return true;
            }
            if (val.Type == Vb6Value.ValueType.Single)
            {
                tout = (T)(object)(float)val.Value!;
                return true;
            }
            return false;
        }
        if (typeof(T) == typeof(double))
        {
            if (val.Type == Vb6Value.ValueType.Integer)
            {
                tout = (T)(object)(double)(int)val.Value!;
                return true;
            }
            if (val.Type == Vb6Value.ValueType.Single)
            {
                tout = (T)(object)(float)val.Value!;
                return true;
            }
            if (val.Type == Vb6Value.ValueType.Double)
            {
                tout = (T)(object)(double)val.Value!;
                return true;
            }
            return false;
        }

        return false;
    }

    public bool TryUnpack<T>(Vb6Value left, Vb6Value right, out T tleft, out T tright)
    {
        tright = default!;
        return TryUnpack(left, out tleft) && TryUnpack(right, out tright);
    }

    public bool TryConvert(ref Vb6Value a, ref Vb6Value b)
    {
        if (a.Type == b.Type)
            return true;

        if (a.Type == Vb6Value.ValueType.Integer &&
            b.Type == Vb6Value.ValueType.String)
            return TryConvert(ref b, ref a);

        if (a.Type == Vb6Value.ValueType.String &&
            b.Type == Vb6Value.ValueType.Integer)
        {
            if (int.TryParse(a.Value?.ToString() ?? "", out var aInt))
            {
                a = new Vb6Value(aInt);
                return true;
            }
            else
                throw new Exception("Type mismatch");
        }
        throw new Exception("Type mismatch");
    }

    public Vb6Value.ValueType ExtractType(VB6Parser.AsTypeClauseContext? asTypeClause, bool isArray)
    {
        Vb6Value.ValueType type = Vb6Value.ValueType.EmptyVariant;
        if (asTypeClause != null)
        {
            if (asTypeClause.NEW() != null)
                throw new NotImplementedException("New as type not implemented");
            if (asTypeClause.fieldLength() != null)
                throw new NotImplementedException("fieldLength as type not implemented");
            if (asTypeClause.type().complexType() != null)
                throw new NotImplementedException("complex type as type not implemented");
            if (asTypeClause.type().baseType().STRING() != null)
                type = Vb6Value.ValueType.String;
            else if (asTypeClause.type().baseType().INTEGER() != null)
                type = Vb6Value.ValueType.Integer;
            else if (asTypeClause.type().baseType().SINGLE() != null)
                type = Vb6Value.ValueType.Single;
            else if (asTypeClause.type().baseType().DOUBLE() != null)
                type = Vb6Value.ValueType.Double;
            else if (asTypeClause.type().baseType().BOOLEAN() != null)
                type = Vb6Value.ValueType.Boolean;
            else
                throw new NotImplementedException("base type " + asTypeClause.type().baseType().GetChild(0) + " not implemented");
        }

        if (isArray)
            type = new Vb6Value.ValueType(type, true);

        return type;
    }
}