﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace OgcToolkit.Ogc.Filter.V110
{

#pragma warning disable 3009
    partial class Mul:
        IBinaryOperator
    {

        Func<Expression, Expression, BinaryExpression> IBinaryOperator.OperatorExpression
        {
            get
            {
                return Expression.Multiply;
            }
        }
    }
#pragma warning restore 3009
}
