﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace OgcToolkit.Ogc.Filter.V110
{

    partial class PropertyIsGreaterThan:
        IBinaryComparisonOperator
    {

        Func<Expression, Expression, BinaryExpression> IBinaryOperator.OperatorExpression
        {
            get
            {
                return Expression.GreaterThan;
            }
        }
    }
}
