﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace OgcToolkit.Ogc.Filter.V110
{

#pragma warning disable 3009
    partial class Or:
        IBinaryLogicalOperator
    {

        Func<Expression, Expression, BinaryExpression> IBinaryLogicalOperator.OperatorExpression
        {
            get
            {
                return Expression.OrElse;
            }
        }
    }
#pragma warning restore 3009
}
